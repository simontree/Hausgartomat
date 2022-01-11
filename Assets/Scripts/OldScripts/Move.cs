using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO.Ports;
using UnityEngine.EventSystems;

public class Move : MonoBehaviour, IPointerClickHandler
{
    private SerialPort sp = new SerialPort("COM11", 9600);
    public bool ledLit = false;
    public bool correctLit = false;
    private int arduinoByte = -1;
    public GameObject lightIcon;
    public GameObject tempIcon;
    //public Light light;

    //Fan and Temp
    public bool fanOn = false;
    public bool correctTemp = false;

    // Start is called before the first frame update
    void Start()
    {
        //gameObject.GetComponent<Renderer>().material.color = Color.green;
        sp.Open();
        sp.ReadTimeout = 100;
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        /*if (Input.GetKeyDown(KeyCode.Space))
        {
            ledOnOff();
        }*/

        if (Input.GetKeyDown(KeyCode.C))
        {
            sp.Close();
        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            sp.Open();
        }

        if (sp.IsOpen)
        {
            try
            {
                arduinoByte = sp.ReadByte();
                Debug.Log(arduinoByte);
                switch (arduinoByte)
                {
                    case 1: // Too much light, Led + ambient 
                        correctLit = false;
                        ledLit = true;
                        gameObject.GetComponent<Renderer>().material.color = Color.red;
                        break;
                    case 2: // Too much light, Only ambient
                        correctLit = false;
                        ledLit = false;
                        gameObject.GetComponent<Renderer>().material.color = Color.red;
                        break;
                    case 3: // Good enough light, ambient + led
                        correctLit = true;
                        ledLit = true;
                        gameObject.GetComponent<Renderer>().material.color = Color.green;
                        break;
                    case 4: // Good enough light, only ambient
                        correctLit = true;
                        ledLit = false;
                        gameObject.GetComponent<Renderer>().material.color = Color.green;
                        break;
                    case 5: // Too little light, ambient only
                        correctLit = false;
                        ledLit = false;
                        gameObject.GetComponent<Renderer>().material.color = Color.black;
                        break;
                    case 6: // Too little light, ambient + ledOn (busted, obstructed)
                        correctLit = false;
                        ledLit = true;
                        gameObject.GetComponent<Renderer>().material.color = Color.black;
                        break;
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
                        //Debug.Log("Case error!");
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

    public void ledOnOff()
    {
        if (!ledLit)
        {
            WriteToArduino("LEDUP");
            ledLit = true;
        }
        else
        {
            WriteToArduino("LEDDOWN");
            ledLit = false;
        }
    }

    public void OnMouseDown()
    {
        if (!ledLit)
        {
            WriteToArduino("LEDUP");
            ledLit = true;
        }
        else
        {
            WriteToArduino("LEDDOWN");
            ledLit = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        
    }
}
