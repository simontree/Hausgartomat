using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumidIcon : MonoBehaviour
{
    public bool pumpOn = false;
    private void OnMouseDown()
    {
        pumpOn = !pumpOn;
    }

    public bool getPumpState()
    {
        return pumpOn;
    }
}
