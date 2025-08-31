using UnityEngine;

public class TB_CameraRotationTracking : MonoBehaviour
{
    [Header("태그")]
    public string compareTag = "Player";

    [Header("추적할 태그 / 오프셋")]
    public string trackTargetTag = "Player";
    public Vector3 offset;

    [Header("부가 설정")]
    [Tooltip("Scene이 시작할 때, 추적할 태그를 가진 오브젝트를 탐색하는 데에 실패한다면\n작동할 때 한번 더 탐색한다.\n재탐색 이후에도 오브젝트가 없다면 작동 중지한다.")]
    public bool shouldRetryFinding = true;

    private GameObject trackTargetObj;

    private void Start()
    {
        trackTargetObj = GameObject.FindWithTag(trackTargetTag);

        if (trackTargetObj == null && !shouldRetryFinding)
        {
            this.enabled = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag(compareTag))
        {
            if (trackTargetObj == null)
            {
                trackTargetObj = GameObject.FindWithTag(trackTargetTag);

                if (trackTargetObj == null)
                {
                    this.enabled = false;
                    return;
                }
            }

            CameraMovement.RotationTracking(trackTargetObj.transform, offset);
        }
    }
}
