
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;

public class StoryBookManager : MonoBehaviour
{
    public StoryBook m_BookPicker;
    public StoryBook[] m_StoryBooks; // 0 -> Libro Selva, 1 -> Movi Dick, 2 -> Alicia Maravillas, 3 -> Principito

    [Header("Text Variables")]
    public TextMeshProUGUI m_QuestionText;
    public TextMeshProUGUI m_AnswerLeftText; // Index 0 -> m_StoryBook.m_BookPages[index].m_Answer[0];
    public TextMeshProUGUI m_AnswerRightText; // Index 1 -> m_StoryBook.m_BookPages[index].m_Answer[1];

    [Header("UI Elements")]
    public GameObject RightButton;
    public GameObject LeftButton;

    private int m_QuestionIndex = 0; // En realidad va de 1 a n (el 0 es para cargar la primera pagina y luego ponerle el indice 1)
    //private int m_CurrentChoiceIndex = -1;
    private StoryBook m_CurrentBook;
    private int m_ChosenBook = 0;// 1 -> Libro Selva, 2 -> Movi Dick, 3 -> Alicia Maravillas, 4 -> Principito
    private int[] m_Choices = new int[6]; // -1 = empty



    private void Start()
    {
        ClearChoices();
        m_CurrentBook = m_BookPicker;

        if (m_CurrentBook != null && m_QuestionText != null && m_AnswerLeftText != null && m_AnswerRightText != null)
        {
            StartBook();
        }
        else {
            Debug.LogWarning("Fill the StoryBookManager variables in the inspector motherfucker"); 
        }
    }

    private void ClearChoices() {

        for (int i = 0; i < m_Choices.Length; i++)
        {
            m_Choices[i] = -1;
        }

    }

    private void ClearText() {

        m_QuestionText.text = "";
        m_AnswerLeftText.text = "";
        m_AnswerRightText.text = "";

    }

    private void ShowButtons(bool active) {

        RightButton.SetActive(active);
        LeftButton.SetActive(active);

    }

    private void ShowNextButton(bool active)
    {

        RightButton.SetActive(active);
        LeftButton.SetActive(false);

    }

    private void LoadPage(int index) {

        if (index < m_CurrentBook.m_BookPages.Length && m_CurrentBook.m_BookPages[index] != null)
        {

            ClearText(); // Clears the previous text

            if (m_CurrentBook.m_BookPages[index].m_Answer.Length == 0)
            {

                if (m_CurrentBook.m_BookPages[index].m_Question == "")
                {
                    Debug.LogWarning("Fill the question");
                }

                m_QuestionText.text = m_CurrentBook.m_BookPages[index].m_Question;
                m_AnswerRightText.text = "Siguiente";
                ShowNextButton(true);

            }
            else if (m_CurrentBook.m_BookPages[index].m_Answer.Length == 2)
            {

                if (m_CurrentBook.m_BookPages[index].m_Question == "" || m_CurrentBook.m_BookPages[index].m_Answer[0] == "" || m_CurrentBook.m_BookPages[index].m_Answer[1] == "")
                {
                    Debug.LogWarning("You filled all the BookPage scriptable object variables?");
                }

                //Show the new page text
                m_QuestionText.text = m_CurrentBook.m_BookPages[index].m_Question;
                m_AnswerLeftText.text = m_CurrentBook.m_BookPages[index].m_Answer[0];
                m_AnswerRightText.text = m_CurrentBook.m_BookPages[index].m_Answer[1];

                ShowButtons(true);

            }
            else {

                Debug.LogError("More than two answers per question not implemented");
                m_QuestionText.text = "More than two answers per question not implemented";

            }

            //Update the page index
            m_QuestionIndex++;

        }
        else if (index >= m_CurrentBook.m_BookPages.Length)
        {

            //THE END
            Debug.Log("The end: \n");

            if (m_CurrentBook == m_BookPicker)
            {

                Debug.Log("Chosen book: " + m_ChosenBook);

                if (m_ChosenBook < m_StoryBooks.Length && m_StoryBooks[m_ChosenBook] != null) {
                    m_CurrentBook = m_StoryBooks[m_ChosenBook];
                }

                StartBook();    

            }
            else {

                for (int i = 0; i < m_Choices.Length; i++)
                {

                    Debug.Log(m_Choices[i]);

                }

            }

        }
        else
        { //m_StoryBook.m_BookPages[index] == null

            Debug.LogWarning("Page unasigned");

            //Update the page index
            m_QuestionIndex++;

        }

    }

    public void LoadNextPage(bool left) {

        //ARREGLAR INDICE 2 (Sale 4)
        if (m_CurrentBook == m_BookPicker && m_QuestionIndex == 2) //Alerta, trozo cutre palero
        {
            if (left)
            {
                m_ChosenBook += 1;
                print("+1");
            }
            else {
                m_ChosenBook += 2;
                print("+2");
            }

        }
        else if(m_CurrentBook == m_BookPicker && m_QuestionIndex == 3)
        {

            if (m_ChosenBook == 1)
            {

                if (left)
                {
                    m_ChosenBook = 0; //Libro selva
                }
                else {
                    m_ChosenBook = 1; //Movie dick
                }

            }
            if (m_ChosenBook == 2)
            {

                if (left)
                {
                    m_ChosenBook = 2; //Alici
                }
                else
                {
                    m_ChosenBook = 3; //Principito
                }

            }

            print("Chosen book: " + m_ChosenBook);

        }

        if (m_CurrentBook.m_BookPages[m_QuestionIndex].m_Answer.Length == 0) {

            LoadPage(m_QuestionIndex * 2);

        }
        else if (left)
        {

            LoadPage(m_QuestionIndex * 2);
        }
        else {

            LoadPage(m_QuestionIndex * 2 + 1);
        }
        
    }

    private void StartBook() {
        //RECUERDA ENVIAR LAS OPCIONES ANTES SI ES LA SEGUNDA FASE
        ShowButtons(false);
        ClearText();
        ClearChoices();
        m_QuestionIndex = 0;
        LoadPage(0);
    }


    //public void SaveOption(int indexToSave)
    //{ // if true, the right option was chose, if not, was the left 

    //    if (m_PageIndex < m_StoryBook.m_BookPages.Length)
    //    {

    //        if (right)
    //        {

    //            m_Choices[m_PageIndex] = m_PageIndex * 2 + 1;

    //        }
    //        else
    //        {

    //            m_Choices[m_PageIndex] = m_PageIndex * 2;

    //        }

    //    }
    //}


}
