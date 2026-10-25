using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public void NewGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenScore()
    {
        SceneManager.LoadScene("Score");
    }

    public void OpenHowToPlay()
    {
        SceneManager.LoadScene("HowToPlay");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
