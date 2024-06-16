using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UISudokuPanel : UIPanel
{
    [SerializeField]
    private Sudoku.SudokuGrid board;

    [SerializeField] 
    private Button returnButton;

    [SerializeField]
    private Button mainMenuButton;

    private void Awake()
    {
        returnButton.onClick.AddListener(ReturnToLevelSelection);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
    }

    public override void Open()
    {
        
    }

    public void CreateSudoku(int level, float difficult)
    {
        board.CreateSudoku(level, difficult);
    }

    public void ReturnToLevelSelection()
    {
        UIManager.Instance.OpenUI(UINames.UILevelSelection);
        
        Close();
    }

    public void ReturnToMainMenu()
    {
        UIManager.Instance.OpenUI(UINames.UIStartUp);
        
        Close();
    }
}
