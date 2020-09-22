using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class BookManager : MonoBehaviour
{

    [Header("Controllers")]
    public RightPageController m_RightPage;
    public LeftPageController m_LeftPage;
    public AnimationsController m_AnimationsController;
    public VideoPlayer m_VideoPlayer;
    public IRSensorController m_IRSensorController;

    [Header("Data controllers")]
    public BookSavedData m_SavedData;

    [Header("Index")]
    private int m_QuestionIndex = 0; // From 0 to currentBook.Length
    private Phases m_CurrentPhase = Phases.CustomerSelector; // 0 -> Customers selector phase, 1 -> Book picker phase, 2 -> Story phase

    [Header("Variables Mix")]
    private bool m_InputActive = false;
    private StoryBook m_CurrentBookPicker;
    private StoryBook m_CurrentStoryBook;

    private bool m_LeftOptionChosen = false;
    private int m_StupidVariable = -1;
    public float m_SecondsBetweenFades = 1.0f;
    public float m_SecondsForReading = 5.0f;



    private void Awake()
    {
        m_VideoPlayer.Prepare();
        
    }
    private void Start()
    {
        if (m_VideoPlayer.isPrepared)
        {
            ResetBook();
        }
        else {
            Debug.LogWarning("Video player not prepared yet");
            ResetBook();
        }
    }

    private void Update()
    {

        CheckInput();
    }



    private void ResetVariables()
    {
        m_CurrentBookPicker = null;
        m_CurrentStoryBook = null;
        m_QuestionIndex = 0;

        m_SavedData.ResetData();
    }



    private void ResetBook() 
    {
        m_SavedData.IncreaseTimesLooped();

        //Reset 
        ResetVariables();

        SetPhase(Phases.CustomerSelector);

    }

    private void SetPhase( Phases phase) { //Here we can initialize each fase variables

        m_CurrentPhase = phase;

        switch (phase) {

            case Phases.CustomerSelector: //Customers selector phase

                Debug.Log("-----CUSTOMER SELECTOR PHASE-----");

                //Decimos cual sera el proximo sprite (Sin cambiar la imagen)
                m_LeftPage.SetCurrentSprite(LeftPageImages.Aventureros);
                // Cambiamos la imagen a partir de m_CurrentSprite
                m_LeftPage.SetSpriteFromCurrent();

                m_RightPage.WritePage("Erase una vez, un grupo de aventureros,dispuestos a vivir una experiencia única.", "", "");

                m_InputActive = true; //Deberia ser despues del fade In

                //Fade in
                m_AnimationsController.PlayOpenBook();
                break;

            case Phases.BookPicker: //Book picker phase

                Debug.Log("-----BOOK PICKER PHASE-----");
                m_QuestionIndex = 0;
                break;

            case Phases.Story: //Story phase

                Debug.Log("-----STORY PHASE-----");
                m_QuestionIndex = 0;
                NextRightPage();

                break;

            default:
                Debug.LogError("Incorrect phase");
                break;
        }

    }

   
    private void SetSelectedCustomers(int numCustomers, int bookPickerIndex)
    {

        m_InputActive = false;
        m_SavedData.SetNumberOfExplorers(numCustomers); // n from 2 to 6
        m_RightPage.SetCurrentBookPicker(bookPickerIndex); // n from 0 to 4

        SetPhase(Phases.BookPicker); // Start book picker phase
        m_AnimationsController.PlayFadeOutReading(); //Fade out (Then, when ends will trigger next page)
        //m_AnimationsController.PlayFadeOutImage(); //conservamos la imagen


    }

    public void NextRightPage() { // set all and then triggers the wanit and the fade in animation

        switch (m_CurrentPhase) {

            case Phases.CustomerSelector:

                //Just wait until number of customers is selected
                break;

            case Phases.BookPicker:

                BookPickerPageManager();
               
                break;

            case Phases.Story:

                FableStoryPageManager();

                break;

            default:

                Debug.LogError("This phase is not implemented yet");

                break;
        }
        
    }

    private void FableStoryPageManager() {

        #region 2 Customers

        if (m_SavedData.GetNumberOfExplorers() == 2)
        {
            RightPageType nextPageType;

            switch (m_QuestionIndex)
            {

                case 0:

                    m_LeftOptionChosen = m_RightPage.RandomizePage(m_QuestionIndex * 2); // or 1 we don't care
                    m_SavedData.SaveChoice(m_QuestionIndex, m_LeftOptionChosen);
                    m_AnimationsController.PlayFadeInReading();

                    m_QuestionIndex++;

                    break;

                case 1: //FALTA GUARDAR LA OPCIÓ

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);
                        StartCoroutine(FadeInNextPage(nextPageType));

                    }
                    else
                    { // right chosen

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);
                        StartCoroutine(FadeInNextPage(nextPageType));

                    }

                    m_SavedData.SaveChoice(m_QuestionIndex, m_LeftOptionChosen);
                    m_QuestionIndex++;

                    break;

                case 2:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        m_LeftOptionChosen = m_RightPage.RandomizePage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        m_LeftOptionChosen = m_RightPage.RandomizePage(m_QuestionIndex * 2 + 1);

                    }

                    m_SavedData.SaveChoice(m_QuestionIndex, m_LeftOptionChosen);
                    m_AnimationsController.PlayFadeInReading();
                    m_QuestionIndex++;                 

                    break;


                case 3:

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        m_LeftOptionChosen = m_RightPage.RandomizePage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        m_LeftOptionChosen = m_RightPage.RandomizePage(m_QuestionIndex * 2 + 1);

                    }

                    m_SavedData.SaveChoice(m_QuestionIndex, m_LeftOptionChosen);
                    m_AnimationsController.PlayFadeInReading();
                    m_QuestionIndex++;

                    


                    break;

                case 4:

                    nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);
                    m_AnimationsController.PlayFadeInReading();
                    m_SavedData.PrintChoices();
                    m_QuestionIndex++;
                    break;

                case 5: //The end

                    m_SavedData.SendAnswersViaOSC();
                    ResetBook();

                    return;

                default:
                    Debug.LogWarning("question index out of range?");
                    break;

            }

        }

        #endregion

        #region 3 Customers

        //if (m_NumberOfExplorers == 3)
        //{

        //    switch (m_QuestionIndex)
        //    {

        //        case 0:

        //            LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);
        //            break;

        //        case 1:

        //            if (m_LeftOptionChosen) // Left chosen
        //            {

        //                SaveChoice(m_QuestionIndex * 2);
        //                RandomizePage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

        //            }
        //            else
        //            { // right chosen

        //                SaveChoice(m_QuestionIndex * 2 + 1);
        //                RandomizePage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

        //            }

        //            break;

        //        case 2:

        //            if (m_LeftOptionChosen) // Left chosen
        //            {

        //                SaveChoice(m_QuestionIndex * 2);
        //                RandomizePage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

        //            }
        //            else
        //            { // right chosen

        //                SaveChoice(m_QuestionIndex * 2 + 1);
        //                RandomizePage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

        //            }

        //            break;

        //        case 3: //SOBRA?

        //            if (m_LeftOptionChosen) // Left chosen
        //            {

        //                SaveChoice(m_QuestionIndex * 2);
        //                RandomizePage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

        //            }
        //            else
        //            { // right chosen

        //                SaveChoice(m_QuestionIndex * 2 + 1);
        //                RandomizePage(m_QuestionIndex * 2 + 1, m_StoryBooks[m_ChosenBook]);

        //            }

        //            break;

        //        case 4:

        //            if (m_LeftOptionChosen) // Left chosen
        //            {

        //                SaveChoice(m_QuestionIndex * 2);

        //            }
        //            else
        //            { // right chosen

        //                SaveChoice(m_QuestionIndex * 2 + 1);

        //            }

        //            LoadPage(m_QuestionIndex * 2, m_StoryBooks[m_ChosenBook]);

        //            break;

        //        case 5: //The end

        //            Debug.Log("The end of the second phase: \n");

        //            for (int i = 0; i < m_Choices.Length; i++)
        //            {

        //                Debug.Log(m_Choices[i]);

        //            }

        //            //ENVIAR ANTES LOS DATOS A PABLO
        //            SendAnswersViaOSC();
        //            StartCustomersSelectorsPhase();

        //            break;

        //        default:
        //            Debug.LogWarning("question index out of range?");
        //            break;

        //    }

        //}

        #endregion

        #region 6 Customers

        if (m_SavedData.GetNumberOfExplorers() == 6)
        {
            RightPageType nextPageType;

            switch (m_QuestionIndex)
            {

                case 0:

                    nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);
                    StartCoroutine(FadeInNextPage(nextPageType));

                    m_QuestionIndex++;
                    break;

                case 1:

                    m_SavedData.SaveChoice(0, m_LeftOptionChosen);

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);
                        StartCoroutine(FadeInNextPage(nextPageType));

                    }
                    else
                    { // right chosen

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);
                        StartCoroutine(FadeInNextPage(nextPageType));

                    }

                    m_QuestionIndex++;
                    break;

                case 2:

                    m_SavedData.SaveChoice(1, m_LeftOptionChosen);

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);
                        StartCoroutine(FadeInNextPage(nextPageType));

                    }
                    else
                    { // right chosen

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);
                        StartCoroutine(FadeInNextPage(nextPageType));

                    }

                    m_QuestionIndex++;
                    break;

                case 3: 

                    m_SavedData.SaveChoice(2, m_LeftOptionChosen);

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);
                        StartCoroutine(FadeInNextPage(nextPageType));

                    }
                    else
                    { // right chosen

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);
                        StartCoroutine(FadeInNextPage(nextPageType));

                    }

                    m_QuestionIndex++;
                    break;

                case 4:

                    m_SavedData.SaveChoice(3, m_LeftOptionChosen);

                    nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);
                    m_AnimationsController.PlayFadeInReading();
                    m_QuestionIndex++;

                    break;

                case 5: //The end

                    m_SavedData.SendAnswersViaOSC();
                    ResetBook();

                    return;

                default:
                    Debug.LogWarning("question index out of range?");
                    break;

            }

        }

        #endregion

    }



    private void BookPickerPageManager() {


        switch (m_SavedData.GetNumberOfExplorers())
        {

            case 2:
                RightPageType nextPageType;

                switch (m_QuestionIndex)
                {

                    case 0:

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);
                        StartCoroutine(FadeInNextPage(nextPageType));
                        break;

                    case 1:

                        if (m_LeftOptionChosen)
                        {
                            m_SavedData.SetChosenBook(m_RightPage.GetRandomLeft());
                            m_RightPage.SetCurrentFableBook(m_RightPage.GetRandomLeft());
                            m_LeftPage.SetCurrentIndexFromRandomBookIndex(m_RightPage.GetRandomLeft());
                            print("Chosen book: " + m_SavedData.GetChosenBook());
                        }
                        else
                        {

                            m_SavedData.SetChosenBook(m_RightPage.GetRandomRight());
                            m_RightPage.SetCurrentFableBook(m_RightPage.GetRandomRight());
                            m_LeftPage.SetCurrentIndexFromRandomBookIndex(m_RightPage.GetRandomRight());
                            print("Chosen book: " + m_SavedData.GetChosenBook());

                        }

                        SetPhase(Phases.Story);

                        return;

                    default:
                        Debug.LogWarning("question index out of range?");
                        break;

                }

                m_QuestionIndex++;

                break;

            case 3: //The same for 3 to 6 explorers
            case 4:
            case 5:
            case 6:

                switch (m_QuestionIndex)
                {

                    case 0: //Primera pagina

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);
                        StartCoroutine(FadeInNextPage(nextPageType));
                        break;

                    case 1:

                        if (m_LeftOptionChosen)
                        {
                            m_StupidVariable = -1;
                            nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);             
                            m_LeftPage.SetCurrentSprite(LeftPageImages.ProfundidadesDesconocido); //m_LeftPage.SetDepthsOfTheUnknownSprite();
                            StartCoroutine(FadeInNextPage(nextPageType));
                        }
                        else
                        {
                            m_StupidVariable = -2;
                            nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);
                            m_LeftPage.SetCurrentSprite(LeftPageImages.EspacioTiempo);  //m_LeftPage.SetSpaceTimeSprite();
                            StartCoroutine(FadeInNextPage(nextPageType));

                        }
                        break;

                    case 2:

                        if (m_StupidVariable == -1)
                        {

                            if (m_LeftOptionChosen)
                            {
                                m_SavedData.SetChosenBook(0); //Libro selva
                                m_RightPage.SetCurrentFableBook(0);
                                m_LeftPage.SetCurrentSprite(LeftPageImages.Selva); //m_LeftPage.SetJungleSprite();
                            }
                            else
                            {
                                m_SavedData.SetChosenBook(1); //Movie dick
                                m_RightPage.SetCurrentFableBook(1);
                                m_LeftPage.SetCurrentSprite(LeftPageImages.Oceano); //m_LeftPage.SetSeaSprite();

                            }

                        }
                        if (m_StupidVariable == -2)
                        {

                            if (m_LeftOptionChosen)
                            {
                                m_SavedData.SetChosenBook(2); //Alici
                                m_RightPage.SetCurrentFableBook(2);
                                m_LeftPage.SetCurrentSprite(LeftPageImages.Tunel); //m_LeftPage.SetTunnelSprite();
                            }
                            else
                            {
                                m_SavedData.SetChosenBook(3); //Principito
                                m_RightPage.SetCurrentFableBook(3);
                                m_LeftPage.SetCurrentSprite(LeftPageImages.Universo); //m_LeftPage.SetUniverseSprite();
                            }

                        }
                        print("Chosen book: " + m_SavedData.GetChosenBook());

                        SetPhase(Phases.Story);

                        return;

                    default:
                        Debug.LogError("m_QuestionIndex out of range");
                        break;
                }

                m_QuestionIndex++;

                break;

            default:
                Debug.LogWarning("Number of explorers incorrect!");
                break;

        }

    }



    IEnumerator FadeInNextPage(RightPageType pageType) {

        yield return new WaitForSeconds(m_SecondsBetweenFades);

        //Trigger fade in animation
        m_AnimationsController.FadeInRightPage(pageType);

        if (m_QuestionIndex != 0) {

            m_AnimationsController.PlayFadeInImage();

        }

        m_IRSensorController.ActiveSensors(true);
    }

    public void StartReadingTime() {

        print("Start reading time");
        StartCoroutine(FadeOutReading());

    }

    IEnumerator FadeOutReading()
    {

        yield return new WaitForSeconds(m_SecondsForReading);

        m_AnimationsController.PlayFadeOutReading();
        m_AnimationsController.PlayFadeOutImage();


    }

    public void OnOptionSelected(bool leftChosen) {

        m_LeftOptionChosen = leftChosen;

        //fade out the page
        m_AnimationsController.FadeOutRightPage(m_RightPage.GetCurrentRightPageType());
        m_AnimationsController.PlayFadeOutImage();
        m_LeftPage.SetTunnelSprite();


    }

    public void PlaySmoke()
    {
        m_VideoPlayer.Stop();
        m_VideoPlayer.Play();
    }
    private void CheckInput()
    {



        if (m_InputActive && m_CurrentPhase == 0)
        {

            if (Input.GetKeyDown(KeyCode.Alpha2))
            {

                print("Nº of explorers: 2");
                SetSelectedCustomers(2, 0);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha3))
            {

                print("Nº of explorers: 3");
                SetSelectedCustomers(3, 1);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha4))
            {

                print("Nº of explorers: 4");
                SetSelectedCustomers(4, 2);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha5))
            {

                print("Nº of explorers: 5");
                SetSelectedCustomers(5, 3);

            }
            else if (Input.GetKeyDown(KeyCode.Alpha6))
            {
                print("Nº of explorers: 6");
                SetSelectedCustomers(6, 4);
            }
        }

    }

    public void SetNewImage() {

        m_LeftPage.SetSpriteFromCurrent();
    
    }

    public void OnFadeInEnded() { //Triggered at the end of all the fade in animations as an event
    

    
    }

}
