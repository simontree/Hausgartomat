using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightIcon : MonoBehaviour
{

    public bool lightOn = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseDown()
    {
        lightOn = !lightOn;
    }

    public bool getLightState()
    {
        return lightOn;
    }
}
