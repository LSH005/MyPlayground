using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public float doorMoveTime = 2.0f;

    public static int currentElevatorFloor;
    public static bool isDoorMoving = false;
    public static bool isElevatorMoving = false;
    public static bool isDoorOpened = false;
    public static bool isDoorControlDisabled = false;

    private float autoCloseCount = 0;
    private DoorMovement[] allDoors;
    private CallButton[] allCallButtons;
    private ElevatorMovement ElevatorMovement;

    private void Awake()
    {
        allDoors = FindObjectsOfType<DoorMovement>();
        allCallButtons = FindObjectsOfType<CallButton>();
        ElevatorMovement = FindObjectOfType<ElevatorMovement>();
        currentElevatorFloor = 1;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            OpenAllDoors();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            CloseAllDoors();
        }

        if (autoCloseCount >= 0)
        {
            autoCloseCount -= Time.deltaTime;
            if (autoCloseCount < 0)
            {
                CloseAllDoors();
            }
        }
    }

    public void OpenAllDoors()
    {
        isDoorOpened = true;

        ButtonOff(currentElevatorFloor);
        autoCloseCount = 3.0f;

        if (allDoors == null || allDoors.Length == 0)
        {
            Debug.LogWarning("문이 존재하지 않거나, DoorMovement 스크립트가 없음");
            return;
        }

        foreach (DoorMovement door in allDoors)
        {
            door.DoorOpen(doorMoveTime);
        }
    }

    public void CloseAllDoors()
    {
        isDoorOpened = false;

        if (allDoors == null || allDoors.Length == 0)
        {
            Debug.LogWarning("문이 존재하지 않거나, DoorMovement 스크립트가 없음");
            return;
        }

        foreach (DoorMovement door in allDoors)
        {
            door.DoorClose(doorMoveTime);
        }
    }

    public void ButtonOff(int floorNumber)
    {
        foreach (CallButton button in allCallButtons)
        {
            if (button.floorNumber == floorNumber)
            {
                button.ResetButton();
            }
        }
    }
}
