using UnityEngine;


[RequireComponent(typeof(BoxCollider2D))]
public class NodeMovement : MonoBehaviour
{
    public int id;
    public int parentId;
    public bool parented = false;
    public bool isRoad = false;
    public bool isStartPoint = false;
    public bool isEndPoint = false;
    public float nodeSpacing = 1f;
    public GameObject newNode;

    private BoxCollider2D nodeCollider;
    private SpriteRenderer spriteRenderer;
    private readonly Vector2[] directions = new Vector2[]
    {
        Vector2.up,
        Vector2.right,
        Vector2.down,
        Vector2.left
    };

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        nodeCollider = GetComponent<BoxCollider2D>();
        if (nodeCollider == null)
        {
            Debug.LogError("BoxCollider2D ¾øÀ½");
        }

        PathfindManager.allQualifiedNodes.Add(this);
        isStartPoint = false;
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

        if (!hasEmptySpace)
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
                GameObject node = Instantiate(newNode, spawnPosition, Quaternion.identity);

                NodeMovement newNodeScript = node.GetComponent<NodeMovement>();
                if (newNodeScript != null)
                {
                    newNodeScript.id = PathfindManager.nodeId;
                    newNodeScript.parentId = id;
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