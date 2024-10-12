using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using V_TicTacToe;

namespace V_TicTacToe
{
    public class CanSelectRender : MonoBehaviour, ISpriteRendererStatus
    {
        [SerializeField] private SpriteRenderer canSelectRender;

        public void Hide()
        {
            canSelectRender.enabled = false;
        }

        public void Show()
        {
            canSelectRender.enabled = true;
        }
    }
}