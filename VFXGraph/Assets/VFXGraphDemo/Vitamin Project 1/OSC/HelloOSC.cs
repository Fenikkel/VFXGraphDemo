using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelloOSC : MonoBehaviour
{
    public OSC osc;

    public int[] m_Choices = { 1, 0, 1, 1, 0, 0 };
    public bool m_Alerta = false;
    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(transform.position, Vector3.up, 20 * Time.deltaTime);

        OscMessage message = new OscMessage();

        message.address = "/HelloOSC";
        message.values.Add(transform.rotation.y);
        message.values.Add(m_Choices[0]);
        message.values.Add(m_Choices[1]);
        message.values.Add(m_Choices[2]);
        message.values.Add(m_Choices[3]);
        message.values.Add(m_Choices[4]);
        message.values.Add(m_Choices[5]);
        message.values.Add(m_Alerta);
        //message.values.Add(false);
        osc.Send(message);
    }
}
