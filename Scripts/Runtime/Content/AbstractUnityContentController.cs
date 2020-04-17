using Anvil.CSharp.Content;
using UnityEngine;

namespace Anvil.Unity.Content
{
    public abstract class AbstractUnityContentController : AbstractContentController
    {
        //TODO: Remove later on
        private ResourceRequest m_ResourceRequest;
        protected AbstractUnityContentController(string contentGroupID, string contentLoadingID) : base(contentGroupID, contentLoadingID)
        {
        }

        public override void Load()
        {
            //TODO: Need to load all the required assets.
            //TODO: Need to load the actual prefab or scene.
            //TODO: Need to allow the user to insert their own logic in here.
            
            //For now we'll just assume it's a prefab and we're Resources.Loading it.
            //TODO: Support addressables

            m_ResourceRequest = Resources.LoadAsync<GameObject>(ContentLoadingID);
            m_ResourceRequest.completed += HandleOnResourceLoaded;
        }
        
        private void HandleOnResourceLoaded(AsyncOperation asyncOperation)
        {
            GameObject instance = (GameObject)GameObject.Instantiate(m_ResourceRequest.asset);
            //TODO: Properly sanitize the name with a Regex via util method
            instance.name = instance.name.Replace("(Clone)", string.Empty);
            Content = instance.GetComponent<IContent>();

            m_ResourceRequest.completed -= HandleOnResourceLoaded;
            LoadComplete();
        }
    }
}

