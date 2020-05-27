using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class IntroManager : MonoBehaviour
{
    [SerializeField]
    private float m_introLength = 141f;

    // Start is called before the first frame update
    void Awake()
    {
        StartCoroutine(IntroStop());    
    }

    public void SkipIntro()
    {
        SceneManager.LoadScene("Heddwyn");
    }

    IEnumerator IntroStop()
    {
        yield return new WaitForSeconds(m_introLength);

        SceneManager.LoadScene("Heddwyn");
    }
}
