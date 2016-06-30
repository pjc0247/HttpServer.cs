Adaptor
====

```cs
public class MyAdaptor : IAdaptor {
  public void OpOpen(Session session) {
  }
  public void OnClose(Session session) {
  }
  
  public void OnRequest(Session session, WebRequest request) {
  }
}
```

연결 종료시키기
----
기본적으로 `OnRequest` 메소드에서 예외를 발생시키면 `500 Internal Server Error` 응답을 보냅니다.

```cs
public void OnRequest(Session session, WebRequest request) {
  throw new InvalidOperationException("BYE");
}
```

특수한 예외
----
몇몇 익셉션은 예약되어있으며 `500 Internal Server Error`로 처리되지 않습니다.

```cs
throw new CloseWebSocketException(
  WebSocket.StatusCode.ServerError, "BYE");
```
```cs
throw new RequestTooLongException();
```
