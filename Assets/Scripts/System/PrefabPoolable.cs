using TTT.Core.Events;
using Unity.VisualScripting;
using UnityEngine;

namespace TTT.System
{
    public abstract class PrefabPoolable : MonoBehaviour
    {
        public virtual void OnCreated()
        {
        }
        public virtual void OnInitialized()
        {
        }
        public virtual void OnReleased()
        {
        }
        public virtual void OnDestroyed()
        {
        }
    }
}
