using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_ReturnVector3NullChannel", menuName = "ScriptableObject/Channel/V_ReturnVector3NullChannel")]
    public class V_ReturnVector3NullChannel : ScriptableObject
    {
        private Func<int, Vector3?> _channel;

        public void AddListener(Func<int,Vector3?> func)
        {
            _channel += func;
        }

        public void RemoveListener(Func<int,Vector3?> func)
        {
            _channel -= func;
        }

        public Vector3? RunChannel(int inputValue)
        {
            return _channel?.Invoke(inputValue);
        }
    }
}
