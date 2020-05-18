using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static bool isOccupied = false;
    public static bool win = false;

    private void Awake()
    {
        if(SceneManager.GetActiveScene().buildIndex == 3)
        {
            Image endBack = GameObject.FindGameObjectWithTag("EndBackground").GetComponent<Image>();

            if (!win)
                endBack.color = Color.red;
        }
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
