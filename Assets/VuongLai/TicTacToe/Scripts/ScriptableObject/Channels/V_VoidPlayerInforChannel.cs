using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace V_TicTacToe
{ [CreateAssetMenu(fileName = "V_VoidPlayerInforChannel", menuName = "ScriptableObject/Channel/V_VoidPlayerInforChannel")]
    public class V_VoidPlayerInforChannel : ScriptableObject
    {
        private UnityEvent<PlayerInfor> _value;

        public void AddListener(UnityAction<PlayerInfor> action)
        {
            _value.AddListener(action);
        }

        public void RemoveListener(UnityAction<PlayerInfor> action)
        {
            _value.RemoveListener(action);
        }

        public void RunChannel(PlayerInfor playerInfor)
        {
            _value?.Invoke(playerInfor);
        }
    }
}
