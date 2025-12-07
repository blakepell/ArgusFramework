/*
 * @author            : Blake Pell
 * @initial date      : 2021-07-01
 * @last updated      : 2025-12-07
 * @copyright         : Copyright (c) 2003-2025, All rights reserved.
 * @license           : MIT
 * @website           : http://www.blakepell.com
 */

using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Argus.Memory
{
    /// <summary>
    /// Dependency Injection wrapper for applications allowing for late-binding/mutable container behavior.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AppServices
    {
        /// <summary>
        /// Mechanism for retrieving a service object.
        /// </summary>
        public IServiceProvider ServiceProvider
        {
            get
            {
                lock (_syncLock)
                {
                    // Ensure we never return null to avoid crashes in ActivatorUtilities
                    return field ??= new ServiceCollection().BuildServiceProvider();
                }
            }
            private set
            {
                lock (_syncLock)
                {
                    field = value;
                }
            }
        }

        /// <summary>
        /// Returns the instance of <see cref="AppServices"/>. If the instance has not yet
        /// been created it will be created on this call.
        /// </summary>
        private static AppServices Instance => _instance ?? GetInstance();

        /// <summary>
        /// Internal reference for the <see cref="AppServices"/> instance.
        /// </summary>
        private static volatile AppServices? _instance;

        /// <summary>
        /// Global lock object used to protect access to both the Collection and the Provider.
        /// Unified locking prevents race conditions during the Rebuild phase.
        /// </summary>
        private static readonly object _syncLock = new();

        /// <summary>
        /// A reference to the service collection so that services can be added at a time
        /// later than the initial registration of objects.
        /// </summary>
        public static ServiceCollection ServiceCollection
        {
            get
            {
                lock (_syncLock)
                {
                    return field ??= new ServiceCollection();
                }
            }
            set
            {
                lock (_syncLock)
                {
                    field = value;
                }
            }
        }

        /// <summary>
        /// Static constructor.
        /// </summary>
        static AppServices()
        {
            // Initialize immediately to prevent null scenarios.
            ServiceCollection = new ServiceCollection();
        }

        /// <summary>
        /// Initializes the dependencies via action which allows the caller to register classes
        /// and interfaces. Init can only be called once but it allows the caller to pass in
        /// an <see cref="Action"/> to handle registering the DI.
        /// </summary>
        /// <param name="action"></param>
        public static void Init(Action<ServiceCollection> action)
        {
            lock (_syncLock)
            {
                var services = new ServiceCollection();
                action.Invoke(services);

                // Replace the static collection and rebuild the provider
                ServiceCollection = services;
                Instance.ServiceProvider = services.BuildServiceProvider();
            }
        }

        /// <summary>
        /// Registers a singleton type that will be created the first time it is used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void AddSingleton<T>() where T : class
        {
            lock (_syncLock)
            {
                // Prevent duplicates, because this isn't an instance we'll not throw
                // an exception here on duplicates.
                if (ServiceCollection.Any(x => x.ServiceType == typeof(T)))
                {
                    return;
                }

                ServiceCollection.AddSingleton<T>();
                Instance.ServiceProvider = ServiceCollection.BuildServiceProvider();
            }
        }

        /// <summary>
        /// Registers a type singleton with the specific instance provided.
        /// </summary>
        public static void AddSingleton<T>(T instance) where T : class
        {
            lock (_syncLock)
            {
                // Prevent duplicates which can confuse the provider
                if (ServiceCollection.Any(x => x.ServiceType == typeof(T)))
                {
                    throw new InvalidOperationException($"{typeof(T).Name} already has a singleton instance registered.");
                }

                ServiceCollection.AddSingleton(instance);
                Instance.ServiceProvider = ServiceCollection.BuildServiceProvider();
            }
        }

        /// <summary>
        /// Registers a type and its instance as an object.
        /// </summary>
        /// <param name="serviceType"></param>
        /// <param name="implementationInstance"></param>
        public static void AddSingleton(Type serviceType, object implementationInstance)
        {
            lock (_syncLock)
            {
                if (ServiceCollection.Any(x => x.ServiceType == serviceType))
                {
                    throw new InvalidOperationException($"{serviceType.GetType().Name} already has a singleton instance registered.");
                }

                ServiceCollection.AddSingleton(serviceType, implementationInstance);
                Instance.ServiceProvider = ServiceCollection.BuildServiceProvider();
            }
        }

        /// <summary>
        /// Allows for the registration of dependency injected services via an <see cref="Action"/>.
        /// </summary>
        /// <param name="action"></param>
        public static void AddService(Action<ServiceCollection> action)
        {
            lock (_syncLock)
            {
                action.Invoke(ServiceCollection);
                Instance.ServiceProvider = ServiceCollection.BuildServiceProvider();
            }
        }

        /// <summary>
        /// Gets a service of type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T? GetService<T>()
        {
            return Instance.ServiceProvider.GetService<T>();
        }

        /// <summary>
        /// Gets a service of the provided type.
        /// </summary>
        /// <param name="type"></param>
        public static object? GetService(Type type)
        {
            return Instance.ServiceProvider.GetService(type);
        }

        /// <summary>
        /// Gets a service of type <see cref="T"/>. If the service doesn't exist an exception
        /// will be thrown.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetRequiredService<T>() where T : notnull
        {
            return Instance.ServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// Gets a service of the provided type. If the service doesn't exist an exception
        /// will be thrown.
        /// </summary>
        /// <param name="type"></param>
        public static object GetRequiredService(Type type)
        {
            return Instance.ServiceProvider.GetRequiredService(type);
        }

        /// <summary>
        /// Creates an instance of an object and injects any dependencies into it that
        /// are required via the constructor of that object.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T CreateInstance<T>()
        {
            return ActivatorUtilities.CreateInstance<T>(Instance.ServiceProvider);
        }

        /// <summary>
        /// Creates an instance of an object and injects any dependencies into it that
        /// are required via the constructor of that object.
        /// </summary>
        /// <param name="type"></param>
        public static object CreateInstance(Type type)
        {
            return ActivatorUtilities.CreateInstance(Instance.ServiceProvider, type);
        }

        /// <summary>
        /// Gets the current instance or creates a new one.
        /// </summary>
        private static AppServices GetInstance()
        {
            // Double-check locking for performance and thread safety
            if (_instance != null)
            {
                return _instance;
            }

            lock (_syncLock)
            {
                return _instance ??= new AppServices();
            }
        }

        /// <summary>
        /// Updates the <see cref="ServiceProvider"/> with the contents of the services registered
        /// in the <see cref="ServiceCollection"/>.
        /// </summary>
        public static void BuildServiceProvider()
        {
            lock (_syncLock)
            {
                // We access the backing field directly or via the property, 
                // but since we are inside the lock, we ensure atomicity.
                Instance.ServiceProvider = ServiceCollection.BuildServiceProvider();
            }
        }

        /// <summary>
        /// If a type has been registered with the DI container.
        /// </summary>
        /// <param name="type"></param>
        public static bool IsRegistered(Type type)
        {
            lock (_syncLock)
            {
                return ServiceCollection.Any(sd => sd.ServiceType == type);
            }
        }

        /// <summary>
        /// If a type has been registered with the DI container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static bool IsRegistered<T>()
        {
            lock (_syncLock)
            {
                return ServiceCollection.Any(sd => sd.ServiceType == typeof(T));
            }
        }

        /// <summary>
        /// If a singleton instance / type has been registered with the DI container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static bool IsSingletonRegistered<T>()
        {
            lock (_syncLock)
            {
                return ServiceCollection.Any(sd => sd.ServiceType == typeof(T) && sd.Lifetime == ServiceLifetime.Singleton);
            }
        }
    }
}