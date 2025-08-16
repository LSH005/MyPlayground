//using System.Collections;
//using UnityEngine;
//using UnityEngine.SceneManagement;

//public class ScreenTransition : MonoBehaviour
//{
//    public GameObject Curtain;

//    public static ScreenTransition Instance;

//    private bool isTransitioning = false;
//    private GameObject[] uiObjects;

//    private void Awake()
//    {
//        if (Instance == null)
//        {
//            Instance = this;
//            DontDestroyOnLoad(gameObject);
//        }
//        else
//        {
//            Destroy(gameObject);
//        }
//    }

//    void Update()
//    {
//        if (Input.GetKeyDown(KeyCode.R))
//        {
//            ScreenTransitionGoto("ScreenTransition", 0.5f, 1f, 0.5f, 1f, 0.5f);
//        }
//    }


//    static void ScreenTransitionGoto(string SceneName, float WaitTime1, float FadeOutTime, float WaitTime2, float fadeInTime, float WaitTime3)
//    {
//        if (Instance == null)
//        {
//            Debug.LogError("ScreenTransition 인스턴스가 존재하지 않음.");
//            return;
//        }

//        if (Instance.isTransitioning) return;

//        Instance.StartCoroutine(Instance.TransitionCoroutine(SceneName, FadeOutTime, WaitTime, fadeInTime));
//    }

//    private IEnumerator TransitionCoroutine(string SceneName, float WaitTime1, float FadeOutTime, float WaitTime2, float fadeInTime, float WaitTime3)
//    {
//        isTransitioning = true;

//        SetUI();
//        SetUIObjectsActive(false);

//        GameObject curtainInstance = Instantiate(Curtain);
//        SpriteRenderer curtainRenderer = curtainInstance.GetComponent<SpriteRenderer>();
//        if (curtainRenderer == null)
//        {
//            Debug.LogError("Curtain에 SpriteRenderer 컴포넌트 없음");
//            Destroy(curtainInstance);
//            isTransitioning = false;
//            yield break;
//        }

//        Color curtainColor = curtainRenderer.color;
//        curtainColor.a = 0f;
//        curtainRenderer.color = curtainColor;

//        Camera mainCamera = Camera.main;
//        curtainInstance.transform.position = new Vector3(mainCamera.transform.position.x, mainCamera.transform.position.y, 0f);
//        curtainInstance.transform.localScale = Vector3.one * 1000f;

//        DontDestroyOnLoad(curtainInstance);

//        yield return StartCoroutine(FadeCurtain(curtainRenderer, 1f, fadeOutTime));

//        if (waitTime > 0f)
//        {
//            yield return new WaitForSeconds(waitTime);
//        }

//        SceneManager.LoadScene(sceneName);
//        yield return null;
//        SetUI();
//        SetUIObjectsActive(false);

//        yield return StartCoroutine(FadeCurtain(curtainRenderer, 0f, fadeInTime));

//        SetUIObjectsActive(true);
//        Destroy(curtainInstance);
//        isTransitioning = false;
//    }

//    private IEnumerator FadeCurtain(SpriteRenderer renderer, float targetAlpha, float duration)
//    {
//        float startAlpha = renderer.color.a;
//        float elapsedTime = 0f;

//        while (elapsedTime < duration)
//        {
//            elapsedTime += Time.deltaTime;
//            float newAlpha = Mathf.Lerp(startAlpha, targetAlpha, elapsedTime / duration);
//            Color newColor = renderer.color;
//            newColor.a = newAlpha;
//            renderer.color = newColor;
//            yield return null;
//        }

//        Color finalColor = renderer.color;
//        finalColor.a = targetAlpha;
//        renderer.color = finalColor;
//    }

//    private void SetUI()
//    {
//        uiObjects = GameObject.FindGameObjectsWithTag("UI");
//    }

//    private void SetUIObjectsActive(bool isActive)
//    {
//        foreach (GameObject ui in uiObjects)
//        {
//            ui.SetActive(isActive);
//        }
//    }
//}
