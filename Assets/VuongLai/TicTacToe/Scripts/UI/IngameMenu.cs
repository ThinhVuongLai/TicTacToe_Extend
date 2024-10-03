using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace V_TicTacToe
{
    public class IngameMenu : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _playerText;

        [Header("Ingame")]
        [SerializeField] private GameObject ingameObject;
        [SerializeField] private TextMeshProUGUI player1ScoreText;

        [Header("Win")]
        [SerializeField] private GameObject winObject;
        [SerializeField] private TextMeshProUGUI winText;

        [Header("Draw")]
        [SerializeField] private GameObject drawObject;

        [Header("Button")]
        [SerializeField] private Button _changePlayerButton;
        [SerializeField] private Button resetButton;

        [Header("Channel")]
        [SerializeField] private V_VoidChannel showWinChannel;
        [SerializeField] private V_VoidChannel showDrawChannel;
        [SerializeField] private V_VoidChannel changePlayerChannel;
        [SerializeField] private V_VoidChannel endTurnChannel;
        [SerializeField] private V_VoidChannel showIngameMenuChannel;
        [SerializeField] private V_VoidChannel resetLevelChannel;
        [SerializeField] private V_IntegerChannel updatePlayer1ScoreChannel;

        [Header("Storage")]
        [SerializeField] private V_IntegerStorage currentPlayerId;
        [SerializeField] private V_BooleanStorage isPlayed;

        private void Awake()
        {
            _changePlayerButton.onClick.AddListener(OnChangePlayer);
            resetButton.onClick.AddListener(OnClickResetButton);

            _changePlayerButton.gameObject.SetActive(false);
        }

        private void OnEnable()
        {
            endTurnChannel.AddListener(OnEndTurn);
            showIngameMenuChannel.AddListener(ShowIngame);
            showWinChannel.AddListener(ShowWin);
            showDrawChannel.AddListener(ShowDraw);

            updatePlayer1ScoreChannel.AddListener(OnUpdatePlayer1Score);
        }

        private void OnDisable()
        {
            endTurnChannel.RemoveListener(OnEndTurn);
            showIngameMenuChannel.RemoveListener(ShowIngame);
            showWinChannel.RemoveListener(ShowWin);
            showDrawChannel.RemoveListener(ShowDraw);

            updatePlayer1ScoreChannel.RemoveListener(OnUpdatePlayer1Score);
        }

        private void OnChangePlayer()
        {
            if (currentPlayerId.Value.Equals(0))
            {
                currentPlayerId.Value = 1;
            }
            else if (currentPlayerId.Value.Equals(1))
            {
                currentPlayerId.Value = 0;
            }

            UpdatePlayerText();

            isPlayed.SetValue(false);

            _changePlayerButton.gameObject.SetActive(false);

            changePlayerChannel.RunVoidChannel();
        }

        private void UpdatePlayerText()
        {
            if (currentPlayerId.Value.Equals(0))
            {
                _playerText.SetText("Player1");
            }
            else if (currentPlayerId.Value.Equals(1))
            {
                _playerText.SetText("Player2");
            }
        }

        private void OnClickResetButton()
        {
            isPlayed.SetValue(false);

            resetLevelChannel.RunVoidChannel();
        }

        private void OnEndTurn()
        {
            _changePlayerButton.gameObject.SetActive(true);
        }

        private void ShowIngame()
        {
            UpdatePlayerText();

            resetButton.gameObject.SetActive(false);
            winObject.SetActive(false);
            drawObject.SetActive(false);

            ingameObject.SetActive(true);
            _changePlayerButton.gameObject.SetActive(false);
        }

        private void ShowWin()
        {
            _changePlayerButton.gameObject.SetActive(false);
            ingameObject.SetActive(false);
            drawObject.SetActive(false);

            winObject.SetActive(true);

            resetButton.gameObject.SetActive(true);

            if (currentPlayerId.Value.Equals(0))
            {
                winText.SetText("Player1 Win");
            }
            else if (currentPlayerId.Value.Equals(1))
            {
                winText.SetText("Player2 Win");
            }
        }

        private void ShowDraw()
        {
            _changePlayerButton.gameObject.SetActive(false);
            ingameObject.SetActive(false);
            winObject.SetActive(false);

            drawObject.SetActive(true);

            resetButton.gameObject.SetActive(true);
        }

        private void OnUpdatePlayer1Score(int currentScore)
        {
            string scoreString = $"Player1 Score: {currentScore}";
            player1ScoreText.SetText(scoreString);
        }
    }
}