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

        private bool _created;
        private bool _finished;

        /// <summary>
        /// 是否已经创建好可用的数独谜题
        /// </summary>
        public bool IsCreated => _created;

        /// <summary>
        /// 是否已经解决创建好的谜题
        /// </summary>
        public bool Finished => _created && _finished;

        public int GetCellValue(int row, int column)
        {
            return gridNumber[row, column];
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
            for (int i = 0; i < Length && emptyNum > 0; ++i)
            {
                for (int j = 0; j < Length && emptyNum > 0; ++j)
                {
                    int randomValue = Random.Range(0, totalNum);
                    if (randomValue < emptyNum)
                    {
                        puzzleNumber[i, j] = 0;
                        --emptyNum;
                    }
                    --totalNum;
                }
            }

            // 检查并确保所有不同的数字都出现过
            
              

            // 将谜题备份用于重开本局游戏
            System.Array.Copy(puzzleNumber, gridNumber, gridNumber.Length);
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
            if (IsContainInRow(puzzle, row, value))
            {
                return false;
            }

            if (IsContainInColumn(puzzle, column, value))
            {
                return false;
            }

            if (IsContainInSubgrid(puzzle, row, column, value))
            {
                return false;
            }

            return true;
        }

        private bool IsContainInRow(int[,] puzzle, int row, int value)
        {
            for (int i = 0; i < _length; ++i)
            {
                if (puzzle[row, i] == value)
                {
                    return true;
                }
            }

            return false;
        }
        
        private bool IsContainInColumn(int[,] puzzle, int column, int value)
        {
            for (int i = 0; i < _length; ++i)
            {
                if (puzzle[i, column] == value)
                {
                    return true;
                }
            }

            return false;
        }
        
        private bool IsContainInSubgrid(int[,] puzzle, int row, int column, int value)
        {
            int gridRow = row / _level;
            int gridColumn = column / _level;
            
            for (int i = 0; i < _level; ++i)
            {
                for(int j = 0;j<_level;++j)
                {
                    if (puzzle[gridRow * _level + i, gridColumn * _level + j] == value)
                    {
                        return true;
                    }
                }
            }

            return false;
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
    }
}