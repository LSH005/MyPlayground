using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerBoxControl : MonoBehaviour
{
    [Header("Ʈ���Źڽ� ID")]
    [Tooltip("Ʈ���� ��ȣ�� ������ ������.\nfalse�� ��� ��ȣ�� ������ �ʰ� �ڽŸ� ��Ȱ��ȭ��.")]
    public bool isSignalSender = true;  
    [Tooltip("Ʈ���Źڽ� ID �� (uint)")]
    public uint triggerBoxID = 0;

    [Header("���� ����")]
    [Tooltip("Scene �ε� ���� ������ Ȱ��ȭ�Ǵ� �ð�")]
    public float activationDelay = 0;
    [Tooltip("������ ID���� ��Ȱ��ȭ ��ȣ�� ���� �ּ� �Ÿ�")]
    public float DeactivationDistance = 0;
    [Tooltip("��ȸ�������� ���� ����\ntrue�� �� �� �۵� ���� �� Ʈ���Źڽ��� ���ŵ�.")]
    public bool isDisposable = false;

    [Header("�±�")]
    public string compareTag = "Player";

    private float sqrDeactivationDistance;
    private BoxCollider2D boxCol;

    private void Awake()
    {
        boxCol = GetComponent<BoxCollider2D>();
        boxCol.isTrigger = true;
        sqrDeactivationDistance = DeactivationDistance * DeactivationDistance;

        if (activationDelay > 0)
        {
            boxCol.enabled = false;
            StartCoroutine(DelayActivation(activationDelay));
        }
        else
        {
            boxCol.enabled = true;
        }
    }

    private IEnumerator DelayActivation(float delay)
    {
        yield return new WaitForSeconds(delay);
        boxCol.enabled = true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(compareTag))
        {
            if (isSignalSender)
            {
                TriggerBoxControl[] allControls = FindObjectsOfType<TriggerBoxControl>();

                foreach (TriggerBoxControl control in allControls)
                {
                    if (this != control)
                    {
                        if (control.triggerBoxID == this.triggerBoxID)
                        {
                            Vector3 directionVector = this.transform.position - control.transform.position;

                            if (directionVector.sqrMagnitude > sqrDeactivationDistance)
                            {
                                control.Disable();
                            }

                        }
                        else
                        {
                            control.Enable();
                        }
                       
                    }
                }
            }

            Disable();

            if (isDisposable)
            {
                Destroy(gameObject);
            }
        }
    }

    public void Disable()
    {
        boxCol.enabled = false;
    }
    
    public void Enable()
    {
        boxCol.enabled = true;
    }
}
