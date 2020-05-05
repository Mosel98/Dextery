using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static bool isOccupied = false;

    public void SaveGame()
    {
        // SaveByXML();
    }

    public void LoadGame()
    {

    }

    public void ExitGame()
    {
        Application.Quit();
    }

    private void SaveByXML()
    {

    }
}
