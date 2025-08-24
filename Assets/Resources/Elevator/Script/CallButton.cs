using UnityEngine;
using UnityEngine.UI;

public class CallButton : MonoBehaviour
{
    public int floorNumber;

    private bool waitForStop = false;
    private bool isPressed = false;
    private Color originalColor;
    private Button button;
    private Image buttonImage;
    private ElevatorMovement elevatorMovement;
    private ElevatorManager manager;

    private void Awake()
    {
        button = GetComponent<Button>();
        buttonImage = GetComponent<Image>();
        elevatorMovement = FindObjectOfType<ElevatorMovement>();
        manager = FindObjectOfType<ElevatorManager>();
        originalColor = buttonImage.color;
    }

    void Start()
    {
        button.onClick.AddListener(ButtonClicked);
    }

    private void Update()
    {
        if (waitForStop)
        {
            if (isAllStop())
            {
                waitForStop = false;
                elevatorMovement.MoveElevatorTo(floorNumber);
            }
        }
    }

    public void ButtonClicked()
    {
        if (isPressed) return;

        isPressed = true;
        SetColor(Color.green);

        if (ElevatorManager.currentElevatorFloor == floorNumber)
        {
            if (!ElevatorManager.isElevatorMoving)
            {
                manager.OpenAllDoors();
            }
        }
        else
        {
            waitForStop = true;
        }
    }

    public void SetColor(Color colorToChange)
    {
        buttonImage.color = colorToChange;
    }
    public void ResetButton()
    {
        buttonImage.color = originalColor;
        isPressed = false;
    }

    public bool isAllStop()
    {
        return !ElevatorManager.isDoorMoving && !ElevatorManager.isDoorOpened && !ElevatorManager.isElevatorMoving;
    }
}
