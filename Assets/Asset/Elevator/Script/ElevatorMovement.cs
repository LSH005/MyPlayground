using UnityEngine;
using System.Collections;

public class ElevatorMovement : MonoBehaviour
{
    public float floorSpacing = 5.0f;
    public float elevatorSpeed = 1f;

    private Coroutine movementCoroutine;
    private ElevatorManager manager;

    private void Awake()
    {
        manager = FindObjectOfType<ElevatorManager>();
    }
    public void MoveElevatorTo(int targetFloor)
    {
        if (targetFloor == ElevatorManager.currentElevatorFloor)
        {
            AfterMove();
            return;
        }

        ElevatorManager.isElevatorMoving = true;
        ElevatorManager.currentElevatorFloor = targetFloor;
        Vector3 targetPos = Vector3.zero;
        targetPos.y = floorSpacing * (targetFloor - 1);
        movementCoroutine = StartCoroutine(MoveElevatorTo(targetPos));
    }

    private IEnumerator MoveElevatorTo(Vector3 target)
    {
        while (transform.localPosition != target)
        {
            transform.localPosition = Vector3.MoveTowards(transform.localPosition, target, elevatorSpeed * Time.deltaTime);
            yield return null;
        }

        yield return new WaitForSeconds(0.5f);

        AfterMove();
    }

    private void AfterMove()
    {
        manager.OpenAllDoors();
        ElevatorManager.isDoorOpened = true;
        ElevatorManager.isElevatorMoving = false;
        movementCoroutine = null;
    }
}
