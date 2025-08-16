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
        // 씬 로딩 즉시 시작
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

        // ScreenTransition 스크립트의 FinishTransition 코루틴 호출
        if (ScreenTransition.Instance != null)
        {
            ScreenTransition.Instance.StartCoroutine(ScreenTransition.Instance.FinishTransition(SceneName, FadeInTime, WaitTime2, curtainRenderer, curtainInstance));
            Destroy(gameObject);
        }
        else
        {
            Debug.LogError("ScreenTransition 인스턴스 못 찾음");
        }
    }
}
