using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.EventSystems;

public class TempIconScript : MonoBehaviour, IPointerClickHandler
{
    private SerialPort sp = new SerialPort("COM11", 9600);
    public bool fanOn = false;
    public bool correctTemp = false;
    private int arduinoByte = -1;
    //public Light light;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<Renderer>().material.color = Color.green;
        //sp.Open();
        sp.ReadTimeout = 100;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (sp.IsOpen)
        {
            try
            {
                arduinoByte = sp.ReadByte();
                Debug.Log(arduinoByte);
                switch (arduinoByte)
                {
                    case 7: // Tôo hot, fan on
                        correctTemp = false;
                        fanOn = true;
                        gameObject.GetComponent<Renderer>().material.color = Color.red;
                        break;
                    case 8: // Too hot, fan off
                        correctTemp = false;
                        fanOn = false;
                        gameObject.GetComponent<Renderer>().material.color = Color.red;
                        break;
                    case 9: // Good, fan on
                        correctTemp = true;
                        fanOn = true;
                        gameObject.GetComponent<Renderer>().material.color = Color.green;
                        break;
                    case 10: // Good, fan off
                        correctTemp = true;
                        fanOn = false;
                        gameObject.GetComponent<Renderer>().material.color = Color.green;
                        break;
                    case 11: // Too cold, fan on
                        correctTemp = false;
                        fanOn = false;
                        gameObject.GetComponent<Renderer>().material.color = Color.black;
                        break;
                    case 12: // Too cold, fan off
                        correctTemp = false;
                        fanOn = true;
                        gameObject.GetComponent<Renderer>().material.color = Color.black;
                        break;
                    default:
                        Debug.Log("Case error! Fan");
                        break;
                }
                //Debug.Log(sp.ReadByte());
            }
            catch (System.Exception)
            {

            }

        }
    }
    private void WriteToArduino(string message)
    {
        sp.WriteLine(message);
        sp.BaseStream.Flush();
    }

    public void fanOnOff()
    {
        if (!fanOn)
        {
            WriteToArduino("FANUP");
            fanOn = true;
        }
        else
        {
            WriteToArduino("FANDOWN");
            fanOn = false;
        }
    }

    public void OnMouseDown()
    {
        
        if (!fanOn)
        {
            WriteToArduino("FANON");
            fanOn = true;
        }
        else
        {
            WriteToArduino("LEDDOWN");
            fanOn = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log(eventData.pointerCurrentRaycast.gameObject.name);
        throw new System.NotImplementedException();
    }
}


/*
 No funcionaria abrir dos puertos, tiene que pasar en el mismo Move (Cambiarle nombre)

 */