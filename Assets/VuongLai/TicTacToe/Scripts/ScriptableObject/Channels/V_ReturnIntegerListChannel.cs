using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_ReturnIntegerListChannel", menuName = "ScriptableObject/Channel/V_ReturnIntegerListChannel")]
    public class V_ReturnIntegerListChannel : ScriptableObject
    {
        private Func<int, List<int>> _value;

        public void AddListener(Func<int, List<int>> func)
        {
            _value += func;
        }

        public void RemoveListener(Func<int, List<int>> func)
        {
            _value -= func;
        }

        public List<int> RunChannel(int inputValue)
        {
            return _value?.Invoke(inputValue);
        }
    }
}
