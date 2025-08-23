using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public float doorMoveTime = 2.0f;

    public static int currentElevatorFloor;
    public static bool isDoorMoving = false;
    public static bool isElevatorMoving = false;
    public static bool isDoorOpened = false;

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

    }

    public void OpenAllDoors()
    {
        ButtonOff(currentElevatorFloor);

        if (allDoors == null || allDoors.Length == 0)
        {
            Debug.LogWarning("���� �������� �ʰų�, DoorMovement ��ũ��Ʈ�� ����");
            return;
        }

        foreach (DoorMovement door in allDoors)
        {
            door.DoorOpen(doorMoveTime);
        }
    }

    public void CloseAllDoors()
    {
        if (allDoors == null || allDoors.Length == 0)
        {
            Debug.LogWarning("���� �������� �ʰų�, DoorMovement ��ũ��Ʈ�� ����");
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
                break;
            }
        }
    }
}
