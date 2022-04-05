# Addressable Test

## 시나리오
씬별로 그룹핑하여 리모트 설정  
엔트리씬 > 홈씬 <> 프로필씬  
  

## 확인해볼거
- [x] 어셋 빌드한거 리모트에 업로드후 씬별 이동 잘 되는지
- [x] 번들 다운로드중 에러발생한 경우
- [x] 기존 번들 내용이 바뀌었을 경우
  - Android 실 기기에서 되는거 확인함
  - iOS 실 기기에서 되는거 확인함
- [x] 배포용앱과 번들 분리하기(Analyze -> Unfixable -> ... Duplicate 이거에 걸리면 해당 어셋이 앱과 번들에 같이 들어있는거임)



## 알아야할거
- CRC disable
- Addressable Anlyze > Fixable Rules 에 걸리는 것들 상위 그룹으로 위치시켜야함. 안하면 안드로이드가 제대로 안됨
- 다운로드한 어셋이 자주색으로 이상하게 보인다? -> 디펜던시중 어드레서블 체크가 빠진거
- 빌드 플레이할때 어드레서블로 같이 빌드할건지 옵션 해제 해서 혹시라도 플레이 눌러서 번들이 다시 만들어지지 않도록 하자
  - AddressableAssetSettings > Build -> Build Addressables on Player Build
- 폴더로 사용가능, 단 번들파일이 폴더 하나로 묶여서 나온다