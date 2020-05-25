using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepSounds : MonoBehaviour
{
    [SerializeField]
    private AudioSource m_leftFootstep;
    [SerializeField]
    private AudioSource m_rightFootstep;

    public void leftFoot() 
    {
        m_leftFootstep.Play();
    }

    public void rightFoot()
    {
        m_rightFootstep.Play();
    }
       }