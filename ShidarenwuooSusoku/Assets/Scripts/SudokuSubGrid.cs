using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 大网格下的小网格
/// </summary>
public class SudokuSubGrid : MonoBehaviour
{
    public Vector2Int coordinate; //子网格所属的坐标
    public SudokuCell[,] cells { get; private set; }
    
    // 创建所属的单元格
    private void Awake()
    {
        cells = new SudokuCell[SudokuGameManager.cellLength, SudokuGameManager.cellLength];
    }

    // 设置行列坐标
    public void SetCoordinate(int row, int col)
    {
        coordinate = new Vector2Int(row, col);
    }
    
    // 初始化网格
    public void InitCells()
    {
        for (int i = 0; i < SudokuGameManager.cellLength; i++)
        {
            for (int j = 0; j < SudokuGameManager.cellLength; j++)
            {
                cells[i,j] = Instantiate(SudokuGameManager.Instance.SudokuCell_Prefab,transform);
                cells[i,j].SetCoordinate(coordinate.x * SudokuGameManager.cellLength + i,coordinate.y * SudokuGameManager.cellLength +j);
                cells[i,j].SetSubGridParent(this);
                cells[i,j].InitValues(1);
            }
        }
        
        
       
        
       
       
    }
    
}