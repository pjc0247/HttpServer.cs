using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Net.Sockets;
using System.Net;
using System.IO;

namespace HttpServ
{
    using WebSocket;
    using Http;

    public class Session
    {
        private long id { get; set; }

        private ISessionImpl impl { get; set; }
        private ISessionImpl upgradeImplTo { get; set; }

        internal Server server { get; private set; }
        private TcpClient sock { get; set; }
        private Stream stream { get; set; }

        private bool isWebSocket { get; set; }
        private bool continueReceiving { get; set; }
        private int lastRequestTime { get; set; }

        public SessionState state { get; internal set; }

        public Session(TcpClient sock, Stream stream, Server server, long id)
        {
            if (stream == null)
                throw new ArgumentException("stream -> null");

            this.id = id;
            this.sock = sock;
            this.server = server;
            this.stream = stream;
            this.state = SessionState.Ready;

            this.impl = new HttpSession(server);
            impl.session = this;
        }

        internal void Open()
        {
            ThreadPool.QueueUserWorkItem((_) =>
            {
                IOWorker(sock);
            }, sock);

            server.adaptor.OpOpen(this);
        }
        public void Close()
        {
            if (state == SessionState.Closed)
                return;

            stream.Close();
            sock.Close();

            server.adaptor.OnClose(this);

            state = SessionState.Closed;
        }

        internal void SendLastResponse(WebResponse response)
        {
            if (state != SessionState.Opened) return;

            try {
                var responseBytes = impl.OnWriteData(null, response);

                state = SessionState.Closing;
                stream.WriteAsync(responseBytes, 0, responseBytes.Length)
                    .ContinueWith((e) =>
                    {
                        Close();
                    });
            }
            catch(Exception e)
            {
                Console.WriteLine(e);
                Close();
            }
        }

        /// <summary>
        /// 현재 세션을 웹소켓으로 업그레이드한다.
        /// </summary>
        internal void UpgradeToWebSocket()
        {
            if (impl is WebSocketSession)
                throw new InvalidOperationException("current session is already websocket");
            if (upgradeImplTo != null)
                throw new InvalidOperationException("already upgrading-state");

            isWebSocket = true;
            upgradeImplTo = new WebSocketSession();
        }
        /// <summary>
        /// 연속된 다음 요청을 처리하도록 한다.
        /// 이 메소드가 호출되지 않으면 응답 후 연결이 끊어진다.
        /// </summary>
        internal void AcceptNextRequest()
        {
            continueReceiving = true;
        }

        private async void IOWorker(TcpClient sock)
        {
            var buffer = new byte[1024];
            var errorQuit = false;

            state = SessionState.Opened;
            continueReceiving = true;
            lastRequestTime = Environment.TickCount;

            while (isWebSocket || continueReceiving)
            {
                continueReceiving = false;

                try
                {
                    using (var cts = new CancellationTokenSource(5000))
                    {
                        if (isWebSocket == false)
                        {
                            cts.Token.Register(() => { Close(); });

                            if (Environment.TickCount - lastRequestTime >= server.config.requestTimeout)
                                throw new RequestTimeoutException();
                        }

                        var read = await stream.ReadAsync(buffer, 0, buffer.Length);
                        var segment = new ArraySegment<byte>(buffer, 0, read);

                        if (read <= 0)
                            throw new CloseSessionException();

                        Console.WriteLine(Encoding.UTF8.GetString(segment.ToArray()));

                        Retry:
                        foreach (var request in impl.OnReceiveData(segment))
                        {
                            var response = server.ProcessRequest(this, request);

                            server.Postprocess(this, request, response);
                            var responseBytes = impl.OnWriteData(request, response);

                            await stream.WriteAsync(responseBytes, 0, responseBytes.Length);

                            if (upgradeImplTo != null)
                            {
                                var upgradable = impl as IProtocolUpgradable;
                                if (upgradable == null)
                                    throw new InvalidOperationException("previous protocol not upgradable");

                                impl = upgradeImplTo;
                                segment = new ArraySegment<byte>(upgradable.GetTrailingData());
                                upgradeImplTo = null;

                                if (segment.Count > 0)
                                    goto Retry;
                            }

                            lastRequestTime = Environment.TickCount;
                        }
                    }
                }
                catch (ObjectDisposedException e)
                {
                    errorQuit = true;
                }
                catch (CloseSessionException e)
                {
                    if (e.response != null)
                        SendLastResponse(e.response);

                    errorQuit = true;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);

                    var response = impl.OnErrorClose(e);
                    if (response != null)
                        SendLastResponse(response);

                    errorQuit = true;
                }

                if (errorQuit)
                    break;
            }

            if (state == SessionState.Opened)
                Close();
        }
    }
}
