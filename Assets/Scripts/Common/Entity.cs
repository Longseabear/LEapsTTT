using Sirenix.OdinInspector;
using TTT.Node;
using TTT.System;

namespace TTT.Common
{
    public abstract class Entity
    {
        [ShowInInspector, ReadOnly] public int ID { get; set; }
    }

    public abstract class FlowNodeEntity : Entity
    {
        public FlowNode Parent { get; private set; }
        [ShowInInspector, ReadOnly] public string Name { get; private set; }
        public virtual void Register(FlowNode parent)
        {
            Parent = parent;
            ID = UltimateFlowManager.Instance.Register(this);

            string ParentName = string.Empty;
            if(parent != null)
            {
                ParentName = parent.Name + "/";
            }
            Name = $"{ParentName}{this.GetType().Name}({ID})";

            UltimateFlowManager.Instance.RegisterUsingName(this);
        }
    }
}
