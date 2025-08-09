using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class NodeMovement : MonoBehaviour
{
    public int id;
    public bool parentalAuthority = true;
    public bool parented = false;
    public bool isRoad = false;
    public bool isStartPoint = false;
    public bool isEndPoint = false;
    public float nodeSpacing = 1f;
    public GameObject newNode;

    private BoxCollider2D nodeCollider;
    private readonly Vector2[] directions = new Vector2[]
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

    private void Awake()
    {
        nodeCollider = GetComponent<BoxCollider2D>();
        if (nodeCollider == null)
        {
            Debug.LogError("BoxCollider2D 없음");
        }

        PathfindManager.allQualifiedNodes.Add(this);
    }

    public void PermissionsUpdate()
    {
        if (nodeCollider == null)
        {
            return;
        }

        Vector2 searchSize = nodeCollider.size;

        bool hasEmptySpace = false;

        foreach (Vector2 direction in directions)
        {
            Vector2 searchPosition = (Vector2)transform.position + (direction * nodeSpacing);
            Collider2D[] colliders = Physics2D.OverlapBoxAll(searchPosition, searchSize, 0f);

            bool foundObstacleOrNode = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject == this.gameObject)
                {
                    continue;
                }

                if (collider.CompareTag("obstacle") || collider.CompareTag("Node"))
                {
                    foundObstacleOrNode = true;
                    break;
                }
            }

            if (!foundObstacleOrNode)
            {
                hasEmptySpace = true;
                break;
            }
        }

        parentalAuthority = hasEmptySpace;
        if (!parentalAuthority)
        {
            PathfindManager.allQualifiedNodes.Remove(this);
        }
    }

    public void SummonNode()
    {
        foreach (Vector2 direction in directions)
        {
            Vector2 spawnPosition = (Vector2)transform.position + (direction * nodeSpacing);

            Collider2D[] colliders = Physics2D.OverlapBoxAll(spawnPosition, nodeCollider.size, 0f);

            bool foundObstacleOrNode = false;
            foreach (Collider2D collider in colliders)
            {
                if (collider.gameObject != this.gameObject && (collider.CompareTag("obstacle") || collider.CompareTag("Node")))
                {
                    foundObstacleOrNode = true;
                    break;
                }
            }

            if (!foundObstacleOrNode)
            {
                GameObject newGameObject = Instantiate(newNode, spawnPosition, Quaternion.identity);

                NodeMovement newNodeScript = newGameObject.GetComponent<NodeMovement>();
                if (newNodeScript != null)
                {
                    newNodeScript.id = PathfindManager.nodeId;
                    // PathfindManager.nodeId++;
                    // 정적 변수로 변경함에 따라 이곳에서 id가 바뀌지 않음
                }
            }
        }

        parented = true;
    }

    private void OnDestroy()
    {
        PathfindManager.allQualifiedNodes.Remove(this);
    }
}