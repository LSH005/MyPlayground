using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathfindManager : MonoBehaviour
{
    public GameObject obstacle;
    public GameObject node;
    public readonly int maxAttempts = 500;
    public Vector2 targetPos;

    public static int nodeId;
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
                nodeId = 1;

                isActivating = true;

                GameObject trackerObject = GameObject.FindWithTag("Tracker");
                if (trackerObject != null)
                {
                    GameObject newNode = Instantiate(node, trackerObject.transform.position, Quaternion.identity);
                    NodeMovement nodeMovementScript = newNode.GetComponent<NodeMovement>();
                    if (nodeMovementScript != null)
                    {
                        nodeMovementScript.id = nodeId;
                        nodeMovementScript.isStartPoint = true;
                        nodeId++;
                    }
                    else
                    {
                        Debug.LogError("�����տ� NodeMovement ��ũ��Ʈ ����");
                    }

                }
                else
                {
                    Debug.LogError("Tracker �±׸� ���� ������Ʈ ����");
                }

                GameObject targetObject = GameObject.FindWithTag("Target");
                if (targetObject != null)
                {
                    targetPos = targetObject.transform.position;
                }
                else
                {
                    Debug.LogError("Target �±׸� ���� ������Ʈ ����");
                }


                int attempts = 0;
                while (attempts <= maxAttempts)
                {
                    attempts++;

                    if (allQualifiedNodes.Count == 0)
                    {
                        Debug.LogError("���� �θ� ��� ����");
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
                        if (parentNode.id <= nodeId)
                        {

                        }

                        parentNode.SummonNode();
                        nodeId++;

                        float distance = 0;
                        bool isEndPoint = false;
                        foreach (NodeMovement node in allQualifiedNodes)
                        {
                            distance = Vector2.Distance(node.transform.position, targetPos);
                            if (distance < parentNode.nodeSpacing - 0.01f)
                            {
                                node.parented= true;
                                isEndPoint=true;
                                break;
                            }
                        }
                        if (isEndPoint) break;
                    }
                }

                NodeMovement[] allNodes = FindObjectsOfType<NodeMovement>();
                foreach (NodeMovement node in allNodes)
                {
                    if (!node.parented)
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
            Debug.LogWarning("��� ����");
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