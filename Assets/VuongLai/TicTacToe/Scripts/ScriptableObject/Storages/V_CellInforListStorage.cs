using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "V_CellInforListStorage", menuName = "ScriptableObject/Storage/V_CellInforListStorage")]
    public class V_CellInforListStorage : ScriptableObject
    {
        private List<CellInfor> _value;

        public List<CellInfor> Value
        {
            get
            {
                if (_value == null)
                {
                    _value = new List<CellInfor>();
                }

                return _value;
            }
            set
            {
                if (_value == null)
                {
                    _value = new List<CellInfor>();
                }
                _value = value;
            }
        }
    }
}
