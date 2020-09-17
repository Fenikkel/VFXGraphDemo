using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokeAnimationController : MonoBehaviour
{
    private Animator m_Animator;

    private void Awake()
    {
        m_Animator = GetComponent<Animator>();

        if (m_Animator == null) {

            Debug.LogError("Link an animator to the game object");
        
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space)) {

            Debug.Log("Spaced");
            TriggerSmoke();
        
        }
    }

    public void TriggerSmoke() {

        m_Animator.SetTrigger("Smoke");

    
    }
}
