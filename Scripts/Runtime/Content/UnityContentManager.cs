using Anvil.CSharp.Content;
using UnityEngine;

namespace Anvil.Unity.Content
{
    public class UnityContentManager : AbstractContentManager
    {
        public readonly Transform ContentRoot;

        public UnityContentManager(Transform contentRoot) : base()
        {
            ContentRoot = contentRoot;
            GameObject.DontDestroyOnLoad(ContentRoot);
        }

        protected override AbstractContentGroup ConstructContentGroup(AbstractContentGroupConfigVO configVO)
        {
            UnityContentGroup unityContentGroup = new UnityContentGroup(this, configVO);
            return unityContentGroup;
        }

        protected override void LogWarning(string msg)
        {
            Debug.LogWarning(msg);
        }
    }
}

