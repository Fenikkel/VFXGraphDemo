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

        m_AnimationsController.SetHoldImage(false);
    }



    private void ResetBook() 
    {
        m_SavedData.IncreaseTimesLooped();

        //Reset 
        ResetVariables();
        m_IRSensorController.ActiveSensors(false);


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

                //Set pages types
                m_RightPage.SetCurrentRightPageType(RightPageType.ExplorersSelector);
                m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                m_RightPage.WritePage("Erase una vez, un grupo de aventureros,dispuestos a vivir una experiencia única.", "", "");

                //Fade in Hold image
                m_AnimationsController.SetHoldImage(false);

                //Fade in
                m_AnimationsController.FadeInRightPage(RightPageType.ExplorersSelector);
                m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                //Fade out Hold image
                m_AnimationsController.SetHoldImage(true);

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

        EnableKeyboard(false);
        m_SavedData.SetNumberOfExplorers(numCustomers); // n from 2 to 6
        m_RightPage.SetCurrentBookPicker(bookPickerIndex); // n from 0 to 4

        SetPhase(Phases.BookPicker); // Start book picker phase
        m_AnimationsController.FadeOutRightPage(RightPageType.ExplorersSelector); //Fade out (Then, when ends will trigger next page)
         //conservamos la imagen. No hay fade out de la pagina izquierda


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

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.OneRandom);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(false);

                    //Randomize
                    m_LeftOptionChosen = m_RightPage.RandomizePage(m_QuestionIndex * 2); // or 1 we don't care

                    //Save data
                    m_SavedData.SaveChoice(m_QuestionIndex, m_LeftOptionChosen);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.OneRandom);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;

                    break;

                case 1:
                    

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Hold image
                    m_AnimationsController.SetHoldImage(true);

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);
                        //StartCoroutine(FadeInNextPage(nextPageType));

                    }
                    else
                    { // right chosen

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);
                        //StartCoroutine(FadeInNextPage(nextPageType));

                    }

                    m_SavedData.SaveChoice(m_QuestionIndex, m_LeftOptionChosen);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TwoOptions);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    m_QuestionIndex++;

                    break;

                case 2:


                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.OneRandom);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Hold image
                    m_AnimationsController.SetHoldImage(true);

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        m_LeftOptionChosen = m_RightPage.RandomizePage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        m_LeftOptionChosen = m_RightPage.RandomizePage(m_QuestionIndex * 2 + 1);

                    }
                    //Save data
                    m_SavedData.SaveChoice(m_QuestionIndex, m_LeftOptionChosen);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.OneRandom);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);


                    m_QuestionIndex++;                 

                    break;


                case 3:


                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.OneRandom);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Hold image
                    m_AnimationsController.SetHoldImage(true);

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        m_LeftOptionChosen = m_RightPage.RandomizePage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        m_LeftOptionChosen = m_RightPage.RandomizePage(m_QuestionIndex * 2 + 1);

                    }

                    //Save data
                    m_SavedData.SaveChoice(m_QuestionIndex, m_LeftOptionChosen);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.OneRandom);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);


                    m_QuestionIndex++;

                    break;

                case 4:

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TheEnd);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Hold image
                    m_AnimationsController.SetHoldImage(true);

                    nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TheEnd);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image
                    m_AnimationsController.SetHoldImage(false);

                    m_SavedData.PrintChoices();

                    m_QuestionIndex++;
                    break;

                case 5: //The end
                    print("HASTA AQUI");
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

        if (m_SavedData.GetNumberOfExplorers() == 3)
        {
            RightPageType nextPageType;

            switch (m_QuestionIndex)
            {

                case 0:

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(false);

                    nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TwoOptions);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;

                case 1:

                    //Save data
                    m_SavedData.SaveChoice(0, m_LeftOptionChosen);

                    //Clear previous text
                    m_RightPage.ClearAllText();

                    //Randomize without show it
                    if (m_LeftOptionChosen) // Left chosen
                    {
                        m_LeftOptionChosen = m_RightPage.RandomizeAndAddPage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        m_LeftOptionChosen = m_RightPage.RandomizeAndAddPage(m_QuestionIndex * 2 + 1);

                    }


                    m_QuestionIndex++;
                    FableStoryPageManager();
                    break;

                case 2:
                    //Save data
                    m_SavedData.SaveChoice(1, m_LeftOptionChosen);

                    //Randomize without show it
                    if (m_LeftOptionChosen) // Left chosen
                    {
                        m_LeftOptionChosen = m_RightPage.RandomizeAndAddPage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        m_LeftOptionChosen = m_RightPage.RandomizeAndAddPage(m_QuestionIndex * 2 + 1);

                    }


                    m_QuestionIndex++;
                    FableStoryPageManager();
                    break;

                case 3:

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.MultipleRandom);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(2, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    if (m_LeftOptionChosen) // Left chosen
                    {
                        m_LeftOptionChosen = m_RightPage.RandomizeAndAddPage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        m_LeftOptionChosen = m_RightPage.RandomizeAndAddPage(m_QuestionIndex * 2 + 1);

                    }

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.MultipleRandom);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;


                case 4:


                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TheEnd);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(3, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TheEnd);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(false);
                    m_QuestionIndex++;
                    m_SavedData.PrintChoices();

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

        #region 4 Customers

        if (m_SavedData.GetNumberOfExplorers() == 4)
        {
            RightPageType nextPageType;

            switch (m_QuestionIndex)
            {

                case 0:

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(false);

                    nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TwoOptions);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;

                case 1:

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(0, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen
                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);
                    }

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TwoOptions);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;

                case 2:
                    //Save data
                    m_SavedData.SaveChoice(1, m_LeftOptionChosen);

                    //Clear previous text
                    m_RightPage.ClearAllText();

                    //Randomize without show it
                    if (m_LeftOptionChosen) // Left chosen
                    {
                        m_LeftOptionChosen = m_RightPage.RandomizeAndAddPage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        m_LeftOptionChosen = m_RightPage.RandomizeAndAddPage(m_QuestionIndex * 2 + 1);

                    }


                    m_QuestionIndex++;
                    FableStoryPageManager();
                    break;

                case 3:

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.MultipleRandom);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(2, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    if (m_LeftOptionChosen) // Left chosen
                    {
                        m_LeftOptionChosen = m_RightPage.RandomizeAndAddPage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        m_LeftOptionChosen = m_RightPage.RandomizeAndAddPage(m_QuestionIndex * 2 + 1);

                    }

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.MultipleRandom);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;


                case 4:


                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TheEnd);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(3, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TheEnd);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(false);
                    m_QuestionIndex++;
                    m_SavedData.PrintChoices();

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

        #region 5 Customers

        if (m_SavedData.GetNumberOfExplorers() == 5)
        {
            RightPageType nextPageType;

            switch (m_QuestionIndex)
            {

                case 0:

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(false);

                    nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TwoOptions);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;

                case 1:

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(0, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen
                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);
                    }

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TwoOptions);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;

                case 2:

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(1, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);

                    }

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TwoOptions);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;

                case 3:



                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.OneRandom);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(2, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    if (m_LeftOptionChosen) // Left chosen
                    {
                        m_LeftOptionChosen = m_RightPage.RandomizePage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        m_LeftOptionChosen = m_RightPage.RandomizePage(m_QuestionIndex * 2 + 1);

                    }

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.OneRandom);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;


                case 4:


                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TheEnd);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(3, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TheEnd);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(false);
                    m_QuestionIndex++;
                    m_SavedData.PrintChoices();

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

        #region 6 Customers

        if (m_SavedData.GetNumberOfExplorers() == 6)
        {
            RightPageType nextPageType;

            switch (m_QuestionIndex)
            {

                case 0:

                    print("6 hasta aqui");

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(false);

                    nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TwoOptions);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;

                case 1:

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(0, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen
                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);
                    }

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TwoOptions);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;

                case 2:

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(1, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);

                    }

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TwoOptions);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;

                case 3:



                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(2, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    if (m_LeftOptionChosen) // Left chosen
                    {

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    }
                    else
                    { // right chosen

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);

                    }

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TwoOptions);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(true);

                    m_QuestionIndex++;
                    break;

                case 4:
                    

                    //SetcurrentPage
                    m_RightPage.SetCurrentRightPageType(RightPageType.TheEnd);
                    m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                    //Save data
                    m_SavedData.SaveChoice(3, m_LeftOptionChosen);

                    //Hold image in fade in
                    m_AnimationsController.SetHoldImage(true);

                    nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                    //Fade in
                    m_AnimationsController.FadeInRightPage(RightPageType.TheEnd);
                    m_AnimationsController.FadeInLeftPage(LeftPageType.Image);

                    //Hold image in fade out
                    m_AnimationsController.SetHoldImage(false);
                    m_QuestionIndex++;
                    m_SavedData.PrintChoices();

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
            #region 2 explorers
            case 2:

                RightPageType nextPageType;

                switch (m_QuestionIndex)
                {

                    case 0:

                        //SetCurrentPageType
                        m_RightPage.SetCurrentRightPageType(RightPageType.FourOptions); //just for this case
                        m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                        //Hold image in fade in
                        m_AnimationsController.SetHoldImage(true);

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);

                        //Fade in
                        m_AnimationsController.FadeInRightPage(m_RightPage.GetCurrentRightPageType());
                        m_AnimationsController.FadeInLeftPage(m_LeftPage.GetCurrentLeftPageType());

                        //Hold image in fade out
                        m_AnimationsController.SetHoldImage(false);

                        break;

                    case 1:

                        if (m_LeftOptionChosen)
                        {
                            //Save data
                            m_SavedData.SetChosenBook(m_RightPage.GetRandomLeft());
                            m_RightPage.SetCurrentFableBook(m_RightPage.GetRandomLeft());

                            //Set left page image
                            m_LeftPage.SetCurrentIndexFromRandomBookIndex(m_RightPage.GetRandomLeft());
                            m_LeftPage.SetSpriteFromCurrent();

                            print("Chosen book: " + m_SavedData.GetChosenBook());
                        }
                        else
                        {
                            // Save data
                            m_SavedData.SetChosenBook(m_RightPage.GetRandomRight());
                            m_RightPage.SetCurrentFableBook(m_RightPage.GetRandomRight());

                            //Set left page image
                            m_LeftPage.SetCurrentIndexFromRandomBookIndex(m_RightPage.GetRandomRight());
                            m_LeftPage.SetSpriteFromCurrent();

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
            #endregion

            case 3: //The same for 3 to 6 explorers
            case 4:
            case 5:
            case 6:

                switch (m_QuestionIndex)
                {

                    case 0: //Primera pagina

                        //SetCurrentPageType
                        m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions); 
                        m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);



                        //Hold image in fade in
                        m_AnimationsController.SetHoldImage(true);

                        nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);


                        //Fade in
                        m_AnimationsController.FadeInRightPage(m_RightPage.GetCurrentRightPageType());
                        m_AnimationsController.FadeInLeftPage(m_LeftPage.GetCurrentLeftPageType());

                        //Hold image in fade out
                        m_AnimationsController.SetHoldImage(false);

                        break;

                    case 1:

                        //SetCurrentPageType
                        m_RightPage.SetCurrentRightPageType(RightPageType.TwoOptions);
                        m_LeftPage.SetCurrentLeftPageType(LeftPageType.Image);

                        //Hold image in fade in
                        m_AnimationsController.SetHoldImage(false);

                        if (m_LeftOptionChosen)
                        {
                            m_StupidVariable = -1;
                            nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2);     
                            
                            //Set current sprite
                            m_LeftPage.SetCurrentSprite(LeftPageImages.ProfundidadesDesconocido);
                        }
                        else
                        {
                            m_StupidVariable = -2;
                            nextPageType = m_RightPage.LoadRightPage(m_QuestionIndex * 2 + 1);

                            //Set current sprite
                            m_LeftPage.SetCurrentSprite(LeftPageImages.EspacioTiempo);

                        }

                        //Set the sprite in the image
                        m_LeftPage.SetSpriteFromCurrent();

                        //Fade in
                        m_AnimationsController.FadeInRightPage(m_RightPage.GetCurrentRightPageType());
                        m_AnimationsController.FadeInLeftPage(m_LeftPage.GetCurrentLeftPageType());

                        //Hold image in fade out
                        m_AnimationsController.SetHoldImage(false);

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

                        //Set the sprite in the image
                        m_LeftPage.SetSpriteFromCurrent();

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


    private void WaitBetweenFadesTriggerer() {

        StartCoroutine(WaitBetweenFades());
    
    }

    IEnumerator WaitBetweenFades() {
        print("WaitingBetweenFades");
        yield return new WaitForSeconds(m_SecondsBetweenFades);

        NextRightPage();
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


    IEnumerator ReadingTime()
    {
        Debug.Log("WAITING FOR READING");
        yield return new WaitForSeconds(m_SecondsForReading);

        m_AnimationsController.FadeOutLeftPage(m_LeftPage.GetCurrentLeftPageType());
        m_AnimationsController.FadeOutRightPage(m_RightPage.GetCurrentRightPageType());


    }

    public void OnOptionSelected(bool leftChosen) {

        m_LeftOptionChosen = leftChosen;

        m_IRSensorController.ActiveSensors(false);

        //fade out the page
        m_AnimationsController.FadeOutRightPage(m_RightPage.GetCurrentRightPageType());

        m_AnimationsController.FadeOutLeftPage(m_LeftPage.GetCurrentLeftPageType());


    }

    public void PlaySmoke()
    {
        m_VideoPlayer.Stop();
        m_VideoPlayer.Play();
    }
    private void CheckInput()
    {

        if (m_InputActive && m_CurrentPhase == Phases.CustomerSelector)
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

    public void OnRightFadeInEnded() { //Triggered at the end of all the fade in animations as an event

        switch (m_RightPage.GetCurrentRightPageType()) {

            case RightPageType.ExplorersSelector:

                EnableKeyboard(true);

                break;

            case RightPageType.OneRandom:

                StartCoroutine(ReadingTime());

                break;

            case RightPageType.MultipleRandom:

                StartCoroutine(ReadingTime());

                break;

            case RightPageType.TheEnd:

                StartCoroutine(ReadingTime());

                break;

            default:
                Debug.LogWarning("Right page type not implemented");
                break;
        
        }
            
    }

    private void EnableKeyboard(bool enable) {

        m_InputActive = enable;

        //Debug
        if (enable)
        {
            Debug.Log("Keyboard ENABLED");
        }
        else {
            Debug.Log("Keyboard DISABLED");
        }


    }

    public void EnableSensors() { //Called at the end of FadeInText

        m_IRSensorController.ActiveSensors(true);
    
    }


}
