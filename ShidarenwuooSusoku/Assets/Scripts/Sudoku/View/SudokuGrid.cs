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

        private List<SudokuCell> cells = new List<SudokuCell>();
        private List<SudokuSubGrid> subGrids = new List<SudokuSubGrid>();

        private SudokuLogic logic;
        
        public event Action<int> OnClickCell;
        
        public void CreateSudoku(SudokuLogic logicInstance)
        {
            this.logic = logicInstance;
            CreateGrid();
            
            // 根据当前Grid的大小进行缩放
            float length = subGrids[0].rectTransform.rect.size.x * logic.Level;
            float scale = rectTransform.rect.size.x / length;
            rectTransform.localScale = new Vector3(scale, scale, 1);
        }

        public void ChangeCellValue(int index, int value)
        {
            if (index < 0 || index >= cells.Count)
            {
                return;
            }
            
            var cell = cells[index];
            if (!cell.CanEdite())
            {
                return;
            }
            
            cell.SetNumber(value);
            // 判断是否有重复项
            logic.AnswerPuzzle(index, value);
            
            // 刷新Cell状态
            RefreshCellState();
        }
        
        
        private void CreateGrid()
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
                    cell.OnClickCell += index => OnClickCell?.Invoke(index);
                }
                
                // 做初始化
                cell.gameObject.SetActive(true);
                int row = i / logic.Length;
                int column = i % logic.Length;
                cell.name = $"Cell{row}{column}";
                int value = logic.GetCellValue(row, column);
                cell.SetIndex(i);
                if (value > 0)
                {
                    cell.SetNumber(value);
                    cell.DisableEdite();
                }
                else
                {
                    cell.Clear();
                    cell.EnableEdite();
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

        private void RefreshCellState()
        {
            for (int i = 0; i < cells.Count; ++i)
            {
                var cell = cells[i];
                if (cell.CanEdite())
                {
                    cell.SetState(!logic.IsConflictAt(i));
                }
            }
        }
        
        private void Awake()
        {
            cellPrefab.gameObject.SetActive(false);
            subGridPrefab.gameObject.SetActive(false);
        }
    }
}