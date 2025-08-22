using System.Collections.Generic;
using UnityEngine;

public class ElevatorManager : MonoBehaviour
{
    public float doorMoveTime = 2.0f;

    private List<DoorMovement> allDoors;

    private void Awake()
    {
        allDoors = new List<DoorMovement>(FindObjectsOfType<DoorMovement>());
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
        if (allDoors == null || allDoors.Count == 0)
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
        if (allDoors == null || allDoors.Count == 0)
        {
            Debug.LogWarning("문이 존재하지 않거나, DoorMovement 스크립트가 없음");
            return;
        }

        foreach (DoorMovement door in allDoors)
        {
            door.DoorClose(doorMoveTime);
        }
    }
}
