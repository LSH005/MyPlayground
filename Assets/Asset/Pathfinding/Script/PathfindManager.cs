using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PathfindManager : MonoBehaviour
{
    [Header("Setting")]
    public int maxAttempts = 500;
    public bool showRoad = false;
    public bool doRemoveRedundantRoad = true;
    public bool doCleanUpNodes = true;
    public bool moveTracker = true;
    [Header("Assets")]
    public GameObject obstacle;
    public GameObject node;
    public static int nodeId;
    public static List<NodeMovement> allQualifiedNodes = new List<NodeMovement>();

    private float nodeSpacing;
    private bool isActivated = false;
    private bool isArrival = false;
    private Vector2 targetPos;
    private Vector2 trackerPos;
    private Vector2 currentBacktracingPoint;
    private NodeMovement parentNode;
    private List<NodeMovement> allRoadNodes = new List<NodeMovement>();
    private NodeMovement[] allNodes;
    //private readonly Vector2[] directions = new Vector2[]
    //{
    //    Vector2.up,
    //    Vector2.right,
    //    Vector2.down,
    //    Vector2.left
    //};

    void Update()
    {
        if (!isActivated)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("새 탐색 시작");

                Setup();
                PathFind();
                if (isArrival)
                {
                    BacktracePath();

                    if (showRoad)
                    {
                        if (doRemoveRedundantRoad) RemoveRedundantRoadNodes();
                        if (doCleanUpNodes) CleanUpNodes();
                    }
                    else
                    {
                        NodeMovement[] allNodes = FindObjectsOfType<NodeMovement>();
                        foreach (NodeMovement node in allNodes)
                        {
                            SpriteRenderer spriteRenderer = node.GetComponent<SpriteRenderer>();
                            if (spriteRenderer != null)
                            {
                                spriteRenderer.enabled = false;
                            }
                        }
                        RemoveRedundantRoadNodes();
                        CleanUpNodes();
                    }

                    if (moveTracker)
                    {
                        GameObject trackerObject = GameObject.FindWithTag("Tracker");
                        if (trackerObject != null)
                        {
                            TrackerMovement trackerMovementScript = trackerObject.GetComponent<TrackerMovement>();
                            if (trackerMovementScript != null)
                            {
                                trackerMovementScript.TrackingNode();
                            }
                            else
                            {
                                Debug.LogError("Tracker 오브젝트에 TrackerMovement 스크립트 없음");
                            }
                        }
                        else
                        {
                            Debug.LogError("Tracker 태그를 가진 오브젝트 없음");
                        }
                    }
                }
                else
                {
                    Debug.LogError($"길이 없거나, 노드 {maxAttempts}개 이상으로 넓은 탐색 범위 구조가 있음.");
                    NodeMovement[] allNodes = FindObjectsOfType<NodeMovement>();
                    foreach (NodeMovement node in allNodes)
                    {
                        SpriteRenderer spriteRenderer = node.GetComponent<SpriteRenderer>();
                        if (spriteRenderer != null)
                        {
                            spriteRenderer.color = Color.red;
                        }
                    }

                }
            }

            if (Input.GetMouseButton(0))
            {
                Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                if (Input.GetKey(KeyCode.D))
                {
                    Collider2D[] obstacles = Physics2D.OverlapCircleAll(mousePosition, 0.05f);
                    foreach (Collider2D obstacleCollider in obstacles)
                    {
                        if (obstacleCollider.CompareTag("obstacle") && obstacleCollider.GetComponent<obstacleRemover>() != null)
                        {
                            Destroy(obstacleCollider.gameObject);
                        }
                    }
                }
                else
                {
                    Instantiate(obstacle, mousePosition, Quaternion.identity);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene("Pathfinding");
        }
    }

    private NodeMovement FindNodeInRadius(Vector2 position, float radius, NodeMovement[] nodes)
    {
        foreach (NodeMovement node in nodes)
        {
            if (Vector2.Distance(node.transform.position, position) <= radius)
            {
                return node;
            }
        }
        return null;
    }

    void FindClosestNodeToTarget()
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

    void Setup()
    {
        nodeId = 1;
        isActivated = true;
        allQualifiedNodes.Clear();

        GameObject trackerObject = GameObject.FindWithTag("Tracker");
        trackerPos = trackerObject.transform.position;
        if (trackerObject != null)
        {
            GameObject newNode = Instantiate(node, trackerObject.transform.position, Quaternion.identity);
            NodeMovement nodeMovementScript = newNode.GetComponent<NodeMovement>();
            if (nodeMovementScript != null)
            {
                nodeMovementScript.id = nodeId;
                nodeSpacing = nodeMovementScript.nodeSpacing / 2f;
                nodeMovementScript.isStartPoint = true;
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
    }

    void PathFind()
    {
        int attempts = 0;
        isArrival = false;
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
                //if (parentNode.id <= nodeId)
                //{

                //}

                parentNode.SummonNode();
                nodeId++;

                float distance = 0;
                bool isEndPoint = false;
                foreach (NodeMovement node in allQualifiedNodes)
                {
                    distance = Vector2.Distance(node.transform.position, targetPos);
                    if (distance < parentNode.nodeSpacing)
                    {
                        node.isEndPoint = true;
                        node.parented = true;
                        isEndPoint = true;
                        break;
                    }
                }

                if (isEndPoint)
                {
                    isArrival = true;
                    break;
                }

            }
        }
        Debug.Log("경로 탐색 종료");
    }

    void BacktracePath()
    {
        //NodeMovement[] allNodes = FindObjectsOfType<NodeMovement>();
        //foreach (NodeMovement node in allNodes)
        //{
        //    if (!node.parented)
        //    {
        //        Destroy(node.gameObject);
        //    }
        //}

        NodeMovement[] remainingNodes = FindObjectsOfType<NodeMovement>();
        List<NodeMovement> remainingNodeList = new List<NodeMovement>(remainingNodes);

        NodeMovement currentNode = FindNodeInRadius(targetPos, nodeSpacing, remainingNodes);
        if (currentNode == null)
        {
            Debug.LogError("목표 지점에 해당하는 노드 없음");
            return;
        }
        else
        {
            currentNode.isRoad = true;
            remainingNodeList.Remove(currentNode);
        }

        while (currentNode != null && !currentNode.isStartPoint)
        {
            NodeMovement parentNode = null;
            foreach (NodeMovement node in remainingNodeList)
            {
                if (
                    node.id == currentNode.parentId &&
                    node.parented &&
                    Vector2.Distance(node.transform.position, currentNode.transform.position) <= nodeSpacing * 2
                    )
                {
                    parentNode = node;
                    break;
                }
            }

            if (parentNode == null)
            {
                Debug.LogError($"ID 가 {currentNode.parentId} 인 노드 없음");
                break;
            }
            else
            {
                currentNode = parentNode;
                currentNode.isRoad = true;
                remainingNodeList.Remove(currentNode);
            }

            currentNode = parentNode;
        }
    }

    private void RemoveRedundantRoadNodes()
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

        if (allRoadNodes.Count < 3)
        {
            Debug.LogWarning("최적화할 경로 노드가 부족함.");
            return;
        }

        // ID 순으로 정렬
        allRoadNodes.Sort((a, b) => a.id.CompareTo(b.id));

        int endNodeIndex = allRoadNodes.Count - 1;
        int currentCasterNodeIndex = allRoadNodes[0].id - 1;
        int targetNodeIndex = currentCasterNodeIndex + 2;
        Debug.Log("경로 최적화 시작");
        while (targetNodeIndex <= endNodeIndex)
        {
            Vector2 rayOrigin = allRoadNodes[currentCasterNodeIndex].transform.position;
            Vector2 rayTarget = allRoadNodes[targetNodeIndex].transform.position;

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
                        Debug.Log($"{currentCasterNodeIndex}번 (ID {allRoadNodes[currentCasterNodeIndex].id}) 노드에서 {targetNodeIndex}번 (ID {allRoadNodes[targetNodeIndex].id}) 노드로 가던 중 장애물 있음");
                        currentCasterNodeIndex = targetNodeIndex - 1;
                        targetNodeIndex = currentCasterNodeIndex + 2;
                        isObstaclesFound = true;
                        break;
                    }
                }
            }

            if (!isObstaclesFound)
            {
                allRoadNodes[targetNodeIndex - 1].isRoad = false;
                targetNodeIndex++;
            }
        }
        Debug.Log("경로 최적화 끝");
    }
    void CleanUpNodes()
    {
        allNodes = FindObjectsOfType<NodeMovement>();
        foreach (NodeMovement node in allNodes)
        {
            if (!node.isRoad)
            {
                Destroy(node.gameObject);
            }
        }
        Debug.Log("경로 확정함");
    }
}