using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IRSensorController : MonoBehaviour
{
    private bool m_SensorsActive = false;

    [Header("Controllers")]
    public BookManager m_BookManager;
    public OSCReceiver m_OSCReceiver;

    [Header("Sliders")]
    public Slider m_LeftSlider;
    public Slider m_RightSlider;
    


    private void Update()
    {
        if (m_SensorsActive) {

            float leftValue = m_OSCReceiver.GetLeftSensorValue();
            float rightValue = m_OSCReceiver.GetRightSensorValue();


            m_RightSlider.value = rightValue / 100.0f;
            m_LeftSlider.value = leftValue / 100.0f;

            if (leftValue == 100.0f) {

                Debug.Log("LEFT SENSOR");
                m_BookManager.OnOptionSelected(true);
                ActiveSensors(false);

            }
            else if (rightValue == 100.0f) {

                Debug.Log("RIGHT SENSOR");
                m_BookManager.OnOptionSelected(false);
                ActiveSensors(false);
            }
        
        
        }
    }

    public void ActiveSensors(bool active) {

        m_SensorsActive = active;

        if (active)
        {

            Debug.Log("SENSOR ACTIVE");

        }
        else
        {
            Debug.Log("SENSOR DISABLED");
        }

    }



}
