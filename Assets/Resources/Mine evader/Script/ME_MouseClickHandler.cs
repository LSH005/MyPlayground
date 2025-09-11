using System.Collections;
using UnityEngine;

public class ME_MouseClickHandler : MonoBehaviour
{
    public float requiredPressTime = 0.75f;
    public float rayMaxDistance = 40;
    public LayerMask buttonLayer;

    private float mouseDownTimer;
    private Coroutine autoClickCoroutine;

    private void Start()
    {
        StartCoroutine(ClickHandler());
    }

    IEnumerator ClickHandler()
    {
        while (true)
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayMaxDistance, buttonLayer))
                {
                    // SendMessageOptions.DontRequireReceiver는 오류가 있어도 출력하지 말라는 것
                    hit.collider.gameObject.SendMessage("LeftClicked", SendMessageOptions.DontRequireReceiver);
                }

                mouseDownTimer = 0;
            }

            if (Input.GetMouseButton(0))
            {
                mouseDownTimer += Time.deltaTime;

                if (mouseDownTimer >= requiredPressTime)
                {
                    autoClickCoroutine = StartCoroutine(AutoClick());
                    yield return new WaitUntil(() => Input.GetMouseButtonUp(0));
                    StopCoroutine(autoClickCoroutine);
                    mouseDownTimer = 0f;
                }
            }

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, rayMaxDistance, buttonLayer))
                {
                    // SendMessageOptions.DontRequireReceiver는 오류가 있어도 출력하지 말라는 것
                    hit.collider.gameObject.SendMessage("RightClicked", SendMessageOptions.DontRequireReceiver);
                }
            }

            yield return null;
        }
    }

    IEnumerator AutoClick()
    {
        while (true)
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, rayMaxDistance, buttonLayer))
            {
                // SendMessageOptions.DontRequireReceiver는 오류가 있어도 출력하지 말라는 것
                hit.collider.gameObject.SendMessage("LeftClicked", SendMessageOptions.DontRequireReceiver);
            }

            yield return new WaitForSeconds(0.05f);
        }
    }
}
