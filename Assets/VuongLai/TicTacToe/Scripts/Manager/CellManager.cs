using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class CellManager : MonoBehaviour
    {
        [Header("Storage")]
        [SerializeField] private V_IntegerStorage currentPlayerId;

        [Header("Channel")]
        [SerializeField] private V_ReturnPlayerInforChannel getPlayer1InforChannel;
        [SerializeField] private V_ReturnPlayerInforChannel getPlayer2InforChannel;
        [SerializeField] private V_VoidPlayerInforChannel setPlayer1InforChannel;
        [SerializeField] private V_VoidPlayerInforChannel setPlayer2InforChannel;

        private List<CellInfor> cellInfors = new List<CellInfor>();

        private CellPlayerBehavior OccupyCell(int cellId)
        {
            CellPlayerBehavior cellPlayerBehavior = CellPlayerBehavior.NotOccupy;

            if (cellId < 0 && cellId >= cellInfors.Count)
            {
                return cellPlayerBehavior;
            }
            CellInfor currentCellInfor = cellInfors[cellId];

            PlayerInfor player1Infor = getPlayer1InforChannel.RunChannel();
            PlayerInfor player2Infor = getPlayer2InforChannel.RunChannel();

            if (currentCellInfor.OccupyPlayerId <= 0)
            {
                if (currentCellInfor.ArmyAmount > 0)
                {
                    if (currentPlayerId.Equals(player1Infor.PlayerId))
                    {
                        player1Infor.ArmyAmount += currentCellInfor.ArmyAmount;
                        player1Infor.CurrentCellId = currentCellInfor.CellId;
                        setPlayer1InforChannel.RunChannel(player1Infor);
                    }
                    else if (currentPlayerId.Equals(player2Infor.PlayerId))
                    {
                        player2Infor.ArmyAmount += currentCellInfor.ArmyAmount;
                        player2Infor.CurrentCellId = currentCellInfor.CellId;
                        setPlayer2InforChannel.RunChannel(player2Infor);
                    }

                    cellPlayerBehavior = CellPlayerBehavior.Occupy;
                    currentCellInfor.OccupyPlayerId = currentPlayerId.Value;
                    currentCellInfor.ArmyAmount = 0;
                }
            }
            else
            {
                if (currentCellInfor.OccupyPlayerId.Equals(currentPlayerId))
                {
                    if (currentPlayerId.Equals(player1Infor.PlayerId))
                    {
                        player1Infor.ArmyAmount += 1;
                        player1Infor.CurrentCellId = currentCellInfor.CellId;
                        setPlayer1InforChannel.RunChannel(player1Infor);
                    }
                    else if (currentPlayerId.Equals(player2Infor.PlayerId))
                    {
                        player2Infor.ArmyAmount += 1;
                        player2Infor.CurrentCellId = currentCellInfor.CellId;
                        setPlayer2InforChannel.RunChannel(player2Infor);
                    }

                    cellPlayerBehavior = CellPlayerBehavior.BackPlayerCell;
                }
                else
                {
                    PlayerInfor opponentInfor = null;
                    if (currentPlayerId.Equals(player1Infor.PlayerId)
                        && currentCellInfor.CellId.Equals(player2Infor.CurrentCellId))
                    {
                        opponentInfor = player2Infor;
                    }
                    else if (currentPlayerId.Equals(player2Infor.PlayerId)
                        && currentCellInfor.CellId.Equals(player1Infor.CurrentCellId))
                    {
                        opponentInfor = player1Infor;
                    }

                    if (opponentInfor == null)
                    {
                        cellPlayerBehavior = CellPlayerBehavior.Occupy;
                        if (currentPlayerId.Equals(player1Infor.PlayerId)
                            && player1Infor.ArmyAmount > 0)
                        {
                            player1Infor.ArmyAmount -= 1;
                            player1Infor.CurrentCellId = currentCellInfor.CellId;
                            setPlayer1InforChannel.RunChannel(player1Infor);

                            currentCellInfor.OccupyPlayerId = player1Infor.PlayerId;
                        }
                        else if (currentPlayerId.Equals(player2Infor.PlayerId)
                            && player2Infor.ArmyAmount > 0)
                        {
                            player2Infor.ArmyAmount -= 1;
                            player2Infor.CurrentCellId = currentCellInfor.CellId;
                            setPlayer2InforChannel.RunChannel(player2Infor);

                            currentCellInfor.OccupyPlayerId = player2Infor.PlayerId;
                        }
                        else
                        {
                            cellPlayerBehavior = CellPlayerBehavior.NotOccupy;
                        }
                    }
                    else
                    {
                        if (currentPlayerId.Equals(player1Infor.PlayerId))
                        {
                            player1Infor.ArmyAmount -= opponentInfor.ArmyAmount;
                            if (player1Infor.ArmyAmount < 0)
                            {
                                player1Infor.ArmyAmount = 0;
                            }
                            player1Infor.CurrentCellId = currentCellInfor.CellId;
                            setPlayer1InforChannel.RunChannel(player1Infor);

                            opponentInfor.ArmyAmount -= player1Infor.ArmyAmount;
                            if (opponentInfor.ArmyAmount < 0)
                            {
                                opponentInfor.ArmyAmount = 0;
                            }
                            setPlayer2InforChannel.RunChannel(opponentInfor);

                            if (opponentInfor.ArmyAmount > player1Infor.ArmyAmount)
                            {
                                currentCellInfor.OccupyPlayerId = opponentInfor.PlayerId;
                            }
                            else if (opponentInfor.ArmyAmount < player1Infor.ArmyAmount)
                            {
                                currentCellInfor.OccupyPlayerId = player1Infor.PlayerId;
                                cellPlayerBehavior = CellPlayerBehavior.Occupy;
                            }
                        }
                        else if (currentPlayerId.Equals(player2Infor.PlayerId))
                        {
                            player2Infor.ArmyAmount -= opponentInfor.ArmyAmount;
                            if (player2Infor.ArmyAmount < 0)
                            {
                                player2Infor.ArmyAmount = 0;
                            }
                            player2Infor.CurrentCellId = currentCellInfor.CellId;
                            setPlayer2InforChannel.RunChannel(player2Infor);

                            opponentInfor.ArmyAmount -= player2Infor.ArmyAmount;
                            if (opponentInfor.ArmyAmount < 0)
                            {
                                opponentInfor.ArmyAmount = 0;
                            }
                            setPlayer1InforChannel.RunChannel(opponentInfor);

                            if (opponentInfor.ArmyAmount > player2Infor.ArmyAmount)
                            {
                                currentCellInfor.OccupyPlayerId = opponentInfor.PlayerId;
                            }
                            else if (opponentInfor.ArmyAmount < player2Infor.ArmyAmount)
                            {
                                currentCellInfor.OccupyPlayerId = player2Infor.PlayerId;
                                cellPlayerBehavior = CellPlayerBehavior.Occupy;
                            }
                        }
                    }
                }
            }

            return cellPlayerBehavior;
        }
    }

    [System.Serializable]
    public class CellInfor : MonoBehaviour
    {
        [SerializeField] private int cellId;
        [SerializeField] private int occupyPlayerId;//-1 la chua co Player nao chiem duoc cell nay
        [SerializeField] private int armyAmount;

        public int CellId
        {
            get => cellId;
            set
            {
                cellId = value;
            }
        }

        public int OccupyPlayerId
        {
            get => occupyPlayerId;
            set
            {
                occupyPlayerId = value;
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

    public enum CellPlayerBehavior
    {
        Occupy,
        BackPlayerCell,
        NotOccupy,
    }
}