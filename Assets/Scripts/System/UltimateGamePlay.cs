using Sirenix.OdinInspector;
using System.Collections.Generic;
using TTT.Core;
using TTT.GmaeObject;
using TTT.Map;
using TTT.Measures;
using TTT.Players;
using TTT.Rhythms;
using TTT.Simulation;
using UnityEngine;
using Timer = TTT.Rhythms.Timer;

namespace TTT.System
{
    [RequireComponent(typeof(Timer)), RequireComponent(typeof(UIBoard))]
    public partial class UltimateGamePlay : MonoBehaviour
    {
        public static UltimateGamePlay Instance { get; private set; }
        [ShowInInspector] public UIBoard UIBoard { get; private set; }
        [ShowInInspector] public Measure CurrentMeasure { get; set; }
        [ShowInInspector] public Player Attacker { get;  set; }
        [ShowInInspector] public Player Defender { get; set; }

        public Board Board => UIBoard.Board;

        public void OnValidate()
        {
            UIBoard = GetComponent<UIBoard>();
        }

        public void Initialize()
        {
            UIBoard = GetComponent<UIBoard>();
        }


        public void PlayerSwap()
        {
            var tmp = Attacker;
            Attacker = Defender;
            Defender = tmp;
        }

        public void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
            Initialize();
        }
    }

    // Debug
    public partial class UltimateGamePlay : MonoBehaviour
    {
        [Button("WinPlanDebug")]
        void WinPlanDebug(int direction, int x, Color color)
        {
            if(direction == 1)
            {
                foreach (var cell in Board.GetActivatedVerticalCells(x))
                {
                    cell.IsEnabled = true;
                    UIBoard.CellToUICell[cell].enabled = true;
                    UIBoard.CellToUICell[cell].GetComponent<Renderer>().material.color = color;
                };
            }

            else if(direction == 2)
            {
                foreach (var cell in Board.GetActivatedHorizontalCells(x))
                {
                    cell.IsEnabled = true;
                    UIBoard.CellToUICell[cell].enabled = true;
                    UIBoard.CellToUICell[cell].GetComponent<Renderer>().material.color = color;
                };
            }else if(direction == 3)
            {
                foreach (var cell in Board.GetActivatedSlashCells(x))
                {
                    cell.IsEnabled = true;
                    UIBoard.CellToUICell[cell].enabled = true;
                    UIBoard.CellToUICell[cell].GetComponent<Renderer>().material.color = color  ;
                };
            }
            else if (direction == 4)
            {
                foreach (var cell in Board.GetActivatedBackSlashCells(x))
                {
                    cell.IsEnabled = true;
                    UIBoard.CellToUICell[cell].enabled = true;
                    UIBoard.CellToUICell[cell].GetComponent<Renderer>().material.color = color;
                };
            }
        }

        [Button("Clear Board")]
        public void ClearBoard()
        {
            Debug.Log($"Request Clear Board {UltimatePrefabManager.Instance.GetActiveInstances<Symbol>().Count}");
            UltimatePrefabManager.Instance.GetActiveInstances<Symbol>().ForEach(symbol => symbol.ReleaseSmooth());
            // UIBoard.Initialize();
        }
    }
}
