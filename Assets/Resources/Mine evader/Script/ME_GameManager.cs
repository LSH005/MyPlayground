using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class ME_GameManager : MonoBehaviour
{
    public static ME_GameManager Instance { get; private set; }
    public static List<GameObject> allNonBombTiles = new List<GameObject>();

    public Transform tileLoot;
    public GameObject tilePrefab;
    public int gameSize = 15;
    public int bombProbability = 15;

    public bool hasClickedOnce = false;
    public bool isInOperation = false;

    private List<GameObject> allTiles = new List<GameObject>();
    private List<GameObject> allBombTiles = new List<GameObject>();

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void AddGameSize(int value)
    {
        gameSize += value;
        gameSize = Mathf.Clamp(gameSize, 10, 200);
    }

    public void AddBombProbability(int value)
    {
        bombProbability += value;
        bombProbability = Mathf.Clamp(bombProbability, 1, 99);
    }

    public void SetGame()
    {
        tileLoot.position = Vector3.zero;

        for (int y = gameSize; y >= 1; y--)
        {
            for (int x = 0; x < gameSize; x++)
            {
                GameObject spawnedTile = Instantiate(tilePrefab, new Vector3(x, -gameSize + y, 0), Quaternion.identity);
                spawnedTile.transform.SetParent(tileLoot);
                spawnedTile.name = $"Tile [{x:D2}/{y:D2}]";
                allTiles.Add(spawnedTile);
            }
        }

        tileLoot.position = new Vector3(((float)-gameSize / 2) + 0.5f, 0, 0);
        isInOperation = true;
    }

    public void SetBombTiles(Vector3 startPosition)
    {
        if (hasClickedOnce) return;
        hasClickedOnce = true;

        float startRadius = 3f;

        List<GameObject> potentialBombTiles = new List<GameObject>();
        foreach (GameObject tile in allTiles)
        {
            if (Vector3.Distance(tile.transform.position, startPosition) > startRadius)
            {
                potentialBombTiles.Add(tile);
            }
        }

        int bombCount = (int)((gameSize * gameSize) * (bombProbability / 100f));

        potentialBombTiles = potentialBombTiles.OrderBy(x => Random.value).ToList();
        int countToTake = Mathf.Clamp(bombCount, 0, potentialBombTiles.Count-1);

        allBombTiles = potentialBombTiles.Take(countToTake).ToList();

        foreach (GameObject currentTile in allTiles)
        {
            ME_TileHandler tileHandler = currentTile.GetComponent<ME_TileHandler>();

            if (tileHandler != null)
            {
                if (allBombTiles.Contains(currentTile))
                {
                    tileHandler.isBomb = true;
                }
                else
                {
                    tileHandler.isBomb = false;
                }
            }
            else Debug.LogError("ME_TileHandler ¾øÀ½");
        }

        foreach (GameObject currentTile in allTiles)
        {
            ME_TileHandler tileHandler = currentTile.GetComponent<ME_TileHandler>();

            if (tileHandler != null && !tileHandler.isBomb)
            {
                tileHandler.SetTileNumber();
            }
        }

        allNonBombTiles = allTiles.Except(allBombTiles).ToList();
    }

    public void GameOver()
    {
        isInOperation = false;

        foreach (GameObject currentTile in allTiles)
        {
            ME_TileHandler tileHandler = currentTile.GetComponent<ME_TileHandler>();

            if (tileHandler != null)
            {
                tileHandler.MineDisclosure();
            }
        }
    }


    public void StopGame()
    {
        hasClickedOnce = false;
        isInOperation = false;
        ClearAllTiles();
    }

    public void ClearAllTiles()
    {
        foreach (GameObject tile in allTiles)
        {
            if (tile != null)
            {
                Destroy(tile);
            }
        }

        allTiles.Clear();
    }
}