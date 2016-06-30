Protocol 추가하기
====
하나의 웹서버는 `HTTP/1.1` 혹은 `WebSocket` 처럼 복수개의 프로토콜을 지원할 수 있습니다.<br>
아래 문서는 새로운 프로토콜을 구현할 때 어떻게 해야 하는지를 설명합니다.

```cs
public class MyProtocol : ISessionImpl {
  public Session session { get; set; }
  
  // 이 메소드를 구현하여 바이너리 데이터를 WebRequest로 변환합니다.
  public IEnumerable<WebRequest> OnReceiveData(ArraySegment<byte> data) {
    // 네트워크로부터 데이터가 수신되면 이 메소드로 전달됩니다.
    // 
    // data 자체는 단순 바이너리 데이터이므로 패킷과 패킷 사이의 경계를 
    // ㅁㄴㅇㄻㄴㄹ
    
    yield return request;
  }
  
  // 이 메소드를 구현하여 WebResponse를 바이너리 데이터로 변환합니다.
  public byte[] OnWriteData(WebRequest request, WebResponse response) {
    // request 값은 null일 수 있습니다.
  }
}
```

업그레이드 가능한 프로토콜
----
`IProtocolUpgradable` 인터페이스를 추가하여 이 프로토콜이 업그레이드 가능한 프로토콜임을 나타냅니다.
```cs
public class MyProtocol : ISessionImpl, IProtocolUpgradable {
  /* .... */
  
  // 이 메소드를 구현하여 남은 데이터를 반환합니다.
  public byte[] GetTrailingData() {
    return trailingData;
  }
}
```
