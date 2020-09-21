using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftPageController : MonoBehaviour
{

    public Sprite[] m_LeftPageImages;
    public Image m_Image;
    private int m_CurrentSpriteIndex = 0;

    public void SetAdventurersSprite() {
        
        m_Image.sprite = m_LeftPageImages[0];

    }

    public void SetDepthsOfTheUnknownSprite()
    {

        m_Image.sprite = m_LeftPageImages[1];

    }

    public void SetSpaceTimeSprite()
    {

        m_Image.sprite = m_LeftPageImages[2];

    }

    public void SetJungleSprite()
    {

        m_Image.sprite = m_LeftPageImages[3];

    }

    public void SetSeaSprite()
    {

        m_Image.sprite = m_LeftPageImages[4];

    }

    public void SetTunnelSprite()
    {

        m_Image.sprite = m_LeftPageImages[5];

    }

    public void SetUniverseSprite()
    {

        m_Image.sprite = m_LeftPageImages[6];

    }

    public void SetSpriteIndex(int newSpriteIndex) {  // 0 -> The jungle book, 1 --> Moby dick

        m_CurrentSpriteIndex = newSpriteIndex;

    }

    public void SetSpriteFromCurrentIndex()
    {

        switch (m_CurrentSpriteIndex)
        {

            case 0:

                SetAdventurersSprite();

                break;

            case 1:

                SetJungleSprite();

                break;

            case 2:

                SetSeaSprite(); ;

                break;

            case 3:

                SetTunnelSprite();

                break;

            case 4:

                SetUniverseSprite();

                break;

            default:

                break;

        }


    }

    public void SetCurrentIndexFromRandomBookIndex(int random)
    {

        switch (random)
        {

            case 0:

                m_CurrentSpriteIndex = 3; //Selva

                break;

            case 1:

                m_CurrentSpriteIndex = 4;

                break;

            case 2:

                m_CurrentSpriteIndex = 5;

                break;

            case 3:

                m_CurrentSpriteIndex = 6;

                break;

            default:
                Debug.LogWarning("Out of range");
                break;

        }


    }

}
