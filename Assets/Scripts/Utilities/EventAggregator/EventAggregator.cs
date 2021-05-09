using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public static class EventAggregator
{
    public static void Subscribe<T>(Action<object, T> subscribeEvent)
    {
        EventHolder<T>.Event += subscribeEvent;
    }

    public static void Unsubscribe<T>(Action<object, T> unsubscribeEvent)
    {
        EventHolder<T>.Event -= unsubscribeEvent;
    }

    public static void Post<T>(object sender, T eventData)
    {
        EventHolder<T>.Post(sender, eventData);
    }

    private static class EventHolder<T>
    {
        public static event Action<object, T> Event;

        public static void Post(object sender, T eventData)
        {
            Event?.Invoke(sender, eventData);
        }
    }
}
