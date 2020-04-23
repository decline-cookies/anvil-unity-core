using Anvil.CSharp.Content;
using UnityEngine;

namespace Anvil.Unity.Content
{
    /// <summary>
    /// A concrete implementation of a <see cref="AbstractContentManager"/> specific to Unity
    /// </summary>
    public class UnityContentManager : AbstractContentManager
    {
        /// <summary>
        /// A <see cref="Transform"/> that this Content Manager corresponds to in the Unity hierarchy
        /// </summary>
        public readonly Transform ContentRoot;
        
        /// <summary>
        /// Constructs a new instance of a <see cref="UnityContentManager"/>
        /// </summary>
        /// <param name="contentRoot"><inheritdoc cref="ContentRoot"/></param>
        public UnityContentManager(Transform contentRoot) : base()
        {
            ContentRoot = contentRoot;
            GameObject.DontDestroyOnLoad(ContentRoot);
        }

        protected override void DisposeSelf()
        {
            GameObject.Destroy(ContentRoot.gameObject);
            base.DisposeSelf();
        }

        protected override AbstractContentGroup CreateGroup(ContentGroupConfigVO configVO)
        {
            UnityContentGroup unityContentGroup = new UnityContentGroup(this, configVO);
            return unityContentGroup;
        }

        protected override void LogWarning(string message)
        {
            Debug.LogWarning(message);
        }
    }
}

