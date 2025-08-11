using System.Collections.Generic;
using UnityEngine;

public class TrackerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public bool skipNodeTracking = false;
    public float skipNodeTrackingPeriod = 0.25f;

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

        targetPos = allRoadNodes[trackingNodeIndex].transform.position;
        UpdateDirection();
    }

    private void UpdateDirection()
    {
        direction = (targetPos - (Vector2)transform.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler(0, 0, angle);
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
}