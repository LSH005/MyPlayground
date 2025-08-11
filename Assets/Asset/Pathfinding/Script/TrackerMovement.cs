using System.Collections.Generic;
using UnityEngine;

public class TrackerMovement : MonoBehaviour
{
    [Header("설정")]
    public float moveSpeed = 5f;    // 모르면 초등영어를 다시 배우고 와야 함, 그리고 코딩 접으셈.
    [Header("노드 추적 중단 설정")]
    public bool skipNodeTracking = false;   // 효율적인 이동을 위해 현재 노드 추적을 중단할지 여부 (이하 모든 주석에서 "노추중" 이라 칭함.)
    public float skipNodeTrackingPeriod = 0f;    // 노추중의 계산 주기
    public float skipNodeTrackingRange = 0.75f;    // 노추중을 하기 위해 다음 노드로부터 멀어져야 하는 최소 거리

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
                Debug.LogError("위치 손실 발생");
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
            Debug.Log("추적 종료");
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

        Debug.Log("추적 시작");
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
            Debug.Log($"{trackingNodeIndex-1}번 길 (ID {allRoadNodes[trackingNodeIndex-1].id}) 노드에서 {trackingNodeIndex}번 길 (ID {allRoadNodes[trackingNodeIndex].id}) 노드로 가는 도중 {trackingNodeIndex+1}번 길 (ID {allRoadNodes[trackingNodeIndex+1].id}) 노드로 가는 직선을 찾음.");

            UpdateNextTarget();
        }
    }
}