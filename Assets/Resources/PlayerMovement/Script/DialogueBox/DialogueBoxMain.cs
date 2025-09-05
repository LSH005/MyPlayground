using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(BoxCollider2D))]
public class DialogueBoxMain : MonoBehaviour
{
    [Header("플레이어 태그")]
    public string playerTag = "Player";
    public Vector2 playerPosition = Vector2.zero;
    public bool facingRight = true;

    [Header("대화 말풍선")]
    public GameObject keyMarker;
    public GameObject dialogueBubble;
    [Header("대화 정보 SO")]
    public Dialogue[] Dialogue;
    public int DialogueIndex;  // 첫 대화의 Dialogue 배열 인덱스.
    public bool isMultiConversation = true;     // 여러 번 작동시켜 Dialogue 배열을 순차적으로 재생시킬지에 대한 여부.

    private bool isDialogueActive = false;
    private bool isTyping = false;
    private GameObject dialogueBubbleInstance;
    private Dialogue currentDialogue;


    private void Awake()
    {
        keyMarker.SetActive(false);

        BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
        boxCol.isTrigger = true;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        keyMarker.SetActive(true);
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        keyMarker.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (other.tag == playerTag)
        {
            if (!isDialogueActive)
            {
                if (Input.GetKeyDown(KeyCode.F))
                {
                    other.GetComponent<PlayerController>().DisableControl(facingRight, playerPosition);
                    keyMarker.SetActive(false);
                    StartDialogue();
                }
            }
        }
    }

    private void StartDialogue()
    {
        if (Dialogue != null)
        {
            isDialogueActive = true;
            currentDialogue = Dialogue[DialogueIndex];
            StartCoroutine(TypeSentenceCoroutine());
        }
        else
        {
            Debug.LogError("할당된 SO가 없음");
        }
    }

    public IEnumerator TypeSentenceCoroutine()
    {
        int sectionIndex = 0;
        DialogueLine dialogueSection = currentDialogue.lines[sectionIndex];
        
        yield return null;

        if (dialogueSection.sentence != null)
        {

        }
        else
        {
            Debug.LogError("할당된 SO에 문자열 배열 없음");
        }
    }
}