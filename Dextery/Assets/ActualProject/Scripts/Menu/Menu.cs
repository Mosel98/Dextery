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

    public void StartGame()
    {
        m_mainPanel.SetActive(false);
        StartCoroutine(LoadLevel("Heddwyn"));
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    IEnumerator LoadLevel(string _levelName)
    {
        m_crossfade.SetTrigger("Start");

        yield return new WaitForSeconds(m_crossfadeTime);

        SceneManager.LoadScene(_levelName);
    }
}
