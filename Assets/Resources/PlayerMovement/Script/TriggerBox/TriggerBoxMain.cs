using UnityEngine;
using System.Collections;

[RequireComponent(typeof(BoxCollider2D))]
public class TriggerBoxMain : MonoBehaviour
{
    [Header("트리거박스 ID")]
    public bool isSignalSender = true;  //트리거 신호를 보낼지 결정. false일 경우 신호를 보내지 않고 자신만 비활성화함. 
    public uint triggerBoxID = 0;   // 트리거박스 ID

    [Header("동작 설정")]
    public float activationDelay = 0;   // Scene 로드 이후 동작이 활성화되는 시간
    public float DeactivationDistance = 0;  // 동일한 ID에서 비활성화 신호를 보낼 최소 거리
    public bool isDisposable = false;   // 일회용인지에 대한 여부 true면 한 번 작동 이후 이 트리거박스는 제거됨.

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
