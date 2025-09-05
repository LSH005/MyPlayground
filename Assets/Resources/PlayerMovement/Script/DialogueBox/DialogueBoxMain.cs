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
            Debug.LogError("�Ҵ�� SO�� ����");
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
            Debug.LogError("�Ҵ�� SO�� ���ڿ� �迭 ����");
        }
    }
}