using System;
using System.Collections.Generic;

namespace UnityUtilities.Events {
    public interface IObservable<T> {
        void NotifySubscribers(T data);
        void RemoveSubscriber(ISubscriber<T> subscriber);
        ISubscriber<T> Subscribe(Action<T> action);
    }

    public abstract class ObservableBase<T> : IObservable<T> {

        protected List<ISubscriber<T>> _subscribers;
        protected T _currentDataState;

        public ObservableBase() {
            _subscribers = new List<ISubscriber<T>>();
        }

        public void NotifySubscribers(T data) {

            _currentDataState = data;

            if(_subscribers?.Count > 0) {
                foreach(var observer in _subscribers) {
                    observer.ActionOnObservableChange(data);
                }
            }
        }

        public void RemoveAllSubscribers() {
            foreach(var subscriber in _subscribers) {
                subscriber.Unsubscribe();
            }
            _subscribers.Clear();
        }

        public void RemoveSubscriber(ISubscriber<T> subscriber) {
            _subscribers.Remove(subscriber);
        }

        public T GetLastEmittedValue() {
            return _currentDataState;
        }

        public abstract ISubscriber<T> Subscribe(Action<T> action);
    }

}
