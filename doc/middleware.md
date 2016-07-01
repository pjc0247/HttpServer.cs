미들웨어
====
미들웨어는 웹 서버와 어댑터 사이에서 요청을 가로챌 수 있는 기능을 제공합니다.<br>
웹 서버에서 어댑터로 가기 전(Preprocess)와 / 어댑터에서 돌아온 응답이 돌아온 후 웹 서버가 처리하기 전(Postprocess) 시점에 훅을 거는것이 가능합니다.

```cs
public class MyMiddleware : IMiddleware {
  public WebResponse OnPreprocess(Session session, WebRequest request) {
    // 만약 null 이 아닌 값을 리턴하면 요청은 어댑터로 가지 않고
    // 미들웨어 레벨에서 끝나게 됩니다.
    //
    // 몇몇 패킷(컨트롤 프레임)등은 어댑터로 넘어갈 필요가 전혀 없으며
    // 그러한 패킷에 대한 처리를 이곳에서 할 수 있습니다.
    // (웹소켓의 핑/퐁/종료 핸드쉐이킹)
  }
  
  void OnPostprocess(Session session, WebRequest request, WebResponse response) {
  
  }
}
```
