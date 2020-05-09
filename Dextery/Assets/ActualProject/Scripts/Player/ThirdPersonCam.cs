using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ThirdPersonCam : MonoBehaviour
{
    private PlayerControls m_controls = null;
    private Vector2 m_lookDelta;


    private void Awake()
    {
        m_controls = new PlayerControls();
    }


    // Update is called once per frame
    void Update()
    {
        CinemachineCore.GetInputAxis = GetAxisCustom;
    }

    // change to new Input System
    public float GetAxisCustom(string axisName)
    {
        m_lookDelta = m_controls.Camera.Rotation.ReadValue<Vector2>();
        m_lookDelta.Normalize();
        

        if(axisName == "Mouse X")
        {
            return m_lookDelta.x;
        }
        else if (axisName =="Mouse Y")
        {
            return m_lookDelta.y;
        }

        return 0;
    }


    private void OnEnable()
    {
        m_controls.Camera.Enable();
    }

    private void OnDisable()
    {
        m_controls.Camera.Disable();
    }
}
