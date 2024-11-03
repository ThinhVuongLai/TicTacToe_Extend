using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_LevelStatusStorage", menuName = "ScriptableObject/Storage/V_LevelStatusStorage")]
    public class V_LevelStatusStorage : ScriptableObject
    {
        private LevelStatus _value;

        public LevelStatus Value
        {
            get
            {
                return _value;
            }
            set
            {
                _value = value;
            }
        }
    }
}