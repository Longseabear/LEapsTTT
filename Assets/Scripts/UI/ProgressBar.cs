using TTT.Rhythm;
using UnityEngine;
using UnityEngine.UI;

namespace TTT.UI
{
    class ProgressBar : MonoBehaviour
    {
        public Image Background;
        public Image Bar;
        public INormalizedValue Target;

        public float Progress
        {
            get => Bar.fillAmount;
            set
            {
                Bar.fillAmount = value;
            }
        }
        public void Bind(INormalizedValue target)
        {
            Target = target;
        }
        public void Update()
        {
            if(Target != null)
            {
                Progress = Target.NormalizedValue;
            }
        }
    }
}
