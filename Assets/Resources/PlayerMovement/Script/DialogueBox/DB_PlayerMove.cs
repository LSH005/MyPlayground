using Unity.VisualScripting;
using UnityEngine;

public class DB_PlayerMove : MonoBehaviour
{
    [Header("제외 범위 활성화")]
    public bool useExclude = false;
    [Header("대화 반복 횟수 제외 범위")]
    public int minExcludeCount;
    public int maxExcludeCount;
    [Header("플레이어 태그")]
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
            else Debug.LogError("Player 오브젝트에 PlayerController 스크립트가 없습니다.");
        }
        else Debug.LogError("씬에서 'Player' 태그를 가진 오브젝트를 찾을 수 없습니다.");
    }
}
