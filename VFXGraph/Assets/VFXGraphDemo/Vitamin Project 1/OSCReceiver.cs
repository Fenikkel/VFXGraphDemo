using UnityEngine;
using System.Collections;

// https://thomasfredericks.github.io/UnityOSC/

public class OSCReceiver : MonoBehaviour
{

    public OSC osc;
    private float m_LeftSensor = 0;
    private float m_RightSensor = 0;

    void Start()
    {
        osc.SetAddressHandler("/sensor", OnReceiveSensor);
        osc.SetAllMessageHandler(OnAnyMessageReceived);
    }


    void OnAnyMessageReceived(OscMessage message) {

        m_LeftSensor = message.GetFloat(0);
        m_RightSensor = message.GetFloat(1);
        //print("El index 0: " + message.GetFloat(0));
        //print("El index 1: " + message.GetFloat(1));


    }

    void OnReceiveSensor(OscMessage message)
    {
        float valorProximidad = message.GetFloat(0);

        //Debug.Log(valorProximidad);
    }

    public float GetLeftSensorValue() {

        return m_LeftSensor;
    
    }

    public float GetRightSensorValue()
    {

        return m_RightSensor;

    }

}