using System.Collections.Generic;
using System.Linq;

namespace TTT.Core.Events
{
    public interface IPublisher<T> where T : Event
    {
        void Subscribe(ISubscriber<T> subscriber);
        void Unsubscribe(ISubscriber<T> subscriber);
        void Notify(T data);
    }

    public class Publisher<T> : IPublisher<T> where T : Event
    {
        private List<ISubscriber<T>> _subscribers = new List<ISubscriber<T>>();

        public void Subscribe(ISubscriber<T> subscriber)
        {
            if (!_subscribers.Contains(subscriber))
            {
                _subscribers.Add(subscriber);
            }
        }

        public void Unsubscribe(ISubscriber<T> subscriber)
        {
            if (_subscribers.Contains(subscriber))
            {
                _subscribers.Remove(subscriber);
            }
        }

        public void Notify(T data)
        {
            foreach (var subscriber in _subscribers.ToList())
            {
                subscriber.Recieve(data);
            }
        }
    }
}
