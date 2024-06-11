using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SudokuLevelConfig
{
    [SerializeField]
    [Range(2, 3)]
    public int level;

    [SerializeField]
    [Range(0f, 1f)]
    public float difficult;
}
