using UnityEngine;

[RequireComponent(typeof(AudioSource), typeof(BoxCollider2D))]
public class DialogueBoxMain : MonoBehaviour
{
    [Header("��ȭ ��ǳ��")]
    public GameObject keyMarker;
    public GameObject dialogueBubble;
    [Header("��ȭ ���� SO")]
    public Dialogue[] Dialogue;
    public int startDialogueIndex;  // ù ��ȭ�� Dialogue �迭 �ε���.
    public bool isMultiConversation = true;     // ���� �� �۵����� Dialogue �迭�� ���������� �����ų���� ���� ����.

    private bool isDialogueActive = false;
    private GameObject dialogueBubbleInstance;

    void OnTriggerStay2D(Collider2D other)
    {
        if (Input.GetKeyDown(KeyCode.W))
        {

        }
    }
}
