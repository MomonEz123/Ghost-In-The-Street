using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public float gameTime;
    public bool gameActive;

    // jumlah musuh yang dikalahkan di permainan ini, index = (int)EnemyType
    [System.NonSerialized] public int[] killsPerType = new int[4];
    private bool scoreSaved;
    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }

    void Start()
    {
        gameActive = true;
    }

    void Update()
    {
        if (gameActive)
        {
            gameTime += Time.deltaTime;
            UIControlller.Instance.UpdateTimer(gameTime);
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.P))
            {
                Pause();
            }
        }
    }

    public void AddKill(EnemyType type)
    {
        killsPerType[(int)type]++;
    }

    // simpan hasil permainan ke riwayat skor (hanya sekali per permainan)
    private void SaveScore()
    {
        if (scoreSaved) return;
        scoreSaved = true;
        ScoreData.SaveRun(gameTime, killsPerType);
    }

    public void GameOver()
    {
        gameActive = false;
        SaveScore();
        StartCoroutine(ShowGameOverScreen());
    }

    IEnumerator ShowGameOverScreen()
    {
        yield return new WaitForSeconds(1.5f);
        UIControlller.Instance.gameOverPanel.SetActive(true);
        AudioController.Instance.PlaySound(AudioController.Instance.gameOver);
    }

    public void Restart()
    {
        SaveScore();
        Time.timeScale = 1f;
        SceneManager.LoadScene("Game");
    }

    public void Pause()
    {
        if (UIControlller.Instance.pausePanel.activeSelf == false && UIControlller.Instance.gameOverPanel.activeSelf == false)
        {
            UIControlller.Instance.pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
        else
        {
            UIControlller.Instance.pausePanel.SetActive(false);
            Time.timeScale = 1f;
        }
    }

    public void QuitGame()
    {
        SaveScore();
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SaveScore();
        SceneManager.LoadScene("Main Menu");
        Time.timeScale = 1f;
    }
}
