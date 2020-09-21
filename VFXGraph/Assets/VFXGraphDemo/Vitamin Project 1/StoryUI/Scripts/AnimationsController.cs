using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AnimationsController : MonoBehaviour
{
    public Animator m_BookAnimations;


    private void Awake()
    {

        if (m_BookAnimations == null) {

            Debug.LogError("Link an animator to the game object");
        
        }
    }

    void Update()
    {
        //print(IsOpenBookPlaying());
        //Debug smoke
        /* if (Input.GetKeyDown(KeyCode.Space)) {

             //Debug.Log("Spaced");
             TriggerSmoke();

         }

         print(IsSmokePlaying());
        */
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{

        //    m_BookAnimations.SetTrigger("OpenBook");

        //}

        //if (Input.GetKeyDown(KeyCode.Alpha2))
        //{

        //    m_BookAnimations.SetTrigger("OpenBook_End");

        //}

        //if (Input.GetKeyDown(KeyCode.Alpha3))
        //{
        //    m_BookAnimations.SetTrigger("TwoChoices");

        //}

        //if (Input.GetKeyDown(KeyCode.Alpha4))
        //{
        //    m_BookAnimations.SetTrigger("TwoChoices_End");

        //}

        //if (Input.GetKeyDown(KeyCode.Alpha5))
        //{
        //    m_BookAnimations.SetTrigger("ReadingMode");

        //}

        //if (Input.GetKeyDown(KeyCode.Alpha6))
        //{
        //    m_BookAnimations.SetTrigger("ReadingMode_End");

        //}


    }

    public void PlayFadeOutImage() {

        m_BookAnimations.SetTrigger("FadeOutImage");
    
    }
    public void PlayFadeInImage()
    {
        m_BookAnimations.SetTrigger("FadeInImage");
    }

    public void PlayOpenBook() {

        m_BookAnimations.SetTrigger("OpenBook");

    }

    public void PlayOpenBookEnd()
    {

        m_BookAnimations.SetTrigger("OpenBook_End");

    }

    public void PlayTwoChoices()
    {

        m_BookAnimations.SetTrigger("TwoChoices");

    }

    public void PlayTwoChoicesEnd()
    {

        m_BookAnimations.SetTrigger("TwoChoices_End");

    }

    public bool IsOpenBookPlaying() // Not work is OpenBookPlaying don't have exit time (Stays at the last frame)
    {
        if (m_BookAnimations.GetCurrentAnimatorStateInfo(0).IsName("OpenBook"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public bool IsOpenBookEndPlaying() // Not work is OpenBookPlaying don't have exit time (Stays at the last frame)
    {
        if (m_BookAnimations.GetCurrentAnimatorStateInfo(0).IsName("OpenBook_End"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlayFadeOutText() {

        m_BookAnimations.SetTrigger("FadeOutText");
    }

    public void PlayFadeInText()
    {

        m_BookAnimations.SetTrigger("FadeInText");
    }

    public void PlayFadeInReading() {

        m_BookAnimations.SetTrigger("FadeInReading");

    }

    public void PlayFadeOutReading()
    {

        m_BookAnimations.SetTrigger("FadeOutReading");

    }



    public void FadeInRightPage(RightPageType pageType) {

        switch (pageType)
        {

            case RightPageType.Reading:
                PlayFadeInText();
                break;

            case RightPageType.TwoOptions:
                PlayFadeInText();
                break;

            case RightPageType.FourOptions:
                PlayFadeInText();
                break;

            case RightPageType.Image:
                PlayFadeInText();
                break;

            default:
                Debug.Log("This page type don't have any animation yet");
                break;

        }

    }

    public void FadeOutRightPage(RightPageType pageType)
    {

        switch (pageType)
        {

            case RightPageType.Reading:
                PlayFadeOutText();
                break;

            case RightPageType.TwoOptions:
                PlayFadeOutText();
                break;

            case RightPageType.FourOptions:
                PlayFadeOutText();
                break;

            case RightPageType.Image:
                PlayFadeOutText();
                break;

            default:
                Debug.Log("This page type don't have any animation yet");
                break;

        }

    }


}
