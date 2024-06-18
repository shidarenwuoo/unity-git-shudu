using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sudoku
{
    /// <summary>
    /// 用于执行数独逻辑，独立于表现层
    /// </summary>
    public class SudokuLogic
    {
        private int _level;
        private int _length;
        private float _difficulty;

        public int Level => _level;
        public int Length => _length;
        public float difficulty => _difficulty;
        
        /// <summary>
        /// 生成的数独最终解(不确保唯一性),可作为参考答案
        /// </summary>
        private int[,] puzzleAnswer;

        /// <summary>
        /// 初始谜题，用于重设状态所用
        /// </summary>
        private int[,] puzzleNumber;

        /// <summary>
        /// 储存玩家解题过程中的中间值
        /// </summary>
        private int[,] gridNumber;

        /// <summary>
        /// 解题过程中产生的冲突位置
        /// </summary>
        private List<int> conflictIndexes = new List<int>();

        private int puzzleCount;

        private bool _finished;

        /// <summary>
        /// 是否已经解决创建好的谜题
        /// </summary>
        public bool Finished => puzzleCount <= 0 && conflictIndexes.Count <= 0;

        /// <summary>
        /// 获取答题过程中的数值
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public int GetCellValue(int row, int column)
        {
            return gridNumber[row, column];
        }

        /// <summary>
        /// 清空以作答的内容
        /// </summary>
        public void ClearPuzzleAnswer()
        {
            puzzleCount = 0;
            for (int i = 0; i < _length; ++i)
            {
                for (int j = 0; j < _length; ++j)
                {
                    int value = puzzleNumber[i, j];
                    gridNumber[i, j] = value;
                    if (value == 0)
                    {
                        ++puzzleCount;
                    }
                }
            }
        }

        /// <summary>
        /// 解答当前谜题
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <param name="value"></param>
        public void AnswerPuzzle(int row, int column, int value)
        {
            if (puzzleNumber[row, column] != 0)
            {
                // 输入位置是题面值，不能修改，题面值永远是合法值
                return;
            }

            int originValue = gridNumber[row, column];
            gridNumber[row, column] = value;
            if (originValue == 0 && value != 0)
            {
                --puzzleCount;
            }
            else if (originValue != 0 && value == 0)
            {
                ++puzzleCount;
            }
            
            // 刷新当前冲突情况
            RefreshConflictInfo(row, column);
        }

        public void AnswerPuzzle(int index, int value)
        {
            int row = index / _length;
            int column = index % _length;
            AnswerPuzzle(row, column, value);
        }

        public bool IsConflictAt(int index)
        {
            return conflictIndexes.Contains(index);
        }
        
        /// <summary>
        /// 获取参考答案
        /// </summary>
        /// <param name="row"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public int GetReferenceAnswer(int row, int column)
        {
            return puzzleAnswer[row, column];
        }
        
        public void CreateNewPuzzle(int level, float hard)
        {
            if (level < 2)
            {
                Debug.LogWarning("数独等级至少要为2，自动将等级调整至2.");
                level = 2;
            }

            _level = level;
            _length = level * level;
            _difficulty = Mathf.Clamp01(hard);
            CreatePuzzleInternal();
        }

        public int PositionConvertToIndex(int row, int column)
        {
            return row * _length + column;
        }

        private void CreatePuzzleInternal()
        {
            if (_level < 2)
            {
                Debug.LogError("数独等级小于2，无法生成！");
                return;
            }

            gridNumber = new int[_length, _length];
            puzzleAnswer = new int[_length, _length];
            puzzleNumber = new int[_length, _length];
            CreatePuzzleAnswer();
            CreatePuzzleFromAnswer();
        }

        /// <summary>
        /// 随机创建一个完成的满足要求的数独
        /// </summary>
        private void CreatePuzzleAnswer()
        {
            List<int> numberList = new List<int>();
            for (int i = 1; i <= _length; ++i)
            {
                numberList.Add(i);
            }

            // 随机分配第一行
            for (int i = 0; i < _length; ++i)
            {
                int randomIndex = Random.Range(0, _length - i);
                int value = numberList[randomIndex];
                puzzleAnswer[0, i] = value;
                // 将随机出来的数字放置到列表最后，防止被重复选中
                numberList[randomIndex] = numberList[_length - i - 1];
                numberList[_length - i - 1] = value;
            }

            // 随机分配第一列
            // 先处理前level个数，避免在同一个子网格内重复
            for (int i = 1; i < _level; ++i)
            {
                // 第一个子网格的数已经放在数组后面了，随机的时候避免取值即可
                int randomIndex = Random.Range(0, _length - i - _level);
                int value = numberList[randomIndex];
                puzzleAnswer[i, 0] = value;

                // 交换数值位置，防止被重复选中
                numberList[randomIndex] = numberList[_length - i - _level - 1];
                numberList[_length - i - _level - 1] = numberList[_length - i - 1];
                numberList[_length - i - 1] = value;
            }

            // 继续分配剩下数值
            for (int i = _level; i < _length; ++i)
            {
                int randomIndex = Random.Range(0, _length - i);
                int value = numberList[randomIndex];
                puzzleAnswer[i, 0] = value;
                // 将随机出来的数字放置到列表最后，防止被重复选中
                numberList[randomIndex] = numberList[_length - i - 1];
                numberList[_length - i - 1] = value;
            }

            // 为最后的子网格添加_level个合法数字(这个算法目前只对3x3的有效，暂时移除)
            // for (int i = _length - 1; i > _length - 1 - _level; --i)
            // {
            //     // 整理可用于随机的数字
            //     int randomRange = _length;
            //     // 移除对应列
            //     int invalidNum = puzzleAnswer[i, 0];
            //     int invalidNumIndex = numberList.FindIndex(v => v == invalidNum);
            //     if (invalidNumIndex >= _length - randomRange)
            //     {
            //         numberList[invalidNumIndex] = numberList[_length - randomRange];
            //         numberList[_length - randomRange] = invalidNum;
            //         --randomRange;
            //     }
            //     // 移除对应行
            //     invalidNum = puzzleAnswer[0, i];
            //     invalidNumIndex = numberList.FindIndex(v => v == invalidNum);
            //     if (invalidNumIndex >= _length - randomRange)
            //     {
            //         numberList[invalidNumIndex] = numberList[_length - randomRange];
            //         numberList[_length - randomRange] = invalidNum;
            //         --randomRange;
            //     }
            //     // 移除相同子网格
            //     for (int j = i + 1; j < _length; ++j)
            //     {
            //         invalidNum = puzzleAnswer[j, j];
            //         invalidNumIndex = numberList.FindIndex(v => v == invalidNum);
            //         if (invalidNumIndex >= _length - randomRange)
            //         {
            //             numberList[invalidNumIndex] = numberList[_length - randomRange];
            //             numberList[_length - randomRange] = invalidNum;
            //             --randomRange;
            //         }
            //     }
            //     
            //     int randomIndex = Random.Range(0, randomRange);
            //     int value = numberList[_length - randomIndex - 1];
            //     puzzleAnswer[i, i] = value;
            // }

            if (!SolveSudoku(puzzleAnswer, 0))
            {
                Debug.LogError("随机出来的数独无解！！！");
                ClearPuzzle(puzzleAnswer, Length);
                // 重新随机新的谜题
                CreatePuzzleAnswer();
            }
        }

        private void CreatePuzzleFromAnswer()
        {
            System.Array.Copy(puzzleAnswer, puzzleNumber, puzzleAnswer.Length);

            int minEmptyNum = Level * Level;
            int maxEmptyNum = (Length - 1) * (Length - 1);
            int emptyNum = (int)(minEmptyNum * (1 - difficulty) + maxEmptyNum * difficulty);
            int totalNum = Length * Length;
            puzzleCount = 0;
            for (int i = 0; i < Length && emptyNum > 0; ++i)
            {
                for (int j = 0; j < Length && emptyNum > 0; ++j)
                {
                    int randomValue = Random.Range(0, totalNum);
                    if (randomValue < emptyNum)
                    {
                        puzzleNumber[i, j] = 0;
                        ++puzzleCount;
                        --emptyNum;
                    }
                    --totalNum;
                }
            }

            ClearPuzzleAnswer();
        }

        private bool SolveSudoku(int[,] puzzle, int startIndex)
        {
            int emptyRow = 0;
            int emptyColumn = 0;
            bool hasSolved = true;

            int count = _length * _length;
            for (int i = startIndex; i < count; ++i)
            {
                int row = i / _length;
                int column = i % _length;
                if (puzzle[row, column] <= 0)
                {
                    emptyRow = row;
                    emptyColumn = column;
                    hasSolved = false;
                    break;
                }
            }

            if (hasSolved)
            {
                return true;
            }
            
            for (int i = 1; i <= _length; ++i)
            {
                if (IsValidNumberInSudoku(puzzle, emptyRow, emptyColumn, i))
                {
                    puzzle[emptyRow, emptyColumn] = i;

                    if (SolveSudoku(puzzle, emptyRow * _length + emptyColumn))
                    {
                        return true;
                    }
                    else
                    {
                        puzzle[emptyRow, emptyColumn] = 0;
                    }
                }
            }

            return false;
        }

        private bool IsValidNumberInSudoku(int[,] puzzle, int row, int column, int value)
        {
            if (value == 0)
            {
                return true;
            }
            
            for (int i = 0; i < _length; ++i)
            {
                if (i != column && puzzle[row, i] == value)
                {
                    return false;
                }
            }

            for (int i = 0; i < _length; ++i)
            {
                if (i != row && puzzle[i, column] == value)
                {
                    return false;
                }
            }

            int gridRow = row / _level;
            int gridColumn = column / _level;
            for (int i = gridRow * _level; i < gridRow * _level + _level; ++i)
            {
                if (i == row)
                {
                    continue;
                }

                for (int j = gridColumn * _level; j < gridColumn * _level + _level; ++j)
                {
                    if (j == column)
                    {
                        continue;
                    }

                    if (puzzle[i, j] == value)
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        private void ClearPuzzle(int[,] array, int length)
        {
            for(int i = 0; i < length; ++i)
            {
                for(int j = 0; j < length; ++j)
                {
                    array[i, j] = 0;
                }
            }
        }

        private void RefreshConflictInfo(int row, int column)
        {
            RefreshSingleConflictInfo(row, column);
            
            // 更新所有与其有关的row, column, grid的冲突情况
            for (int i = 0; i < _length; ++i)
            {
                if (i == column)
                {
                    continue;
                }
                RefreshSingleConflictInfo(row, i);
            }

            for (int i = 0; i < _length; ++i)
            {
                if (i == row)
                {
                    continue;
                }
                RefreshSingleConflictInfo(i, column);
            }

            int gridRow = row / _level;
            int gridColumn = column / _level;
            for (int i = gridRow * _level; i < gridRow * _level + _level; ++i)
            {
                if (i == row)
                {
                    continue;
                }

                for (int j = gridColumn * _level; j < gridColumn * _level + _level; ++j)
                {
                    if (j == column)
                    {
                        continue;
                    }

                    RefreshSingleConflictInfo(i, j);
                }
            }
        }

        private void RefreshSingleConflictInfo(int row, int column)
        {
            if (puzzleNumber[row, column] != 0)
            {
                // 属于谜面，无需判断
                return;
            }
            
            int targetIndex = PositionConvertToIndex(row, column);

            bool isValid = IsValidNumberInSudoku(gridNumber, row, column, gridNumber[row, column]);
            bool isConflict = conflictIndexes.Contains(targetIndex);
            if (!isConflict && isValid)
            {
                return;
            }
            
            // 更新所有相关的冲突情况
            if (!isConflict)
            {
                conflictIndexes.Add(targetIndex);
            }
            else if (isValid)
            {
                conflictIndexes.Remove(targetIndex);
            }
        }
    }
}