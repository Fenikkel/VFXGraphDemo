using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaskPanelController : MonoBehaviour
{
    //public RectTransform m_ParentPanelTransform;
    public RectTransform m_MaskPanelTransform;
    public Animator m_SmokeAnimator;
    //public RectTransform m_MaskedPanelTransform;

    private Vector3 m_InitialPos;
    private Vector3 m_FinalPos;

    public int m_InterpolationFramesCount = 45; // Number of frames to completely interpolate between the 2 positions
    private int m_ElapsedFrames = 0;

    private bool m_Interpolate = false;


    void Start()
    {
        if (m_MaskPanelTransform != null) {

            m_InitialPos = new Vector3(0.0f, -1 * m_MaskPanelTransform.rect.height / 2, 0.0f);
            m_FinalPos = new Vector3(1.0f * m_MaskPanelTransform.rect.width, -1 * m_MaskPanelTransform.rect.height / 2, 0.0f);
           
        }
        else {
            Debug.LogError("There is no rect transform");
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (m_Interpolate) {

            float interpolationRatio = (float)m_ElapsedFrames / m_InterpolationFramesCount;

            Vector3 interpolatedPosition = Vector3.Lerp(m_InitialPos, m_FinalPos, interpolationRatio);

            m_ElapsedFrames = (m_ElapsedFrames + 1) % (m_InterpolationFramesCount + 1);  // reset elapsedFrames to zero after it reached (interpolationFramesCount + 1)


            if (m_ElapsedFrames == 0)
            {
                m_Interpolate = false;
            }
            else
            {
                m_MaskPanelTransform.localPosition = interpolatedPosition;
            }

        }

        if (Input.GetKeyDown(KeyCode.Space)) {

            m_Interpolate = true;
        }



    }

    private void RestartPanel() {

        print(m_MaskPanelTransform.localPosition);
        print(-1 * m_MaskPanelTransform.localScale.x);
        //final pos
        m_MaskPanelTransform.localPosition = new Vector3(1.0f * m_MaskPanelTransform.rect.width, -1 * m_MaskPanelTransform.rect.height / 2, 0.0f);
        m_MaskPanelTransform.localPosition = new Vector3(0.0f, -1 * m_MaskPanelTransform.rect.height / 2, 0.0f);
        print(m_MaskPanelTransform.localPosition);

        //m_MaskedPanelTransform.localPosition =  new Vector3(1.5f * m_MaskPanelTransform.rect.width, -1 * m_MaskPanelTransform.rect.height / 2, 0.0f);
        //m_MaskedPanelTransform.transform.position = m_ParentPanelTransform.transform.position;
    }
    
}
