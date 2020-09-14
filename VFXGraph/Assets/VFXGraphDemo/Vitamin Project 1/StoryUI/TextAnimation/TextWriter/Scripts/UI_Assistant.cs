
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

//Falta la funcion para pasar de golpe el texto: https://youtu.be/ZVh4nH8Mayg?t=1154
//Falta la funcion de sonar mientras se escribe

public class UI_Assistant : MonoBehaviour {

    


    // [SerializeField] private TextWriter m_TextWriter; // No longer needed because we are working with instances
    private TextMeshProUGUI m_MessageText;


    private void Awake() {
        m_MessageText = transform.Find("Message").Find("MessageText").GetComponent<TextMeshProUGUI>();
        
        
    }

    private void Start() {

        //m_MessageText.text = "Hollo";
        //m_TextWriter.AddWriter(m_MessageText, "Hello world", 1f);
        //m_TextWriter.AddWriter(m_MessageText, "01234567890123456789", .1f, false);
        //m_TextWriter.AddWriter(m_MessageText, "01234567890123456789", .1f, true);
        TextWriter.AddWriter_Static(m_MessageText, "01234567890123456789", .1f, true);
    }

}
