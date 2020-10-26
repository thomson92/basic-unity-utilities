using System;
using System.Collections.Generic;

namespace UnityUtilities.Events {
    public interface ISubscription {
        void AddSubscriber(IBaseSubscriber subscriber);
        void RemoveSubscriber(IBaseSubscriber subscriber);
        void Unsubscribe();
        bool Any();
    }

    public class Subscription : ISubscription {
        private List<IBaseSubscriber> _subscribers;

        public Subscription() {
            _subscribers = new List<IBaseSubscriber>();
        }

        public void AddSubscriber(IBaseSubscriber subscriber) {
            if(!_subscribers.Contains(subscriber)) {
                _subscribers.Add(subscriber);
            } else {
                Console.WriteLine("Subscriber already added");
            }
        }

        public void AddSubscriber(IBaseSubscriber[] subscriber) {
            _subscribers.AddRange(subscriber);
        }

        public bool Any() {
            return _subscribers.Count > 0;
        }

        public void RemoveSubscriber(IBaseSubscriber subscriber) {
            _subscribers.Remove(subscriber);
        }

        public void Unsubscribe() {
            foreach(var sub in _subscribers) {
                sub.Unsubscribe();
            }
            _subscribers.Clear();
        }
    }
}
