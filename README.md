Scene 설명 및 조작법

# ScreenEffectsTest
여러 가지 사진 모아두고 화면 필터 테스트한 곳
조작법 없음

# Pathfinding
길 찾기 시스템.

왼쪽의 트래커는 오른쪽의 목표까지 도달하는 길을 찾아냄.

## Pathfinding 조작법
(추적 시작 이전에) 마우스 좌클릭 누르고 있기 : 마우스 위치에 장애물 그리기

(추적 시작 이전에) [D] 를 누른 상태로 마우스 좌클릭 누르고 있기 : 마우스에 닿은 장애물 지우기

스페이스 바 : 추적 시작

[R] : 씬 다시 불러오기 (리셋)

## Pathfinding 설정
(PathfindManager 오브젝트의 설정)

maxAttempts : 길 탐색의 범위 (int)

showRoad : 찾은 길을 표시할지의 여부

doRemoveRedundantRoad : 확정된 길에서 필요 없는 동선을 무시할지의 여부

doCleanUpNodes : 필요 없는 동선을 전부 제거할지의 여부


(Tracker 오브젝트의 설정)

moveSpeed : 트래커의 이동 속도 (float)

# KeyPad
키패드를 구현한 곳.

리셋할 때마다 4자리의 무작위 비밀번호가 설정됨.

"HACK" 이라고 적힌 버튼을 누르면 비밀번호를 알아낼 수 있음.


## KeyPad 조작법
[R] : 씬 다시 불러오기 (리셋)