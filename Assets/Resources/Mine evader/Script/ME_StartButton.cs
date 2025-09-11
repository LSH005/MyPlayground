using TMPro;
using UnityEngine;

public class ME_StartButton : MonoBehaviour
{
    public Color startButtonColor;
    public Color endButtonColor;
    public TextMeshPro buttonText;

    private bool isStarted = false;
    private SpriteRenderer spriteRenderer;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void LeftClicked()
    {
        if (isStarted)
        {
            ME_GameManager.Instance.StopGame();
            spriteRenderer.color = startButtonColor;
            buttonText.text = "시작";
        }
        else
        {
            ME_GameManager.Instance.StartGame();
            spriteRenderer.color = endButtonColor;
            buttonText.text = "중지";
        }

        isStarted = !isStarted;
    }
}
