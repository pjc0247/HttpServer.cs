HttpServer.cs
----
HttpServer implementation with C#.<br?

[doc](doc/)

Core
----
* [x] [Streaming HTTP request parsing.](src/HttpServ/Http/HttpParser.cs)
  * [ ] LINQ 남발하는 파서 정리
  * [ ] C++/CLI로 파서 재작성
* [x] Response with simple format
* [x] [gzip encoding](src/HttpServ/Http/Middlewares/GzipEncoder.cs)
* [ ] chunked encoding
* [x] [keep alive](src/HttpServ/Http/HttpSession.cs#L44-L51)
  * [x] timeout (server-side)
* [x] [Interface for middleware](src/HttpServ/IMiddleware.cs)
* [ ] WebSocket
  * [x] [Handshaking](src/HttpServ/WebSocket/Middlewares/WebSocketHandshaker.cs)
    * [ ] 버전 체크
    * [ ] 서브프로토콜
  * [x] Protocol Upgrading
  * [x] [Frame parsing](src/WebSocketParser/WebSocketParser.cpp)
    * [x] Payload7
    * [x] Payload16
    * [ ] Payload64
  * [x] Frame contstruction
    * [x] Payload7
    * [x] Payload16
    * [ ] Payload64  
  * [x] [fin 0/1 (Segmented packet)](src/HttpServ/WebSocket/WebSocketSession.cs#L68-L85)
  * [ ] 연결 관리
    * [ ] 먼저 보내기
    * [x] [먼저 끊기](src/HttpServ/Common/CloseSessionException.cs)
  * [x] [PingPong](src/HttpServ/WebSocket/Middlewares/PingPong.cs)
  * [ ] Echo Server
  * [x] 프레그먼트
    * [x] 프레그먼트 메세지 수신(조합)
    * [x] 프레그먼트 사이에 오는 컨트롤 프레임 처리
    * [ ] 프레그먼트 메세지 송신
* [ ] Exception handling
  * [ ] Parsing error (400)
  * [x] Internal server error (500)
  * [ ] 잘못된 버전 (505)
  * [x] 마스크 없는 웹소켓 패킷
  * [x] Request too long
  * [ ] Request timeout (408)
    * [x] HTTP
    * [ ] WebSocket
* [x] HTTPS
* [ ] Streaming response
* [ ] HTTP2.0
  * [x] Protocol upgrading
    * [x] H2C
    * [x] Direct
    * [ ] H2 (ALPN)
  * [ ] HPACK
    * [ ] Static Table
    * [ ] Dynamic Table
    * [x] String Decoding
    * [ ] String Encoding
  * [ ] Stream
  * [x] Ping
  * [ ] Setting
  * [ ] Data
    * [ ] 조각난 데이타 프레임
    * [x] Padding
    
Adaptor
----
* [x] Adaptor 연결고리 제작
* [ ] 애플리케이션과의 연결고리 제작
* [ ] CSX Adaptor implementation
* [x] [Static Adaptor implementation (FileServer)](src/StaticFileAdaptor/Program.cs)
* [ ] OWIN Adaptor (Nancy)
* [ ] CGI
  * [x] PHP
  
Application
----
* [ ] 간단하게 프로그래밍 가능한 어플리케이션 레이어 구성


USAGE
====

Creat Server
----
```cs
// HTTP SERVER
var serv = ServerFactory.CreateHttp<SimpleAdaptor>();

// HTTPS SERVER
var cert = new X509Certificate2("server.pfx", "password");
var serv = ServerFactory.CreateHttps<SimpleAdaptor>(cert);

serv.Open(9916);
```
