using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

// Taruh di GameObject kosong di scene "Score"
public class ScoreManager : MonoBehaviour
{
    [Header("Gambar musuh (urutan: Tipe 1, Tipe 2, Tipe 3, Boss)")]
    [SerializeField] private Sprite[] enemySprites = new Sprite[4];

    [Header("Ringkasan")]
    [SerializeField] private TMP_Text bestTimeText;
    [SerializeField] private TMP_Text totalTimeText;
    [SerializeField] private TMP_Text gamesPlayedText;
    [SerializeField] private Image[] totalKillIcons = new Image[4];
    [SerializeField] private TMP_Text[] totalKillTexts = new TMP_Text[4];

    [Header("Riwayat")]
    [SerializeField] private Transform historyContent;
    [SerializeField] private ScoreEntryUI entryPrefab;
    [SerializeField] private GameObject emptyText;

    [Header("Konfirmasi hapus (opsional)")]
    [SerializeField] private GameObject confirmDeletePanel;
    [SerializeField] private TMP_Text confirmDeleteText;

    // -1 = hapus semua, selain itu = index permainan yang mau dihapus
    private int pendingDeleteIndex = -1;

    void Start()
    {
        Time.timeScale = 1f;
        if (confirmDeletePanel != null) confirmDeletePanel.SetActive(false);
        Refresh();
    }

    public void Refresh()
    {
        ScoreSaveFile data = ScoreData.Load();

        float bestTime = 0f;
        float totalTime = 0f;
        int[] totalKills = new int[4];
        foreach (RunRecord run in data.runs)
        {
            if (run.duration > bestTime) bestTime = run.duration;
            totalTime += run.duration;
            for (int i = 0; i < 4; i++) totalKills[i] += run.kills[i];
        }

        if (bestTimeText != null) bestTimeText.text = "Best Time: " + ScoreData.FormatTime(bestTime);
        if (totalTimeText != null) totalTimeText.text = "Total Time Play: " + ScoreData.FormatTime(totalTime);
        if (gamesPlayedText != null) gamesPlayedText.text = "Total Play: " + data.runs.Count;

        for (int i = 0; i < 4; i++)
        {
            if (i < totalKillTexts.Length && totalKillTexts[i] != null)
            {
                totalKillTexts[i].text = "x" + totalKills[i];
            }
            if (i < totalKillIcons.Length && totalKillIcons[i] != null && i < enemySprites.Length && enemySprites[i] != null)
            {
                totalKillIcons[i].sprite = enemySprites[i];
            }
        }

        // isi ulang daftar riwayat
        if (historyContent != null)
        {
            foreach (Transform child in historyContent)
            {
                Destroy(child.gameObject);
            }
            if (entryPrefab != null)
            {
                for (int i = 0; i < data.runs.Count; i++)
                {
                    ScoreEntryUI entry = Instantiate(entryPrefab, historyContent);
                    entry.Setup(data.runs[i], enemySprites, i, this);
                }
            }
        }

        if (emptyText != null) emptyText.SetActive(data.runs.Count == 0);
    }

    // Tombol Hapus Semua: kalau ada panel konfirmasi, tampilkan dulu
    public void DeleteButton()
    {
        AskDelete(-1, "Hapus semua skor?");
    }

    // Tombol hapus di tiap baris riwayat (dipanggil otomatis oleh ScoreEntryUI)
    public void DeleteRunButton(int index)
    {
        AskDelete(index, "Hapus skor ini?");
    }

    private void AskDelete(int index, string message)
    {
        pendingDeleteIndex = index;
        if (confirmDeletePanel != null)
        {
            if (confirmDeleteText != null) confirmDeleteText.text = message;
            confirmDeletePanel.SetActive(true);
        }
        else
        {
            ConfirmDelete();
        }
    }

    public void ConfirmDelete()
    {
        if (pendingDeleteIndex < 0)
        {
            ScoreData.DeleteAll();
        }
        else
        {
            ScoreData.DeleteRun(pendingDeleteIndex);
        }
        pendingDeleteIndex = -1;
        if (confirmDeletePanel != null) confirmDeletePanel.SetActive(false);
        Refresh();
    }

    public void CancelDelete()
    {
        if (confirmDeletePanel != null) confirmDeletePanel.SetActive(false);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
}
