/*
 * @author            : Blake Pell
 * @initial date      : 2021-07-01
 * @last updated      : 2022-07-01
 * @copyright         : Copyright (c) 2003-2022, All rights reserved.
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
        /// Initializes the dependencies via action which allows the caller to register classes
        /// and interfaces.  Init can only be called once but it allows the caller to pass in
        /// an <see cref="Action"/> to handle registering the DI so that the DI can handle both
        /// Common library classes or client/environment specific classes if this is ever ported
        /// to other platforms.
        /// </summary>
        /// <param name="action"></param>
        public static void Init(Action<ServiceCollection> action)
        {
            var services = new ServiceCollection();
            action.Invoke(services);
            Instance.ServiceProvider = services.BuildServiceProvider();
            _serviceCollection = services;
        }

        /// <summary>
        /// Registers a singleton type that will be created the first time it is used.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void AddSingleton<T>() where T : class
        {
            _serviceCollection ??= new ServiceCollection();
            _serviceCollection.AddSingleton<T>();
            Instance.ServiceProvider = _serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Registers a type singleton with the copy of the object that should be presented on
        /// dependency injection.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance">The specific instance that should be added as a singleton.</param>
        public static void AddSingleton<T>(T instance) where T : class
        {
            _serviceCollection ??= new ServiceCollection();
            _serviceCollection.AddSingleton(instance);
            Instance.ServiceProvider = _serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Allows for the registration of dependency injected services via an <see cref="Action"/>.
        /// </summary>
        /// <param name="action"></param>
        public static void AddService(Action<ServiceCollection> action)
        {
            _serviceCollection ??= new ServiceCollection();
            action.Invoke(_serviceCollection);
            Instance.ServiceProvider = _serviceCollection.BuildServiceProvider();
        }

        /// <summary>
        /// Gets a service of type <see cref="T"/>.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetService<T>()
        {
            return Instance.ServiceProvider.GetService<T>();
        }

        /// <summary>
        /// Gets a service of type <see cref="T"/>.  If the service doesn't exist an exception
        /// will be thrown.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static T GetRequiredService<T>()
        {
            return Instance.ServiceProvider.GetRequiredService<T>();
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
        /// A reference to the service collection so that services can be added at a time
        /// later than the initial registration of objects.
        /// </summary>
        private static ServiceCollection _serviceCollection;

    }
}