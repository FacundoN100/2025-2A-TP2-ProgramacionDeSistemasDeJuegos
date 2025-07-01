
using System;
using System.Collections.Generic;


public static class GameServiceRegistry
{
    private static readonly Dictionary<Type, object> _services = new();

    
    public static void Add<T>(T service) where T : class
    {
        var t = typeof(T);
        if (!_services.ContainsKey(t) && service != null)
            _services[t] = service;
    }

    
    public static T Get<T>() where T : class
    {
        _services.TryGetValue(typeof(T), out var svc);
        return svc as T;
    }

   
    public static void Remove<T>() where T : class
    {
        _services.Remove(typeof(T));
    }
}