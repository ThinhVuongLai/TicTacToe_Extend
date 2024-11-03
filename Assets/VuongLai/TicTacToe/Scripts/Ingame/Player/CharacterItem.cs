using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using TheAiAlchemist;
using UnityEngine;

namespace V_TicTacToe
{
    public class CharacterItem : MonoBehaviour
    {
        [SerializeField] private int playerId;
        [SerializeField] private TextMesh armorAmountText;

        [Header("Storage")]
        [SerializeField] private V_IntegerStorage currentPlayerId;
        [SerializeField] private V_IntegerStorage currentNumber;

        [Header("Channel")]
        [SerializeField] private V_VoidChannel finishCharacterMoveChannel;
        [SerializeField] private V_IntegerChannel checkWinNumberChannel;
        [SerializeField] private V_Vector3Channel moveCharacterChannel;
        [SerializeField] private V_ReturnPlayerInforChannel getPlayerInforChannel;
        [SerializeField] private V_VoidChannel updatePlayerInforChannel;


        private void OnEnable()
        {
            moveCharacterChannel.AddListener(MoveItem);
            updatePlayerInforChannel.AddListener(OnUpdatePlayerInfor);
        }

        private void OnDisable()
        {
            moveCharacterChannel.RemoveListener(MoveItem);
            updatePlayerInforChannel.RemoveListener(OnUpdatePlayerInfor);
        }

        public void InitItem()
        {
            PlayerInfor playerInfor = getPlayerInforChannel.RunChannel();
            UpdateArmorText(playerInfor.ArmyAmount);
        }

        public void MoveItem(Vector3 targetPosition)
        {
            if(!currentPlayerId.Value.Equals(playerId))
            {
                return;
            }

            transform.DOMove(targetPosition, 0.5f).SetEase(Ease.Linear).OnComplete(() =>
            {
                FinishMoveAction();
            });
        }

        public void UpdateArmorText(int armorAmount)
        {
            if(armorAmountText)
            {
                armorAmountText.text = armorAmount.ToString();
            }
        }

        private void FinishMoveAction()
        {
            finishCharacterMoveChannel.RunVoidChannel();
            checkWinNumberChannel.RunIntegerChannel(currentNumber.Value);
        }

        private void OnUpdatePlayerInfor()
        {
            PlayerInfor playerInfor = getPlayerInforChannel.RunChannel();
            UpdateArmorText(playerInfor.ArmyAmount);
        }
    }
}