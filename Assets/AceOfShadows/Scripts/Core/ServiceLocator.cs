using UnityEngine;

namespace AceOfShadows
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Simple service locator for registering and resolving services by type.
    /// </summary>
    public static class ServiceLocator
    {
        private static readonly Dictionary<Type, object> services = new Dictionary<Type, object>();

        /// <summary>
        /// Registers a service instance for the specified type T.
        /// </summary>
        public static void Register<T>(T service)
        {
            var type = typeof(T);
            if (services.ContainsKey(type))
            {
                services[type] = service;
            }
            else
            {
                services.Add(type, service);
            }
        }

        /// <summary>
        /// Resolves the service instance registered for the specified type T.
        /// </summary>
        public static T Resolve<T>()
        {
            var type = typeof(T);
            if (services.TryGetValue(type, out var service))
            {
                return (T)service;
            }
            throw new InvalidOperationException($"Service of type {type} is not registered.");
        }
    }

}
