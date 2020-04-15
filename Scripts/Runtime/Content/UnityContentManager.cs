using Anvil.CSharp.Content;
using UnityEngine;

namespace Anvil.Unity.Content
{
    public class UnityContentManager : AbstractContentManager<UnityContentGroup,AbstractUnityContentController,AbstractUnityContent>
    {
        public readonly Transform ContentRoot;

        public UnityContentManager(Transform contentRoot) : base()
        {
            ContentRoot = contentRoot;
        }

        protected override UnityContentGroup ConstructContentGroup(string id, Vector3 localPosition, Transform gameObjectRoot = null)
        {
            return new UnityContentGroup(this, id, localPosition, gameObjectRoot);
        }

        protected override void LogWarning(string msg)
        {
            Debug.LogWarning(msg);
        }


        public AbstractContentManager CreateContentGroup(string id, Vector3 localPosition, Transform gameObjectRoot = null)
        {
            if (m_ContentGroups.ContainsKey(id))
            {
                throw new ArgumentException($"Content Groups ID of {id} is already registered with the Content Manager!");
            }
            
            AbstractContentGroup contentGroup = new AbstractContentGroup(this, id, localPosition, gameObjectRoot);
            m_ContentGroups.Add(contentGroup.ID, contentGroup);

            AddLifeCycleListeners(contentGroup);
            
            return this;
        }
        
        
    }
}

