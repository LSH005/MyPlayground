using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    public GameObject Curtain;

    public static ScreenTransition Instance;

    private bool isTransitioning = false;
    private GameObject[] uiObjects;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            ScreenTransitionGoto("ScreenTransition", "LoadingScreen_1", 1f, 0.5f, 5f, 0.5f, 1f);
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            ScreenTransitionGoto("ScreenTransition", "LoadingScreen_2", 0f, 0.5f, 5f, 0.5f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            ScreenTransitionGoto("ScreenTransition", "LoadingScreen_1", 0f, 0.5f, 0f, 0.5f, 0f);
        }
    }

    static public void ScreenTransitionGoto(string SceneName, string LoadingSceneName, float WaitTime1, float FadeOutTime, float LoadingTime, float FadeInTime, float WaitTime2)
    {
        if (Instance == null)
        {
            Debug.LogError("ScreenTransition 인스턴스가 존재하지 않음");
            return;
        }

        if (Instance.isTransitioning) return;
        Instance.StartTransitionInternal(SceneName, LoadingSceneName, WaitTime1, FadeOutTime, LoadingTime, FadeInTime, WaitTime2);
    }

    private void StartTransitionInternal(string SceneName, string LoadingSceneName, float WaitTime1, float FadeOutTime, float LoadingTime, float FadeInTime, float WaitTime2)
    {
        StartCoroutine(Transition(SceneName, LoadingSceneName, WaitTime1, FadeOutTime, LoadingTime, FadeInTime, WaitTime2));
    }

    private IEnumerator Transition(string SceneName, string LoadingSceneName, float WaitTime1, float FadeOutTime, float LoadingTime, float FadeInTime, float WaitTime2)
    {
        isTransitioning = true;
        SetUI();
        SetUIObjectsActive(false);

        if (WaitTime1 > 0)
        {
            yield return new WaitForSeconds(WaitTime1);
        }

        GameObject curtainInstance = Instantiate(Curtain);
        DontDestroyOnLoad(curtainInstance);
        SpriteRenderer curtainRenderer = curtainInstance.GetComponent<SpriteRenderer>();

        if (curtainRenderer == null)
        {
            Debug.LogError("Curtain에 SpriteRenderer 컴포넌트 없음");
            Destroy(curtainInstance);
            isTransitioning = false;
            yield break;
        }

        Vector3 cameraPosition = Camera.main.transform.position;
        curtainInstance.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 0f);
        curtainInstance.transform.localScale = new Vector3(5000f, 5000f, 1f);

        yield return StartCoroutine(FadeCurtain(curtainRenderer, 1f, FadeOutTime));

        if (LoadingTime <= 0f)
        {
            SceneManager.LoadScene(SceneName);
            yield return null;

            // 즉시 전환 마무리
            yield return StartCoroutine(FinishTransition(SceneName, FadeInTime, WaitTime2, curtainRenderer, curtainInstance));
        }
        else
        {
            // 로딩 씬으로 즉시 이동
            SceneManager.LoadScene(LoadingSceneName);
            yield return null;

            StartCoroutine(FadeCurtain(curtainRenderer, 0f, 0f));

            // LoadingSceneManager에 정보 전달
            LoadingSceneManager loadingManager = FindObjectOfType<LoadingSceneManager>();
            if (loadingManager != null)
            {
                loadingManager.StartLoadAndTransition(SceneName, LoadingTime, FadeInTime, WaitTime2, curtainRenderer, curtainInstance);
            }
            else
            {
                Debug.LogError("LoadingSceneManager 스크립트를 찾을 수 없습니다. LoadingScene에 해당 스크립트를 추가했는지 확인하세요.");
            }
        }
    }

    private IEnumerator FadeCurtain(SpriteRenderer renderer, float targetAlpha, float duration)
    {
        if (renderer == null || duration <= 0)
        {
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, targetAlpha);
            yield break;
        }

        float startAlpha = renderer.color.a;
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
            renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, newAlpha);
            yield return null;
        }

        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, targetAlpha);
    }

    public IEnumerator FinishTransition(string SceneName, float FadeInTime, float WaitTime2, SpriteRenderer curtainRenderer, GameObject curtainInstance)
    {
        StartCoroutine(FadeCurtain(curtainRenderer, 1f, 0f));
        SetUI();
        SetUIObjectsActive(false);

        // 커튼 페이드 인 (투명해짐)
        yield return StartCoroutine(FadeCurtain(curtainRenderer, 0f, FadeInTime));

        // 두 번째 대기 시간
        if (WaitTime2 > 0)
        {
            yield return new WaitForSeconds(WaitTime2);
        }

        SetUIObjectsActive(true);
        Destroy(curtainInstance);
        isTransitioning = false;
    }

    private void SetUI()
    {
        uiObjects = GameObject.FindGameObjectsWithTag("UI");
    }

    private void SetUIObjectsActive(bool isActive)
    {
        foreach (GameObject ui in uiObjects)
        {
            if (ui != null)
            {
                ui.SetActive(isActive);
            }
        }
    }
}