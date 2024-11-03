using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_ReturnCellPlayerBehaviorChannel", menuName = "ScriptableObject/Channel/V_ReturnCellPlayerBehaviorChannel")]
    public class V_ReturnCellPlayerBehaviorChannel : ScriptableObject
    {
        private Func<int,CellPlayerBehavior> channel;

        public void AddListener(Func<int,CellPlayerBehavior> func)
        {
            channel += func;
        }

        public void RemoveListener(Func<int, CellPlayerBehavior> func)
        {
            channel -= func;
        }

        public CellPlayerBehavior RunChannel(int inputValue)
        {
            if(channel==null)
            {
                return CellPlayerBehavior.None;
            }

            return channel.Invoke(inputValue);
        }
    }
}