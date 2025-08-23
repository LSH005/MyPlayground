using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class DoorControlButton : MonoBehaviour
{
    public bool isOpenButton;

    private Color originalColor;
    private Button button;
    private Image buttonImage;
    private Coroutine FlickerCoroutine;
    private ElevatorManager manager;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        originalColor = buttonImage.color;
        manager = FindObjectOfType<ElevatorManager>();
    }

    private void Start()
    {
        button.onClick.AddListener(ButtonClicked);
    }

    public void ButtonClicked()
    {
        StartCoroutine(Flicker(Color.green));

        if (ElevatorManager.isDoorControlDisabled) return;

        if (isOpenButton) manager.OpenAllDoors();
        else manager.CloseAllDoors();
    }

    IEnumerator Flicker(Color colorToChange)
    {
        buttonImage.color = colorToChange;
        yield return new WaitForSeconds(0.2f);
        buttonImage.color = originalColor;
    }
}
