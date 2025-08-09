using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayGround : MonoBehaviour
{
    void Start()
    {
        float a = 0.1f;
        float b = 0.2f;
        float c = a + b;

        if (c == 0.3f)
        {
            print("YAYYYYYYYYYYYYYYYYYYAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAAA");
        }
        else
        {
            print("UH NOPE");
        }

        print(c);
    }
}
