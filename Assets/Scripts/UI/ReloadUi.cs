using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ReloadUi : MonoBehaviour
{
    Slider slider;
    private float minValue;
    private float maxValue;

    private void Awake()
    {
        slider = GetComponent<Slider>();
    }

    private void Update()
    {
        slider.value = minValue / maxValue;
    }

    public void setReload(float _reLoadTimer, float _maxReLoadTimer)
    {
        minValue = _reLoadTimer;
        maxValue = _maxReLoadTimer;
    }
}
