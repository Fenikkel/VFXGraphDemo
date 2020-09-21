using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BookSavedData : MonoBehaviour
{
    public OSC m_OSC;

    [Header("Data Saved")]
    private int m_ChosenBook = -1; // 0 -> Libro Selva, 1 -> Movi Dick, 2 -> Alicia Maravillas, 3 -> Principito
    private int[] m_Choices = new int[4]; // -1 = empty
    private int m_NumberOfExplorers = 0;
    private int m_TimesLooped = 0; //Number of times the book was played. Non repetitive number.

    public void ResetData() {
        m_ChosenBook = -1;
        m_NumberOfExplorers = 0;
        ClearChoices();
    }

    private void ClearChoices()
    {
        for (int i = 0; i < m_Choices.Length; i++)
        {
            m_Choices[i] = -1;
        }
    }

    public void IncreaseTimesLooped()
    {
        m_TimesLooped++;
    }

    public void SendAnswersViaOSC()
    {

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

            for (int i = 0; i < 4; i++)
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
        else
        {

            Debug.LogError("Asign the OSC script in the inspector");

        }

    }
    public void SaveChoice(int choiceIndex, bool leftChoice) {

        if (leftChoice)
        {

            m_Choices[choiceIndex] = 0; //left

        }
        else {

            m_Choices[choiceIndex] = 1; //right

        }

    }

    public void PrintChoices() {

        for (int i = 0; i < m_Choices.Length; i++)
        {
            print("Choice [" + i + "]: " + m_Choices[i] + "\n");
        }
    
    }

    #region Getters and Setters

    public void SetChosenBook(int newChosen) {
        m_ChosenBook = newChosen;
    }

    public int GetChosenBook()
    {
        return m_ChosenBook;
    }

    public void SetNumberOfExplorers(int numberExplorers)
    {
        m_NumberOfExplorers = numberExplorers;
    }

    public int GetNumberOfExplorers()
    {
        return m_NumberOfExplorers;
    }

    public int GetTimesLooped()
    {
        return m_TimesLooped;
    }

    public int[] GetChoices() {

        return m_Choices;
    }

    #endregion
}
