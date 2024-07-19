using System;

namespace TTT.Actions
{
    [Serializable]
    public abstract class ActionEvent
    {
        public abstract void Execute();
        public abstract void Undo();
    }
}
