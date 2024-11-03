using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_LevelResultStorage", menuName = "ScriptableObject/Storage/V_LevelResultStorage")]
    public class V_LevelResultStorage : ScriptableObject
    {
        private LevelResult _value;

        public LevelResult Value
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
