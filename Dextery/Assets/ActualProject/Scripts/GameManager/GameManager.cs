using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Script by Mario Luetzenkirchen
public class GameManager : MonoBehaviour
{
    public static bool isOccupied = false;
    public static bool win = false;
    public static bool firstTime = true;

    private void Awake()
    {
        switch(SceneManager.GetActiveScene().name)
        {
            case "MainMenu":
                if (!firstTime)
                {
                    ClearStaticVariable();
                    firstTime = false;
                }                    
                break;
            case "TheEnd":
                if (!win)
                {
                    Image endBack = GameObject.FindGameObjectWithTag("EndBackground").GetComponent<Image>();

                    endBack.color = Color.red;
                }
                break;
        }
    }

    private void ClearStaticVariable()
    {
        PlayerAttributes.ClearLvl();
        Inventory.ClearInventory();
        QuestSystem.ClearQuestList();
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
