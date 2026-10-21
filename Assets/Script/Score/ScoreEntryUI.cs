using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Satu baris di daftar riwayat skor (dipasang di prefab baris)
public class ScoreEntryUI : MonoBehaviour
{
    public TMP_Text dateText;
    public TMP_Text durationText;

    // urutan: Tipe 1, Tipe 2, Tipe 3, Boss
    public Image[] enemyIcons = new Image[4];
    public TMP_Text[] killTexts = new TMP_Text[4];

    // tombol hapus untuk baris ini (opsional)
    public Button deleteButton;

    public void Setup(RunRecord run, Sprite[] enemySprites, int index, ScoreManager manager)
    {
        if (deleteButton != null)
        {
            deleteButton.onClick.RemoveAllListeners();
            deleteButton.onClick.AddListener(() => manager.DeleteRunButton(index));
        }

        if (dateText != null) dateText.text = run.date;
        if (durationText != null) durationText.text = ScoreData.FormatTime(run.duration);

        for (int i = 0; i < 4; i++)
        {
            if (i < killTexts.Length && killTexts[i] != null)
            {
                killTexts[i].text = "x" + run.kills[i];
            }
            if (i < enemyIcons.Length && enemyIcons[i] != null && enemySprites != null && i < enemySprites.Length && enemySprites[i] != null)
            {
                enemyIcons[i].sprite = enemySprites[i];
            }
        }
    }
}
