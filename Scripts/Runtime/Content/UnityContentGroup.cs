using Anvil.CSharp.Content;
using UnityEngine;

namespace Anvil.Unity.Content
{
    /// <summary>
    /// A concrete implementation of <see cref="AbstractContentGroup"/> specific to Unity.
    /// </summary>
    public class UnityContentGroup : AbstractContentGroup
    {
        /// <summary>
        /// The <see cref="Transform"/> that this Group corresponds to in the Unity hierarchy.
        /// </summary>
        public Transform ContentGroupRoot { get; private set; }

        /// <summary>
        /// Creates a new <see cref="UnityContentGroup"/> to be used with a <see cref="UnityContentManager"/>
        /// </summary>
        /// <param name="contentManager">The <see cref="AbstractContentManager"/> that controls this Content Group</param>
        /// <param name="configVO">The <see cref="UnityContentGroupConfigVO"/> to configure construction of this Content Group</param>
        internal UnityContentGroup(AbstractContentManager contentManager, ContentGroupConfigVO configVO) : base(contentManager, configVO)
        {
            InitGameObject();
            OnLoadComplete += Self_OnLoadComplete;
        }

        protected override void DisposeSelf()
        {
            OnLoadComplete -= Self_OnLoadComplete;
            if (ContentGroupRoot != null)
            {
                Object.Destroy(ContentGroupRoot.gameObject);
            }
            base.DisposeSelf();
        }

        private void InitGameObject()
        {
            //TODO: Is there a way to have these nicely strong typed?
            UnityContentGroupConfigVO vo = (UnityContentGroupConfigVO)ConfigVO;
            UnityContentManager contentManger = (UnityContentManager)ContentManager;

            GameObject groupRootGO = new GameObject($"[CL - {vo.ID}]");
            Object.DontDestroyOnLoad(groupRootGO);
            ContentGroupRoot = groupRootGO.transform;
            Transform parent = vo.GameObjectRoot == null
                ? contentManger.ContentRoot
                : vo.GameObjectRoot;
            //TODO: https://github.com/scratch-games/anvil-unity-core/issues/34
            ContentGroupRoot.SetParent(parent, false);
            ContentGroupRoot.localPosition = vo.LocalPosition;
            ContentGroupRoot.localRotation = Quaternion.identity;
            ContentGroupRoot.localScale = Vector3.one;
        }

        private void Self_OnLoadComplete(AbstractContentController contentController)
        {
            AbstractUnityContent content = (AbstractUnityContent)ActiveContentController.Content;
            Transform transform = content.transform;
            //TODO: https://github.com/scratch-games/anvil-unity-core/issues/34
            transform.SetParent(ContentGroupRoot, false);
            transform.localScale = Vector3.one;
        }
    }
}
