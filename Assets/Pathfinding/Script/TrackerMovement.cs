using System.Collections.Generic;
using UnityEngine;

public class TrackerMovement : MonoBehaviour
{
    public float moveSpeed = 1f;
    public bool canMove = false;

    private int trackingNodeIndex;
    private List<NodeMovement> allRoadNodes = new List<NodeMovement>();
    private NodeMovement[] allNodes;
    private Vector2 targetPos;
    private Vector2 direction;

    private void Update()
    {
        if (canMove)
        {
            if (Vector2.Distance(transform.position, targetPos) < 0.1f)
            {
                transform.position = allRoadNodes[trackingNodeIndex].transform.position;
                trackingNodeIndex++;

                if (trackingNodeIndex >= allRoadNodes.Count)
                {
                    canMove = false;
                    Debug.Log("추적 종료");
                    return;
                }

                targetPos = allRoadNodes[trackingNodeIndex].transform.position;
                direction = (targetPos - (Vector2)transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, angle);
            }

            transform.position += (Vector3)direction * moveSpeed * Time.deltaTime;
        }
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