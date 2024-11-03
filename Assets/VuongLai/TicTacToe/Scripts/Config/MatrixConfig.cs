using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    [CreateAssetMenu(fileName = "MatrixConfig", menuName = "ScriptableObject/Config/MaxtrixConfig")]
    public class MatrixConfig : ScriptableObject
    {
        [SerializeField] private Vector2Int matrixNumber;
        [SerializeField] private Vector2 minPivot;
        [SerializeField] private Vector2 maxPivot;
        [SerializeField] private float stepVertical;
        [SerializeField] private float stepHorizontal;

        public Vector2Int MatrixNumber
        {
            get => matrixNumber;
        }

        public Vector2 MinPivot
        {
            get => minPivot;
        }

        public Vector2 MaxPivot
        {
            get => maxPivot;
        }

        public float StepVertical
        {
            get => stepVertical;
        }

        public float StepHorizontal
        {
            get => stepHorizontal;
        }
    }
}
