using System;
using UnityEngine;
using UnityEngine.UI;

public class HPBar : MonoBehaviour
{
    public Slider hpSlider;

    public void SetValue(int maxHealth, int currentHealth)
    {
        float value = Math.Max((float)currentHealth / maxHealth, 0);
        hpSlider.value = value;
    }

    public void Show() 
    {
        if (hpSlider != null)
        {
            hpSlider.gameObject.SetActive(true);
        }
    }

    public void Hide()
    {
        if (hpSlider != null)
        {
            hpSlider.gameObject.SetActive(false);
        }
    }
}