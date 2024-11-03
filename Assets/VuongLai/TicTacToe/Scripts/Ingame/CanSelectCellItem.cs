using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace V_TicTacToe
{
    public class CanSelectCellItem : MonoBehaviour
    {
        [SerializeField] private CanSelectRender canSelectRender;

        public void SetShowCanSelect(bool showCanSelect)
        {
            if (showCanSelect)
            {
                canSelectRender.Show();
            }
            else
            {
                canSelectRender.Hide();
            }
        }
    }
}
