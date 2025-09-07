using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTransitionBox : MonoBehaviour
{
    [Header("동작 설정")]
    public float activationDelay = 0;   // Scene 로드 이후 동작이 활성화되는 시간
    public string targetSceneName;  // 이동할 Scene 이름
    public string loadingSceneName; // 로딩 Scene 이름
    public Color CurtainColor = Color.black;    // 페이드 인/아웃에 사용할 커튼 오브젝트 색상
    public float WaitTime1; // 페이드 아웃 이전 대기 시간
    public float FadeOutTime = 0.5f;    // 페이드 아웃 지속시간
    public float LoadingTime = 5f;  // 보장할 최소 로딩 시간
    public float FadeInTime = 0.5f; // 페이드 인 지속시간
    public float WaitTime2; // 페이드 인 이후 대기시간

    [Header("플레이어 태그")]
    public string compareTag = "Player";    // 감지할 플레이어 태그.
    [Header("시작 지점에서 플레이어 움직임")]
    public bool start_shouldMove = false;   // 로딩 시작 지점에서 움직여야 하는지에 대한 여부. true여야 움직임.
    public bool start_shouldGoRight = false;    // start_shouldMove 가 true일 경우, 어디로 움직여야 할지에 대한 여부. true면 오른쪽으로 달림 (멈추지 않음)
    [Header("목표 지점에서 플레이어 움직임")]
    public Vector3 playerPosition;  // 목표 지점에서 플레이어가 생성될 위치
    public bool end_shouldMove = false;     // 목표 지점에서 움직여야 하는지에 대한 여부. true여야 움직임.
    public bool end_shouldGoRight = false;  // end_shouldMove 가 true일 경우, 어디로 움직여야 할지에 대한 여부. true면 오른쪽으로 달림 (멈추지 않음)
    public float waitMoveDuration;  // 목표 지점에서 플레이어가 playerPosition에 고정될 시간.
    public float moveDuration;  // 목표 지점에서 얼마나 오래 움직일지에 대한 시간

    private BoxCollider2D boxCol;
    private PlayerController playerController;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        boxCol.isTrigger = true;

        if (activationDelay > 0)
        {
            boxCol.enabled = false;
            StartCoroutine(DelayActivation(activationDelay));
        }
        else
        {
            boxCol.enabled = true;
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private IEnumerator DelayActivation(float delay)
    {
        yield return new WaitForSeconds(delay);
        boxCol.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(compareTag))
        {
            playerController = other.GetComponent<PlayerController>();

            if (playerController != null)
            {
                DontDestroyOnLoad(this.gameObject);

                if (start_shouldMove)
                {
                    if (start_shouldGoRight) playerController.RunTo(true);
                    else playerController.RunTo(false);
                }

                ScreenTransition.ScreenTransitionGoto(targetSceneName, loadingSceneName, CurtainColor, WaitTime1, FadeOutTime, LoadingTime, FadeInTime, WaitTime2);
            }
            else Debug.LogError($"(이동 전) {compareTag} 를 태그로 하는 오브젝트에 PlayerController 없음");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetSceneName)
        {
            StartCoroutine(EndMove());
        }
    }

    private IEnumerator EndMove()
    {
        GameObject playerObject = GameObject.FindGameObjectWithTag(compareTag);
        if (playerObject != null)
        {
            playerController = playerObject.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.transform.position = playerPosition;

                if (end_shouldMove)
                {
                    if (waitMoveDuration > 0)
                    {
                        playerController.DisableControl(true, playerPosition.x);

                        float elapsedTime = 0f;
                        while (elapsedTime < waitMoveDuration)
                        {
                            yield return null;
                            playerController.transform.position = playerPosition;
                            elapsedTime += Time.deltaTime;
                        }
                    }

                    if (end_shouldGoRight) playerController.RunTo(true);
                    else playerController.RunTo(false);

                    yield return new WaitForSeconds(moveDuration);

                    playerController.EnableContorl();
                }
            }
            else Debug.LogError($"(이동 후) {compareTag} 를 태그로 하는 오브젝트에 PlayerController 없음");
        }
        else
        {
            // 플레이어가 없으면 컷신이라 판단하겠음.
        }

        Destroy(this.gameObject);
    }
}
