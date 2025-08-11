using System.Collections.Generic;
using UnityEngine;

public class TrackerMovement : MonoBehaviour
{
    [Header("����")]
    public float moveSpeed = 5f;    // �𸣸� �ʵ� �ٽ� ���� �;� ��, �׸��� �ڵ� ������.
    [Header("��� ���� �ߴ� ����")]
    public bool skipNodeTracking = false;   // ȿ������ �̵��� ���� ���� ��� ������ �ߴ����� ���� (���� ��� �ּ����� "������" �̶� Ī��.)
    public float skipNodeTrackingPeriod = 0f;    // �������� ��� �ֱ�
    public float skipNodeTrackingRange = 0.75f;    // �������� �ϱ� ���� ���� ���κ��� �־����� �ϴ� �ּ� �Ÿ�

    private int trackingNodeIndex;
    private float pastTime = 0;
    private bool canMove = false;
    private List<NodeMovement> allRoadNodes = new List<NodeMovement>();
    private NodeMovement[] allNodes;
    private Vector2 targetPos;
    private Vector2 previousPos;
    private Vector2 direction;

    private void Update()
    {
        if (canMove)
        {
            if (Vector2.Distance(transform.position, targetPos) < moveSpeed / 100f)
            {
                transform.position = allRoadNodes[trackingNodeIndex].transform.position;
                UpdateNextTarget();
            }

            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;

            if (Vector2.Distance(transform.position, targetPos) < Vector2.Distance(previousPos, targetPos))
            {
                previousPos = transform.position;
            }
            else
            {
                Debug.LogError("��ġ �ս� �߻�");
                UpdateDirection();
            }

            if (skipNodeTracking && trackingNodeIndex + 1 < allRoadNodes.Count && Vector2.Distance(transform.position, targetPos) > skipNodeTrackingRange)
            {
                if (skipNodeTrackingPeriod <= 0)
                {
                    SkipNodeTracking();
                }
                else
                {
                    pastTime += Time.deltaTime;
                    if (skipNodeTrackingPeriod <= pastTime)
                    {
                        pastTime = 0;
                        SkipNodeTracking();
                    }
                }
            }
        }
    }

    private void UpdateNextTarget()
    {
        trackingNodeIndex++;

        if (trackingNodeIndex >= allRoadNodes.Count)
        {
            canMove = false;
            Debug.Log("���� ����");
            return;
        }

        previousPos = transform.position;
        targetPos = allRoadNodes[trackingNodeIndex].transform.position;
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        direction = (targetPos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        previousPos = transform.position;
    }

    public void TrackingNode()
    {
        allRoadNodes.Clear();
        allNodes = FindObjectsOfType<NodeMovement>();
        foreach (NodeMovement node in allNodes)
        {
            if (node.isRoad)
            {
                allRoadNodes.Add(node);
            }
        }
        allRoadNodes.Sort((a, b) => a.id.CompareTo(b.id));

        trackingNodeIndex = 0;
        targetPos = allRoadNodes[trackingNodeIndex].transform.position;

        direction = (targetPos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
        previousPos = transform.position;

        canMove = true;

        Debug.Log("���� ����");
        /*
        while (allRoadNodes.Count - 1 >= trackingNodeIndex)
        {
            targetPos = allRoadNodes[trackingNodeIndex].transform.position;

            direction = (targetPos - (Vector2)transform.position).normalized;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, angle);

            while (Vector2.Distance(transform.position, targetPos) > 0.1f)
            {
                transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
            }
            transform.position = targetPos;
            trackingNodeIndex++;
        }
        */

    }
    private void SkipNodeTracking()
    {
        Vector2 rayOrigin = transform.position;
        Vector2 rayTarget = allRoadNodes[trackingNodeIndex + 1].transform.position;
        Vector2 rayDirection = (rayTarget - rayOrigin).normalized;
        float rayDistance = Vector2.Distance(rayOrigin, rayTarget);

        RaycastHit2D[] allHits = Physics2D.CircleCastAll(rayOrigin, 0.25f, rayDirection, rayDistance);

        bool isObstaclesFound = false;
        foreach (RaycastHit2D hit in allHits)
        {
            if (hit.collider.CompareTag("obstacle"))
            {
                if (hit.distance < rayDistance)
                {
                    isObstaclesFound = true;
                    break;
                }
            }
        }

        if (!isObstaclesFound)
        {
            Debug.Log($"{trackingNodeIndex-1}�� �� (ID {allRoadNodes[trackingNodeIndex-1].id}) ��忡�� {trackingNodeIndex}�� �� (ID {allRoadNodes[trackingNodeIndex].id}) ���� ���� ���� {trackingNodeIndex+1}�� �� (ID {allRoadNodes[trackingNodeIndex+1].id}) ���� ���� ������ ã��.");

            UpdateNextTarget();
        }
    }
}