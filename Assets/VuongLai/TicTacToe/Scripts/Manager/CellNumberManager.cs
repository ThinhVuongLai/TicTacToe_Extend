using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class CellNumberManager : MonoBehaviour
    {
        [SerializeField] private V_ReturnIntegerListChannel getCellNumberAroundChannel;

        private void OnEnable()
        {
            getCellNumberAroundChannel.AddListener(GetCellNumbersAroundCellNumber);
        }

        private void OnDisable()
        {
            getCellNumberAroundChannel.RemoveListener(GetCellNumbersAroundCellNumber);
        }

        public List<int> GetCellNumbersAroundCellNumber(int cellNumber)
        {
            List<int> returnCellNumbers = new List<int>();

            int rowIndex = cellNumber / 3;

            int minValue = 0;
            int maxValue = 0;

            //Get hang ngang
            minValue = rowIndex * 3;
            maxValue = (rowIndex * 3) + 2;

            if ((cellNumber - 1) >= minValue)
            {
                returnCellNumbers.Add(cellNumber - 1);
            }
            
            if ((cellNumber + 1) <= maxValue)
            {
                returnCellNumbers.Add(cellNumber + 1);
            }

            //Get hang doc
            if ((cellNumber - 3) >= 0)
            {
                returnCellNumbers.Add(cellNumber - 3);
            }

            maxValue = 3 * 3;
            if ((cellNumber + 3) < maxValue)
            {
                returnCellNumbers.Add(cellNumber + 3);
            }

            //Get duong cheo chinh va cheo phu
            if ((rowIndex - 1) >= 0)
            {
                minValue = (Mathf.Max(0, rowIndex - 1) * 3);
                maxValue = (Mathf.Max(0, rowIndex - 1) * 3) + 2;
                if ((cellNumber - 2) >= minValue && (cellNumber - 2) <= maxValue)
                {
                    returnCellNumbers.Add(cellNumber - 2);
                }

                if ((cellNumber - 4) >= minValue && (cellNumber - 4) <= maxValue)
                {
                    returnCellNumbers.Add(cellNumber - 4);
                }
            }

            if ((rowIndex + 1) <= 2)
            {
                minValue = Mathf.Min(2, rowIndex + 1) * 3;
                maxValue = (Mathf.Min(2, rowIndex + 1) * 3) + 2;
                if ((cellNumber + 2) >= minValue && (cellNumber + 2) <= maxValue)
                {
                    returnCellNumbers.Add(cellNumber + 2);
                }

                if ((cellNumber + 4) >= minValue && (cellNumber + 4) <= maxValue)
                {
                    returnCellNumbers.Add(cellNumber + 4);
                }
            }

            return returnCellNumbers;
        }
    }
}