using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

public class AnimationsController : MonoBehaviour
{
    public Animator m_SmokeAnimator;
    public Animator m_MaskAnimator;
    public Animator m_RightAnswerPanelAnimator;
    public Animator m_LeftAnswerPanelAnimator;


    private void Awake()
    {

        if (m_SmokeAnimator == null || m_MaskAnimator == null || m_RightAnswerPanelAnimator == null) {

            Debug.LogError("Link an animator to the game object");
        
        }
    }

    void Update()
    {
        //Debug smoke
       /* if (Input.GetKeyDown(KeyCode.Space)) {

            //Debug.Log("Spaced");
            TriggerSmoke();
        
        }

        print(IsSmokePlaying());
       */
    }

    public void FadeInNextButton()
    {
        m_RightAnswerPanelAnimator.SetTrigger("FadeIn");
    }

    public void FadeOutNextButton()
    {

        m_RightAnswerPanelAnimator.SetTrigger("FadeOut");
 
    }

    public void FadeInButtons()
    {

        m_RightAnswerPanelAnimator.SetTrigger("FadeIn");
        m_LeftAnswerPanelAnimator.SetTrigger("FadeIn");
    }

    public void FadeOutButtons()
    {

        m_RightAnswerPanelAnimator.SetTrigger("FadeOut");
        m_LeftAnswerPanelAnimator.SetTrigger("FadeOut");
    }

    public void TriggerSmoke() {

        m_SmokeAnimator.SetTrigger("Smoke");
        m_MaskAnimator.SetTrigger("Smoke");
    }

    public bool IsSmokePlaying() {


        if (m_SmokeAnimator.GetCurrentAnimatorStateInfo(0).IsName("DiskSmokeAnim"))
        {

            return true;

        }
        else {

            return false;
        
        }
    }

    public bool IsRightAnswerPanelFadingOut()
    {


        if (m_RightAnswerPanelAnimator.GetCurrentAnimatorStateInfo(0).IsName("FadeOut"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}
