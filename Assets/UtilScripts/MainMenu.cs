using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void playGame()
    {
        SceneManager.LoadScene("FlyFishing");
    }

    public void quitGame()
    {
        Application.Quit();
    }
}
