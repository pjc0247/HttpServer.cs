using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Diagnostics;

namespace HttpServ.CGI
{
    public class CgiAdaptor : IAdaptor
    {
        public void OpOpen(Session session)
        {
            Console.WriteLine("OnOpen");
        }
        public void OnClose(Session session)
        {
            Console.WriteLine("OnClose");
        }

        public WebResponse OnRequest(Session session, WebRequest req)
        {
            if (req is Http.HttpRequest)
                return OnHttpRequest((Http.HttpRequest)req);
            else if (req is WebSocket.WebSocketRequest)
                throw new NotImplementedException("");

            return null;
        }

        private Http.HttpResponse OnHttpRequest(Http.HttpRequest request)
        {
            Console.WriteLine("SimpleAdaptor::OnHttpRequest " + request.requestUri);

            return ExecCgi(request);
        }

        private Http.HttpResponse ExecCgi(Http.HttpRequest request)
        {
            Process p = new Process();
            var env = p.StartInfo.EnvironmentVariables;

            var tokens = request.requestUri.Split(new char[] { '?' }, 2);
            
            if (tokens.Length != 2)
                env["QUERY_STRING"] = "";
            else
                env["QUERY_STRING"] = tokens[1];

            env["REQUEST_URI"] = request.requestUri;
            env["DOCUMENT_URI"] = "/";

            var scriptPath = request.path;
            if (scriptPath.EndsWith("/"))
                scriptPath += "index.php";
            scriptPath = scriptPath.Substring(1);

            env["SCRIPT_FILENAME"] = scriptPath;
            env["SCRIPT_NAME"] = "IM JINWOO";

            env["GATEWAY_INTERFACE"] = "CGI/1.1";

            env["SERVER_SOFTWARE"] = "ASDF";
            env["REDIRECT_STATUS"] = "CGI";

            env["SERVER_ADDR"] = "127.0.0.1";
            env["SERVER_NAME"] = "JIWNOOSERVER";
            env["SERVER_PORT"] = "80";
            env["SERVER_PROTOCOL"] = "HTTP/1.1";
            env["SERVER_SIGNATURE"] = "";
            env["SERVER_SOFTWARE"] = "HttpServ.cs";

            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.RedirectStandardError = true;
            p.StartInfo.FileName = "C:\\Users\\hyun\\Downloads\\php-7.0.8-nts-Win32-VC14-x86\\php-cgi.exe";
            p.StartInfo.WorkingDirectory = "C:\\Users\\hyun\\Downloads\\php-7.0.8-nts-Win32-VC14-x86\\";

            p.Start();

            if (p.WaitForExit(1000))
            {

            }
            else
            {
                throw new InvalidOperationException("CGI::TimeOut");
            }

            Console.WriteLine("END PHP");

            var buffer = p.StandardOutput.ReadToEnd();
            var error = p.StandardError.ReadToEnd();

            Console.WriteLine(error);

            Console.WriteLine("PHP DATA");
            Console.WriteLine(buffer);

            var builder = new Http.HttpRequestBuilder();
            builder.parser.ForceSkipRequestLine();
            builder.parser.ForceAllowContentWithoutLength();

            foreach(var w in builder.Write(Encoding.UTF8.GetBytes(buffer)))
                ;
            var cgiResult = builder.WriteEof();

            foreach(var header in cgiResult.headers)
            {
                Console.WriteLine($"{header.Key} : {header.Value}");
            }

            Console.WriteLine(Encoding.UTF8.GetString(cgiResult.content));

            var response = new Http.HttpResponse(Http.ResponseCode.OK);

            foreach(var header in cgiResult.headers)
            {
                response.headers[header.Key] = header.Value;
            }
            response.SetContent(cgiResult.content);

            return response;
        }
    }
}
