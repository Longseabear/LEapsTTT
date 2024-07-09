using Sirenix.OdinInspector;
using TTT.Assets.Scripts.System;
using TTT.Core;
using UnityEngine;

namespace TTT.Assets.Scripts.Map
{
    public class UICell : MonoBehaviour
    {
        [ShowInInspector] public Cell Cell { get; private set; }

        private Material _material { get; set; }
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
            Cell.OnMouseEnter(mousePosition, UltimateGamePlay.Instance.Timer.ElapsedTime);
        }

        void OnMouseExit()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Cell.OnMouseExit(mousePosition, UltimateGamePlay.Instance.Timer.ElapsedTime);
        }

        void OnMouseDown()
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Cell.OnMouseDown(mousePosition, UltimateGamePlay.Instance.Timer.ElapsedTime);
        }
    }
}
