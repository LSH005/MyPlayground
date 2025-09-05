using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(BoxCollider2D))]
public class DialogueBoxMain : MonoBehaviour
{
    [Header("�÷��̾� �±�")]
    public string playerTag = "Player";
    public Vector2 playerPosition = Vector2.zero;
    public bool facingRight = true;

    [Header("��ȭ ��ǳ��")]
    public GameObject keyMarker;
    public GameObject dialogueBubble;
    [Header("��ȭ ���� SO")]
    public Dialogue[] Dialogue;
    public int DialogueIndex;  // ù ��ȭ�� Dialogue �迭 �ε���.
    public bool isMultiConversation = true;     // ���� �� �۵����� Dialogue �迭�� ���������� �����ų���� ���� ����.

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
                Debug.LogError("�Ҵ�� �����տ� DialogueBubbleReference ��ũ��Ʈ ����.");
                return;
            }
        }
        else Debug.LogError("��ǳ�� ������ �Ҵ���� ����.");

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
                else Debug.LogError("ù �ε����� ������ ����ų�, ���빰�� ��� ����.");
            }
            else Debug.LogError("�迭�� ��ȿ�� ������ ���.");
        }
        else Debug.LogError("�Ҵ�� SO�� ����");
    }

    public IEnumerator TypeSentenceCoroutine()
    {
        for (int i = 0; i < currentDialogue.section.Length; i++)  // ���� ��ü �ݺ�
        {
            DialogueLine dialogueSection = currentDialogue.section[i];

            bubbleScript.transform.position = dialogueSection.bubblePosition;
            bubbleScript.SetBubbleOffset(dialogueSection.bubbleBodyOffset);
            bubbleScript.SetTailToLower(dialogueSection.isTailDown);
            bubbleScript.SetText("");

            if (dialogueSection.sentence != null)   // ���� ���ڿ� �迭 ��ȿ�� �˻� 1
            {
                if (dialogueSection.sentence.Length > 0)    // ���� ���ڿ� �迭 ��ȿ�� �˻� 2
                {
                    for (int j = 0; j < dialogueSection.sentence.Length; j++)   // ���� ���� ���ڿ� �ݺ�
                    {
                        if (!string.IsNullOrEmpty(dialogueSection.sentence[j])) // ���� ���ڿ� �迭 ��ȿ�� �˻� 3
                        {
                            string fullDialogueText = dialogueSection.sentence[j];
                            string DialogueText = "";
                            bool hasSound = dialogueSection.typingSound != null;

                            float timer = 0f;
                            int charIndex = 0;

                            // FŰ�� ������ ���� �������� �Ѿ�� ���
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
                        else Debug.LogError("���ڿ��� �����");
                    }
                }
                else Debug.LogError("�迭�� �����");
            }
            else Debug.LogError("�Ҵ�� SO�� �迭 ��ü�� ����");
        }
    }
}