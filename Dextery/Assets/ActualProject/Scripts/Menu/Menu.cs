using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject m_startPanel;
    [SerializeField]
    private GameObject m_mainPanel;

    [SerializeField]
    private Animator m_panelAnimator;
    [SerializeField]
    private Animator m_selectionToggle;

    public void SwitchPanels()
    {
        m_panelAnimator.SetBool("isButtonPressed", true);

        // deactivate Panel when done with animation
        if(this.m_panelAnimator.GetCurrentAnimatorStateInfo(0).IsName("StartPanelVanish"))
        {
            m_startPanel.SetActive(false);
        }
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Heddwyn");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
