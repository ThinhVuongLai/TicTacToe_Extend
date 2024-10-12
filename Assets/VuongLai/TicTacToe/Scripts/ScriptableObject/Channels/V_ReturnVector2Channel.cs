using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_ReturnVector2Channel", menuName = "ScriptableObject/Channel/V_ReturnVector2Channel")]
    public class V_ReturnVector2Channel : ScriptableObject
    {
        private Func<int, Vector3> _value;

        public void AddListener(Func<int,Vector3> func)
        {
            _value += func;
        }

        public void Remove(Func<int, Vector3> func)
        {
            _value -= func;
        }

        public Vector3? RunChannel(int inputValue)
        {
            Vector3? outputValue= _value?.Invoke(inputValue);

            return outputValue;
        }
    }
}
