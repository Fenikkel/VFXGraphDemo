
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.AccessControl;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

//TextWriter it's a dialog
public class TextWriter : MonoBehaviour
{

    private static TextWriter m_Instance; // With this, we dont need a reference on UI_Assistant of the TextWritter
    private List<TextWriterSingle> m_TextWriterSingleList; // List of dialogs

    private void Awake()
    {
        m_Instance = this;
        m_TextWriterSingleList = new List<TextWriterSingle>();
    }

    public static void AddWriter_Static(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
    {

        m_Instance.AddWriter(uiText, textToWrite, timePerCharacter, invisibleCharacters);
    }

        private void AddWriter(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters)
    {
        m_TextWriterSingleList.Add( new TextWriterSingle(uiText, textToWrite, timePerCharacter, invisibleCharacters));
    }

    private void Update()
    {
        //Debug.Log("Number of dialogs active: " + m_TextWriterSingleList.Count);

        //Call the Update of each instance on the list
        for (int i = 0; i < m_TextWriterSingleList.Count; i++)
        {
            bool instanceEnded = m_TextWriterSingleList[i].Update();

            //If the instance has ended, we remove it (destroy it). So we don't have zombie instances wasting performance.
            if (instanceEnded) {

                m_TextWriterSingleList.RemoveAt(i);
                i--;
            
            }
        }
    }





    public class TextWriterSingle { //Nest class

        private TextMeshProUGUI m_UiText;
        private string m_TextToWrite;
        private int m_CharacterIndex;
        private float m_TimePerCharacter;
        private float m_Timer;
        private bool m_InvisibleCharacters; // make the characters spawn at the correct side instead of in the center

        /*Represents a single TextWriter instance. This is useful when we want to have multiple dialogs working at the same time*/

        public TextWriterSingle(TextMeshProUGUI uiText, string textToWrite, float timePerCharacter, bool invisibleCharacters) //It's a constructor
        {
            m_UiText = uiText;
            m_TextToWrite = textToWrite;
            m_TimePerCharacter = timePerCharacter;
            m_InvisibleCharacters = invisibleCharacters;
            m_CharacterIndex = 0;
        }

        // Returns true when the text is complete
        public bool Update()
        {
            m_Timer -= Time.deltaTime;
            while (m_Timer <= 0f) // When the time is out // If we put an If instead of a While, when we have a FPS drop, it will write slower
            {
                // Display next character
                m_Timer += m_TimePerCharacter;
                m_CharacterIndex++;
                string text = m_TextToWrite.Substring(0, m_CharacterIndex); //We get the next letter to animate


                if (m_InvisibleCharacters)
                {

                    //we put complete the string with the other characters but with the alpha color to 0
                    text += "<color=#00000000>" + m_TextToWrite.Substring(m_CharacterIndex) + "</color>"; //<color=#RRGGBBAA>

                }
                m_UiText.text = text;

            }

            if (m_CharacterIndex >= m_TextToWrite.Length) //Entire string displayed, we reached the end
            {
                m_UiText = null;
                return true;
            }

            return false; // we still don't completed the string of the dialog
        }

    }

}


