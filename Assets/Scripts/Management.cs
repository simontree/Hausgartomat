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
    private HumidIcon _humidIconScript;

    public GameObject lightIcon;
    public GameObject tempIcon;
    public GameObject humidIcon;

    public bool correctLit = false;
    public bool correctTemp = false;
    public bool correctHumid = false;

    private bool oldLedState = false;
    private bool oldFanState = false;
    private bool oldPumpState = false;

    // Start is called before the first frame update
    void Start()
    {
        _lightScript = lightIcon.GetComponent<LightIcon>();
        _tempIconScript = tempIcon.GetComponent<TempIcon>();
        _humidIconScript = humidIcon.GetComponent<HumidIcon>();
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
                //Debug.Log(arduinoByte);
                if (arduinoByte.Length==1)
                {
                    switch (arduinoByte)
                    {   //LIGHT
                        case "a":  //Too little, led off
                            correctLit = false;
                            lightIcon.GetComponent<Renderer>().material.color = Color.black;
                            Debug.Log("Too low Light.Led OFF");
                            break;
                        case "b": // Too little, led on
                            correctLit = false;
                            lightIcon.GetComponent<Renderer>().material.color = Color.black;
                            Debug.Log("Too low Light.Led ON");
                            break;
                        case "c": // Good, led off
                            correctLit = true;
                            lightIcon.GetComponent<Renderer>().material.color = Color.green;
                            Debug.Log("Optimum Light. Led OFF");
                            break;
                        case "d": // Good, led on
                            correctLit = true;
                            lightIcon.GetComponent<Renderer>().material.color = Color.green;
                            Debug.Log("Optimum Light.Led ON");
                            break;
                        case "e": // Too much, led off 
                            correctLit = false;
                            lightIcon.GetComponent<Renderer>().material.color = Color.red;
                            Debug.Log("Too much Light.Led OFF");
                            break;
                        case "f": // Too much, led on
                            correctLit = false;
                            gameObject.GetComponent<Renderer>().material.color = Color.red;
                            Debug.Log("Too much Light.Led ON");
                            break;

                        //TEMPERATURE
                        case "g": // Too cold, fan off
                            correctTemp = false;
                            tempIcon.GetComponent<Renderer>().material.color = Color.black;
                            Debug.Log("Too cold. Fan OFF");
                            break;
                        case "h": // Too cold, fan on
                            correctTemp = false;
                            tempIcon.GetComponent<Renderer>().material.color = Color.black;
                            Debug.Log("Too cold. Fan ON");
                            break;
                        case "i": // Good, fan off
                            correctTemp = true;
                            tempIcon.GetComponent<Renderer>().material.color = Color.green;
                            Debug.Log("Optimum Temp. Fan OFF");
                            break;
                        case "j": // Good, fan on
                            correctTemp = true;
                            tempIcon.GetComponent<Renderer>().material.color = Color.green;
                            Debug.Log("Optimum Temp. Fan ON");
                            break;
                        case "k": // Too hot, fan off
                            correctTemp = false;
                            tempIcon.GetComponent<Renderer>().material.color = Color.red;
                            Debug.Log("Too hot Temp. Fan OFF");
                            break;
                        case "l": // Too hot, fan on
                            correctTemp = false;
                            tempIcon.GetComponent<Renderer>().material.color = Color.red;
                            Debug.Log("Too hot Temp. Fan ON");
                            break;

                        //HUMIDITY
                        case "m": // Too dry, pump off
                            correctHumid = false;
                            humidIcon.GetComponent<Renderer>().material.color = Color.black;
                            Debug.Log("Too dry. Pump OFF");
                            break;
                        case "n": // Too dry, pump on
                            correctHumid = false;
                            humidIcon.GetComponent<Renderer>().material.color = Color.black;
                            Debug.Log("Too dry. Pump ON");
                            break;
                        case "o": // Good, pump off
                            correctHumid = true;
                            humidIcon.GetComponent<Renderer>().material.color = Color.green;
                            Debug.Log("Optimum Humidity. Pump OFF");
                            break;
                        case "p": // Good, pump on
                            correctHumid = true;
                            humidIcon.GetComponent<Renderer>().material.color = Color.green;
                            Debug.Log("Optimum Humidity. Pump ON");
                            break;
                        case "q": // Too wet, pump off
                            correctHumid = false;
                            humidIcon.GetComponent<Renderer>().material.color = Color.red;
                            Debug.Log("Too wet Temp. Pump OFF");
                            break;
                        case "r": // Too wet, pump on
                            correctHumid = false;
                            humidIcon.GetComponent<Renderer>().material.color = Color.red;
                            Debug.Log("Too wet Temp. Pump ON");
                            break;


                        //ERROR 
                        default:
                            Debug.Log("Case error!");
                            break;
                    }
                    getSensorsStates();
                    Slower();
                }
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

    //Turn pump on off
    public void pumpSwitch(bool iconSensorState)
    {
        if (iconSensorState)
        {
            WriteToArduino("PUMPUP");
        }
        else
        {
            WriteToArduino("PUMPDOWN");
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
        bool newLedState = _lightScript.getLightState();
        bool newFanState = _tempIconScript.getFanState();
        bool newPumpState = _humidIconScript.getPumpState();

        if (oldLedState != newLedState)
        {
            lightSwitch(newLedState);
            oldLedState = newLedState;
        }
        if(oldFanState != newFanState)
        {
            fanSwitch(newFanState);
            oldFanState = newFanState;
        }
        if(oldPumpState != newPumpState)
        {
            pumpSwitch(newPumpState);
            oldPumpState = newPumpState;
        }
    }

    //Slower Checks
    IEnumerator Slower()
    {
        print(Time.time);
        yield return new WaitForSeconds(1);
    }
}


