using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadingSceneManager : MonoBehaviour
{
    public void StartLoadAndTransition(string SceneName, float LoadingTime, float FadeInTime, float WaitTime2, SpriteRenderer curtainRenderer, GameObject curtainInstance)
    {
        DontDestroyOnLoad(gameObject);
        StartCoroutine(LoadAndTransition(SceneName, LoadingTime, FadeInTime, WaitTime2, curtainRenderer, curtainInstance));
    }

    private IEnumerator LoadAndTransition(string SceneName, float LoadingTime, float FadeInTime, float WaitTime2, SpriteRenderer curtainRenderer, GameObject curtainInstance)
    {
        // �� �ε� ��� ����
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

        // ScreenTransition ��ũ��Ʈ�� FinishTransition �ڷ�ƾ ȣ��
        if (ScreenTransition.Instance != null)
        {
            ScreenTransition.Instance.StartCoroutine(ScreenTransition.Instance.FinishTransition(SceneName, FadeInTime, WaitTime2, curtainRenderer, curtainInstance));
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("ScreenTransition �ν��Ͻ� �� ã��");
        }
    }
}
