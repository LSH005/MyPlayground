using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerBoxMain : MonoBehaviour
{
    [Header("Ʈ���Źڽ� ID")]
    public bool isSignalSender = true;  //Ʈ���� ��ȣ�� ������ ����. false�� ��� ��ȣ�� ������ �ʰ� �ڽŸ� ��Ȱ��ȭ��. 
    public uint triggerBoxID = 0;   // Ʈ���Źڽ� ID

    [Header("���� ����")]
    public float activationDelay = 0;   // Scene �ε� ���� ������ Ȱ��ȭ�Ǵ� �ð�
    public float DeactivationDistance = 0;  // ������ ID���� ��Ȱ��ȭ ��ȣ�� ���� �ּ� �Ÿ�
    public bool isDisposable = false;   // ��ȸ�������� ���� ���� true�� �� �� �۵� ���� �� Ʈ���Źڽ��� ���ŵ�.

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
                TriggerBoxMain[] allControls = FindObjectsOfType<TriggerBoxMain>();

                foreach (TriggerBoxMain control in allControls)
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
