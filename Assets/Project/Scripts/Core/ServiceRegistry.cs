using System;
using System.Collections.Generic;
using UnityEngine;

namespace LS.Core
{
    public class ServiceRegistry
    {
        private readonly Dictionary<Type, object> _services = new();

        public void Register<T>(T service) where T : class
        {
            _services[typeof(T)] = service;
        }

        public T Resolve<T>() where T : class
        {
            if (_services.TryGetValue(typeof(T), out var service)) return (T)service;

            throw new InvalidOperationException($"[GameContext] Service {typeof(T).Name} not registered!");
        }

    }
}
