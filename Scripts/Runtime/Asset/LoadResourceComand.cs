using Anvil.CSharp.Command;
using System;
using UnityEngine;

namespace Anvil.Unity.Asset
{
    public class LoadResourceCommand<T> : AbstractCommand where T : UnityEngine.Object
    {
        private readonly string m_Path;
        private ResourceRequest m_ResourceRequest;

        public T Resource
        {
            get
            {
                if (State != CommandState.Completed)
                {
                    throw new Exception($"Tired to get {nameof(Resource)} on {this} but State was {State} instead of {CommandState.Completed}!");
                }

                return (T)m_ResourceRequest.asset;
            }
        }

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

        public T CreateInstance()
        {
            if (State != CommandState.Completed)
            {
                throw new Exception($"Tired to call {nameof(CreateInstance)} on {this} but State was {State} instead of {CommandState.Completed}!");
            }

            return (T)UnityEngine.Object.Instantiate(Resource);
        }
    }
}
