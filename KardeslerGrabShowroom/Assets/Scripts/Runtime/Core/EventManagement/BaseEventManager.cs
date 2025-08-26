using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;

namespace IboshEngine.Runtime.Core.EventManagement
{
    /// <summary>
    /// Base class for event management.
    /// </summary>
    /// <typeparam name="T">The type of the enum.</typeparam>
    public class BaseEventManager<T> where T : Enum
    {
        private readonly Dictionary<T, Action> _events = new();
        private readonly Dictionary<T, Delegate> _genericEvents = new();

        /// <summary>
        /// Adds a listener to the event.
        /// </summary>
        /// <param name="gameEvent">The event to listen to.</param>
        /// <param name="action">The action to perform when the event is triggered.</param>
        public void AddListener(T gameEvent, Action action)
        {
            if (!_events.TryAdd(gameEvent, action))
                _events[gameEvent] += action;
        }

        /// <summary>
        /// Removes a listener from the event.
        /// </summary>
        /// <param name="gameEvent">The event to remove the listener from.</param>
        /// <param name="action">The action to remove from the event.</param>
        public void RemoveListener(T gameEvent, Action action)
        {
            if (!_events.ContainsKey(gameEvent)) return;

            _events[gameEvent] -= action;
            if (_events[gameEvent] == null)
                _events.Remove(gameEvent);
        }

        /// <summary>
        /// Adds a listener to the generic event.
        /// </summary>
        /// <typeparam name="TU">The type of the parameter.</typeparam>
        /// <param name="gameEvent">The event to listen to.</param>
        /// <param name="action">The action to perform when the event is triggered.</param>
        public void AddListener<TU>(T gameEvent, Action<TU> action)
        {
            if (!_genericEvents.TryAdd(gameEvent, action))
                _genericEvents[gameEvent] = Delegate.Combine(_genericEvents[gameEvent], action);
        }

        /// <summary>
        /// Removes a listener from the generic event.
        /// </summary>
        /// <typeparam name="TU">The type of the parameter.</typeparam>
        /// <param name="gameEvent">The event to remove the listener from.</param>
        /// <param name="action">The action to remove from the event.</param>
        public void RemoveListener<TU>(T gameEvent, Action<TU> action)
        {
            if (!_genericEvents.ContainsKey(gameEvent)) return;

            _genericEvents[gameEvent] = Delegate.Remove(_genericEvents[gameEvent], action);
            if (_genericEvents[gameEvent] == null)
                _genericEvents.Remove(gameEvent);
        }

        /// <summary>
        /// Broadcasts the event.
        /// </summary>
        /// <param name="gameEvent">The event to broadcast.</param>
        /// <param name="delay">The delay before the event is broadcast.</param>
        public async void Broadcast(T gameEvent, float delay = 0)
        {
            if (delay > 0) await UniTask.Delay(TimeSpan.FromSeconds(delay));

            if (_events.TryGetValue(gameEvent, out Action @event) && @event != null)
                @event.Invoke();
        }

        /// <summary>
        /// Broadcasts the generic event.
        /// </summary>
        /// <typeparam name="TU">The type of the parameter.</typeparam>
        /// <param name="gameEvent">The event to broadcast.</param>
        /// <param name="param">The parameter to pass to the event.</param>
        public async void Broadcast<TU>(T gameEvent, TU param, float delay = 0)
        {
            if (delay > 0) await UniTask.Delay(TimeSpan.FromSeconds(delay));

            if (_genericEvents.TryGetValue(gameEvent, out Delegate genericEvent) && genericEvent is Action<TU> action)
                action.Invoke(param);

            Broadcast(gameEvent);
        }
    }
}