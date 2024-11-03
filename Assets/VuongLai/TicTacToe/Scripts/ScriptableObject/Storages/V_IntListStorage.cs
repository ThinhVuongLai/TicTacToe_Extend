using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_IntListStorage", menuName = "ScriptableObject/Storage/V_IntListStorage")]
    public class V_IntListStorage : ScriptableObject
    {
        private List<int> _value = new List<int>();

        public List<int> Value
        {
            get => _value;
            set
            {
                _value = Value;
            }
        }
    }
}
