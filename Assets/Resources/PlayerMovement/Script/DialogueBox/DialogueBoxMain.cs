using System.Collections;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(BoxCollider2D))]
public class DialogueBoxMain : MonoBehaviour
{
    [Header("�÷��̾� �±�")]
    public string playerTag = "Player"; // �÷��̾� �±�
    public float playerHorizontalPosition = 0.0f;   // ��ȭ ���� �� �÷��̾ �̵��� ��ġ
    public bool facingRight = true; // �÷��̾ �������� �����ִ����� ���� ����. false�� ������ ��

    [Header("��ȭ ��ǳ��")]
    public GameObject keyMarker;    // �÷��̾ ���� ���ʿ� ������ ��� ���̴� ��ȣ�ۿ� ǥ�ÿ� ������Ʈ
    public GameObject dialogueBubble;   // ��ȭ ��ǳ�� ������. �� �����տ��� "DialogueBubbleReference" ��ũ��Ʈ�� �ʿ���.
    [Header("��ȭ ���� SO")]
    public Dialogue[] Dialogue; // SO �迭
    public int dialogueRepetition;  // Dialogue (SO �迭) �ε���. ��ȭ�� ������ 1 ������.
    public bool repeatDialogue = false;     //dialogueRepetition �� �������� �ʾ� �ϳ��� ��ȭ���� ������ �ݺ���ų���� ���� ����. true���� �۵�.
    [Header("�۵� �� ����")]
    public string functionNameToCall = "OnDialogueEnd";

    private bool isDialogueActive = false;
    private bool isDialogueEnd = false;
    private Dialogue currentDialogue;
    private DialogueBubbleReference bubbleScript;
    private AudioSource AudioSource;
    private PlayerController playerController;
    private MonoBehaviour[] onDialogueEndScripts;


    private void Awake()
    {
        keyMarker.SetActive(false);

        BoxCollider2D boxCol = GetComponent<BoxCollider2D>();
        boxCol.isTrigger = true;
        AudioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        MonoBehaviour[] allScripts = GetComponents<MonoBehaviour>();

        onDialogueEndScripts = allScripts.Where(
                script =>
                script != this && // ���� ��ũ��Ʈ ����
                script.GetType().GetMethod(functionNameToCall) != null // Ư�� �Լ��� �ִ��� Ȯ��
                ).ToArray();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isDialogueEnd) return;
        keyMarker.SetActive(false);
    }

    void OnTriggerStay2D(Collider2D other)
    {
        if (isDialogueEnd || isDialogueActive) return;

        if (other.tag == playerTag)
        {
            if (Input.GetKey(KeyCode.F))    // ��ȣ�ۿ� ����
            {
                playerController = other.GetComponent<PlayerController>();
                if (playerController != null)
                {
                    playerController.DisableControl(facingRight, playerHorizontalPosition);
                    keyMarker.SetActive(false);
                    StartDialogue();
                }
                else Debug.LogError($"{other.name} �� playerController ����");
            }
            
        }
        if (!isDialogueActive)
        {
            keyMarker.SetActive(true);
        }
    }

    // SO �迭 Ȯ�� �� ���̾�α� ����
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
            if (dialogueRepetition >= 0 && dialogueRepetition < Dialogue.Length)
            {
                if (Dialogue[dialogueRepetition] != null)
                {
                    isDialogueActive = true;
                    currentDialogue = Dialogue[dialogueRepetition];
                    StartCoroutine(TypeSentenceCoroutine());
                }
                else Debug.LogError("ù �ε����� ������ ����ų�, ���빰�� ��� ����.");
            }
            else Debug.LogError("�迭�� ��ȿ�� ������ ���.");
        }
        else Debug.LogError("�Ҵ�� SO�� ����");
    }

    // ���õ� SO ��ü�� ����ϴ� �ڷ�ƾ
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
        EndDialogue();
    }

    // ��� ���� ������ �ż���
    private void EndDialogue()
    {
        isDialogueActive = false;
        StopCoroutine(TypeSentenceCoroutine());
        Destroy(bubbleScript.gameObject);

        if (!repeatDialogue)
        {
            dialogueRepetition++;

            if (dialogueRepetition >= Dialogue.Length)
            {
                isDialogueEnd = true;
                Destroy(keyMarker);
            }
        }

        if (onDialogueEndScripts.Length > 0)
        {
            foreach (MonoBehaviour script in onDialogueEndScripts)
            {
                script.GetType().GetMethod(functionNameToCall).Invoke(script, new object[] { dialogueRepetition });
            }
        }
    }
}