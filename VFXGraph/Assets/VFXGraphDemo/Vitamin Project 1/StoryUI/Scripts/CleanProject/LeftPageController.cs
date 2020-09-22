using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LeftPageController : MonoBehaviour
{

    public Sprite[] m_LeftPageImages;
    public Image m_Image;
    private LeftPageImages m_CurrentSprite;

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

    public void SetCurrentSprite(LeftPageImages sprite) {  // 0 -> The jungle book, 1 --> Moby dick

        m_CurrentSprite = sprite;

    }

    public void SetSpriteFromCurrent()
    {

        m_Image.sprite = m_LeftPageImages[(int)m_CurrentSprite]; //Este parse FUNCIONA?

        /*
        switch (m_CurrentSprite)
        {

            case LeftPageImages.Aventureros:

                SetAdventurersSprite();

                break;

            case LeftPageImages.ProfundidadesDesconocido:

                SetDepthsOfTheUnknownSprite();

                break;

            case LeftPageImages.EspacioTiempo:

                SetSpaceTimeSprite();

                break;

            case LeftPageImages.Selva:

                SetJungleSprite();

                break;

            case LeftPageImages.Oceano:

                SetSeaSprite(); ;

                break;

            case LeftPageImages.Tunel:

                SetTunnelSprite();

                break;

            case LeftPageImages.Universo:

                SetUniverseSprite();

                break;

            default:

                break;

        }
        */


    }

    public void SetCurrentIndexFromRandomBookIndex(int random)
    {

        switch (random)
        {

            case 0:

                m_CurrentSprite = LeftPageImages.Selva; //Selva

                break;

            case 1:

                m_CurrentSprite = LeftPageImages.Oceano;

                break;

            case 2:

                m_CurrentSprite = LeftPageImages.Tunel;

                break;

            case 3:

                m_CurrentSprite = LeftPageImages.Universo;

                break;

            default:
                Debug.LogWarning("Out of range");
                break;

        }


    }

}
