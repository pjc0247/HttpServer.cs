Adaptor
====
어댑터는 웹 서버와 웹 어플리케이션을 연결하는 브릿지 역할을 합니다.<br>
웹 서버가 데이터 또는 이벤트(연결/종료)를 받으면 어댑터로 전달하고, 어댑터는 이 데이터를 가지고 다시 웹 어플리케이션이 이해할 수 있는 포멧으로 변경해 웹 어플리케이션에게 전달합니다.<br>
<br>
__참고__<br>
* https://github.com/owin/owin
* https://ko.wikipedia.org/wiki/%EA%B3%B5%EC%9A%A9_%EA%B2%8C%EC%9D%B4%ED%8A%B8%EC%9B%A8%EC%9D%B4_%EC%9D%B8%ED%84%B0%ED%8E%98%EC%9D%B4%EC%8A%A4
<Br>

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
