using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.Ports;
using UnityEngine;

public class Management : MonoBehaviour
{
    private SerialPort sp = new SerialPort("COM11", 9600);
    private string arduinoByte = "";

    private LightIcon _lightScript;
    private TempIcon _tempIconScript;
    public GameObject lightIcon;
    public GameObject tempIcon;

    public bool correctLit = false;
    public bool correctTemp = false;

    // Start is called before the first frame update
    void Start()
    {
        _lightScript = lightIcon.GetComponent<LightIcon>();
        _tempIconScript = tempIcon.GetComponent<TempIcon>();
        sp.Open();
        sp.ReadTimeout = 100;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        openClosePort();

        if (sp.IsOpen)
        {
            try
            {
                arduinoByte = sp.ReadLine();
                Debug.Log(arduinoByte);
                if (arduinoByte.Length==1)
                {
                    switch (arduinoByte)
                    {
                        case "a": // Too much light, Led + ambient 
                            correctLit = false;
                            lightIcon.GetComponent<Renderer>().material.color = Color.red;
                            Debug.Log("Too much Light.Led ON");
                            break;
                        case "b": // Too much light, Only ambient
                            correctLit = false;
                            lightIcon.GetComponent<Renderer>().material.color = Color.red;
                            Debug.Log("Too much Light.Led OFF");
                            break;
                        case "c": // Good enough light, ambient + led
                            correctLit = true;
                            lightIcon.GetComponent<Renderer>().material.color = Color.green;
                            break;
                        case "d": // Good enough light, only ambient
                            correctLit = true;
                            lightIcon.GetComponent<Renderer>().material.color = Color.green;
                            break;
                        case "e": // Too little light, ambient only
                            correctLit = false;
                            lightIcon.GetComponent<Renderer>().material.color = Color.black;
                            break;
                        case "f": // Too little light, ambient + ledOn (busted, obstructed)
                            correctLit = false;
                            gameObject.GetComponent<Renderer>().material.color = Color.black;
                            break;
                        case "g": // Tôo hot, fan on
                            correctTemp = false;
                            tempIcon.GetComponent<Renderer>().material.color = Color.red;
                            break;
                        case "h": // Too hot, fan off
                            correctTemp = false;
                            tempIcon.GetComponent<Renderer>().material.color = Color.red;
                            break;
                        case "i": // Good, fan on
                            correctTemp = true;
                            tempIcon.GetComponent<Renderer>().material.color = Color.green;
                            break;
                        case "j": // Good, fan off
                            correctTemp = true;
                            tempIcon.GetComponent<Renderer>().material.color = Color.green;
                            break;
                        case "k": // Too cold, fan on
                            correctTemp = false;
                            tempIcon.GetComponent<Renderer>().material.color = Color.black;
                            break;
                        case "l": // Too cold, fan off
                            correctTemp = false;
                            tempIcon.GetComponent<Renderer>().material.color = Color.black;
                            break;
                        default:
                            Debug.Log("Case error!");
                            break;
                    }
                    getSensorsStates();
                    Slower();
                }
                
                //Debug.Log(sp.ReadByte());
                //Debug.Log(sp.ReadLine());
                //Debug.Log(arduinoByte);
                //Debug.Log(arduinoByte.Length);
            }
            catch (System.Exception)
            {

            }
            
        }

    }

    //Write to Arduino
    private void WriteToArduino(string message)
    {
        sp.WriteLine(message);
        sp.BaseStream.Flush();
    }

    //Turn led on off
    public void lightSwitch(bool iconSensorState)
    {
        if (iconSensorState)
        {
            WriteToArduino("LEDUP");
        }
        else
        {
            WriteToArduino("LEDDOWN");
        }
        Debug.Log("light: " + iconSensorState);
    }


    //Turn fan on off
    public void fanSwitch(bool iconSensorState)
    {
        if (iconSensorState)
        {
            WriteToArduino("FANUP");
        }
        else
        {
            WriteToArduino("FANDOWN");
        }

        Debug.Log("fan: " + iconSensorState);
    }


    //Serial Port Open and close

    private void openClosePort()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            sp.Close();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            sp.Open();
        }
    }
    private void getSensorsStates()
    {
        lightSwitch(_lightScript.getLightState());
        fanSwitch(_tempIconScript.getFantate());
    }

    IEnumerator Slower()
    {
        print(Time.time);
        yield return new WaitForSeconds(1);
    }
}


