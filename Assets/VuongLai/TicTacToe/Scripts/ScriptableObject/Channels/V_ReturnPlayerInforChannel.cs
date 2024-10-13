using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_ReturnPlayerInforChannel", menuName = "ScriptableObject/Channel/V_ReturnPlayerInforChannel")]
    public class V_ReturnPlayerInforChannel : ScriptableObject
    {
        private Func<PlayerInfor> _value;

        public void AddListener(Func<PlayerInfor> func)
        {
            _value += func;
        }

        public void RemoveListener(Func<PlayerInfor> func)
        {
            _value -= func;
        }

        public PlayerInfor RunChannel()
        {
            return _value?.Invoke();
        }
    }
}