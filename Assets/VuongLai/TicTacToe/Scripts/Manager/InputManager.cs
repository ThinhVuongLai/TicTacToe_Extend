using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class InputManager : MonoBehaviour
    {
        [Header("System")]
        [SerializeField] private InputSystem inputSystem;

        [Header("Channel")]
        [SerializeField] private V_Vector3Channel touchItemChannel;
        [SerializeField] private V_VoidChannel endTurnChannel;
        [SerializeField] private V_VoidChannel resetLevelChannel;
        [SerializeField] private V_ReturnIntegerListChannel GetCellNumberAroundChannel;

        [Header("Storage")]
        [SerializeField] private V_Vector2Storage currentMatrixPosition;
        [SerializeField] private V_IntegerStorage currentNumber;
        [SerializeField] private V_IntegerStorage currentPlayerId;
        [SerializeField] private V_IntegerStorage player1CellIndex;
        [SerializeField] private V_IntegerStorage player2CellIndex;
        [SerializeField] private V_LevelStatusStorage currentLevelStatus;

        [Header("Config")]
        [SerializeField] private MatrixConfig matrixConfig;

        private List<Vector2> hasItemPosition = new List<Vector2>();
        private Vector2 matrixPosition = Vector2.zero;

        private void OnEnable()
        {
            inputSystem.eventMouseTouch.AddListener(TouchItem);
            endTurnChannel.AddListener(OnEndTurn);
            resetLevelChannel.AddListener(OnResetLevel);
        }

        private void OnDisable()
        {
            inputSystem.eventMouseTouch.RemoveListener(TouchItem);
            endTurnChannel.RemoveListener(OnEndTurn);
            resetLevelChannel.RemoveListener(OnResetLevel);
        }

        //Get Item Position From touch Position
        private Vector3? GetItemPosition(Vector3 touchPosition)
        {
            int currentNumber = -1;

            float halfStepVertical = Mathf.Abs(matrixConfig.StepVertical) / 2;
            float halfStepHorizontal = Mathf.Abs(matrixConfig.StepHorizontal) / 2;

            if (touchPosition.x < (matrixConfig.MinPivot.x - halfStepHorizontal) || touchPosition.x > (matrixConfig.MaxPivot.x + halfStepHorizontal)
                || touchPosition.y < (matrixConfig.MinPivot.y - halfStepVertical) || touchPosition.y > (matrixConfig.MaxPivot.y + halfStepVertical))
            {
                matrixPosition = new Vector2(-1, -1);
                currentMatrixPosition.Value = matrixPosition;

                this.currentNumber.Value = currentNumber;

                Debug.Log("Out Space");
                Debug.Log($"CurrentNumber: {currentNumber}");
                return null;
            }

            float positionX = 0;
            float matrixPositionX = 0;
            int xIndex = 0;
            if (touchPosition.x < matrixConfig.MinPivot.x)
            {
                positionX = matrixConfig.MinPivot.x;

            }
            else if (touchPosition.x > matrixConfig.MaxPivot.x)
            {
                positionX = matrixConfig.MaxPivot.x;
                matrixPositionX = matrixConfig.MatrixNumber.x - 1;

                xIndex = (int)(matrixConfig.MatrixNumber.x - 1);
            }
            else
            {
                float distance = touchPosition.x - matrixConfig.MinPivot.x;
                int indexX = (int)(distance / matrixConfig.StepHorizontal);

                if (touchPosition.x - ((indexX * matrixConfig.StepHorizontal) + matrixConfig.MinPivot.x) > halfStepHorizontal)
                {
                    indexX++;
                }

                matrixPositionX = indexX;

                xIndex = indexX;

                positionX = (indexX * matrixConfig.StepHorizontal) + matrixConfig.MinPivot.x;
            }

            float positionY = 0;
            float matrixPositionY = 0;
            int yIndex = 0;
            if (touchPosition.y < matrixConfig.MinPivot.y)
            {
                positionY = matrixConfig.MinPivot.y;
                matrixPositionY = matrixConfig.MatrixNumber.y - 1;
            }
            else if (touchPosition.y > matrixConfig.MaxPivot.y)
            {
                positionY = matrixConfig.MaxPivot.y;

                yIndex = (int)(matrixConfig.MatrixNumber.y - 1);
            }
            else
            {
                float distance = touchPosition.y - matrixConfig.MinPivot.y;
                int indexY = (int)(distance / matrixConfig.StepVertical);

                if (touchPosition.y - ((indexY * matrixConfig.StepVertical) + matrixConfig.MinPivot.y) > halfStepVertical)
                {
                    indexY++;
                }

                matrixPositionY = matrixConfig.MatrixNumber.y - (indexY) - 1;
                yIndex = indexY;

                positionY = (indexY * matrixConfig.StepVertical) + matrixConfig.MinPivot.y;
            }

            Vector2 itemPosition = new Vector2(positionX, positionY);
            matrixPosition = new Vector2(matrixPositionX, matrixPositionY);
            currentMatrixPosition.Value = matrixPosition;

            currentNumber = xIndex + (yIndex * (int)matrixConfig.MatrixNumber.x);
            this.currentNumber.Value = currentNumber;

            //if (HasItem(matrixPosition))
            //{
            //    return null;
            //}

            return itemPosition;
        }

        private bool HasItem(Vector2 position)
        {
            return hasItemPosition.Contains(position);
        }

        private void OnEndTurn()
        {
            hasItemPosition.Add(currentMatrixPosition.Value);
        }

        private void TouchItem(Vector3 touchPosition)
        {
            Vector3? valueItemPosition = GetItemPosition(touchPosition);
            if (valueItemPosition != null)
            {
                bool canRunTouchItemChannel = false;
                if (currentLevelStatus.Value.Equals(LevelStatus.InPlayGame))
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

                    List<int> CellNumberAround = GetCellNumberAroundChannel.RunChannel(currentCellIndex);

                    if (CellNumberAround != null && CellNumberAround.Count > 0
                        && CellNumberAround.Contains(currentNumber.Value))
                    {
                        canRunTouchItemChannel = true;
                    }
                }
                else if (currentLevelStatus.Value.Equals(LevelStatus.Player1Choose)
                    || currentLevelStatus.Value.Equals(LevelStatus.Player2Choose))
                {
                    canRunTouchItemChannel = true;
                }

                if(canRunTouchItemChannel)
                {
                    Vector3 itemPosition = valueItemPosition.Value;
                    touchItemChannel.RunVector3Channel(itemPosition);
                }
                else
                {
                    Debug.LogError($"Out of Can Select Cell");
                }
            }
        }

        private void OnResetLevel()
        {
            hasItemPosition.Clear();
        }
    }

    [System.Serializable]
    public class MatrixPosition
    {
        public int matrixX = 0;
        public int matrixY = 0;
    }
}