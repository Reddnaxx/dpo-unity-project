using System;
using System.Collections.Generic;
using UniRx;

namespace _00_Scripts.Helpers
{
    public static class Events
    {
        private static readonly Dictionary<string, Subject<object>> EventSubjects = new();
        private static readonly Dictionary<string, List<string>> EventCallbacks = new();

        public static IObservable<T> On<T>(string eventName, string callbackName)
        {
            if (!EventSubjects.ContainsKey(eventName))
            {
                EventSubjects[eventName] = new Subject<object>();
                EventCallbacks[eventName] = new List<string>();
            }

            EventCallbacks[eventName].Add(callbackName);
            return EventSubjects[eventName].OfType<object, T>();
        }

        public static void Publish<T>(string eventName, T eventData)
        {
            if (EventSubjects.TryGetValue(eventName, out var @event))
            {
                @event.OnNext(eventData);
            }
        }

        public static void Off(string eventName, string callbackName)
        {
            if (!EventSubjects.ContainsKey(eventName)) return;

            EventCallbacks[eventName].Remove(callbackName);

            if (EventCallbacks[eventName].Count != 0) return;

            EventSubjects[eventName].OnCompleted();
            EventSubjects.Remove(eventName);
            EventCallbacks.Remove(eventName);
        }
    }
}