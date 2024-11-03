using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class CellPositionManager : MonoBehaviour
    {
        [Header("Channel")]
        [SerializeField] private V_ReturnVector3NullChannel GetCellPositionChannel;

        [Header("Config")]
        [SerializeField] private MatrixConfig matrixConfig;

        private int maxIndex;

        private void Awake()
        {
            maxIndex = (matrixConfig.MatrixNumber.x * matrixConfig.MatrixNumber.y) - 1;
        }

        private void OnEnable()
        {
            GetCellPositionChannel.AddListener(GetCellPosition);
        }

        private void OnDisable()
        {
            GetCellPositionChannel.RemoveListener(GetCellPosition);
        }

        public Vector3? GetCellPosition(int cellIndex)
        {
            Vector3? returnPosition = null;
            if (cellIndex < 0 || cellIndex > maxIndex)
            {
                return returnPosition;
            }

            int rowIndex = cellIndex / matrixConfig.MatrixNumber.x;
            int columnIndex = cellIndex % matrixConfig.MatrixNumber.x;

            float xPosition = (columnIndex * matrixConfig.StepHorizontal) + matrixConfig.MinPivot.x;

            float yPosition = (rowIndex * matrixConfig.StepVertical) + matrixConfig.MinPivot.y;

            returnPosition = new Vector3(xPosition, yPosition, 0);

            return returnPosition;
        }
    }
}
