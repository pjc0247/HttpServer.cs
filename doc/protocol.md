Protocol 추가하기
====

```cs
public class MyProtocol : ISessionImpl {
  public Session session { get; set; }
  
  // 이 메소드를 구현하여 바이너리 데이터를 WebRequest로 변환합니다.
  public IEnumerable<WebRequest> OnReceiveData(ArraySegment<byte> data) {
  }
  
  // 이 메소드를 구현하여 WebResponse를 바이너리 데이터로 변환합니다.
  public byte[] OnWriteData(WebRequest request, WebResponse response) {
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
