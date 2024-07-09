using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using TTT.Core;
using TTT.GmaeObject;
using TTT.Map;
using TTT.Rhythm;
using TTT.System;
using UnityEngine;
using Timer = TTT.Rhythm.Timer;

namespace TTT.Assets.Scripts.System
{
    [RequireComponent(typeof(Timer)), RequireComponent(typeof(UIBoard))]
    public partial class UltimateGamePlay : MonoBehaviour
    {
        [Header("Instance")]
        public static UltimateGamePlay Instance;

        [ShowInInspector] public Timer Timer { get; private set; }
        [ShowInInspector] public UIBoard UIBoard { get; private set; }

        public Board Board => UIBoard.Board;

        public void OnValidate()
        {
            Timer = GetComponent<Timer>();
            UIBoard = GetComponent<UIBoard>();
        }

        public void Initialize()
        {
            Timer = GetComponent<Timer>();
            UIBoard = GetComponent<UIBoard>();
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
            UltimatePrefabManager.Instance.GetActiveInstances<Symbol>().ForEach(symbol => symbol.Release());
            UIBoard.Initialize();
        }
    }
}
