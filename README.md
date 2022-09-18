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


# ugs + 어드레서블

### 코드 순서
1. ugs 서비스 초기화 + 익명 로그인
2. remote config 에서 카탈로그 json 주소 가져오기
3. 카탈로그 로드(Addressables.LoadContentCatalogAsync)
4. 카탈로그 업데이트(Addressables.CheckForCatalogUpdates)  - 설정에 수동체크 되있는지 확인
5. 어셋 사용

### 어드레서블 세팅
1. ugs ccd 에서 badge:latest 주소를 유니티 플젝 어드래서블 설정의 remote build path 로 설정해야함 (릴리즈주소는 매번 바뀜)
2. 업로드한 카탈로그 json 의 경로를 remote config 에서 설정해야함
3. Catalog Download Timeout 설정
4. Player Version Override 설정(필수는 아님)
5. 카탈로그 업데이트 수동으로(Only update catalogs manually)
- 헷갈리면 걍 AddressableAssetSettings 옵션들 하나씩 살펴보기

### 기타
- 안드로이드의 경우 UGS 초기화(UnityServices.InitializeAsync)가 안되서 target sdk 33(최신) 으로 설정하니 해결됨

### 리소스 업데이트
1. 어셋 어드레서블 준비
2. 빌드할 플랫폼 설정 Android / iOS 
3. AddressableAssetSettings 설정
 - Profile 확인
 - Player Version Override 값 변경하지말고 그대로 사용 (Update a previous build 로 할경우 파일명이 같아야함)
 - addressable_content_state.bin 파일 위치 확인
4. 어드레서블 빌드 Addressable Groups -> Build -> Update a previous build
5. UGS - CCD 에 빌드된 파일들 업로드(플랫폼 버켓 들어가서 기존꺼 전체 삭제후 업로드 & 릴리즈)
6. 업로드된 catalog...json 파일의 경로를 복사후 UGS - Remote Config 값을 교체 & publish
7. 플랫폼 바꿔서 2번부터 다시 수행
