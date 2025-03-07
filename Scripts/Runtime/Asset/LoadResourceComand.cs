using Anvil.CSharp.Command;
using System;
using UnityEngine;

namespace Anvil.Unity.Asset
{
    /// <summary>
    /// Asynchronous loads a resource and provides convenience methods for instantiating.
    /// </summary>
    /// <typeparam name="T">The type of resource to load/instantiate</typeparam>
    public class LoadResourceCommand<T> : AbstractCommand<LoadResourceCommand<T>> where T : UnityEngine.Object
    {
        private readonly string m_Path;
        private ResourceRequest m_ResourceRequest;

        /// <summary>
        /// Get reference to the source resource.
        /// NOTE: Manipulating this reference will change the source file in the project!
        /// </summary>
        /// <exception cref="InvalidOperationException">Thrown if the command is not <see cref="CommandState.Completed"/>.</exception>
        public T Resource
        {
            get
            {
                if (State != CommandState.Completed)
                {
                    throw new InvalidOperationException($"Tried to get {nameof(Resource)} on {this} but State was {State} instead of {CommandState.Completed}!");
                }

                return (T)m_ResourceRequest.asset;
            }
        }

        /// <summary>
        /// Creates an instance of <see cref="LoadResourceCommand{T}"/>
        /// </summary>
        /// <param name="path">The path to the resource to load</param>
        /// <exception cref="ArgumentException">Thrown for null or empty resource paths</exception>
        public LoadResourceCommand(string path) : base()
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentException($"Argument {nameof(path)} is not valid! {nameof(path)}:{path}");
            }

            m_Path = path;
        }

        protected override void DisposeSelf()
        {
            if (m_ResourceRequest != null)
            {
                m_ResourceRequest.completed -= ResourceRequest_Completed;
            }
            m_ResourceRequest = null;

            base.DisposeSelf();
        }

        protected override void ExecuteCommand()
        {
            m_ResourceRequest = Resources.LoadAsync<T>(m_Path);
            m_ResourceRequest.completed += ResourceRequest_Completed;
        }

        private void ResourceRequest_Completed(AsyncOperation sender)
        {
            m_ResourceRequest.completed -= ResourceRequest_Completed;
            CompleteCommand();
        }

        /// <summary>
        /// Creates a new instance of the loaded resource
        /// </summary>
        /// <returns>An instance of the loaded resource of type <see cref="{T}"/></returns>
        /// <exception cref="InvalidOperationException">Thrown if called before the command is <see cref="CommandState.Completed"/></exception>
        public T CreateInstance()
        {
            if (State != CommandState.Completed)
            {
                throw new InvalidOperationException($"Tried to call {nameof(CreateInstance)} on {this} but State was {State} instead of {CommandState.Completed}!");
            }

            return (T)UnityEngine.Object.Instantiate(Resource);
        }
    }
}
