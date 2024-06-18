using System;
using System.Collections;
using System.Collections.Generic;
using Sudoku;
using UnityEngine;
using UnityEngine.UI;

public class UISudokuPanel : UIPanel
{
    [SerializeField]
    private Sudoku.SudokuGrid board;

    [SerializeField] 
    private UISudokuInput inputPanel;
    
    [SerializeField] 
    private Button returnButton;

    [SerializeField]
    private Button mainMenuButton;

    [SerializeField]
    private GameObject winRoot;
    
    [SerializeField]
    private Button nextLevelButton;
    
    private SudokuLogic logic = new SudokuLogic();
    
    private int currentEditorIndex;

    private int level;
    
    private void Awake()
    {
        returnButton.onClick.AddListener(ReturnToLevelSelection);
        mainMenuButton.onClick.AddListener(ReturnToMainMenu);
        
        nextLevelButton.onClick.AddListener(OnClickFinishButton);
        
        winRoot.SetActive(false);
    }

    public override void Open()
    {
        
    }

    public void CreateSudoku(int level)
    {
        this.level = level;
        var config = SudokuConfig.Instance.GetLevelPuzzle(level);
        logic.CreateNewPuzzle(config.level, config.difficult);
        board.CreateSudoku(logic);
        inputPanel.CreateInputPanel(logic.Length);
        inputPanel.ChangeScale(board.rectTransform.localScale.x);
        inputPanel.Hide();

        board.OnClickCell -= OnClickCell;
        board.OnClickCell += OnClickCell;

        inputPanel.OnClickInput -= OnClickInputValue;
        inputPanel.OnClickInput += OnClickInputValue;
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

    private void OnClickCell(int index)
    {
        currentEditorIndex = index;
        inputPanel.Show();
    }

    private void OnClickInputValue(int value)
    {
        board.ChangeCellValue(currentEditorIndex, value);
        inputPanel.Hide();

        if (logic.Finished)
        {
            winRoot.SetActive(true);
            GameData.SaveLevel(level + 1);
        }
    }

    private void OnClickFinishButton()
    {
        Close();

        UIManager.Instance.OpenUI(UINames.UILevelSelection);
    }
}
