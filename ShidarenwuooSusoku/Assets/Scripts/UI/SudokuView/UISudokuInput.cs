using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISudokuInput : UIBehaviour
{
    [SerializeField]
    private UIInputCell inputCellPrefab;

    [SerializeField]
    private RectTransform inputPanelRoot;

    [SerializeField] 
    private Button cancelButton;

    public event Action<int> OnClickInput;

    private void Awake()
    {
        cancelButton.onClick.AddListener(Hide);
    }

    public void CreateInputPanel(int range)
    {
        inputCellPrefab.gameObject.SetActive(true);
        for (int i = 1; i <= range; ++i)
        {
            var cell = GameObject.Instantiate(inputCellPrefab, inputPanelRoot);
            cell.SetValue(i);
            cell.OnClick += OnClickCell;
        }
        var nullCell = GameObject.Instantiate(inputCellPrefab, inputPanelRoot);
        nullCell.SetValue(0);
        nullCell.OnClick += OnClickCell;
        inputCellPrefab.gameObject.SetActive(false);
    }

    public void ChangeScale(float scale)
    {
        inputPanelRoot.localScale = new Vector3(scale, scale, 1);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    private void OnClickCell(int index)
    {
        OnClickInput?.Invoke(index);
    }
}
