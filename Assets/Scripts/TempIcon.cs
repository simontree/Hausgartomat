using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempIcon : MonoBehaviour
{
    public bool fanOn = false;
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
        fanOn = !fanOn;
    }

    public bool getFantate()
    {
        return fanOn;
    }
}
