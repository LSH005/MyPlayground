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
    private DialogueBubbleReference bubbleScript;
    private AudioSource AudioSource;


    private void Awake()
    {
        keyMarker.SetActive(false);

        BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
        boxCol.isTrigger = true;
        AudioSource = GetComponent<AudioSource>();
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
        if (dialogueBubble != null)
        {
            GameObject newBubble = Instantiate(dialogueBubble);
            bubbleScript = newBubble.GetComponent<DialogueBubbleReference>();

            if (bubbleScript == null)
            {
                Debug.LogError("할당된 프리팹에 DialogueBubbleReference 스크립트 없음.");
                return;
            }
        }
        else Debug.LogError("말풍선 프리팹 할당되지 않음.");

        if (Dialogue != null)
        {
            if (DialogueIndex >= 0 && DialogueIndex < Dialogue.Length)
            {
                if (Dialogue[DialogueIndex] != null)
                {
                    isDialogueActive = true;
                    currentDialogue = Dialogue[DialogueIndex];
                    StartCoroutine(TypeSentenceCoroutine());
                }
                else Debug.LogError("첫 인덱스가 범위를 벗어났거나, 내용물이 비어 있음.");
            }
            else Debug.LogError("배열의 유효한 범위를 벗어남.");
        }
        else Debug.LogError("할당된 SO가 없음");
    }

    public IEnumerator TypeSentenceCoroutine()
    {
        for (int i = 0; i < currentDialogue.section.Length; i++)  // 섹션 전체 반복
        {
            DialogueLine dialogueSection = currentDialogue.section[i];

            bubbleScript.transform.position = dialogueSection.bubblePosition;
            bubbleScript.SetBubbleOffset(dialogueSection.bubbleBodyOffset);
            bubbleScript.SetTailToLower(dialogueSection.isTailDown);
            bubbleScript.SetText("");

            if (dialogueSection.sentence != null)   // 섹션 문자열 배열 유효성 검사 1
            {
                if (dialogueSection.sentence.Length > 0)    // 섹션 문자열 배열 유효성 검사 2
                {
                    for (int j = 0; j < dialogueSection.sentence.Length; j++)   // 섹션 내부 문자열 반복
                    {
                        if (!string.IsNullOrEmpty(dialogueSection.sentence[j])) // 섹션 문자열 배열 유효성 검사 3
                        {
                            string fullDialogueText = dialogueSection.sentence[j];
                            string DialogueText = "";
                            bool hasSound = dialogueSection.typingSound != null;

                            float timer = 0f;
                            int charIndex = 0;

                            // F키를 누르면 다음 문장으로 넘어가는 기능
                            while (charIndex < fullDialogueText.Length)
                            {
                                yield return null;
                                if (Input.GetKeyDown(KeyCode.F))
                                {
                                    bubbleScript.SetText(fullDialogueText);
                                    break;
                                }

                                if (timer >= dialogueSection.intervalTime)
                                {
                                    DialogueText += fullDialogueText[charIndex];
                                    bubbleScript.SetText(DialogueText);
                                    if (hasSound) AudioSource.PlayOneShot(dialogueSection.typingSound);
                                    charIndex++;
                                    timer = 0f;
                                }

                                timer += Time.deltaTime;
                            }
                            yield return null;
                            yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.F));
                        }
                        else Debug.LogError("문자열이 비었음");
                    }
                }
                else Debug.LogError("배열이 비었음");
            }
            else Debug.LogError("할당된 SO에 배열 자체가 없음");
        }
    }
}