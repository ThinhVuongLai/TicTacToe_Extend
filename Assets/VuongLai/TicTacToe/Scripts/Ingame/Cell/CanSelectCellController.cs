using System.Collections;
using System.Collections.Generic;
using TheAiAlchemist;
using UnityEngine;

namespace V_TicTacToe
{
    public class CanSelectCellController : MonoBehaviour
    {
        [Header("Storage")]
        [SerializeField] private V_IntegerStorage currentPlayerId;
        [SerializeField] private V_IntegerStorage player1CellIndex;
        [SerializeField] private V_IntegerStorage player2CellIndex;

        [Header("Channel")]
        [SerializeField] private V_IntegerChannel showCanSelectCellChannel;
        [SerializeField] private V_ReturnIntegerListChannel getCellNumberAroundChannel;
        [SerializeField] private V_ReturnVector3NullChannel getCellPositionChannel;
        [SerializeField] private V_Vector3Channel touchItemChannel;
        [SerializeField] private V_VoidChannel changePlayerChannel;
        [SerializeField] private V_VoidChannel startPlayChannel;

        private ObjectPool _objectPool;

        private void Awake()
        {
            _objectPool = GetComponent<ObjectPool>();
        }

        private void OnEnable()
        {
            showCanSelectCellChannel.AddListener(ShowCanSelectCell);
            touchItemChannel.AddListener(OnTouchItemChannel);
            changePlayerChannel.AddListener(OnChangePlayer);
            startPlayChannel.AddListener(OnStarPlay);
        }

        private void OnDisable()
        {
            showCanSelectCellChannel.RemoveListener(ShowCanSelectCell);
            touchItemChannel.RemoveListener(OnTouchItemChannel);
            changePlayerChannel.RemoveListener(OnChangePlayer);
            startPlayChannel.RemoveListener(OnStarPlay);
        }

        private void OnTouchItemChannel(Vector3 touchPosition)
        {
            HideAllItem();
        }

        private void OnResetLevelChannel()
        {
            HideAllItem();
        }

        private void HideAllItem()
        {
            _objectPool.ResetPool();
        }

        private void OnChangePlayer()
        {
            int currentCellIndex = 0;
            if(currentPlayerId.Value.Equals(0))
            {
                currentCellIndex = player1CellIndex.Value;
            }
            else if(currentPlayerId.Value.Equals(1))
            {
                currentCellIndex = player2CellIndex.Value;
            }

            ShowCanSelectCell(currentCellIndex);
        }

        private void OnStarPlay()
        {
            int currentCellIndex = 0;
            if (currentPlayerId.Value.Equals(0))
            {
                currentCellIndex = player1CellIndex.Value;
            }
            else if (currentPlayerId.Value.Equals(1))
            {
                currentCellIndex = player2CellIndex.Value;
            }

            ShowCanSelectCell(currentCellIndex);
        }

        private void ShowCanSelectCell(int cellIndex)
        {
            List<int> aroundCellIndexs = getCellNumberAroundChannel.RunChannel(cellIndex);

            if (aroundCellIndexs == null || aroundCellIndexs.Count <= 0)
            {
                return;
            }

            Vector3? cellPositoin = Vector3.zero;
            int currentCellIndex = 0;
            GameObject currentItem;

            for (int i = 0, max = aroundCellIndexs.Count; i < max; i++)
            {
                currentCellIndex = aroundCellIndexs[i];
                cellPositoin = getCellPositionChannel.RunChannel(currentCellIndex);
                if (cellPositoin != null)
                {
                    currentItem = _objectPool.GetObject();
                    CanSelectCellItem canSelectCellItem = currentItem.GetComponent<CanSelectCellItem>();
                    canSelectCellItem.gameObject.SetActive(true);

                    canSelectCellItem.transform.position = cellPositoin.Value;
                    canSelectCellItem.SetShowCanSelect(true);
                }
            }
        }
    }
}
