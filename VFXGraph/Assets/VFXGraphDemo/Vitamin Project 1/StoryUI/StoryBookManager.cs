using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class StoryBookManager : MonoBehaviour
{

    public StoryBook m_StoryBook;

    [Header("Text Variables")]
    public TextMeshProUGUI m_QuestionText;
    public TextMeshProUGUI m_AnswerLeftText; // Index 0 -> m_StoryBook.m_BookPages[index].m_Answer[0];
    public TextMeshProUGUI m_AnswerRightText; // Index 1 -> m_StoryBook.m_BookPages[index].m_Answer[1];

    private int m_PageIndex = 0;


    private void Start()
    {
        if (m_StoryBook != null && m_QuestionText != null && m_AnswerLeftText != null && m_AnswerRightText != null)
        {
            LoadPage(0);
        }
        else {
            Debug.LogWarning("Fill the StoryBookManager variables in the inspector motherfucker"); 
        }
    }
    private void ClearText() {

        m_QuestionText.text = "";
        m_AnswerLeftText.text = "";
        m_AnswerRightText.text = "";

    }

    private void LoadPage(int index) {

        if (index < m_StoryBook.m_BookPages.Length && m_StoryBook.m_BookPages[index] != null)
        {

            ClearText(); // Clears the previous text

            if (m_StoryBook.m_BookPages[index].m_Question == "" || m_StoryBook.m_BookPages[index].m_Answer[0] == "" || m_StoryBook.m_BookPages[index].m_Answer[1] == "")
            {
                Debug.LogWarning("You filled all the BookPage scriptable object variables?");
            }

            //Show the new page text
            m_QuestionText.text = m_StoryBook.m_BookPages[index].m_Question;
            m_AnswerLeftText.text = m_StoryBook.m_BookPages[index].m_Answer[0];
            m_AnswerRightText.text = m_StoryBook.m_BookPages[index].m_Answer[1];

            //Update the page index
            m_PageIndex = index;

        }
        else {
            Debug.LogWarning("Page out of range or unasigned");
        }

    }

    public void LoadNextPage() {

        LoadPage(m_PageIndex + 1);
    
    }

}
