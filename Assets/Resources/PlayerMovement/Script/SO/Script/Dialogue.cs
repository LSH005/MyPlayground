using UnityEngine;

[System.Serializable]
public class DialogueLine
{
    [Header("��ǳ�� ��ġ")]
    public Vector3 bubblePosition;  // ��ǳ���� ��ġ. ���� (0,0,0) �̶�� �������� ����.
    public float bubbleBodyOffset;  // ��ǳ�� ������ ��-�� ������
    public bool isTailDown;   // ��ǳ�� ����ǥ�� ���ʿ� �������� ���� ����

    [TextArea(1, 10)]   // �ν����� â���� ǥ�õǴ� �Է¶� �� �ּ�, �ִ� ũ��
    public string[] sentence;    // ��� ���� �迭 (�Էµ��� ������ ���ڿ��� �ѱ�ų�, ���� �������� �Ѿ)

    public float intervalTime; // ���� �ϳ��� ��� ���� �ð�
    public bool canSkip; // �÷��̾ �� ��縦 ������ ������ Ű�� �ѱ� �� �ִ����� ���� ����. true�� �ѱ� �� ����.
    public bool autoSkip; // �ڵ� ��ŵ�� ����. true�� ���ڿ� �ϳ��� ���� ����Ǿ��� ��, autoSkipIntervalTime �� ��� �� �ڵ����� ���� ���� �Ѿ
    public float autoSkipIntervalTime; // �ڵ� ��ŵ���� ����� �ð�
    public AudioClip typingSound;   // ���� �ϳ��� ���� ������ ����� �Ҹ� (�Էµ��� ������ ������� ����)
}

[CreateAssetMenu(fileName = "Totally Awesome Dialogue", menuName = "Dialogue/New Dialogue")]
public class Dialogue : ScriptableObject
{
    public DialogueLine[] section;
}
