using Unity.VisualScripting;
using UnityEngine;

public class DB_PlayerMove : MonoBehaviour
{
    [Header("���� ���� Ȱ��ȭ")]
    public bool useExclude = false;
    [Header("��ȭ �ݺ� Ƚ�� ���� ����")]
    public int minExcludeCount;
    public int maxExcludeCount;
    [Header("�÷��̾� �±�")]
    public string playerTag = "Player";

    public void OnDialogueEnd(int dialogueRepetition)
    {
        if (useExclude)
        {
            if (minExcludeCount <= dialogueRepetition && dialogueRepetition <= maxExcludeCount)
            {
                return;
            }
        }

        GameObject playerObject = GameObject.FindWithTag(playerTag);

        if (playerObject != null)
        {
            PlayerController playerController = playerObject.GetComponent<PlayerController>();

            if (playerController != null)
            {
                playerController.EnableContorl();
            }
            else Debug.LogError("Player ������Ʈ�� PlayerController ��ũ��Ʈ�� �����ϴ�.");
        }
        else Debug.LogError("������ 'Player' �±׸� ���� ������Ʈ�� ã�� �� �����ϴ�.");
    }
}
