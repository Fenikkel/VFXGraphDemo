using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollowingBehaviour : MonoBehaviour
{
    public float m_RayLength = float.PositiveInfinity;
    public LayerMask m_LayerMask;

    //public Transform m_MouseFollower;
    public bool m_FollowingBehaviourEnabled = true;

    private void Start()
    {
        //if (m_MouseFollower == null) {

        //    m_FollowingBehaviourEnabled = false;

        //}
    }


    private void Update()
    {
        
        if (m_FollowingBehaviourEnabled) {

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            
            if (Physics.Raycast(ray, out hit, m_RayLength, m_LayerMask))
            {

                //m_MouseFollower.transform.position = hit.point;
                transform.position = hit.point;

            }

        }

    }

}
