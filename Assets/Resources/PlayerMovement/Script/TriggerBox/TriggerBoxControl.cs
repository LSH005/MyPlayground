using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerBoxControl : MonoBehaviour
{
    [Header("트리거박스 ID")]
    [Tooltip("트리거 신호를 보낼지 결정함.\nfalse일 경우 신호를 보내지 않고 자신만 비활성화함.")]
    public bool isSignalSender = true;  
    [Tooltip("트리거박스 ID 값 (uint)")]
    public uint triggerBoxID = 0;

    [Header("동작 설정")]
    [Tooltip("Scene 로드 이후 동작이 활성화되는 시간")]
    public float activationDelay = 0;
    [Tooltip("동일한 ID에서 비활성화 신호를 보낼 최소 거리")]
    public float DeactivationDistance = 0;
    [Tooltip("일회용인지에 대한 여부\ntrue면 한 번 작동 이후 이 트리거박스는 제거됨.")]
    public bool isDisposable = false;

    [Header("태그")]
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
