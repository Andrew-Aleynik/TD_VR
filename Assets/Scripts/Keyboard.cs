using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Keyboard : MonoBehaviour
{
    private string Name;
    private int max_length = 14;
    public Tops tops;
    public TextMeshProUGUI text;
    public TextMeshProUGUI result;

    private void Awake()
    {
        this.gameObject.SetActive(DataContainer.game_happend);
        if (DataContainer.game_happend)
        {
            DataContainer.game_happend = false;
            result.text = $"Ваш результат: {DataContainer.score} очков. Волн пережито: {DataContainer.waves}"; 
        }
    }
    private void Start()
    {
        Name = "";
    }
    public void EnterSymbol(string Symb)
    {
        if (Name.Length < max_length)
        {
            Name += Symb;
            UpdateDisplay(Name);
        }
    }

    public void EraseSymbol()
    {
        if (Name.Length > 0)
        {
            Name = Name.Remove(Name.Length - 1);
            UpdateDisplay(Name);
        }
    }

    public void Enter()
    {
        tops.AddData(Name, DataContainer.score, DataContainer.waves);
        Name = "";
        gameObject.SetActive(false);
        UpdateDisplay(Name);
    }

    public void UpdateDisplay(string data_to_update)
    {
        text.text = data_to_update;
    }
}
