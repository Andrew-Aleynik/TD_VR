using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

[System.Serializable]
public class RecordData
{
    public float time;
    public int wave;
    public int score;
    public string playerName;
    
    public string GetFormattedTime()
    {
        int minutes = (int)(time / 60);
        int seconds = (int)time % 60;
        return $"{minutes:D2}:{seconds:D2}";
    }
}

public class RecordManager : MonoBehaviour
{
    private string filePath;
    private List<RecordData> records;
    public const int MAX_RECORDS = 10;

    private void Awake()
    {
        filePath = Path.Combine(Application.persistentDataPath, "game_records.json");
        LoadRecords();
    }

    public void AddRecord(float time, int wave, int score, string playerName = "Player")
    {
        RecordData newRecord = new RecordData()
        {
            time = time,
            wave = wave,
            score = score,
            playerName = playerName
        };

        
        records.Add(newRecord);
        
        records = records
            .OrderBy(r => r.score)
            .Take(MAX_RECORDS)
            .ToList();

        SaveToFile();
    }

    public void ClearRecords()
    {
        records.Clear();
        SaveToFile();
    }

    private void LoadRecords()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string json = File.ReadAllText(filePath);
                records = JsonUtility.FromJson<List<RecordData>>(json);
                    
                records = records
                    .OrderBy(r => r.score)
                    .Take(MAX_RECORDS)
                    .ToList();
            }
            catch (Exception e)
            {
                Debug.LogError("Error loading records: " + e.Message);
                records = new List<RecordData>();
            }
        }
        else
        {
            records = new List<RecordData>();
        }
    }

    private void SaveToFile()
    {
        try
        {
            string json = JsonUtility.ToJson(records, true);
            File.WriteAllText(filePath, json);
            Debug.Log("Records saved successfully");
        }
        catch (Exception e)
        {
            Debug.LogError("Error saving records: " + e.Message);
        }
    }
}