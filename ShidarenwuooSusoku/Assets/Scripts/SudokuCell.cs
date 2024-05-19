using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class SudokuCell : MonoBehaviour
{
    public Vector2Int coordinate; // 在大网格里坐标
    public SudokuSubGrid subGrid;
    int value = 0; // 当前格子的值
    public TextMeshProUGUI txtNumber;// 数字控件
    public Button btnNum;// 按钮控件

    void Awake()
    {
        btnNum = GetComponent<Button>();
        txtNumber = GetComponentInChildren<TextMeshProUGUI>();
        btnNum.onClick.AddListener(ButtonClicked);
    }


    // 给网格设置数字
    public void InitValues(int value)
    {
        // 初始化的时候，不为0表示该位置是系统提供的数字，否则就是玩家应该输入的数字
        if (value != 0)
        {
            txtNumber.text = value.ToString();
            txtNumber.color = new Color32(119, 110, 101, 255);
            btnNum.enabled = false;
        }
        else
        {
            btnNum.enabled = true;
            txtNumber.text = " ";
            txtNumber.color = new Color32(0, 102, 187, 255);
        }
    }

    // 设置行列坐标
    public void SetCoordinate(int row, int col)
    {
        coordinate = new Vector2Int(row, col);
        name = row.ToString() + col.ToString();
    }

    // 设置其归属的子网格
    public void SetSubGridParent(SudokuSubGrid sub)
    {
        subGrid = sub;
    }
    
    /// <summary>
    /// 按钮事件
    /// </summary>
    public void ButtonClicked()
    {
        SudokuGameManager.Instance.ActivateInputButton(this);
    }

    /// <summary>
    /// 更新单元格内的数字
    /// </summary>
    /// <param name="newValue"></param>
    public void UpdateValue(int newValue)
    {
        value = newValue;

        if (value != 0)
        {
            txtNumber.text = value.ToString();
        }
        else
        {
            txtNumber.text = "";
        }
    }
}