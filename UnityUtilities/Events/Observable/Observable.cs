using System;

namespace UnityUtilities.Events {
    public class Observable<T> : ObservableBase<T> {

        public override ISubscriber<T> Subscribe(Action<T> action) {
            ISubscriber<T> sub = new Subscriber<T>(this, action);
            _subscribers.Add(sub);

            return sub;
        }
    }
}
