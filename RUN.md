# Unity 실행

1. Python 3.10+에서 `python setup_unity.py` 실행.
2. Unity Hub > Add project from disk > 생성된 `UnityProject` 폴더를 Unity 6.3 편집기 (프로젝트 기준 6000.3.0f1, 설치된 호환 패치 선택 가능)로 엽니다. Built-in 렌더러용 코드입니다.
3. Edit > Project Settings > Player > Active Input Handling을 **Both** 또는 **Input Manager (Old)** 로 설정합니다.
4. 상단 **MetaSonGi > Create Demo Scene** 실행 후 Play. 씬 생성은 기존 열린 씬을 교체하므로 작업 중인 씬은 먼저 저장하세요.
5. 실행 파일은 File > Build Profiles에서 플랫폼을 선택해 Build. 생성 메뉴가 Demo 씬을 빌드 목록에 등록합니다.

자동 장면 생성 코드와 런타임 코드를 포함합니다. 외부 3D 에셋 없이 기본 도형으로 동작합니다. Unity 편집기·실제 헤드셋은 이 개발 환경에 없어 컴파일·기기 검증은 아직 하지 못했습니다.

## 선택: 헤드셋 VR
Unity Package Manager에서 해당 Unity 버전과 호환되는 XR Plug-in Management 및 OpenXR를 설치하고, 대상 플랫폼의 OpenXR를 켠 뒤 Project Validation 항목을 해결하세요. 기본 데스크톱 실행에는 패키지가 필요 없습니다.
공간 전시·공장 프로젝트는 헤드 포즈와 오른손 트리거 레이, 왼손 스틱 이동 코드를 포함합니다. 헤드셋 입력 프로필은 대상 기기에 맞게 설정해야 합니다. OnGUI 패널은 데스크톱 UI이며 VR 안에 표시되지 않습니다. 공장은 월드 콘솔로 조작하고, 전시는 PC에서 작품을 먼저 배치합니다.

저장 파일은 Unity의 `Application.persistentDataPath`에 생성됩니다.

## 음성 입력
별도 터미널에서 `python bridge.py` 실행 → Chrome/Edge로 http://localhost:8767 열기 → 음성 버튼. 브라우저의 음성 인식 지원과 마이크 권한이 필요합니다. 브라우저 제공자의 음성 서비스가 사용될 수 있습니다. 텍스트 명령은 항상 사용 가능합니다. Unity는 큐를 폴링해 가상 관절을 움직입니다.

## 실제 로봇 경계
현재 실제 장비 전송은 구현하지 않았습니다. 대상 기종이 미정이라 모터를 제어하는 프로토콜을 추측하지 않습니다. `IRobotTransport.cs`는 실제 로봇 연결을 위한 인터페이스 계약이며 기본 구현은 DryRun입니다. 실제 어댑터를 작성할 때 관절·속도·작업영역·통신 단절 검증과 물리 비상정지를 추가해야 합니다.
