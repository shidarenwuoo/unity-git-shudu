using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// 数独的大网格
/// </summary>
public class SudokuGrid : MonoBehaviour
{
    // 所有的子网格 需要处理成二维数组
    public SudokuSubGrid[,] subGrids { get; private set; }// 子网格
    public SudokuCell[] cells;// 所有的单元格
    
    void Awake()
    {
        // 获取所有子网格体
        var grid = GetComponentsInChildren<SudokuSubGrid>();
        
        // 建立子网格的二维数组
        subGrids = new SudokuSubGrid[SudokuGameManager.subGirdLength, SudokuGameManager.subGirdLength];
        
        // 通过循环将二维数组分配到指定位置
        int index = 0;
        for (int i = 0; i < SudokuGameManager.subGirdLength; i++)
        {
            for (int j = 0; j < SudokuGameManager.subGirdLength; j++)
            {
                subGrids[i, j] = grid[index++];
                subGrids[i, j].SetCoordinate(i,j);// 设置坐标
                subGrids[i, j].InitCells();// 初始化网格
            }
        }
        cells = GetComponentsInChildren<SudokuCell>();
    }
    
    /// <summary>
    /// 根据坐标获取Cell
    /// </summary>
    /// <param name="row"></param>
    /// <param name="col"></param>
    /// <returns></returns>
    public SudokuCell GetCellByPosition(int row,int col)
    {
        foreach (var cell in cells)
        {
            if (cell.coordinate.x == row && cell.coordinate.y == col)
            {
                return cell;
            }
        }

        return null;
    }
    
    
}