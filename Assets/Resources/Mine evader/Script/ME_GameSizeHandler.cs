using TMPro;
using UnityEngine;

public class ME_GameSizeHandler : MonoBehaviour
{
    public bool isIncrease;
    public TextMeshPro gameSizeText;
    
    private ME_GameManager gameManager;
    private int addValue;

    private void Start()
    {
        gameManager = ME_GameManager.Instance;
        UpdateSizeText();

        if (isIncrease) addValue = 1;
        else addValue = -1;
    }

    public void LeftClicked()
    {
        if (gameManager.isInOperation) return;
        gameManager.AddGameSize(addValue);
        UpdateSizeText();
    }

    public void UpdateSizeText()
    {
        gameSizeText.text = $"{gameManager.gameSize:D3} ¡¿ {gameManager.gameSize:D3}";
    }
}
