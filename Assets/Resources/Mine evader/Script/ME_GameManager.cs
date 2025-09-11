using System.Collections.Generic;
using UnityEngine;


public class ME_GameManager : MonoBehaviour
{
    public static ME_GameManager Instance { get; private set; }
    public static List<GameObject> allTiles = new List<GameObject>();
    public static List<GameObject> allBombTiles = new List<GameObject>();

    public Transform tileLoot;
    public GameObject tilePrefab;
    public int gameSize = 15;
    public int bombProbability = 15;


    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        
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

    public void StartGame()
    {
        tileLoot.position = Vector3.zero;

        for (int y = gameSize; y >= 1; y--)
        {
            for (int x = 0; x < gameSize; x++)
            {
                GameObject spawnedTile = Instantiate(tilePrefab, new Vector3(x, -gameSize + y, 0), Quaternion.identity);
                spawnedTile.transform.SetParent(tileLoot);
                allTiles.Add(spawnedTile);
            }
        }

        tileLoot.position = new Vector3((-gameSize / 2), 0, 0);
    }


    public void StopGame()
    {
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