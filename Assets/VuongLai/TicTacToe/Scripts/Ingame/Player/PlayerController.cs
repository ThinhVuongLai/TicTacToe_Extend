using System.Collections;
using System.Collections.Generic;
using TheAiAlchemist;
using UnityEngine;

namespace V_TicTacToe
{
    public class PlayerController : MonoBehaviour, IPlayerBehavior
    {
        [SerializeField] private PlayerInfor playerInfor;

        [Header("Channel")]
        [SerializeField] private V_Vector3Channel touchItemChannel;
        [SerializeField] private V_Vector3Channel moveCharacterChannel;
        [SerializeField] private V_Vector2Channel checkWinChannel;
        [SerializeField] private V_VoidChannel resetLevelChannel;
        [SerializeField] private V_IntegerChannel checkWinNumberChannel;
        [SerializeField] private V_VoidPlayerInforChannel setPlayerInforChannel;
        [SerializeField] private V_ReturnPlayerInforChannel getPlayerInforChannel;
        [SerializeField] private V_ReturnVector3NullChannel getCellPositionChannel;
        [SerializeField] private V_ReturnCellPlayerBehaviorChannel occupyCellChannel;
        [SerializeField] private V_IntegerChannel playerChooseCellChannel;
        [SerializeField] private V_VoidChannel endChooseChannel;
        [SerializeField] private V_VoidChannel updatePlayerInforChannel;

        [Header("Storage")]
        [SerializeField] private V_IntegerStorage currentPlayerId;
        [SerializeField] private V_BooleanStorage isPlayed;
        [SerializeField] private V_Vector2Storage currentMatrixPosition;
        [SerializeField] private V_IntegerStorage currentNumber;
        [SerializeField] private V_LevelStatusStorage currentLevelStatus;
        [SerializeField] private V_IntegerStorage playerCellIndex;

        [Space]
        [SerializeField] private ObjectPool _characterItemPoolObject;
        private ObjectPool _objectPool;

        private void Awake()
        {
            _objectPool = GetComponent<ObjectPool>();
            isPlayed.SetValue(false);
        }

        private void OnEnable()
        {
            touchItemChannel.AddListener(TouchItem);
            resetLevelChannel.AddListener(Reset);
            getPlayerInforChannel.AddListener(GetPlayerInfor);
            setPlayerInforChannel.AddListener(SetPlayerInfor);
        }

        private void OnDisable()
        {
            touchItemChannel.RemoveListener(TouchItem);
            resetLevelChannel.RemoveListener(Reset);
            getPlayerInforChannel.RemoveListener(GetPlayerInfor);
            setPlayerInforChannel.RemoveListener(SetPlayerInfor);
        }

        public void PlayerTalk()
        {
            Debug.Log($"Is Turn Player have playerId {currentPlayerId.Value}");
        }

        private void TouchItem(Vector3 touchPosition)
        {
            if (isPlayed.GetValue())
            {
                return;
            }

            if (currentPlayerId.Value.Equals(playerInfor.PlayerId))
            {
                isPlayed.SetValue(true);

                GameObject itemObject = _objectPool.GetObject();

                ICheckItemStatus checkItem = itemObject.GetComponent<ICheckItemStatus>();
                if (checkItem != null)
                {
                    CellPlayerBehavior currentCellBehavior = CellPlayerBehavior.None;

                    //checkWinChannel.RunVector2Channel(currentMatrixPosition.Value);
                    //checkWinNumberChannel.RunIntegerChannel(currentNumber.Value);

                    if (currentLevelStatus.Value.Equals(LevelStatus.Player1Choose)
                        || currentLevelStatus.Value.Equals(LevelStatus.Player2Choose))
                    {
                        playerChooseCellChannel.RunIntegerChannel(currentNumber.Value);
                        ShowCharacterItem(currentNumber.Value);

                        currentCellBehavior = CellPlayerBehavior.Occupy;

                        endChooseChannel.RunVoidChannel();
                    }
                    else if (currentLevelStatus.Value.Equals(LevelStatus.InPlayGame))
                    {
                        currentCellBehavior = occupyCellChannel.RunChannel(currentNumber.Value);
                        Vector3? cellPosition = getCellPositionChannel.RunChannel(currentNumber.Value);
                        if (cellPosition != null)
                        {
                            moveCharacterChannel.RunVector3Channel(cellPosition.Value);
                        }
                    }

                    playerCellIndex.Value = currentNumber.Value;

                    if (currentCellBehavior.Equals(CellPlayerBehavior.Occupy))
                    {
                        checkItem.Init(touchPosition);
                        checkItem.SetShowItem(true);

                        itemObject.SetActive(true);
                    }
                }
            }
        }

        private void ShowCharacterItem(int cellIndex)
        {
            var characterItemObject = _characterItemPoolObject.GetObject();

            CharacterItem characterItem = characterItemObject.GetComponent<CharacterItem>();

            if (characterItem == null)
            {
                return;
            }

            characterItem.gameObject.SetActive(true);
            characterItem.InitItem();

            Vector3? cellPosition = getCellPositionChannel.RunChannel(cellIndex);
            if (cellPosition != null)
            {
                characterItemObject.transform.position = cellPosition.Value;
            }
        }

        private void Reset()
        {
            if (_objectPool != null)
            {
                _objectPool.ResetPool();
            }

            if (_characterItemPoolObject != null)
            {
                _characterItemPoolObject.ResetPool();
            }
        }

        private PlayerInfor GetPlayerInfor()
        {
            return playerInfor;
        }

        private void SetPlayerInfor(PlayerInfor playerInfor)
        {
            this.playerInfor = playerInfor;

            updatePlayerInforChannel.RunVoidChannel();
        }
    }

    [System.Serializable]
    public class PlayerInfor
    {
        [SerializeField] private int playerId;
        [SerializeField] private int currentCellId;
        [SerializeField] private int armyAmount;

        public int PlayerId
        {
            get => playerId;
            set
            {
                playerId = value;
            }
        }

        public int CurrentCellId
        {
            get => currentCellId;
            set
            {
                currentCellId = value;
            }
        }

        public int ArmyAmount
        {
            get => armyAmount;
            set
            {
                armyAmount = value;
            }
        }
    }
}

