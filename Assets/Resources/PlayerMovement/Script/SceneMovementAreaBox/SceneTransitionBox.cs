using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTransitionBox : MonoBehaviour
{
    [Header("동작 설정")]
    public float activationDelay = 0;
    public string targetSceneName;
    public string loadingSceneName;
    public Color CurtainColor = Color.black;
    public float WaitTime1;
    public float FadeOutTime = 0.5f;
    public float LoadingTime = 5f;
    public float FadeInTime = 0.5f;
    public float WaitTime2;

    [Header("플레이어 태그")]
    public string compareTag = "Player";
    [Header("시작 지점에서 플레이어 움직임")]
    public bool start_shouldMove = false;
    public bool start_shouldGoRight = false;
    [Header("목표 지점에서 플레이어 움직임")]
    public Vector3 playerPosition;
    public bool end_shouldMove = false;
    public bool end_shouldGoRight = false;
    public float waitMoveDuration;
    public float moveDuration;

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
