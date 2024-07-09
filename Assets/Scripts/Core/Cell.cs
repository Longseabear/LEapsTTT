using Sirenix.OdinInspector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TTT.Core.Events;
using UnityEngine;
using UnityEngine.UIElements;

namespace TTT.Core
{
    [Serializable]
    public class Cell
    {
        [ShowInInspector] public bool IsAssigned => Player > 0;
        [ShowInInspector] public bool IsEnabled { get; set; } = false;

        public int Player = 0;

        public class MouseEventPunlisher
        {
            public Publisher<CellEvents.OnMouseDown> OnMouseDown;
            public Publisher<CellEvents.OnMouseEnter> OnMouseEnter;
            public Publisher<CellEvents.OnMouseExit> OnMouseExit;
            public MouseEventPunlisher()
            {
                OnMouseDown = new Publisher<CellEvents.OnMouseDown>();
                OnMouseEnter = new Publisher<CellEvents.OnMouseEnter>();
                OnMouseExit = new Publisher<CellEvents.OnMouseExit>();
            }

            public void SubscribeAll(object instance)
            {
                if(instance is ISubscriber<CellEvents.OnMouseEnter> mouseEnterSubscriber)
                {
                    OnMouseEnter.Subscribe(mouseEnterSubscriber);
                }

                if (instance is ISubscriber<CellEvents.OnMouseDown> mouseDownSubscriber)
                {
                    OnMouseDown.Subscribe(mouseDownSubscriber);
                }

                if (instance is ISubscriber<CellEvents.OnMouseExit> mouseExitSubscriber)
                {
                    OnMouseExit.Subscribe(mouseExitSubscriber);
                }
            }
        }

        public MouseEventPunlisher MouseEvents { get; private set; }
        public Vector2Int Position { get; private set; }

        public Cell(Vector2Int position)
        {
            MouseEvents = new MouseEventPunlisher();

            Player = 0;
            IsEnabled = false;
            Position = position;
        }

        public void OnMouseEnter(Vector2 position, float eventtime)
        {
            MouseEvents.OnMouseEnter.Notify(new CellEvents.OnMouseEnter(this, position, eventtime));
        }

        public void OnMouseExit(Vector2 position, float eventtime)
        {
            MouseEvents.OnMouseExit.Notify(new CellEvents.OnMouseExit(this, position, eventtime));
        }

        public void OnMouseDown(Vector2 position, float eventtime)
        {
            MouseEvents.OnMouseDown.Notify(new CellEvents.OnMouseDown(this, position, eventtime));
        }
    }
}
