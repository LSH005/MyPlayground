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
            Debug.LogError("ScreenTransition �ν��Ͻ��� �������� ����");
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
            Debug.LogError("Curtain������ ����");
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
            Debug.LogError("Curtain�� SpriteRenderer ������Ʈ ����");
            Destroy(curtainInstance);
            isTransitioning = false;
            yield break;
        }

        Vector3 cameraPosition = Camera.main.transform.position;
        curtainInstance.transform.position = new Vector3(cameraPosition.x, cameraPosition.y, 0f);   // ��ġ
        curtainInstance.transform.localScale = new Vector3(999999f, 999999f, 1f);   // ũ�� (�׳� ��ûũ��)
        curtainRenderer.color = CurtainColor;   // ��
        StartCoroutine(FadeCurtain(curtainRenderer, 0f, 0f));
        yield return StartCoroutine(FadeCurtain(curtainRenderer, 1f, FadeOutTime));

        if (LoadingTime <= 0f)
        {
            SceneManager.LoadScene(SceneName);
            yield return null;

            // ��� ��ȯ ������
            yield return StartCoroutine(FinishTransition());
        }
        else
        {
            // �ε� ������
            SceneManager.LoadScene(LoadingSceneName);
            yield return null;

            StartCoroutine(FadeCurtain(curtainRenderer, 0f, 0f));
            StartCoroutine(Loading());
        }
    }

    private IEnumerator Loading()
    {
        //�� �ε� ��� ����
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneName);
        asyncLoad.allowSceneActivation = false; // �ε� �Ϸ� �� ��� Ȱ��ȭ ����

        // �ּ� �ε� �ð� Ȯ��
        yield return new WaitForSeconds(LoadingTime);

        // ���� �ε� �ð� Ȯ�� (�ּ� �ε� �ð��� ������ �ε���� �ʾ��� ���)
        //while (!asyncLoad.isDone) yield return null;
        while (asyncLoad.progress < 0.9f) yield return null;

        // �ε� �Ϸ� �� ��ǥ ������ ��ȯ
        asyncLoad.allowSceneActivation = true;
        yield return null;
        StartCoroutine(FinishTransition());
    }

    private IEnumerator FinishTransition()
    {
        StartCoroutine(FadeCurtain(curtainRenderer, 1f, 0f));
        SetUI();
        SetUIObjectsActive(false);

        // Ŀư ���̵� �� (��������)
        yield return StartCoroutine(FadeCurtain(curtainRenderer, 0f, FadeInTime));

        // �� ��° ��� �ð�
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