using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SudokuConfig : ScriptableObject
{
    [SerializeField]
    public List<SudokuLevelConfig> sudokuLevels = new List<SudokuLevelConfig>();

    public int LevelCount => sudokuLevels.Count;

    public SudokuLevelConfig GetLevelPuzzle(int level)
    {
        level = Mathf.Clamp(level, 0, LevelCount - 1);

        return sudokuLevels[level];
    }
}
