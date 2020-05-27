using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// script by Tamara
public class Menu : MonoBehaviour
{
    [SerializeField]
    private GameObject m_startPanel;
    [SerializeField]
    private GameObject m_mainPanel;

    [SerializeField]
    private Animator m_panelAnimator;
    [SerializeField]
    private Animator m_crossfade;

    [SerializeField]
    private float m_crossfadeTime = 1.05f;
    
    public void SwitchPanels()
    {
        m_panelAnimator.SetBool("isButtonPressed", true);

        // deactivate Panel when done with animation
        if(this.m_panelAnimator.GetCurrentAnimatorStateInfo(0).IsName("StartPanelVanish"))
        {
            m_startPanel.SetActive(false);
        }
    }

    // Load the level directly whitout Intro and play Loading animatoin
    public void LoadGame()
    {
        m_mainPanel.SetActive(false);
        StartCoroutine(LoadLevel("Heddwyn"));
    }

    // Start game with Intro!
    public void NewGame()
    {
        SceneManager.LoadScene("Intro");
    }

    // Exit the Game 
    public void ExitGame()
    {
        Application.Quit();
    }

    
    IEnumerator LoadLevel(string _levelName)
    {
        // start loading screen animation
        m_crossfade.SetTrigger("Start");

        // wait for it to finish
        yield return new WaitForSeconds(m_crossfadeTime);

        // Start the Game!!
        SceneManager.LoadScene(_levelName);
    }

   
}
