using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI_Prefab;
using UnityEngine;
using UnityEngine.PlayerLoop;
using UnityEngine.UIElements;
using Button = UnityEngine.UI.Button;
using Text = UnityEngine.UI.Text;

public class SettingsSlider : CustomUIComponent
{
    public Button decrementButton;
    public TextMeshProUGUI sliderName;
    [SerializeField] public UnityEngine.UI.Slider slidingBar;
    public TextMeshProUGUI amount;
    public Button incrementButton;
    private float currentAmount = 0f;
    private float minValue;
    private float maxValue;
    public void Initialize(string name, int start,int min, int max )
    {
        currentAmount = start;
        minValue = min;
        maxValue = max;

        Refresh();
        Init();
        sliderName.text = name;
        slidingBar.value = currentAmount;
    }

    public void Refresh()
    {
        slidingBar.minValue = minValue;
        slidingBar.maxValue = maxValue;
    }

    public override void Setup()
    {
        slidingBar.onValueChanged.RemoveAllListeners();
        incrementButton.onClick.RemoveAllListeners();
        decrementButton.onClick.RemoveAllListeners();
    }

    public override void Configure()
    {
        slidingBar.onValueChanged.AddListener(OnSliderValueChanged);
        incrementButton.onClick.AddListener(IncrementAmount);
        decrementButton.onClick.AddListener(DecrementAmount);
    }
    private void OnSliderValueChanged(float value)
    {
        // When the slider value changes, update the amount
        currentAmount = value;
        amount.text = currentAmount.ToString();
    }

    private void IncrementAmount()
    {
        // Increment the amount and move the slider right by one
        currentAmount++;
        if (currentAmount > slidingBar.maxValue)
        {
            currentAmount = slidingBar.maxValue;
        }
        slidingBar.value = currentAmount;
        amount.text = currentAmount.ToString();
    }

    private void DecrementAmount()
    {
        // Decrement the amount and move the slider left by one
        currentAmount--;
        if (currentAmount < slidingBar.minValue)
        {
            currentAmount = slidingBar.minValue;
        }
        slidingBar.value = currentAmount;
        amount.text = currentAmount.ToString();
    }
}