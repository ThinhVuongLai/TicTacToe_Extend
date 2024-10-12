using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class CheckItem : MonoBehaviour, ICheckItemStatus
    {
        [SerializeField] private CheckItemRender checkItemRender;
        [SerializeField] private CanSelectRender canSelectRender;

        public void Init(Vector3 itemPosition)
        {
            transform.position = itemPosition;
        }

        public void SetShowItem(bool showItem)
        {
            if (showItem)
            {
                checkItemRender.Show();
            }
            else
            {
                checkItemRender.Hide();
            }
        }

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