/*
 * @author            : Blake Pell
 * @initial date      : 2021-07-01
 * @last updated      : 2024-11-10
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using Microsoft.Extensions.DependencyInjection;
using System.Diagnostics.CodeAnalysis;

namespace Argus.Memory
{
    /// <summary>
    /// Dependency Injection.
    /// </summary>
    [SuppressMessage("ReSharper", "UnusedMember.Global")]
    public class AppServices
    {
        /// <summary>
        /// Mechanism for retrieving a service object.
        /// </summary>
        public IServiceProvider ServiceProvider { get; set; }

        /// <summary>
        /// Returns the instance of <see cref="AppServices"/>.  If the instance has not yet
        /// been created it will be created on this call.
        /// </summary>
        private static AppServices Instance => _instance ?? GetInstance();

        /// <summary>
        /// Internal reference for the <see cref="AppServices"/> instance.
        /// </summary>
        private static AppServices _instance;

        /// <summary>
        /// Lock object used by <see cref="GetInstance"/>.
        /// </summary>
        private static readonly object InstanceLock = new();

        /// <summary>
        /// A reference to the service collection so that services can be added at a time
        /// later than the initial registration of objects.
        /// </summary>
        public static ServiceCollection? ServiceCollection;

        /// <summary>
        /// Initializes the dependencies via action which allows the caller to register classes
        /// and interfaces.  Init can only be called once but it allows the caller to pass in
        /// an <see cref="Action"/> to handle registering the DI so that the DI can handle both
        /// Common library classes or client/environment specific classes if this is ever ported
        /// to other platforms.
        /// </summary>
        /// <param name="action"></param>
        public static void Init(Action<ServiceCollection?> action)
        {
            var services = new ServiceCollection();
            action.Invoke(services);
            Instance.ServiceProvider = services.BuildServiceProvider();
            ServiceCollection = services;
        }

        /// <summary>
        /// Registers a singleton type that will be created the first time it is used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void AddSingleton<T>() where T : class
        {
            ServiceCollection ??= new ServiceCollection();
            ServiceCollection.AddSingleton<T>();
            Instance.ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Registers a type singleton with the copy of the object that should be presented on
        /// dependency injection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The specific instance that should be added as a singleton.</param>
        public static void AddSingleton<T>(T instance) where T : class
        {
            ServiceCollection ??= new ServiceCollection();
            ServiceCollection.AddSingleton(instance);
            Instance.ServiceProvider = ServiceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Allows for the registration of dependency injected services via an <see cref="Action"/>.
        /// </summary>
        /// <param name="action"></param>
        public static void AddService(Action<ServiceCollection?> action)
        {
            ServiceCollection ??= new ServiceCollection();
            action.Invoke(ServiceCollection);
            Instance.ServiceProvider = ServiceCollection.BuildServiceProvider();
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
        /// Gets a service of type <see cref="T"/>.  If the service doesn't exist an exception
        /// will be thrown.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetRequiredService<T>() where T : notnull
        {
            return Instance.ServiceProvider.GetRequiredService<T>();
        }

        /// <summary>
        /// Gets a service of the provided type.  If the service doesn't exist an exception
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
            lock (InstanceLock)
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
            lock (InstanceLock)
            {
                Instance.ServiceProvider = ServiceCollection.BuildServiceProvider();
            }
        }
        
        /// <summary>
        /// If a type has been registered with the DI container.
        /// </summary>
        /// <param name="type"></param>
        public static bool IsRegistered(Type type)
        {
            ServiceCollection ??= new ServiceCollection();
            var serviceDescriptor = ServiceCollection.FirstOrDefault(sd => sd.ServiceType == type);
            return serviceDescriptor != null;
        }

        /// <summary>
        /// If a type has been registered with the DI container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static bool IsRegistered<T>()
        {
            ServiceCollection ??= new ServiceCollection();
            var serviceDescriptor = ServiceCollection.FirstOrDefault(sd => sd.ServiceType == typeof(T));
            return serviceDescriptor != null;
        }

        /// <summary>
        /// If a singleton instance / type has been registered with the DI container.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static bool IsSingletonRegistered<T>()
        {
            ServiceCollection ??= new ServiceCollection();
            var serviceDescriptor = ServiceCollection.FirstOrDefault(sd => sd.ServiceType == typeof(T) && sd.Lifetime == ServiceLifetime.Singleton);
            return serviceDescriptor != null;
        }
    }
}