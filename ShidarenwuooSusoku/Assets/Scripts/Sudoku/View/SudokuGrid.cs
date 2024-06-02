using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku
{
    public class SudokuGrid : UIBehaviour
    {
        [SerializeField]
        private SudokuCell cellPrefab;
        [SerializeField]  
        private SudokuSubGrid subGridPrefab;

        private SudokuLogic logic = new SudokuLogic();

        private List<SudokuCell> cells = new List<SudokuCell>();
        private List<SudokuSubGrid> subGrids = new List<SudokuSubGrid>();

        public void CreateSudoku(int level, float difficult)
        {
            logic.CreateNewPuzzle(level, difficult);
            CreateGrid();
        }

        public void CreateGrid()
        {
            int gridCount = logic.Level * logic.Level;
            // 根据level创建子网格
            for (int i = 0; i < gridCount; ++i)
            {
                SudokuSubGrid grid;
                if (i < subGrids.Count)
                {
                    grid = subGrids[i];
                }
                else
                {
                    grid = GameObject.Instantiate(subGridPrefab, this.rectTransform);
                    subGrids.Add(grid);
                }

                // 做初始化
                grid.name = $"Grid{i}";
                grid.gameObject.SetActive(true);
                grid.SetCellSize(cellPrefab.rectTransform.rect.size);
                grid.Init(logic.Level, i);
            }

            int cellCount = logic.Length * logic.Length;
            for (int i = 0; i < cellCount; ++i)
            {
                SudokuCell cell;
                if (i < cells.Count)
                {
                    cell = cells[i];
                }
                else
                {
                    cell = GameObject.Instantiate(cellPrefab, this.rectTransform);
                    cells.Add(cell);
                }
                
                // 做初始化
                cell.gameObject.SetActive(true);
                int row = i / logic.Length;
                int column = i % logic.Length;
                cell.name = $"Cell{row}{column}";
                int value = logic.GetCellValue(row, column);
                if (value > 0)
                {
                    cell.SetNumber(value);
                    cell.DisableEditor();
                }
                else
                {
                    cell.Clear();
                    cell.EnableEditor();
                }

                int gridIndex = row / logic.Level * logic.Level + column / logic.Level;
                subGrids[gridIndex].AddCell(cell, row, column);
            }
            
            
            // 处理多余的GameObject
            for (int i = gridCount; i < subGrids.Count; ++i)
            {
                subGrids[i].gameObject.SetActive(false);
            }
            for (int i = cellCount; i < cells.Count; ++i)
            {
                cells[i].gameObject.SetActive(false);
            }
        }


        private void Awake()
        {
            cellPrefab.gameObject.SetActive(false);
            subGridPrefab.gameObject.SetActive(false);
        }
    }
}