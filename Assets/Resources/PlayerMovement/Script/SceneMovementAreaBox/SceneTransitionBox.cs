using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(BoxCollider2D))]
public class SceneTransitionBox : MonoBehaviour
{
    [Header("���� ����")]
    public float activationDelay = 0;   // Scene �ε� ���� ������ Ȱ��ȭ�Ǵ� �ð�
    public string targetSceneName;  // �̵��� Scene �̸�
    public string loadingSceneName; // �ε� Scene �̸�
    public Color CurtainColor = Color.black;    // ���̵� ��/�ƿ��� ����� Ŀư ������Ʈ ����
    public float WaitTime1; // ���̵� �ƿ� ���� ��� �ð�
    public float FadeOutTime = 0.5f;    // ���̵� �ƿ� ���ӽð�
    public float LoadingTime = 5f;  // ������ �ּ� �ε� �ð�
    public float FadeInTime = 0.5f; // ���̵� �� ���ӽð�
    public float WaitTime2; // ���̵� �� ���� ���ð�

    [Header("�÷��̾� �±�")]
    public string compareTag = "Player";    // ������ �÷��̾� �±�.
    [Header("���� �������� �÷��̾� ������")]
    public bool start_shouldMove = false;   // �ε� ���� �������� �������� �ϴ����� ���� ����. true���� ������.
    public bool start_shouldGoRight = false;    // start_shouldMove �� true�� ���, ���� �������� ������ ���� ����. true�� ���������� �޸� (������ ����)
    [Header("��ǥ �������� �÷��̾� ������")]
    public Vector3 playerPosition;  // ��ǥ �������� �÷��̾ ������ ��ġ
    public bool end_shouldMove = false;     // ��ǥ �������� �������� �ϴ����� ���� ����. true���� ������.
    public bool end_shouldGoRight = false;  // end_shouldMove �� true�� ���, ���� �������� ������ ���� ����. true�� ���������� �޸� (������ ����)
    public float waitMoveDuration;  // ��ǥ �������� �÷��̾ playerPosition�� ������ �ð�.
    public float moveDuration;  // ��ǥ �������� �󸶳� ���� ���������� ���� �ð�

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
            else Debug.LogError($"(�̵� ��) {compareTag} �� �±׷� �ϴ� ������Ʈ�� PlayerController ����");
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
            else Debug.LogError($"(�̵� ��) {compareTag} �� �±׷� �ϴ� ������Ʈ�� PlayerController ����");
        }
        else
        {
            // �÷��̾ ������ �ƽ��̶� �Ǵ��ϰ���.
        }

        Destroy(this.gameObject);
    }
}
