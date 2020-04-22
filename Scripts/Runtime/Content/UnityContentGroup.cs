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
            OnLoadComplete += HandleOnLoadComplete;
        }

        protected override void DisposeSelf()
        {
            OnLoadComplete -= HandleOnLoadComplete;
            GameObject.Destroy(ContentGroupRoot.gameObject);
            base.DisposeSelf();
        }

        private void InitGameObject()
        {
            //TODO: Is there a way to have these nicely strong typed?
            UnityContentGroupConfigVO vo = (UnityContentGroupConfigVO)ConfigVO;
            UnityContentManager contentManger = (UnityContentManager)ContentManager;
            
            GameObject groupRootGO = new GameObject($"[CL - {vo.ID}]");
            GameObject.DontDestroyOnLoad(groupRootGO);
            ContentGroupRoot = groupRootGO.transform;
            Transform parent = vo.GameObjectRoot == null
                ? contentManger.ContentRoot
                : vo.GameObjectRoot;
            ContentGroupRoot.SetParent(parent, false);
            ContentGroupRoot.localPosition = vo.LocalPosition;
            ContentGroupRoot.localRotation = Quaternion.identity;
            ContentGroupRoot.localScale = Vector3.one;
        }

        private void HandleOnLoadComplete(AbstractContentController contentController)
        {
            AbstractUnityContent content = (AbstractUnityContent)ActiveContentController.Content;
            Transform transform = content.transform;
            transform.SetParent(ContentGroupRoot, false);
            transform.localScale = Vector3.one;
        }
    }
}

