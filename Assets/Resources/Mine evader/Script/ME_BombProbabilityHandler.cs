using TMPro;
using UnityEngine;

public class ME_BombProbabilityHandler : MonoBehaviour
{
    public bool isIncrease;
    public TextMeshPro bombProbabilityText;

    private ME_GameManager gameManager;
    private int addValue;

    void Start()
    {
        gameManager = ME_GameManager.Instance;
        UpdateBombProbabilityText();

        if (isIncrease) addValue = 1;
        else addValue = -1;
    }

    public void LeftClicked()
    {
        if (gameManager.isInOperation) return;
        gameManager.AddBombProbability(addValue);
        UpdateBombProbabilityText();
    }


    public void UpdateBombProbabilityText()
    {
        bombProbabilityText.text = $"{gameManager.bombProbability:D2} %";
    }
}
