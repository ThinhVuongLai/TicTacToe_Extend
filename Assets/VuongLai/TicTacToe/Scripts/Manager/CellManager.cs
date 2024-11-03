using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace V_TicTacToe
{
    public class CellManager : MonoBehaviour
    {
        [Header("Storage")]
        [SerializeField] private V_IntegerStorage currentPlayerId;
        [SerializeField] private V_CellInforListStorage cellInfors;
        [SerializeField] private V_IntListStorage player1Numbers;
        [SerializeField] private V_IntListStorage player2Numbers;

        [Header("Channel")]
        [SerializeField] private V_ReturnPlayerInforChannel getPlayer1InforChannel;
        [SerializeField] private V_ReturnPlayerInforChannel getPlayer2InforChannel;
        [SerializeField] private V_VoidPlayerInforChannel setPlayer1InforChannel;
        [SerializeField] private V_VoidPlayerInforChannel setPlayer2InforChannel;
        [SerializeField] private V_ReturnCellPlayerBehaviorChannel occupyCellChannel;
        [SerializeField] private V_IntegerChannel playerChooseCellChannel;
        [SerializeField] private V_VoidChannel createRandomArmor;
        [SerializeField] private V_VoidChannel initCellInforsChannel;
        [SerializeField] private V_VoidChannel finishCreateArmorChannel;

        [Header("Config")]
        [SerializeField] private MatrixConfig matrixConfig;

        private void OnEnable()
        {
            occupyCellChannel.AddListener(OccupyCell);
            playerChooseCellChannel.AddListener(PlayerChooseCell);
            createRandomArmor.AddListener(CreateRandomArmor);
            initCellInforsChannel.AddListener(InitCellInfors);
        }

        private void OnDisable()
        {
            occupyCellChannel.RemoveListener(OccupyCell);
            playerChooseCellChannel.RemoveListener(PlayerChooseCell);
            createRandomArmor.RemoveListener(CreateRandomArmor);
            initCellInforsChannel.RemoveListener(InitCellInfors);
        }

        private void InitCellInfors()
        {
            if (cellInfors.Value == null)
            {
                cellInfors.Value = new List<CellInfor>();
            }

            int cellLength = matrixConfig.MatrixNumber.x * matrixConfig.MatrixNumber.y;
            for (int i = 0; i < cellLength; i++)
            {
                cellInfors.Value.Add(new CellInfor() { CellId = i, OccupyPlayerId = -1, ArmyAmount = 0 });
            }
        }

        private void ResetCellInfors()
        {
            for (int i = 0, max = cellInfors.Value.Count; i < max; i++)
            {
                cellInfors.Value[i].OccupyPlayerId = -1;
                cellInfors.Value[i].ArmyAmount = 0;
            }
        }

        private void CreateRandomArmor()
        {
            CellInfor currentCellInfor = null;
            int random;

            for (int i = 0, max = cellInfors.Value.Count; i < max; i++)
            {
                currentCellInfor = cellInfors.Value[i];
                if (currentCellInfor.OccupyPlayerId < 0)
                {
                    random = UnityEngine.Random.Range(1, 6);
                    currentCellInfor.ArmyAmount = random;
                }
            }

            finishCreateArmorChannel.RunVoidChannel();
        }

        private void PlayerChooseCell(int cellId)
        {
            PlayerInfor player1Infor = getPlayer1InforChannel.RunChannel();
            PlayerInfor player2Infor = getPlayer2InforChannel.RunChannel();

            CellInfor currentCellInfor = cellInfors.Value[cellId];
            if (currentPlayerId.Value.Equals(player1Infor.PlayerId))
            {
                player1Infor.ArmyAmount += currentCellInfor.ArmyAmount;
                player1Infor.CurrentCellId = currentCellInfor.CellId;
                setPlayer1InforChannel.RunChannel(player1Infor);
            }
            else if (currentPlayerId.Value.Equals(player2Infor.PlayerId))
            {
                player2Infor.ArmyAmount += currentCellInfor.ArmyAmount;
                player2Infor.CurrentCellId = currentCellInfor.CellId;
                setPlayer2InforChannel.RunChannel(player2Infor);
            }

            currentCellInfor.OccupyPlayerId = currentPlayerId.Value;
            currentCellInfor.ArmyAmount = 0;

            ChangeCellIdList(cellId);
        }

        private void ChangeCellIdList(int cellId)
        {
            PlayerInfor player1Infor = getPlayer1InforChannel.RunChannel();
            PlayerInfor player2Infor = getPlayer2InforChannel.RunChannel();

            if (currentPlayerId.Value.Equals(player1Infor.PlayerId))
            {
                if (!player1Numbers.Value.Contains(cellId))
                {
                    player1Numbers.Value.Add(cellId);
                }

                if (player2Numbers.Value.Contains(cellId))
                {
                    player2Numbers.Value.Remove(cellId);
                }
            }
            else if (currentPlayerId.Value.Equals(player2Infor.PlayerId))
            {
                if (player1Numbers.Value.Contains(cellId))
                {
                    player1Numbers.Value.Remove(cellId);
                }

                if (!player2Numbers.Value.Contains(cellId))
                {
                    player2Numbers.Value.Add(cellId);
                }
            }
        }

        private CellPlayerBehavior OccupyCell(int cellId)
        {
            CellPlayerBehavior cellPlayerBehavior = CellPlayerBehavior.NotOccupy;

            if (cellId < 0 && cellId >= cellInfors.Value.Count)
            {
                return cellPlayerBehavior;
            }
            CellInfor currentCellInfor = cellInfors.Value[cellId];

            PlayerInfor player1Infor = getPlayer1InforChannel.RunChannel();
            PlayerInfor player2Infor = getPlayer2InforChannel.RunChannel();

            if (currentCellInfor.OccupyPlayerId < 0)
            {
                if (currentCellInfor.ArmyAmount > 0)
                {
                    PlayerChooseCell(cellId);

                    cellPlayerBehavior = CellPlayerBehavior.Occupy;
                }
            }
            else
            {
                if (currentCellInfor.OccupyPlayerId.Equals(currentPlayerId.Value))
                {
                    if (currentPlayerId.Value.Equals(player1Infor.PlayerId))
                    {
                        player1Infor.ArmyAmount += 1;
                        player1Infor.CurrentCellId = currentCellInfor.CellId;
                        setPlayer1InforChannel.RunChannel(player1Infor);
                    }
                    else if (currentPlayerId.Value.Equals(player2Infor.PlayerId))
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
                    if (currentPlayerId.Value.Equals(player1Infor.PlayerId)
                        && currentCellInfor.CellId.Equals(player2Infor.CurrentCellId))
                    {
                        opponentInfor = player2Infor;
                    }
                    else if (currentPlayerId.Value.Equals(player2Infor.PlayerId)
                        && currentCellInfor.CellId.Equals(player1Infor.CurrentCellId))
                    {
                        opponentInfor = player1Infor;
                    }

                    if (opponentInfor == null)
                    {
                        cellPlayerBehavior = CellPlayerBehavior.Occupy;
                        if (currentPlayerId.Value.Equals(player1Infor.PlayerId)
                            && player1Infor.ArmyAmount > 0)
                        {
                            player1Infor.ArmyAmount -= 1;
                            player1Infor.CurrentCellId = currentCellInfor.CellId;
                            setPlayer1InforChannel.RunChannel(player1Infor);

                            currentCellInfor.OccupyPlayerId = player1Infor.PlayerId;
                        }
                        else if (currentPlayerId.Value.Equals(player2Infor.PlayerId)
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
                        int playerArmorAmount;
                        int opponentArmorAmount = opponentInfor.ArmyAmount;
                        if (currentPlayerId.Value.Equals(player1Infor.PlayerId))
                        {
                            playerArmorAmount = player1Infor.ArmyAmount;

                            player1Infor.ArmyAmount -= opponentArmorAmount;
                            if (player1Infor.ArmyAmount < 0)
                            {
                                player1Infor.ArmyAmount = 0;
                            }
                            player1Infor.CurrentCellId = currentCellInfor.CellId;
                            setPlayer1InforChannel.RunChannel(player1Infor);

                            opponentInfor.ArmyAmount -= playerArmorAmount;
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
                        else if (currentPlayerId.Value.Equals(player2Infor.PlayerId))
                        {
                            playerArmorAmount = player2Infor.ArmyAmount;

                            player2Infor.ArmyAmount -= opponentArmorAmount;
                            if (player2Infor.ArmyAmount < 0)
                            {
                                player2Infor.ArmyAmount = 0;
                            }
                            player2Infor.CurrentCellId = currentCellInfor.CellId;
                            setPlayer2InforChannel.RunChannel(player2Infor);

                            opponentInfor.ArmyAmount -= playerArmorAmount;
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

            if(cellPlayerBehavior.Equals(CellPlayerBehavior.Occupy))
            {
                ChangeCellIdList(cellId);
            }

            return cellPlayerBehavior;
        }
    }

    [System.Serializable]
    public class CellInfor
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
        None,
        Occupy,
        BackPlayerCell,
        NotOccupy,
    }
}