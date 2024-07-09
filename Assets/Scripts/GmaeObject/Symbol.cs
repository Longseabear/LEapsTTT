using TTT.System;
using UnityEngine;

namespace TTT.GmaeObject
{
    [RequireComponent(typeof(Rigidbody))]
    public class Symbol : PrefabPoolable
    {
        public Rigidbody Rigidbody { get; set; }

        public void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }

        public override void OnInitialized()
        {
            Rigidbody.velocity = Vector3.zero;
            Rigidbody.angularVelocity = Vector3.zero;
        }
        public void Release()
        {
            UltimatePrefabManager.Instance.Release(this);
        }
        public void Update()
        {
        }
    }
}
