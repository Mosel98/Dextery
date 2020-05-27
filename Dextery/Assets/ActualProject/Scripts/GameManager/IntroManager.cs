using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


// Script by Celine & Tamara
public class IntroManager : MonoBehaviour
{
    [SerializeField]
    private float m_introLength = 141f;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(IntroStop());    
    }

    // Skip the Intro immediatly, for the less patient people :3
    public void SkipIntro()
    {
        SceneManager.LoadScene("Heddwyn");
    }

    // Change scene as soon as Video is over
    IEnumerator IntroStop()
    {
        yield return new WaitForSeconds(m_introLength);

        SceneManager.LoadScene("Heddwyn");
    }
}
