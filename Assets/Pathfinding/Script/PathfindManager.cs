using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathfindManager : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject node;
    public readonly int maxAttempts = 500;
    public Vector2 targetPos;

    public static int nodeId = 1;
    public static List<NodeMovement> allQualifiedNodes = new List<NodeMovement>();

    private NodeMovement parentNode;
    private bool isActivating = false;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Pathfinding");
        }

        if (isActivating)
        {

        }
        else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isActivating = true;

                GameObject trackerObject = GameObject.FindWithTag("Tracker");
                if (trackerObject != null)
                {
                    GameObject newNode = Instantiate(node, trackerObject.transform.position, Quaternion.identity);

                    NodeMovement nodeMovementScript = newNode.GetComponent<NodeMovement>();
                    if (nodeMovementScript != null)
                    {
                        nodeMovementScript.id = nodeId;
                        nodeId++;
                    }
                    else
                    {
                        Debug.LogError("프리팹에 NodeMovement 스크립트 없음");
                    }

                }
                else
                {
                    Debug.LogError("Tracker 태그를 가진 오브젝트 없음");
                }

                GameObject targetObject = GameObject.FindWithTag("Target");
                if (targetObject != null)
                {
                    targetPos = targetObject.transform.position;
                }
                else
                {
                    Debug.LogError("Target 태그를 가진 오브젝트 없음");
                }

                int attempts = 0;
                while (attempts <= maxAttempts)
                {
                    attempts++;

                    if (allQualifiedNodes.Count == 0)
                    {
                        Debug.LogError("허용된 부모 노드 없음");
                        break;
                    }

                    for (int i = allQualifiedNodes.Count - 1; i >= 0; i--)
                    {
                        NodeMovement node = allQualifiedNodes[i];
                        if (node != null)
                        {
                            node.PermissionsUpdate();
                        }
                    }

                    if (allQualifiedNodes.Count == 0) break;

                    FindClosestNodeToTarget();
                    if (parentNode != null)
                    {
                        parentNode.SummonNode();
                        nodeId++;

                        float distance = Vector2.Distance(parentNode.transform.position, targetPos);
                        if (distance <= parentNode.nodeSpacing)
                        {
                            break;
                        }
                    }
                }

                NodeMovement[] allNodes = FindObjectsOfType<NodeMovement>();
                foreach (NodeMovement node in allNodes)
                {
                    if (!node.isRoad)
                    {
                        Destroy(node.gameObject);
                    }
                }
            }

            if (Input.GetMouseButton(0))
            {
                Vector3 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                mousePosition.z = 0f;

                Instantiate(obstacle, mousePosition, Quaternion.identity);
            }
        }
    }

    private void FindClosestNodeToTarget()
    {
        if (allQualifiedNodes.Count == 0)
        {
            Debug.LogWarning("노드 없음");
            return;
        }

        float closestDistance = Mathf.Infinity;
        parentNode = null;

        foreach (NodeMovement node in allQualifiedNodes)
        {
            if (node != null)
            {
                float distance = Vector2.Distance(node.transform.position, targetPos);

                if (distance < closestDistance)
                {
                    closestDistance = distance;
                    parentNode = node;
                }
            }
        }
    }
}