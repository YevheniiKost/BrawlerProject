using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ServiceLocator 
{
    private static Dictionary<Type, object> _services = new Dictionary<Type, object>();

    public static void Register<T>(T service) where T : class
    {
        if (_services.ContainsKey(typeof(T)))
        {
            Debug.Log("This service was already registered");
            return;
        }
        _services[typeof(T)] = service;
       // Debug.Log($"Was registered: {typeof(T).Name}");
    }

    public static void Unregister<T>(T service) where T : class
    {
        if (_services.ContainsKey(typeof(T)))
        {
            _services.Remove(typeof(T));
        }
        else
        {
            Debug.Log("This service was already unregistered");
        }
    }

    public static T Resolve<T>() where T : class
    {
        if (_services.ContainsKey(typeof(T)))
        {
            return (T)_services[typeof(T)];
        }
        Debug.Log($"Service with type {typeof(T).Name} was not registered");
        return null;
    }
}
