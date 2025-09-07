using UnityEngine;

public class ScreenTransitionTest : MonoBehaviour
{
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            ScreenTransition.ScreenTransitionGoto("ScreenTransition", "LoadingScreen_1", Color.black, 0f, 0f, 7f, 0f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            ScreenTransition.ScreenTransitionGoto("ScreenTransition", "LoadingScreen_2", new Color32(255, 214, 117, 255), 0f, 0.5f, 7f, 0.5f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            ScreenTransition.ScreenTransitionGoto("ScreenTransition", "LoadingScreen_3", Color.gray, 0f, 0.5f, 7f, 0.5f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            ScreenTransition.ScreenTransitionGoto("ScreenTransition", "LoadingScreen_4", Color.black, 0f, 0.5f, 7f, 0.5f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            ScreenTransition.ScreenTransitionGoto("ScreenTransition", "LoadingScreen_5", Color.black, 0f, 0.5f, 8f, 0.5f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha6))
        {
            ScreenTransition.ScreenTransitionGoto("ScreenTransition", "LoadingScreen_6", Color.black, 0f, 0.5f, 9f, 0.5f, 0f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            ScreenTransition.ScreenTransitionGoto("ScreenTransition", "LoadingScreen_7", Color.black, 0f, 0.5f, 7f, 0.5f, 0f);
        }
    }
}
