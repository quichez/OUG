using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Quichez;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Image fill;
    [SerializeField] private TextMeshProUGUI text;

    public void Initialize(int value)
    {
        text.text = value.ToString();
    }

    public void UpdateHealthBar(HealthSystem healthSystem)
    {
        fill.fillAmount = healthSystem.ResourcePercentage;
        text.text = healthSystem.Current.Value.ToString() + "/"
            + healthSystem.Maximum.Value.ToString();
    }

    private void Start()
    {
        Debug.Log("Health Bar Started");
    }
}
