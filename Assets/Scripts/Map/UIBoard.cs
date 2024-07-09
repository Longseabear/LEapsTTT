using Sirenix.OdinInspector;
using System.Collections.Generic;
using TTT.Assets.Scripts.Map;
using TTT.Core;
using UnityEngine;

namespace TTT.Map
{
    public class UIBoard : MonoBehaviour
    {
        [SerializeField, ShowInInspector] public Board Board { get; private set; }

        public UICell CellPrefab;

        public Dictionary<Cell, UICell> CellToUICell { get; private set; }

        [Button("Initialize")]
        public void Initialize()
        {
            var cellObject = this.transform.Find("Cells");
            if (cellObject != null)
            {
                DestroyImmediate(cellObject.gameObject);
            }
            cellObject = new GameObject($"Cells").transform;
            cellObject.parent = this.transform;

            Board = new Board();
            CellToUICell = new Dictionary<Cell, UICell>();

            foreach (var y in Board.GetY())
            {
                foreach (var x in Board.GetX())
                {
                    var cell= Instantiate<UICell>(CellPrefab, new Vector3(x, CellPrefab.transform.position.y, y), Quaternion.identity, cellObject);
                    cell.Initialize(Board[y, x]);
                    cell.gameObject.name = $"Cell({y},{x})";
                    CellToUICell[Board[y, x]] = cell;
                }
            }
        }

        public void Start()
        {
            Initialize();
        }
    }
}
