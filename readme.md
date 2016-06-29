Core
----
* [x] [스트리밍 HTTP 요청 파싱](src/HttpServ/Http/HttpParser.cs)
  * [ ] LINQ 남발하는 파서 정리
  * [ ] C++/CLI로 파서 재작성
* [x] 간단한 리스폰스 보내기
* [x] [gzip 응답](src/HttpServ/Http/Middlewares/GzipEncoder.cs)
* [ ] chunked 응답 
* [x] [keep alive](src/HttpServ/Http/HttpSession.cs#L44-L51)
  * [x] timeout (server-side)
* [x] [미들웨어 인터페이스 제작](src/HttpServ/IMiddleware.cs)
* [ ] WebSocket
  * [x] [Handshaking](src/HttpServ/WebSocket/Middlewares/WebSocketHandshaker.cs)
    * [ ] 버전 체크
    * [ ] 서브프로토콜
  * [x] 유연한 프로토콜 업그레이드
  * [x] [프레임 파싱](src/WebSocketParser/WebSocketParser.cpp)
    * [x] Payload7
    * [x] Payload16
    * [ ] Payload64
  * [x] 프레임 조립
    * [x] Payload7
    * [ ] Payload16
    * [ ] Payload64  
  * [x] [fin 0/1 (분할된 패킷)](src/HttpServ/WebSocket/WebSocketSession.cs#L68-L85)
  * [ ] 연결 관리
    * [ ] 먼저 보내기
    * [ ] 먼저 끊기
  * [x] [핑퐁](src/HttpServ/WebSocket/Middlewares/PingPong.cs)
  * [ ] 에코 서버
* [ ] 예외 처리
  * [ ] 파싱 에러 (400)
  * [ ] 서버 익셉션 (500)
  * [ ] 잘못된 버전 (505)
  * [ ] 마스크 없는 웹소켓 패킷
* [x] HTTPS

Adaptor
----
* [x] Adaptor 연결고리 제작
* [ ] 애플리케이션과의 연결고리 제작
* [ ] CSX Adaptor 제작
* [ ] Static Adaptor 제작 (단순 파일 서버)
* [ ] OWIN Adaptor (Nancy)

Application
----
* [ ] 간단하게 프로그래밍 가능한 어플리케이션 레이어 구성
