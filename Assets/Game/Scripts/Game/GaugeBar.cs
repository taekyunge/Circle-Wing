using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GaugeBar : MonoBehaviour
{
    [SerializeField] private Transform fillRect;
    [SerializeField] private float minValue;
    [SerializeField] private float maxValue;

    public float value = 0;

    private void Update()
    {
        var scale = fillRect.localScale;

        if (value < minValue)
            value = minValue;

        if (value > maxValue)
            value = maxValue;

        scale.x = (value - minValue) / (maxValue - minValue);

        fillRect.localScale = scale;
    }
}
