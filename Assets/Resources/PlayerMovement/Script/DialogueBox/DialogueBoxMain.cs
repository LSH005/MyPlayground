using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(BoxCollider2D))]
public class DialogueBoxMain : MonoBehaviour
{
    [Header("대화 말풍선")]
    public GameObject keyMarker;
    public GameObject dialogueBubble;
    [Header("대화 정보 SO")]
    public Dialogue[] Dialogue;
    public int startDialogueIndex;  // 첫 대화의 Dialogue 배열 인덱스.
    public bool isMultiConversation = true;     // 여러 번 작동시켜 Dialogue 배열을 순차적으로 재생시킬지에 대한 여부.

    private bool isDialogueActive = false;
    private GameObject dialogueBubbleInstance;

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {

        }
    }
}
