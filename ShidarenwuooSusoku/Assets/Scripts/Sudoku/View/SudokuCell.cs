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

        [SerializeField]
        private Color validColor;
        [SerializeField]
        private Color invalidColor;
        
        private Color disputableColor;
        
        private int index;
        
        public event Action<int> OnClickCell;

        public void SetIndex(int index)
        {
            this.index = index;
        }
        
        public void SetNumber(int value)
        {
            numberText.text = value == 0 ? "" : value.ToString();
        }

        public void SetState(bool valid)
        {
            numberText.color = valid ? validColor : invalidColor;
        }

        public void Clear()
        {
            numberText.text = string.Empty;
        }

        /// <summary>
        /// 设置为可编辑状态，作为需要填写的位置
        /// </summary>
        public void EnableEdite()
        {
            button.enabled = true;
        }

        /// <summary>
        /// 设置为不可编辑状态，作为初始值时使用
        /// </summary>
        public void DisableEdite()
        {
            button.enabled = false;
            numberText.color = disputableColor;
        }

        public bool CanEdite()
        {
            return button.enabled;
        }
        
        private void Awake()
        {
            button.onClick.AddListener(OnClickButton);
            disputableColor = numberText.color;
        }

        private void OnClickButton()
        {
            OnClickCell?.Invoke(index);
        }
    }
}