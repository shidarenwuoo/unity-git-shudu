using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIInputCell : UIBehaviour
{
    [SerializeField]
    private TextMeshProUGUI valueText;

    [SerializeField]
    private Button button;

    private int value;

    public event Action<int> OnClick;
    
    private void Awake()
    {
        button.onClick.AddListener(() =>
        {
            OnClick?.Invoke(value);
        });
    }

    public void SetValue(int value)
    {
        this.value = value;
        valueText.text = value == 0 ? "" : value.ToString();
    }
}
