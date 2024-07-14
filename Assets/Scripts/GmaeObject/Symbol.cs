using DG.Tweening;
using TTT.Rhythms;
using TTT.System;
using UnityEngine;

namespace TTT.GmaeObject
{
    [RequireComponent(typeof(Rigidbody))]
    public class Symbol : PrefabPoolable, IRhythmHandler
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

        public void Receive(Rhythms.Rhythm ryhthm)
        {
        }

        public void Release()
        {
            UltimatePrefabManager.Instance.Release(this);
        }
        public void ReleaseSmooth()
        {
            Renderer renderer = GetComponent<Renderer>();

            // Ensure we have a Renderer to fade out
            if (renderer != null)
            {
                // Get the current color of the material
                Color originalColor = renderer.material.color;

                // Fade the alpha of the material's color to 0 over the duration
                renderer.material.DOFade(0, 1.0f).OnComplete(() => 
                {
                    Release();
                });
            }
        }
        public void Update()
        {
        }
    }
}
