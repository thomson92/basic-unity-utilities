using System;

namespace UnityUtilities.Events {
    public class BehaviourSubject<T> : ObservableBase<T> {

        public BehaviourSubject(T initialSubjectValue) : base() {
            _currentDataState = initialSubjectValue;
        }

        public override ISubscriber<T> Subscribe(Action<T> action) {
            ISubscriber<T> sub = new Subscriber<T>(this, action);
            _subscribers.Add(sub);

            if(_currentDataState != null) {
                // Push current state for new subscriber.
                sub.ActionOnObservableChange(_currentDataState);
            }

            return sub;
        }
    }
}
