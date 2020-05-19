using UnityEngine;
using UnityEngine.SceneManagement;

// Script by Mario Luetzenkirchen
public class StoryManager : MonoBehaviour
{
    [SerializeField]
    private GameObject m_Wrath;
    [SerializeField]
    private Transform m_wrathSpawn;

    private int m_playLvl;
    private bool m_isSpawned = false;

    // Update is called once per frame
    void Update()
    {
        if (!m_isSpawned && (m_playLvl >= 5 && SceneManager.GetActiveScene().buildIndex == 2))
        {
            Instantiate(m_Wrath, m_wrathSpawn.position, m_Wrath.transform.rotation, m_Wrath.transform.parent);
            m_isSpawned = true;
        }
    }

    public void CurrentPlayerLvl(int _lvl)
    {
        m_playLvl = _lvl;
    }
}
