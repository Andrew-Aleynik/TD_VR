using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tops : MonoBehaviour
{
    private List<Tuple<string, int>> _tops_array;
    public TextMeshProUGUI text_mesh;
    private void OnEnable()
    {
        _tops_array = new List<Tuple<string, int>>();
        LoadData();
        UpdateDisplay();
    }
    public void AddData(string Name, int score)
    {
        _tops_array.Add(new Tuple<string, int>(Name, score));
        _tops_array.Sort((a, b) => a.Item2.CompareTo(b.Item2));
        if (_tops_array.Count > 10)
        {
            _tops_array.RemoveRange(10, _tops_array.Count - 10 + 1);
        }
        SaveData();
        UpdateDisplay();
    }

    private void LoadData()
    {
        if (PlayerPrefs.HasKey("Tops"))
        {
            var _tops_string = PlayerPrefs.GetString("Tops");
            var _array = _tops_string.Split("; ", StringSplitOptions.RemoveEmptyEntries);
            foreach(var _tops in _array)
            {
                var _data = _tops.Split("/", StringSplitOptions.RemoveEmptyEntries);
                _tops_array.Add(new Tuple<string, int>(_data[0], int.Parse(_data[1])));
            }
            _tops_array.Sort((a, b) => a.Item2.CompareTo(b.Item2));
        }
    }

    private void SaveData()
    {
        var string_to_save = MakeStringFromData(_tops_array);
        PlayerPrefs.SetString("Tops", string_to_save);
        PlayerPrefs.Save();
    }

    private string MakeStringFromData(List<Tuple<string, int>> _data)
    {
        var string_to_save = string.Empty;
        foreach(var elem in _data)
        {
            string_to_save += elem.Item1 + "/" + elem.Item2 + "; ";
        }
        return string_to_save;
    }

    private void UpdateDisplay()
    {
        text_mesh.text = string.Empty;

        var num = 1;
        foreach (var elem in _tops_array)
        {
            text_mesh.text += num.ToString() + ". " + elem.Item1 + " ________ " + elem.Item2 + " очков\n" ;
            num += 1;
        }
    }
}
