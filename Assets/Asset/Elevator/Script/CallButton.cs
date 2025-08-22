using UnityEngine;
using UnityEngine.UI;

public class CallButton : MonoBehaviour
{
    public int floorNumber;

    private Color originalColor;

    private Button button;
    private Image buttonImage;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        originalColor = buttonImage.color;
    }

    void Start()
    {
        button.onClick.AddListener(ButtonClicked);
    }

    public void ButtonClicked()
    {
        SetColor(Color.green);
    }

    public void SetColor(Color colorToChange)
    {
        buttonImage.color = colorToChange;
    }
    public void ResetColor()
    {
        if (buttonImage != null)
        {
            buttonImage.color = originalColor;
        }
    }
}
