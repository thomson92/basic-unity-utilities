using System;

namespace UnityUtilities.Events {
    public interface IBaseSubscriber {
        void Unsubscribe();
    }

    public interface ISubscriber<T> : IBaseSubscriber {
        void ActionOnObservableChange(T data);
    }

    public class Subscriber<T> : ISubscriber<T> {

        protected Action<T> Action;
        private IObservable<T> _subscribedObservable;

        public Subscriber(IObservable<T> obs, Action<T> action) {
            Action = action;
            _subscribedObservable = obs;
        }

        public void ActionOnObservableChange(T data) {
            Action.Invoke(data);
        }

        public void Unsubscribe() {
            _subscribedObservable.RemoveSubscriber(this);
        }
    }
}
