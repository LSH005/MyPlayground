using UnityEngine;

public class TB_CameraRotationTracking : MonoBehaviour
{
    [Header("�±�")]
    public string compareTag = "Player";

    [Header("������ �±� / ������")]
    public string trackTargetTag = "Player";
    public Vector3 offset;

    [Header("�ΰ� ����")]
    [Tooltip("Scene�� ������ ��, ������ �±׸� ���� ������Ʈ�� Ž���ϴ� ���� �����Ѵٸ�\n�۵��� �� �ѹ� �� Ž���Ѵ�.\n��Ž�� ���Ŀ��� ������Ʈ�� ���ٸ� �۵� �����Ѵ�.")]
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
