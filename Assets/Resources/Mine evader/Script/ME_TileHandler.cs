using System.Collections;
using TMPro;
using UnityEngine;

public class ME_TileHandler : MonoBehaviour
{
    public Sprite normalTile;
    public Sprite flagTile;
    public Sprite openedTile;
    public Sprite mineTile;
    public Sprite flagOnMineTile;
    public Sprite greenMineTile;
    public Sprite originMineTile;
    public GameObject flag;
    public GameObject fireWork;
    public TextMeshPro numberText;
    public LayerMask tileLayer;


    public bool isBomb = false;
    public bool isOpened = false;
    public bool onFlag = false;
    public bool isOriginBomb = false;
    public int tileNumber = 0;

    private float lastClickTime;
    private ME_GameManager gameManager;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void Start()
    {
        gameManager = ME_GameManager.Instance;
        numberText.text = string.Empty;
    }

    public void LeftClicked()
    {
        if (!gameManager.isInOperation) return;

        if (!gameManager.hasClickedOnce)
        {
            gameManager.SetBombTiles(transform.position);
        }

        OpenTile();
    }

    public void OpenTile()
    {
        if (onFlag) return;
        
        if (isBomb)
        {
            isOriginBomb = true;
            gameManager.GameOver();
            return;
        }

        if (isOpened)
        {
            if (Time.time - lastClickTime < 0.4f)
            {
                Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, tileLayer);
                foreach (Collider col in colliders)
                {
                    ME_TileHandler tileHandler = col.GetComponent<ME_TileHandler>();

                    if (tileHandler != null)
                    {
                        if (!tileHandler.isOpened)
                        {
                            tileHandler.OpenTile();
                        }
                    }
                }
            }
            lastClickTime = Time.time;
        }
        else
        {
            isOpened = true;
            spriteRenderer.sprite = openedTile;

            if (tileNumber != 0)
            {
                numberText.transform.localScale = Vector3.one;
            }
            else
            {
                //StartCoroutine(ChainOpenTile());

                Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, tileLayer);
                foreach (Collider col in colliders)
                {
                    ME_TileHandler tileHandler = col.GetComponent<ME_TileHandler>();

                    if (tileHandler != null)
                    {
                        if (!tileHandler.isOpened)
                        {
                            tileHandler.OpenTile();
                        }
                    }
                }
            }

            gameManager.RemoveNonBombTiles(this.gameObject);
        }
    }

    IEnumerator ChainOpenTile()
    {
        yield return new WaitForSeconds(0.1f);

        Collider[] colliders = Physics.OverlapSphere(transform.position, 0.55f, tileLayer);
        foreach (Collider col in colliders)
        {
            ME_TileHandler tileHandler = col.GetComponent<ME_TileHandler>();

            if (tileHandler != null)
            {
                if (!tileHandler.isOpened)
                {
                    tileHandler.OpenTile();
                }
            }
        }
    }

    public void RightClicked()
    {
        if (!gameManager.isInOperation) return;

        if (!isOpened)
        {
            SwitchFlag();
        }
    }

    public void SwitchFlag()
    {
        if (onFlag)
        {
            spriteRenderer.sprite = normalTile;
            GameObject newFlag = Instantiate(flag, transform.position, Quaternion.identity);
        }
        else
        {
            spriteRenderer.sprite = flagTile;
        }

        onFlag = !onFlag;
    }

    public void SetTileNumber()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 1f, tileLayer);
        tileNumber = 0;

        foreach (Collider col in colliders)
        {
            ME_TileHandler tileHandler = col.GetComponent<ME_TileHandler>();

            if (tileHandler != null && tileHandler.isBomb && tileHandler != this)
            {
                tileNumber++;
            }
        }

        if (tileNumber != 0)
        {
            numberText.text = tileNumber.ToString();

            if (tileNumber == 1) numberText.color = new Color32(0, 40, 255, 255);
            else if (tileNumber == 2) numberText.color = new Color32(0, 100, 0, 255);
            else if (tileNumber == 3) numberText.color = new Color32(220, 0, 0, 255);
            else if (tileNumber == 4) numberText.color = new Color32(0, 0, 100, 255);
            else if (tileNumber == 5) numberText.color = new Color32(100, 0, 0, 255);
            else if (tileNumber == 6) numberText.color = new Color32(0, 77, 99, 255);
            else if (tileNumber == 7) numberText.color = new Color32(50, 50, 50, 255);
            else if (tileNumber == 8) numberText.color = new Color32(0, 0, 0, 255);

            numberText.transform.localScale = Vector3.zero;
        }
    }

    public void MineDisclosure()
    {
        if (isBomb)
        {
            if (onFlag)
            {
                spriteRenderer.sprite = flagOnMineTile;
            }
            else
            {
                if (isOriginBomb)
                {
                    spriteRenderer.sprite = originMineTile;
                }
                else
                {
                    spriteRenderer.sprite = mineTile;
                }
            }
        }
    }

    public void MineDisclosureAtWin()
    {
        spriteRenderer.sprite = greenMineTile;
    }

    public void FireWork()
    {
        int repetitions = Random.Range(3, 7);

        for (int i = 0; i < repetitions; i++)
        {
            Instantiate(fireWork, transform.position, transform.rotation);
        }
    }
}
