using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightsOffOn : MonoBehaviour
{

    public Light light;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            light.intensity = 1;
        }
        if (Input.GetKeyDown(KeyCode.DownArrow))
        {
            light.intensity = 0.3f;
        }
    }
}
