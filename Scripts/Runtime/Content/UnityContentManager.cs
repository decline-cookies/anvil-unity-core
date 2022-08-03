using Anvil.CSharp.Content;
using Anvil.CSharp.Logging;
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

            PreventDestroyOnLoad();
        }

        protected override void DisposeSelf()
        {
            if (ContentRoot != null)
            {
                Object.Destroy(ContentRoot.gameObject);
            }

            base.DisposeSelf();
        }

        protected override AbstractContentGroup CreateGroup(ContentGroupConfigVO configVO)
        {
            UnityContentGroup unityContentGroup = new UnityContentGroup(this, configVO);
            return unityContentGroup;
        }

        private void PreventDestroyOnLoad()
        {
            if (ContentRoot.parent == null)
            {
                Object.DontDestroyOnLoad(ContentRoot);
            }
            else
            {
                Logger.Warning(
                    $"This {nameof(UnityContentManager)} may be destroyed on a scene change. ContentRoot: {ContentRoot.name}"
                    + $"\nThe {nameof(ContentRoot)} provided is not at the root of the hierarchy."
                    );
            }
        }
    }
}