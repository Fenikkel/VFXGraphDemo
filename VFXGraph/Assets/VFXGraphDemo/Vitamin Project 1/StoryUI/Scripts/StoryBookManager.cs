
using System;
using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class StoryBookManager : MonoBehaviour
{
    public StoryBook[] m_BookPicker; // For 2 to 6 customers
    public StoryBook[] m_StoryBooks; // 0 -> Libro Selva, 1 -> Movi Dick, 2 -> Alicia Maravillas, 3 -> Principito
    public OSC m_OSC;
    public AnimationsController m_AnimationsController;

    [Header("Text Variables")]
    public TextMeshProUGUI m_QuestionText;
    public TextMeshProUGUI m_AnswerLeftText; // Index 0 -> m_StoryBook.m_BookPages[index].m_Answer[0];
    public TextMeshProUGUI m_AnswerRightText; // Index 1 -> m_StoryBook.m_BookPages[index].m_Answer[1];

    [Header("UI Elements")]
    public GameObject m_RightButtonGO;
    public GameObject m_LeftButtonGO;
    public GameObject m_NextButtonGO;

    [Header("Canvas group (Panels)")]
    public CanvasGroup RightAnswer;
    public CanvasGroup LeftAnswer;

    [Header("Phases")]
    private bool m_CustomersSelectorPhase = false; //Select the customers
    private bool m_FirstPhase = false; //Customers select the book
    private bool m_SecondPhase = false; //Story choices

    [Header("Data Saved")]
    private int m_ChosenBook = -1; // 0 -> Libro Selva, 1 -> Movi Dick, 2 -> Alicia Maravillas, 3 -> Principito
    private int[] m_Choices = new int[5]; // -1 = empty
    private int m_NumberOfExplorers = 0;


    [Header("Variables Mix")]
    private int m_QuestionIndex = 0; // From 0 to n 
    private bool m_InputActive = false;
    private StoryBook m_CurrentBookPicker;
    private StoryBook m_CurrentStoryBook;
    private int m_RandomLeft = -1;
    private int m_RandomRight = -1;
    private bool m_LeftOptionChosen = false;
    private int m_TimesLooped = 0;



    private void ResetVariables()
    {

        m_CurrentBookPicker = null;
        m_CurrentStoryBook = null;
        m_QuestionIndex = 0;
        m_ChosenBook = -1;
        m_NumberOfExplorers = 0;
        m_RandomLeft = -1;
        m_RandomRight = -1;
        ClearChoices(); // m_Choices
        ClearText();

    }

    private void Start()
    {
        StartCustomersSelectorsPhase();
    }

    private void Update()
    {
        CheckInput();
    }


    private void StartCustomersSelectorsPhase() {

        //Reset 
        ResetVariables();
        HideUI(true);

        //Change phase
        m_CustomersSelectorPhase = true;
        m_FirstPhase = false;
        m_SecondPhase = false;

        //Active CustomerSelector 
        //TextWriter.AddWriter_Static(m_QuestionText, "Selecciona número de exploradores por favor", .05f, true);
        m_QuestionText.text = "Selecciona número de exploradores por favor";
        m_AnimationsController.TriggerSmoke();

        StartCoroutine(ActiveInputWhenSmokeEnds());    
    
    }

    IEnumerator ActiveInputWhenSmokeEnds() {

        yield return new WaitForSeconds(1);

        while (m_AnimationsController.IsSmokePlaying() == true)
        {
            yield return null;
        }
        m_InputActive = true;
    }

    private void StartFirstPhase() // When the customers select the book
    {

        m_InputActive = false;

        //Change phase
        m_CustomersSelectorPhase = false;
        m_FirstPhase = true;
        m_SecondPhase = false;

        if (m_CurrentBookPicker != null && m_QuestionText != null && m_AnswerLeftText != null && m_AnswerRightText != null)
        {
            OptionSelected(false); //or true we don't care
        }
        else
        {
            Debug.LogWarning("Fill the StoryBookManager variables in the inspector motherfucker");
        }

    }

    private void StartSecondPhase() {

        //Change phase
        m_CustomersSelectorPhase = false;
        m_FirstPhase = false;
        m_SecondPhase = true;
        m_QuestionIndex = 0;

        if (m_ChosenBook >= 0 && m_ChosenBook <= 3) { //Si se ha elegido mal el libro esto tira para adelante igualmente

            m_CurrentStoryBook = m_StoryBooks[m_ChosenBook];
            OptionSelected(false); //or true, we don't care

        }
    }

    IEnumerator FadeOutButtons() {

        if (m_RightButtonGO.activeSelf && m_LeftButtonGO.activeSelf)
        {
            m_AnimationsController.FadeOutButtons();
        }
        else if (m_RightButtonGO.activeSelf)
        {

            m_AnimationsController.FadeOutNextButton();

        }
        else { 
        
        }


        yield return new WaitForSeconds(0.25f);

        while (m_AnimationsController.IsRightAnswerPanelFadingOut() == true) // del right botton
        {
            yield return null;
        }

        PhaseManager();

    }

    public void OptionSelected(bool left) {

        m_LeftOptionChosen = left;

        StartCoroutine(FadeOutButtons());

    }

    private void PhaseManager() {

        if (m_CustomersSelectorPhase)
        {

            print("Nothing");
        }

        else if (m_FirstPhase)
        {

            BookPickerManager();

        }

        else if (m_SecondPhase)
        {

            StoryManager();

        }

    }



    private void BookPickerManager() {

        #region 2 Customers

        if (m_NumberOfExplorers == 2)
        {

            switch (m_QuestionIndex)
            {

                case 0:

                    LoadPage(m_QuestionIndex * 2, m_CurrentBookPicker); 
                    break;

                case 1:

                    LoadPage(m_QuestionIndex * 2, m_CurrentBookPicker); 
                    break;

                case 2:

                    if (m_LeftOptionChosen)
                    {
                        m_ChosenBook = m_RandomLeft;
                        print("Chosen Book: " + m_ChosenBook);
                    }
                    else {

                        m_ChosenBook = m_RandomRight;
                        print("Chosen Book: " + m_ChosenBook);

                    }

                    StartSecondPhase();

                    break;

                default:
                    Debug.LogWarning("question index out of range?");
                    break;

            }

        }

        #endregion

        #region 3 to 6 explorers

        if (m_NumberOfExplorers > 2 && m_NumberOfExplorers <= 6)
        {
            //Alerta, trozo cutre palero.

            switch (m_QuestionIndex) {

                case 0:

                    LoadPage(m_QuestionIndex * 2, m_CurrentBookPicker);
                    break;

                case 1:

                    LoadPage(m_QuestionIndex * 2, m_CurrentBookPicker);
                    break;

                case 2:

                    if (m_LeftOptionChosen)
                    {
                        m_ChosenBook = -1;
                        LoadPage(m_QuestionIndex * 2, m_CurrentBookPicker);
                    }
                    else 
                    {
                        m_ChosenBook = -2;
                        LoadPage(m_QuestionIndex * 2 + 1, m_CurrentBookPicker);
                    }
                    break;

                case 3:

                    if (m_ChosenBook == -1)
                    {

                        if (m_LeftOptionChosen)
                        {
                            m_ChosenBook = 0; //Libro selva
                        }
                        else
                        {
                            m_ChosenBook = 1; //Movie dick
                        }

                    }
                    if (m_ChosenBook == -2)
                    {

                        if (m_LeftOptionChosen)
                        {
                            m_ChosenBook = 2; //Alici
                        }
                        else
                        {
                            m_ChosenBook = 3; //Principito
                        }

                    }

                    StartSecondPhase();

                    print("Chosen book: " + m_ChosenBook);
                    break;

                default:

                    Debug.LogWarning("question index out of range?");
                    break;

            }

        }

        #endregion
    }

    private void StoryManager() {

        #region 2 Customers

        if (m_NumberOfExplorers == 2) {

            switch (m_QuestionIndex) {

                case 0:

                    RandomizePage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]); // or 1 we don't care
                    break;

                case 1:
                    
                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        LoadPage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 2:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        RandomizePage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        RandomizePage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 3: //SOBRA?

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        RandomizePage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        RandomizePage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 4:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);

                    }

                    LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    break;

                case 5: //The end

                    Debug.Log("The end of the second phase: \n");

                    for (int i = 0; i < m_Choices.Length; i++)
                    {

                        Debug.Log(m_Choices[i]);

                    }

                    //ENVIAR ANTES LOS DATOS A PABLO
                    SendAnswersViaOSC();
                    StartCustomersSelectorsPhase();

                    break;

                default:
                    Debug.LogWarning("question index out of range?");
                    break;
            
            }
        
        }

        #endregion

        #region 3 Customers

        if (m_NumberOfExplorers == 3)
        {

            switch (m_QuestionIndex)
            {

                case 0:

                    LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]); 
                    break;

                case 1:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        RandomizePage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        RandomizePage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 2:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        RandomizePage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        RandomizePage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 3: //SOBRA?

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        RandomizePage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        RandomizePage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 4:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);

                    }

                    LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    break;

                case 5: //The end

                    Debug.Log("The end of the second phase: \n");

                    for (int i = 0; i < m_Choices.Length; i++)
                    {

                        Debug.Log(m_Choices[i]);

                    }

                    //ENVIAR ANTES LOS DATOS A PABLO
                    SendAnswersViaOSC();
                    StartCustomersSelectorsPhase();

                    break;

                default:
                    Debug.LogWarning("question index out of range?");
                    break;

            }

        }

        #endregion
        #region 4 Customers

        if (m_NumberOfExplorers == 4)
        {

            switch (m_QuestionIndex)
            {

                case 0:

                    LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);
                    break;

                case 1:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        LoadPage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 2:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        RandomizePage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        RandomizePage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 3: //SOBRA?

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        RandomizePage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        RandomizePage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 4:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);

                    }

                    LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    break;

                case 5: //The end

                    Debug.Log("The end of the second phase: \n");

                    for (int i = 0; i < m_Choices.Length; i++)
                    {

                        Debug.Log(m_Choices[i]);

                    }

                    //ENVIAR ANTES LOS DATOS A PABLO
                    SendAnswersViaOSC();
                    StartCustomersSelectorsPhase();

                    break;

                default:
                    Debug.LogWarning("question index out of range?");
                    break;

            }

        }


        #endregion

        #region 5 Customers

        if (m_NumberOfExplorers == 5)
        {

            switch (m_QuestionIndex)
            {

                case 0:

                    LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);
                    break;

                case 1:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        LoadPage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 2:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        LoadPage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 3: //SOBRA?

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        RandomizePage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        RandomizePage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 4:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);

                    }

                    LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    break;

                case 5: //The end

                    Debug.Log("The end of the second phase: \n");

                    for (int i = 0; i < m_Choices.Length; i++)
                    {

                        Debug.Log(m_Choices[i]);

                    }

                    //ENVIAR ANTES LOS DATOS A PABLO
                    SendAnswersViaOSC();
                    StartCustomersSelectorsPhase();

                    break;

                default:
                    Debug.LogWarning("question index out of range?");
                    break;

            }

        }

        #endregion

        #region 6 Customers

        if (m_NumberOfExplorers == 6)
        {

            switch (m_QuestionIndex)
            {

                case 0:

                    LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);
                    break;

                case 1:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        LoadPage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 2:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        LoadPage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 3: //SOBRA?

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);
                        LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);
                        LoadPage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

                    }

                    break;

                case 4:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        SaveChoice(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        SaveChoice(m_QuestionIndex * 2 + 1);

                    }

                    LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

                    break;

                case 5: //The end

                    Debug.Log("The end of the second phase: \n");

                    for (int i = 0; i < m_Choices.Length; i++)
                    {

                        Debug.Log(m_Choices[i]);

                    }

                    //ENVIAR ANTES LOS DATOS A PABLO
                    SendAnswersViaOSC();
                    StartCustomersSelectorsPhase();

                    break;

                default:
                    Debug.LogWarning("question index out of range?");
                    break;

            }

        }

        #endregion

    }

    private void RandomizePage(int pageIndex, StoryBook book) {

        m_QuestionIndex++;

        if (book.m_BookPages[pageIndex].m_Question == "" || book.m_BookPages[pageIndex].m_Answer[0] == "" || book.m_BookPages[pageIndex].m_Answer[1] == "")
        {
            Debug.LogWarning("You filled all the BookPage scriptable object variables?");
        }

        ClearCanvas();

        //Show the new page text
        m_QuestionText.text = book.m_BookPages[pageIndex].m_Question;

        System.Random rnd = new System.Random();

        int answerRandomized = rnd.Next(2); // 0 or 1 

        m_QuestionText.text += "\n" + book.m_BookPages[pageIndex].m_Answer[answerRandomized];
        //SaveChoice(pageIndex + answerRandomized); //page index it's already QuestinIndex * 2

        if (answerRandomized == 0) // we set the choice for save later in story manager
        {

            m_LeftOptionChosen = true;

        }
        else {

            m_LeftOptionChosen = false;

        }

        ShowNextButton();

    }

    private void SaveChoice(int indexToSave) 
    {

        m_Choices[m_QuestionIndex] = indexToSave;

    }


    private void LoadPage(int index, StoryBook storyBook) {

        m_QuestionIndex++;

        if (index < storyBook.m_BookPages.Length && storyBook.m_BookPages[index] != null)
        {

            ClearCanvas();

            if (storyBook.m_BookPages[index].m_Answer.Length == 0) // NO HAY RESPUESTAS (Solo texto)
            {

                if (storyBook.m_BookPages[index].m_Question == "")
                {
                    Debug.LogWarning("Fill the question");
                }

                m_QuestionText.text = storyBook.m_BookPages[index].m_Question;
                m_AnimationsController.TriggerSmoke();
                ShowNextButton();

            }
            else if (storyBook.m_BookPages[index].m_Answer.Length == 2) // 2 RESPUESTAS
            {

                if (storyBook.m_BookPages[index].m_Question == "" || storyBook.m_BookPages[index].m_Answer[0] == "" || storyBook.m_BookPages[index].m_Answer[1] == "")
                {
                    Debug.LogWarning("You filled all the BookPage scriptable object variables?");
                }

                //Show the new page text
                m_QuestionText.text = storyBook.m_BookPages[index].m_Question;

                m_AnswerLeftText.text = storyBook.m_BookPages[index].m_Answer[0];
                m_AnswerRightText.text = storyBook.m_BookPages[index].m_Answer[1];
                m_AnimationsController.TriggerSmoke();
                ShowChoiceButtons(true);

            }
            else if (storyBook.m_BookPages[index].m_Answer.Length == 4) // 4 RESPUESTAS (Enseñamos 2 respuestas random)
            {

                if (storyBook.m_BookPages[index].m_Question == "" || storyBook.m_BookPages[index].m_Answer[0] == "" || storyBook.m_BookPages[index].m_Answer[1] == "" || storyBook.m_BookPages[index].m_Answer[2] == "" || storyBook.m_BookPages[index].m_Answer[3] == "")
                {
                    Debug.LogWarning("You filled all the BookPage scriptable object variables?");
                }

                RollRandomAnswers();

                //Show the new page text
                m_QuestionText.text = storyBook.m_BookPages[index].m_Question;
                m_AnswerLeftText.text = storyBook.m_BookPages[index].m_Answer[m_RandomLeft];
                m_AnswerRightText.text = storyBook.m_BookPages[index].m_Answer[m_RandomRight];
                m_AnimationsController.TriggerSmoke();
                ShowChoiceButtons(true);
                

                //Debug.LogError("Still not implemented");

            } 
            else
            {

                Debug.LogError("This number of answers per question not implemented");
                m_QuestionText.text = "This number of answers per question not implemented";

            }


        }
        else if (index >= storyBook.m_BookPages.Length) // Whe completed the book, we go to the next phase or reset
        {

            //THE END


            if (m_FirstPhase) // If we were in the phase were the customer selects the book
            {
                Debug.Log("The end of the first phase: \n");
                StartSecondPhase();

                //if (m_ChosenBook < m_StoryBooks.Length && m_StoryBooks[m_ChosenBook] != null)
                //{
                //    //PASAR a CurrentStoryBook!!!
                //    m_CurrentBookPicker = m_StoryBooks[m_ChosenBook];
                //}
                //else
                //{

                //    Debug.LogError("Somethings goes wrong. We won't pass to the SecondPhase");
                //}

                //StartBookPicker();

            }
            else if (m_SecondPhase)
            {

                Debug.Log("The end of the second phase: \n");

                for (int i = 0; i < m_Choices.Length; i++)
                {

                    Debug.Log(m_Choices[i]);

                }

                StartCustomersSelectorsPhase();

            }
            else {

                Debug.LogError("Update correctly the phase variables");
            
            }

        }
        else
        { //m_StoryBook.m_BookPages[index] == null

            Debug.LogWarning("Page unasigned");

        }

    }

    private void RollRandomAnswers()
    {
        //Es muy importante que las respuestas esten bien ordenadas en el Scriptable Object: 0 -> Libro Selva, 1 -> Movi Dick, 2 -> Alicia Maravillas, 3 -> Principito

        System.Random rnd = new System.Random();

        m_RandomLeft = rnd.Next(4); // 0 to 3 
        m_RandomRight = rnd.Next(4);

        while (m_RandomRight == m_RandomLeft) {
            m_RandomRight = rnd.Next(4);
        }

        Debug.Log("Random Left Option: " + m_RandomLeft);
        Debug.Log("Random right Option: " + m_RandomRight);

    }

    //public void LoadNextPage(bool left) { // Triggered from buttons. Left true when the button pressed was the left one

    //    //Update the page index
    //    m_QuestionIndex++;

    //    #region 2 explorers

    //    if (m_NumberOfExplorers == 2)
    //    {
    //        if (m_FirstPhase && m_QuestionIndex == 2)
    //        {
    //            if (left)
    //            {
    //                m_ChosenBook = m_RandomLeft;
    //            }
    //            else
    //            {
    //                m_ChosenBook = m_RandomRight;
    //            }

    //            Debug.Log("Random Left Option: " + m_RandomLeft);
    //            Debug.Log("Random right Option: " + m_RandomRight);
    //            print("Chosen book: " + m_ChosenBook);
    //        }
    //        else if (m_SecondPhase) {

    //            StoryManager();
    //            return;

   
    //        }
    //    }

    //    #endregion

    //    #region 6 explorers

    //    if (m_NumberOfExplorers > 2 && m_NumberOfExplorers <= 6)
    //    {
    //        //Alerta, trozo cutre palero.

    //        if (m_FirstPhase && m_QuestionIndex == 2) // Primera selección para el libro
    //        {
    //            if (left)
    //            {
    //                m_ChosenBook = -1;
    //            }
    //            else
    //            {
    //                m_ChosenBook = -2;
    //            }

    //        }
    //        else if (m_FirstPhase && m_QuestionIndex == 3) // Segunda y última selección para el libro
    //        {

    //            if (m_ChosenBook == -1)
    //            {

    //                if (left)
    //                {
    //                    m_ChosenBook = 0; //Libro selva
    //                }
    //                else
    //                {
    //                    m_ChosenBook = 1; //Movie dick
    //                }

    //            }
    //            if (m_ChosenBook == -2)
    //            {

    //                if (left)
    //                {
    //                    m_ChosenBook = 2; //Alici
    //                }
    //                else
    //                {
    //                    m_ChosenBook = 3; //Principito
    //                }

    //            }

    //            print("Chosen book: " + m_ChosenBook);

    //        }

    //    }

    //    #endregion


    //    if (m_CurrentBookPicker.m_BookPages[m_QuestionIndex].m_Answer.Length == 0 || left) // Si no hay respuestas o se ha respondido el de la IZQUIERDA...
    //    {

    //        //LoadPage(m_QuestionIndex * 2);
    //    }
    //    else { // Si se ha respondido DERECHA

    //        //LoadPage(m_QuestionIndex * 2 + 1);
    //    }
        
    //}

    private void ClearCanvas()
    {

        //HideUI(true);
        ClearText();

    }

    private void HideUI(bool hide)
    {

        if (hide)
        {
            ShowChoiceButtons(false);
            m_NextButtonGO.SetActive(false);
        }
    }

    private void ClearChoices()
    {

        for (int i = 0; i < m_Choices.Length; i++)
        {
            m_Choices[i] = -1;
        }

    }

    private void ClearText()
    {

        m_QuestionText.text = "";
        m_AnswerLeftText.text = "";
        m_AnswerRightText.text = "";

    }
    

    private void ShowChoiceButtons(bool active)
    {
        if (active) {
            StartCoroutine(FadeInButons());
        }
        else{
            //fade out?
            m_RightButtonGO.SetActive(false);
            m_LeftButtonGO.SetActive(false);
        }


    }

    IEnumerator FadeInButons() {

        yield return new WaitForSeconds(1);

        while (m_AnimationsController.IsSmokePlaying() == true)
        {
            yield return null;
        }

        //Fade IN!!
        m_RightButtonGO.SetActive(true);
        m_LeftButtonGO.SetActive(true);
        m_AnimationsController.FadeInButtons();
        

    }
    IEnumerator FadeInNextButon()
    {

        yield return new WaitForSeconds(1);

        while (m_AnimationsController.IsSmokePlaying() == true)
        {
            yield return null;
        }

        //Fade IN!!
        m_NextButtonGO.SetActive(true);
        m_AnimationsController.FadeInNextButton();
        //LASERES DETECTORES DESACTIVADOS HASTA QUE TERMINE EL FADEIN

    }

    private void ShowNextButton()
    {
        HideUI(true);
        StartCoroutine(FadeInNextButon());
        //m_AnswerRightText.text = "Siguiente";
        //m_RightButton.SetActive(true);
        //m_LeftButton.SetActive(false);

    }

    public void NextButtonBehaviour() {

        //m_AnimationsController.FadeOutNextButton();
        OptionSelected(m_LeftOptionChosen);
    
    }


    private void SendAnswersViaOSC() {

        if (m_OSC != null)
        {
            //Number of customers
            OscMessage message = new OscMessage();
            message.address = "/Explorers";
            message.values.Add(m_NumberOfExplorers);
            m_OSC.Send(message);

            //Chosen Book by the customers
            message = new OscMessage();
            message.address = "/StoryBook";
            message.values.Add(m_ChosenBook);
            m_OSC.Send(message);

            //Choices made by the customers
            message = new OscMessage();
            message.address = "/Choices";

            for (int i = 1; i < 5; i++)
            {
                if (m_Choices[i] % 2 == 0)
                {
                    message.values.Add(0);
                }
                else
                {
                    message.values.Add(1);
                }
            }
            
            m_OSC.Send(message);

            //Number of times looped
            message = new OscMessage();
            message.address = "/TimesLooped";
            m_TimesLooped++;
            message.values.Add(m_TimesLooped);
            m_OSC.Send(message);


        }
        else {

            Debug.LogError("Asign the OSC script in the inspector");
        
        }

    }

    private void CheckInput()
    {

        if (m_InputActive && m_CustomersSelectorPhase)
        {

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {

                print("2");
                m_NumberOfExplorers = 2;
                m_CurrentBookPicker = m_BookPicker[0];
                StartFirstPhase();

            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {

                print("3");
                m_NumberOfExplorers = 3;
                m_CurrentBookPicker = m_BookPicker[1];
                StartFirstPhase();


            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {

                print("4");
                m_NumberOfExplorers = 4;
                m_CurrentBookPicker = m_BookPicker[2];
                StartFirstPhase();

            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {

                print("5");
                m_NumberOfExplorers = 5;
                m_CurrentBookPicker = m_BookPicker[3];
                StartFirstPhase();

            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                print("6");
                m_NumberOfExplorers = 6;
                m_CurrentBookPicker = m_BookPicker[4];
                StartFirstPhase();
            }
        }

    }


}
