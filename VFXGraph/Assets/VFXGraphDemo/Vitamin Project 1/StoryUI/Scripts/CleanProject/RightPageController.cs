using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class RightPageController : MonoBehaviour
{
    [Header("Text Variables")]
    public TextMeshProUGUI m_QuestionText;
    public TextMeshProUGUI m_LeftAnswerText; // Index 0 -> m_StoryBook.m_BookPages[index].m_Answer[0];
    public TextMeshProUGUI m_RightAnswerText; // Index 1 -> m_StoryBook.m_BookPages[index].m_Answer[1];

    [Header("StoryBooks")]
    public StoryBook[] m_BookPicker; // For 2 to 6 customers
    public StoryBook[] m_FablesBooks; // 0 -> Libro Selva, 1 -> Movi Dick, 2 -> Alicia Maravillas, 3 -> Principito

    private StoryBook m_CurrentStoryBook; //It can ba a book picker or a fable

    private int m_RandomLeft = -1;
    private int m_RandomRight = -1;
    private RightPageType m_CurrentRightPageType = RightPageType.Reading;


    public void WritePage(string question, string leftAnswer, string rightAnswer) {

        m_QuestionText.text = question;
        m_LeftAnswerText.text = leftAnswer;
        m_RightAnswerText.text = rightAnswer;


    }

    public RightPageType LoadRightPage(int index) {

        //m_CurrentRightPageType = CheckRightPageType(index);

        if (index < m_CurrentStoryBook.m_BookPages.Length && m_CurrentStoryBook.m_BookPages[index] != null)
        {
            

            switch (m_CurrentRightPageType) {

                case RightPageType.Reading:

                    WritePage(m_CurrentStoryBook.m_BookPages[index].m_Question, "", "");
                    break;

                case RightPageType.TheEnd:

                    WritePage(m_CurrentStoryBook.m_BookPages[index].m_Question, "", "");
                    break;

                case RightPageType.TwoOptions:

                    WritePage(m_CurrentStoryBook.m_BookPages[index].m_Question, m_CurrentStoryBook.m_BookPages[index].m_Answer[0], m_CurrentStoryBook.m_BookPages[index].m_Answer[1]);
                    break;

                case RightPageType.FourOptions:

                    RollRandomAnswers();

                    WritePage(m_CurrentStoryBook.m_BookPages[index].m_Question, m_CurrentStoryBook.m_BookPages[index].m_Answer[m_RandomLeft], m_CurrentStoryBook.m_BookPages[index].m_Answer[m_RandomRight]);

                    //Don't reset the randoms, we use it later

                    break;


                default:
                    Debug.LogWarning("Page type not implemented");
                    break;
            
            
            }

        }
        else {

            Debug.LogWarning("Page unasigned");

        }

        return m_CurrentRightPageType;

    }

    public bool RandomizePage(int pageIndex) //returns true if the left option was chosen
    {
       
        if (m_CurrentStoryBook.m_BookPages[pageIndex].m_Question == "" || m_CurrentStoryBook.m_BookPages[pageIndex].m_Answer[0] == "" || m_CurrentStoryBook.m_BookPages[pageIndex].m_Answer[1] == "")
        {
            Debug.LogWarning("You filled all the BookPage scriptable object variables?");
        }

        System.Random rnd = new System.Random();

        int answerRandomized = rnd.Next(2); // 0 or 1 

        string allToghether = m_CurrentStoryBook.m_BookPages[pageIndex].m_Question + "\n" + m_CurrentStoryBook.m_BookPages[pageIndex].m_Answer[answerRandomized];

        WritePage(allToghether,"","");

        if (answerRandomized == 0) // we set the choice for save later in story manager
        {

            return true; //Left option was chosen

        }
        else
        {

            return false;

        }


    }

    public bool RandomizeAndAddPage(int pageIndex) //returns true if the left option was chosen
    {

        if (m_CurrentStoryBook.m_BookPages[pageIndex].m_Question == "" || m_CurrentStoryBook.m_BookPages[pageIndex].m_Answer[0] == "" || m_CurrentStoryBook.m_BookPages[pageIndex].m_Answer[1] == "")
        {
            Debug.LogWarning("You filled all the BookPage scriptable object variables?");
        }

        System.Random rnd = new System.Random();

        int answerRandomized = rnd.Next(2); // 0 or 1 

        m_QuestionText.text += "\n" + m_CurrentStoryBook.m_BookPages[pageIndex].m_Question + "\n" + m_CurrentStoryBook.m_BookPages[pageIndex].m_Answer[answerRandomized];


        if (answerRandomized == 0) // we set the choice for save later in story manager
        {

            return true; //Left option was chosen

        }
        else
        {

            return false;

        }


    }

    private void RollRandomAnswers()
    {
        //Es muy importante que las respuestas esten bien ordenadas en el Scriptable Object: 0 -> Libro Selva, 1 -> Movi Dick, 2 -> Alicia Maravillas, 3 -> Principito

        System.Random rnd = new System.Random();

        m_RandomLeft = rnd.Next(4); // 0 to 3 
        m_RandomRight = rnd.Next(4);

        while (m_RandomRight == m_RandomLeft)
        {
            m_RandomRight = rnd.Next(4);
        }

        Debug.Log("Random Left Option: " + m_RandomLeft);
        Debug.Log("Random right Option: " + m_RandomRight);

    }

    private RightPageType CheckRightPageType(int index)
    {

        if (m_CurrentStoryBook.m_BookPages[index].m_Answer.Length == 0) // NO HAY RESPUESTAS (Solo texto)
        {
            return RightPageType.Reading;
        }

        else if (m_CurrentStoryBook.m_BookPages[index].m_Answer.Length == 2) // 2 RESPUESTAS
        {
            return RightPageType.TwoOptions;
        }

        else if (m_CurrentStoryBook.m_BookPages[index].m_Answer.Length == 4) // 4 RESPUESTAS (Enseñamos 2 respuestas random)
        {

            return RightPageType.FourOptions;

        }
        else {

            Debug.LogError("This number of answers per question not implemented");
            return RightPageType.Reading;

        }
    }

    public void ClearAllText() {

        m_QuestionText.text = "";
        m_LeftAnswerText.text = "";
        m_RightAnswerText.text = "";

    }



    #region setters and getters


    public void SetCurrentBookPicker(int bookPickerIndex) {

        m_CurrentStoryBook = m_BookPicker[bookPickerIndex];

    }

    public StoryBook GetCurrentStoryBook()
    {

        return m_CurrentStoryBook;

    }

    public void SetCurrentFableBook(int fableBookIndex)
    {

        m_CurrentStoryBook = m_FablesBooks[fableBookIndex];

    }

    public RightPageType GetCurrentRightPageType() {

        return m_CurrentRightPageType;
    
    }
    public void SetCurrentRightPageType(RightPageType pageType)
    {

        m_CurrentRightPageType = pageType;

    }

    public int GetRandomLeft() {

        return m_RandomLeft;

    }

    public int GetRandomRight()
    {

        return m_RandomRight;

    }

    #endregion

}
