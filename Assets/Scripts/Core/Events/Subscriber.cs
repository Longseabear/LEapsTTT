using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TTT.Core.Events
{
    public interface ISubscriber<T>  where T : Event
    {
        void Recieve(T data);
    }
}
