using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Sudoku
{
    public class SudokuCell : UIBehaviour
    {
        [SerializeField] 
        private TextMeshProUGUI numberText;
        [SerializeField] 
        private Button button;


        public void SetNumber(int value)
        {
            numberText.text = value.ToString();
        }

        public void Clear()
        {
            numberText.text = string.Empty;
        }

        /// <summary>
        /// 设置为可编辑状态，作为需要填写的位置
        /// </summary>
        public void EnableEditor()
        {
            button.enabled = true;
        }

        /// <summary>
        /// 设置为不可编辑状态，作为初始值时使用
        /// </summary>
        public void DisableEditor()
        {
            button.enabled = false;
        }


        private void Awake()
        {
            button.onClick.AddListener(OnClickButton);
        }

        private void OnClickButton()
        {
            
        }
    }
}