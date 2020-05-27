using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Script by Mario Luetzenkirchen
public class GameManager : MonoBehaviour
{
    public static bool isOccupied = false;
    public static bool win = false;
    

    [SerializeField]
    private GameObject m_winText;
    [SerializeField]
    private GameObject m_loseText;

    private void Awake()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                ClearStaticVariable();                   
                break;
            case "TheEnd":
                if (!win)
                {
                    Image endBack = GameObject.FindGameObjectWithTag("EndBackground").GetComponent<Image>();

                    endBack.color = Color.red;

                    m_loseText.SetActive(true);
                }
                else if(win)
                {
                    m_winText.SetActive(true);
                }
                break;
        }
    }

    private void ClearStaticVariable()
    {
        PlayerAttributes.ClearLvl();
        Inventory.ClearInventory();
        QuestSystem.ClearQuestList();
        isOccupied = false;
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public static void ToTheEnd(bool _win)
    {
        win = _win;
        SceneManager.LoadScene(3);
    }
}
