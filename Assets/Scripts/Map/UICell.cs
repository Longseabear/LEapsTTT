using Sirenix.OdinInspector;
using TTT.Core;
using TTT.GmaeObject;
using TTT.System;
using UnityEngine;

namespace TTT.Map
{
    public class UICell : MonoBehaviour
    {
        [ShowInInspector] public Cell Cell { get; private set; }

        private Material _material { get; set; }

        private SimulationParam _simulationParam => UltimateSimulationManager.Instance.SimulationParam;

        [ShowInInspector] public bool[] symbolOnCell = new bool[5];

        public void Awake()
        {
            _material = GetComponent<Material>();
        }

        public void Initialize(Cell cell)
        {
            Cell = cell;
            gameObject.SetActive(Cell.IsEnabled);
        }

        void OnMouseEnter()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Cell.OnMouseEnter(mousePosition, _simulationParam.MainTimer.ElapsedTime);
        }

        void OnMouseExit()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Cell.OnMouseExit(mousePosition, _simulationParam.MainTimer.ElapsedTime);
        }

        void OnMouseDown()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Cell.OnMouseDown(mousePosition, _simulationParam.MainTimer.ElapsedTime);
        }


        public void Update()
        {
            symbolOnCell[1] = symbolOnCell[2] = false;
            var detectionCollider = GetComponent<Collider>();

            Collider[] hitColliders = UnityEngine.Physics.OverlapBox(detectionCollider.bounds.center, detectionCollider.bounds.extents, Quaternion.identity);

            foreach (var hitCollider in hitColliders)
            {
                var symbol = hitCollider.gameObject.GetComponentInParent<Symbol>();
                if (symbol != null)
                {
                    symbolOnCell[symbol.Player.PlayerID] = true;
                }
            }

            Color color1 = Color.white;
            Color color2 = Color.white;

            if (!(symbolOnCell[1] ^ symbolOnCell[2])) Cell.Player = 0;
            else Cell.Player = symbolOnCell[1] ? 1 : 2; 
            
            if (symbolOnCell[1]) color1 = Color.red;
            if (symbolOnCell[2]) color2 = Color.blue;

            GetComponent<Renderer>().material.color = Color.Lerp(color1, color2, 0.5f);
        }
    }
}
