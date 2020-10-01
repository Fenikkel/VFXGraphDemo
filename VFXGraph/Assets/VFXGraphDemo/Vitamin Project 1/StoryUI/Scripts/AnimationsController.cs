using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;


public class AnimationsController : MonoBehaviour
{
    public Animator m_BookAnimations;

    private bool m_HoldImageNextFade = false;


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

    #region Right Page (Text)

    public void FadeInRightPage(RightPageType pageType) {

        switch (pageType) {

            case RightPageType.ExplorersSelector:

                PlayFadeInReading();
                break;

            case RightPageType.Reading:

                PlayFadeInReading();

                break;

            case RightPageType.TwoOptions:

                PlayFadeInText();

                break;

            case RightPageType.FourOptions:

                PlayFadeInText();

                break;

            case RightPageType.OneRandom:

                PlayFadeInReading();

                break;

            case RightPageType.MultipleRandom:

                PlayFadeInReading();

                break;

            case RightPageType.BookCover:

                Debug.LogWarning("FadeIn right: This pagetype is not implemented");

                break;

            case RightPageType.TheEnd:

                PlayFadeInReading();

                break;

            default:
                Debug.LogWarning("FadeIn right: This pagetype is not implemented");
                break;
        
        }

    
    }

    public void FadeOutRightPage(RightPageType pageType)
    {

        switch (pageType)
        {

            case RightPageType.ExplorersSelector:

                PlayFadeOutReading();
                break;

            case RightPageType.Reading:
                PlayFadeOutText();
                break;

            case RightPageType.TwoOptions:
                PlayFadeOutText();
                break;

            case RightPageType.FourOptions:
                PlayFadeOutText();
                break;

            case RightPageType.OneRandom:

                PlayFadeOutReading();
                break;

            case RightPageType.MultipleRandom:

                PlayFadeOutReading();
                break;

            case RightPageType.BookCover:
                PlayFadeOutText();
                break;

            case RightPageType.TheEnd:
                PlayFadeOutReading();
                break;

            default:
                Debug.LogWarning("This page type don't have any animation yet");
                break;

        }

    }

    #endregion

    #region Left Page (Image)

    public void FadeInLeftPage(LeftPageType pageType)
    {

        if (m_HoldImageNextFade)
        {

            Debug.Log("HOLDING fade in THE IMAGE");

        }
        else
        {

            switch (pageType)
            {

                case LeftPageType.Image:

                    PlayFadeInImage();

                    break;

                case LeftPageType.BookCoverPage:


                    Debug.LogWarning("FadeIn right: This pagetype is not implemented");

                    break;

                default:
                    Debug.LogWarning("FadeIn right: This pagetype is not implemented");
                    break;

            }

        }


    }

    public void FadeOutLeftPage(LeftPageType pageType)
    {
        if (m_HoldImageNextFade)
        {

            Debug.Log("HOLDING fade out THE IMAGE");

        }
        else
        {

            switch (pageType)
            {

                case LeftPageType.Image:

                    PlayFadeOutImage();
                    break;

                case LeftPageType.BookCoverPage:
                    Debug.Log("This page type don't have any animation yet");
                    break;

                default:

                    break;

            }

        }


    }

    #endregion

    public void SetHoldImage(bool hold)
    {
        m_HoldImageNextFade = hold;
    }


    #region OldAnimations


    public void PlayFadeOutImage()
    {

        m_BookAnimations.SetTrigger("FadeOutImage");

    }
    public void PlayFadeInImage()
    {
        m_BookAnimations.SetTrigger("FadeInImage");
    }

    public void PlayOpenBook()
    {

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

    public void PlayFadeOutText()
    {

        m_BookAnimations.SetTrigger("FadeOutText");
    }

    public void PlayFadeInText()
    {

        m_BookAnimations.SetTrigger("FadeInText");
    }

    public void PlayFadeInReading()
    {

        m_BookAnimations.SetTrigger("FadeInReading");

    }

    public void PlayFadeOutReading()
    {

        m_BookAnimations.SetTrigger("FadeOutReading");

    }


    //public void FadeInRightPage(RightPageType rightType)
    //{

    //    switch (rightType)
    //    {
    //        case RightPageType.ExplorersSelector:
    //            PlayFadeInReading();
    //            break;

    //        case RightPageType.Reading:
    //            PlayFadeInText();
    //            break;

    //        case RightPageType.TwoOptions:
    //            PlayFadeInText();
    //            break;

    //        case RightPageType.FourOptions:
    //            PlayFadeInText();
    //            break;

    //        case RightPageType.BookCover:
    //            PlayFadeInText();
    //            break;

    //        default:
    //            Debug.Log("This page type don't have any animation yet");
    //            break;

    //    }

    //}





    #endregion

}
