using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class LevelManager : MonoBehaviour
    {
        [Header("Channel")]
        [SerializeField] private V_VoidChannel changePlayerChannel;
        [SerializeField] private V_VoidChannel touchItemChannel;
        [SerializeField] private V_VoidChannel showIngameMenuChannel;
        [SerializeField] private V_VoidChannel resetLevelChannel;
        [SerializeField] private V_VoidChannel initCellInforsChannel;
        [SerializeField] private V_VoidChannel createRandomArmor;
        [SerializeField] private V_VoidChannel finishChooseCellChannel;
        [SerializeField] private V_VoidChannel finishCreateArmorChannel;
        [SerializeField] private V_VoidChannel startPlayChannel;

        [Header("Storage")]
        [SerializeField] private V_BooleanStorage isFirstPlayer;
        [SerializeField] private V_IPlayerBehaviorStorage player1;
        [SerializeField] private V_IPlayerBehaviorStorage player2;
        [SerializeField] private V_IntegerStorage currentPlayerId;
        [SerializeField] private V_LevelStatusStorage currentLevelStatus;
        [SerializeField] private V_CellInforListStorage cellInfors;
        [SerializeField] private V_IntListStorage player1Numbers;
        [SerializeField] private V_IntListStorage player2Numbers;
        [SerializeField] private V_IntegerStorage player1CellIndex;
        [SerializeField] private V_IntegerStorage player2CellIndex;

        private void Awake()
        {
            isFirstPlayer.SetValue(true);

            StartCoroutine(CRStartGame());
        }

        private IEnumerator CRStartGame()
        {
            yield return null;

            StartGame();
        }

        private void OnEnable()
        {
            changePlayerChannel.AddListener(ChangePlayer);
            touchItemChannel.AddListener(OnTouchItem);
            resetLevelChannel.AddListener(OnResetLevel);
            finishChooseCellChannel.AddListener(OnFinishChooseCell);
            finishCreateArmorChannel.AddListener(OnFinishCreateArmor);
        }

        private void OnDisable()
        {
            changePlayerChannel.RemoveListener(ChangePlayer);
            touchItemChannel.RemoveListener(OnTouchItem);
            resetLevelChannel.RemoveListener(OnResetLevel);
            finishChooseCellChannel.RemoveListener(OnFinishChooseCell);
            finishCreateArmorChannel.RemoveListener(OnFinishCreateArmor);
        }

        private void StartGame()
        {
            currentPlayerId.Value = 0;
            ChangeStatus(LevelStatus.Player1Choose);
            cellInfors.Value.Clear();
            player1Numbers.Value.Clear();
            player2Numbers.Value.Clear();
            player1CellIndex.Value = -1;
            player2CellIndex.Value = -1;

            showIngameMenuChannel.RunVoidChannel();
            initCellInforsChannel.RunVoidChannel();
        }

        private void OnFinishChooseCell()
        {
            if (currentLevelStatus.Value.Equals(LevelStatus.Player1Choose))
            {
                ChangeStatus(LevelStatus.Player2Choose);
            }
            else if (currentLevelStatus.Value.Equals(LevelStatus.Player2Choose))
            {
                currentLevelStatus.Value = LevelStatus.CreateArmor;
                createRandomArmor.RunVoidChannel();
            }
        }

        private void OnFinishCreateArmor()
        {
            ChangeStatus(LevelStatus.InPlayGame);

            startPlayChannel.RunVoidChannel();
        }

        private void ChangeStatus(LevelStatus levelStatus)
        {
            currentLevelStatus.Value = levelStatus;
        }

        private void OnResetLevel()
        {
            currentPlayerId.Value = 0;
            showIngameMenuChannel.RunVoidChannel();
        }

        public void ChangePlayer()
        {
            if (currentPlayerId.Value.Equals(0))
            {
                Debug.Log("Turn of Player1");
            }
            else if (currentPlayerId.Value.Equals(1))
            {
                Debug.Log("Turn of Player2");
            }
        }

        private void OnTouchItem()
        {
            if (isFirstPlayer.GetValue() == true)
                player1.GetValue().PlayerTalk();
            else
                player2.GetValue().PlayerTalk();
        }
    }

    public enum LevelStatus
    {
        None,
        Player1Choose,
        Player2Choose,
        CreateArmor,
        InPlayGame,
        PauseGame,
        EndGame,
    }
}