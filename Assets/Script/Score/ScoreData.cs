using System;
using System.Collections.Generic;
using UnityEngine;

// Satu catatan hasil permainan
[Serializable]
public class RunRecord
{
    public string date;
    public float duration;
    public int[] kills = new int[4]; // index = (int)EnemyType

    public int TotalKills()
    {
        int total = 0;
        foreach (int k in kills) total += k;
        return total;
    }
}

[Serializable]
public class ScoreSaveFile
{
    public List<RunRecord> runs = new List<RunRecord>();
}

// Menyimpan & membaca riwayat skor lewat PlayerPrefs (format JSON)
public static class ScoreData
{
    private const string SaveKey = "GhostInTheStreet_Scores";
    private const int MaxRuns = 20;

    public static ScoreSaveFile Load()
    {
        string json = PlayerPrefs.GetString(SaveKey, "");
        if (string.IsNullOrEmpty(json)) return new ScoreSaveFile();

        ScoreSaveFile data = JsonUtility.FromJson<ScoreSaveFile>(json);
        return data ?? new ScoreSaveFile();
    }

    public static void SaveRun(float duration, int[] kills)
    {
        ScoreSaveFile data = Load();

        RunRecord run = new RunRecord();
        run.date = DateTime.Now.ToString("dd/MM/yyyy HH:mm");
        run.duration = duration;
        for (int i = 0; i < run.kills.Length && i < kills.Length; i++)
        {
            run.kills[i] = kills[i];
        }

        // permainan terbaru di paling atas
        data.runs.Insert(0, run);
        if (data.runs.Count > MaxRuns)
        {
            data.runs.RemoveRange(MaxRuns, data.runs.Count - MaxRuns);
        }

        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    // hapus satu permainan berdasarkan urutan di daftar (0 = terbaru)
    public static void DeleteRun(int index)
    {
        ScoreSaveFile data = Load();
        if (index < 0 || index >= data.runs.Count) return;

        data.runs.RemoveAt(index);
        PlayerPrefs.SetString(SaveKey, JsonUtility.ToJson(data));
        PlayerPrefs.Save();
    }

    public static void DeleteAll()
    {
        PlayerPrefs.DeleteKey(SaveKey);
        PlayerPrefs.Save();
    }

    public static string FormatTime(float time)
    {
        int min = Mathf.FloorToInt(time / 60f);
        int sec = Mathf.FloorToInt(time % 60f);
        return min + ":" + sec.ToString("00");
    }
}
