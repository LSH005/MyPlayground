using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ScreenTransition : MonoBehaviour
{
    public GameObject Curtain;

    public static ScreenTransition Instance;

    private bool isTransitioning = false;
    private GameObject[] uiObjects;

    private string SceneName;
    private string LoadingSceneName;
    private Color CurtainColor;
    private float WaitTime1;
    private float FadeOutTime;
    private float LoadingTime;
    private float FadeInTime;
    private float WaitTime2;
    private GameObject curtainInstance;
    private SpriteRenderer curtainRenderer;



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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ScreenTransitionGoto("ScreenTransition", "LoadingScreen_1", Color.black, 0f, 0f, 7f, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ScreenTransitionGoto("ScreenTransition", "LoadingScreen_2", new Color32(255, 214, 117, 255), 0f, 0.5f, 7f, 0.5f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ScreenTransitionGoto("ScreenTransition", "LoadingScreen_3", Color.gray, 0f, 0.5f, 7f, 0.5f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ScreenTransitionGoto("ScreenTransition", "LoadingScreen_4", Color.black, 0f, 0.5f, 7f, 0.5f, 0f);
        }
    }

    static public void ScreenTransitionGoto(string SceneName, string LoadingSceneName, Color CurtainColor, float WaitTime1, float FadeOutTime, float LoadingTime, float FadeInTime, float WaitTime2)
    {
        if (Instance == null)
        {
            Debug.LogError("ScreenTransition 인스턴스가 존재하지 않음");
            return;
        }

        if (Instance.isTransitioning)
        {
            return;
        }

        Instance.SceneName = SceneName;
        Instance.LoadingSceneName = LoadingSceneName;
        Instance.CurtainColor = CurtainColor;
        Instance.WaitTime1 = WaitTime1;
        Instance.FadeOutTime = FadeOutTime;
        Instance.LoadingTime = LoadingTime;
        Instance.FadeInTime = FadeInTime;
        Instance.WaitTime2 = WaitTime2;

        if (Instance.isTransitioning) return;
        Instance.StartTransitionInternal();
    }

    private void StartTransitionInternal()
    {
        //if (isTransitioning)
        //{
        //    return;
        //}

        if (Curtain == null)
        {
            Debug.LogError("Curtain프리팹 없음");
            return;
        }

        isTransitioning = true;
        StartCoroutine(StartTransition());
    }

    private IEnumerator StartTransition()
    {
        SetUI();
        SetUIObjectsActive(false);

        if (WaitTime1 > 0)
        {
            yield return new WaitForSeconds(WaitTime1);
        }

        curtainInstance = Instantiate(Curtain);
        DontDestroyOnLoad(curtainInstance);
        curtainRenderer = curtainInstance.GetComponent<SpriteRenderer>();

        if (curtainRenderer == null)
        {
            Debug.LogError("Curtain에 SpriteRenderer 컴포넌트 없음");
            Destroy(curtainInstance);
            isTransitioning = false;
            yield break;
        }

        Vector3 cameraPosition = Camera.main.transform.position;
        curtainInstance.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 0f);   // 위치
        curtainInstance.transform.localScale = new Vector3(999999f, 999999f, 1f);   // 크기 (그냥 엄청크게)
        curtainRenderer.color = CurtainColor;   // 색
        StartCoroutine(FadeCurtain(curtainRenderer, 0f, 0f));
        yield return StartCoroutine(FadeCurtain(curtainRenderer, 1f, FadeOutTime));

        if (LoadingTime <= 0f)
        {
            SceneManager.LoadScene(SceneName);
            yield return null;

            // 즉시 전환 마무리
            yield return StartCoroutine(FinishTransition());
        }
        else
        {
            // 로딩 씬으로
            SceneManager.LoadScene(LoadingSceneName);
            yield return null;

            StartCoroutine(FadeCurtain(curtainRenderer, 0f, 0f));
            StartCoroutine(Loading());
        }
    }

    private IEnumerator Loading()
    {
        //씬 로딩 즉시 시작
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        asyncLoad.allowSceneActivation = false; // 로딩 완료 후 즉시 활성화 방지

        // 최소 로딩 시간 확보
        yield return new WaitForSeconds(LoadingTime);

        // 실제 로딩 시간 확보 (최소 로딩 시간이 지나도 로드되지 않았을 경우)
        //while (!asyncLoad.isDone) yield return null;
        while (asyncLoad.progress < 0.9f) yield return null;

        // 로딩 완료 후 목표 씬으로 전환
        asyncLoad.allowSceneActivation = true;
        yield return null;
        StartCoroutine(FinishTransition());
    }

    private IEnumerator FinishTransition()
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

    private IEnumerator FadeCurtain(SpriteRenderer renderer, float targetAlpha, float duration)
    {
        if (duration > 0)
        {
            float startAlpha = renderer.color.a;
            float elapsedTime = 0f;

            while (elapsedTime < duration)
            {
                elapsedTime += Time.deltaTime;
                float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
                renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, newAlpha);
                yield return null;
            }
        }
        renderer.color = new Color(renderer.color.r, renderer.color.g, renderer.color.b, targetAlpha);
    }
}