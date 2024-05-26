using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Sudoku
{
    public class SudokuSubGrid : UIBehaviour
    {
        [SerializeField]
        private float _space;
        
        private Vector2 _cellSize;

        private int _level;
        private int _index;
        private Vector2 _gridSize;

        public void SetCellSize(Vector2 size)
        {
            _cellSize = size;
        }
        
        public void Init(int level, int index)
        {
            this._level = level;
            this._index = index;

            rectTransform.pivot = new Vector2(0.5f, 0.5f);

            _gridSize = _level * _cellSize + Vector2.one * _space * (_level + 1);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _gridSize.x - 0.5f * _space);
            rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _gridSize.y - 0.5f * _space);
            
            // 根据index计算位置
            float row = index / level;
            float column = index % level;
            Vector2 gridIndex = new Vector2(row, column);
            Vector2 t = gridIndex / (_level - 1) - new Vector2(0.5f, 0.5f);
            Vector2 position = _gridSize * (_level - 1) * t;
            rectTransform.localPosition = position;
        }

        public void AddCell(SudokuCell cell, int row, int column)
        {
            // 检查行列是否属于当前grid
            if (_index != row / _level * _level + column / _level)
            {
                Debug.LogError($"cell所在的行({row})和列({column})不属于当前子网格中({_index})");
                return;
            }
            
            cell.rectTransform.SetParent(rectTransform, false);
            // 计算cell对应grid的行和列
            Vector2 gridIndex = new Vector2(row % _level, column % _level);
            Vector2 t = gridIndex / (_level - 1) - new Vector2(0.5f, 0.5f);
            Vector2 position = (_cellSize + new Vector2(_space, _space)) * (_level - 1) * t;
            cell.rectTransform.localPosition = position;
        }
    }
}