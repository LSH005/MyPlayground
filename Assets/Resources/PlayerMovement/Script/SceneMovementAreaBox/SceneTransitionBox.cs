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

                playerController.DisableControl(false, playerController.transform.position.x);

                if (start_shouldMove)
                {
                    if (start_shouldGoRight) playerController.RunTo(true);
                    else playerController.RunTo(false);
                }

                ScreenTransition.ScreenTransitionGoto(targetSceneName, loadingSceneName, CurtainColor, WaitTime1, FadeOutTime, LoadingTime, FadeInTime, WaitTime2);
            }
            else Debug.LogError($"{compareTag} 를 태그로 하는 오브젝트에 PlayerController 없음");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == targetSceneName)
        {
            
        }
    }
}
