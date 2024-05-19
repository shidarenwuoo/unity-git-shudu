using System.Collections.Generic;
using UnityEngine;

public class SudokuBoard : MonoBehaviour
{
    // 正确答案
    int[,] gridNumber = new int[SudokuGameManager.girdLength, SudokuGameManager.girdLength];
   
    // 谜题
    int[,] puzzleNumber = new int[SudokuGameManager.girdLength, SudokuGameManager.girdLength];

    // 谜题备份，用于重开本局游戏
    int[,] puzzleBak = new int[SudokuGameManager.girdLength, SudokuGameManager.girdLength];

    public SudokuGrid grid;// 网格体
    public void Init()
    {
        // 创建一个有解的数独
        CreateGrid();
        
        // 根据已经创建的答案 来创建 谜题
        CreatePuzzle();
        
        // 根据谜题来初始化按钮
        InitButtons();

    }
    
    // 清理所有资源
    public void Clear()
    {
        // 清理谜题和答案数据
        gridNumber = new int[SudokuGameManager.girdLength, SudokuGameManager.girdLength];
        puzzleNumber = new int[SudokuGameManager.girdLength, SudokuGameManager.girdLength];
        puzzleBak = new int[SudokuGameManager.girdLength, SudokuGameManager.girdLength];
    }

    // 销毁某个物体下面的所有子物体
    void ClearChildren(Transform trans)
    {
        for (int i = 0; i < trans.childCount; i++)
        {
            Destroy(trans.GetChild(i).gameObject);
        }
    }
    
    // 某列是否包含某个数据
    bool ColumnContainsValue(int col, int value)
    {
        for (int i = 0; i < SudokuGameManager.girdLength; i++)
        {
            if (gridNumber[i, col] == value)
            {
                return true;
            }
        }

        return false;
    }
    
    // 某一行是否包含某个数字
    bool RowContainsValue(int row, int value)
    {
        for (int i = 0; i < SudokuGameManager.girdLength; i++)
        {
            if (gridNumber[row, i] == value)
            {
                return true;
            }
        }

        return false;
    }
    
    // 某一个子网格是否包含某个数字
    bool SquareContainsValue(int row, int col, int value)
    {
        // 遍历子单元格(3X3)
        for (int i = 0; i < SudokuGameManager.subGirdLength; i++)
        {
            for (int j = 0; j < SudokuGameManager.subGirdLength; j++)
            {
                // 通过计算坐标 确定是属于哪个子网格
                if (gridNumber[(row / SudokuGameManager.subGirdLength) * SudokuGameManager.cellLength + i, (col / SudokuGameManager.subGirdLength) * SudokuGameManager.cellLength + j] == value)
                {
                    return true;
                }
            }
        }
        return false;
    }

    // 检查某个数是否已经存在于 横 列以及某个3X3的网格里
    bool CheckAll(int row, int col, int value)
    {
        if (ColumnContainsValue(col, value)) // 列是否已经包含该数字
        {
            return false;
        }

        if (RowContainsValue(row, value)) // 行是否已经包含该数字
        {
            return false;
        }

        if (SquareContainsValue(row, col, value)) // 当前子九宫是否包含该数字
        {
            return false;
        }

        return true;
    }

    // 网格数字是否有效
    bool IsValid()
    {
        for (int i = 0; i < SudokuGameManager.girdLength; i++)
        {
            for (int j = 0; j < SudokuGameManager.girdLength; j++)
            {
                if (gridNumber[i, j] == 0)
                {
                    return false;
                }
            }
        }

        return true;
    }

    // 创建一个有效的数独网格
    void CreateGrid()
    {
        List<int> rowList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        List<int> colList = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        
        // 先在00位置随机一个数据
        int value = rowList[Random.Range(0, rowList.Count)];
        gridNumber[0, 0] = value;
        rowList.Remove(value);
        colList.Remove(value);

        // 将其他8个数字随机到第一行中
        for (int i = 1; i < SudokuGameManager.girdLength; i++)
        {
            value = rowList[Random.Range(0, rowList.Count)];
            gridNumber[i, 0] = value;
            rowList.Remove(value);
        }
        
        // 将其他几个数字随机到第一列中
        for (int i = 1; i < SudokuGameManager.girdLength; i++)
        {
            value = colList[Random.Range(0, colList.Count)];
            // 需要判断是否会和第一个子网格有重复
            if (i < 3)
            {
                while (SquareContainsValue(0, 0, value))
                {
                    value = colList[Random.Range(0, colList.Count)]; // reroll
                }
            }

            gridNumber[0, i] = value;
            colList.Remove(value);
        }

        // 再随机对最后一个子网格添加三个合法的数字
        for (int i = 6; i < 9; i++)
        {
            value = Random.Range(1, 10);
            while (SquareContainsValue(0, 8, value) || SquareContainsValue(8, 0, value) ||
                   SquareContainsValue(8, 8, value))
            {
                value = Random.Range(1, 10);
            }

            gridNumber[i, i] = value;
        }
        
        // 先随机生成一个数独的底子，然后对它求解，解出的答案就是完整数独
         SolveSudoku();
    }

    // 对数独求解
    bool SolveSudoku()
    {
        int row = 0;
        int col = 0;
        
        // 如果已经生成完毕 就返回结果
        if (IsValid())
        {
            return true;
        }
        
        // 找到还没有生成数字的位置
        for (int i = 0; i < SudokuGameManager.girdLength; i++)
        {
            for (int j = 0; j < SudokuGameManager.girdLength; j++)
            {
                if (gridNumber[i, j] == 0)
                {
                    row = i;
                    col = j;
                    break;
                }
            }
        }
        
        // 循环 找到合适的数字，满足所有的规则
        for (int i = 1; i <= SudokuGameManager.girdLength; i++)
        {
            if (CheckAll(row, col, i))
            {
                gridNumber[row, col] = i;
                
                // 递归找解 这里很重要，因为是对自身的递归，如果随机的数字正好全部都满足就结束了
                if (SolveSudoku())
                {
                    return true;
                }
                else //如果某次递归找不到解 就会将该位置之 
                {
                    gridNumber[row, col] = 0;
                }
            }
        }

        return false;
    }

    void CreatePuzzle()
    {
        // 根据事先完成的答案 创建谜题
        // 先将答案复制一份出来
        System.Array.Copy(gridNumber, puzzleNumber, gridNumber.Length);

        int difficulty = SudokuGameManager.Instance.difficulty;
        int totalNum = SudokuGameManager.girdLength * SudokuGameManager.girdLength;
        for (int i = 0; i < SudokuGameManager.girdLength && difficulty > 0; ++i)
        {
            for (int j = 0; j < SudokuGameManager.girdLength && difficulty > 0; ++j)
            {
                int randomValue = Random.Range(0, totalNum);
                if (randomValue < difficulty)
                {
                    puzzleNumber[i, j] = 0;
                    --difficulty;
                }
                --totalNum;
            }
        }

        //移除数字，制造难度
        // for (int i = 0; i < SudokuGameManager.Instance.difficulty; i++)
        // {
        //     int row = Random.Range(0, SudokuGameManager.girdLength);
        //     int col = Random.Range(0, SudokuGameManager.girdLength);
        //
        //     // 循环随机，直到随到一个没有处理过的位置
        //     while (puzzleNumber[row, col] == 0)
        //     {
        //         row = Random.Range(0, SudokuGameManager.girdLength);
        //         col = Random.Range(0, SudokuGameManager.girdLength);
        //     }
        //
        //     puzzleNumber[row, col] = 0;
        // }

        // 确保最少要出现8个不同的数字 才能保证唯一解
        List<int> onBoard = new List<int>() { 1, 2, 3, 4, 5, 6, 7, 8, 9 };
        RandomizeList(onBoard);

        for (int i = 0; i < SudokuGameManager.girdLength; i++)
        {
            for (int j = 0; j < SudokuGameManager.girdLength; j++)
            {
                for (int k = 0; k < onBoard.Count - 1; k++)
                {
                    if (onBoard[k] == puzzleNumber[i, j])
                    {
                        onBoard.RemoveAt(k);
                    }
                }
            }
        }

        // 如果剩余的数量大于1 说明没有8个不同的数字 那么就还原几个数字回来
        while (onBoard.Count - 1 > 1)
        {
            int row = Random.Range(0, SudokuGameManager.girdLength);
            int col = Random.Range(0, SudokuGameManager.girdLength);

            if (gridNumber[row, col] == onBoard[0])
            {
                puzzleNumber[row, col] = gridNumber[row, col];
                onBoard.RemoveAt(0);
            }
        }
        
        // 将谜题备份用于重开本局游戏
        System.Array.Copy(puzzleNumber, puzzleBak, gridNumber.Length);
    }

    // 初始化可填写的按钮布局
    void InitButtons()
    {
        for (int i = 0; i < SudokuGameManager.girdLength; i++)
        {
            for (int j = 0; j < SudokuGameManager.girdLength; j++)
            {
                var cell = grid.GetCellByPosition(i,j);
                if (cell != null)
                {
                    cell.InitValues(puzzleNumber[i,j]);
                }
                
            }
        }
    }
    
    //重开本局游戏
    public void RestartGame()
    {
        System.Array.Copy(puzzleBak, puzzleNumber, gridNumber.Length);
        InitButtons();
    }
    
    // 打乱一个List
    void RandomizeList(List<int> l)
    {
        for (var i = 0; i < l.Count - 1; i++)
        {
            // 随机交换两个位置
            int rand = Random.Range(i, l.Count);
            (l[i], l[rand]) = (l[rand], l[i]);
        }
    }
    
    // 将玩家输入更新到谜题中
    public void UpdatePuzzle(int row, int col, int value)
    {
        puzzleNumber[row, col] = value;
    }

    /// <summary>
    /// 判定游戏是否完成
    /// </summary>
    /// <returns></returns>
    public bool CheckComplete()
    {
        // 检查填入的内容和谜底内容是否吻合
        for (int i = 0; i < SudokuGameManager.girdLength; i++)
        {
            for (int j = 0; j < SudokuGameManager.girdLength; j++)
            {
                if (puzzleNumber[i, j] != gridNumber[i, j])
                {
                    return false;
                }
            }
        }
        return true;
    }
    
}