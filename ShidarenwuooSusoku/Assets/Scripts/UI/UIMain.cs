using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIMain : UIBehaviour
{
    [SerializeField]
    private Sudoku.SudokuGrid board;

    [SerializeField]
    [Range(2, 4)]
    private int level;

    [SerializeField]
    [Range(0, 1)]
    private float difficult;

    [SerializeField] 
    private Button button;

    private void Awake()
    {
        button.onClick.AddListener(CreateSudoku);
    }

    public void CreateSudoku()
    {
        board.CreateSudoku(level, difficult);
    }
}
