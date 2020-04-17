using Anvil.CSharp.Content;
using UnityEngine;

namespace Anvil.Unity.Content
{
    public class UnityContentGroup : AbstractContentGroup
    {
        
        public Transform ContentGroupRoot { get; private set; }

        public UnityContentGroup(AbstractContentManager contentManager, AbstractContentGroupConfigVO configVO) : base(contentManager, configVO)
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
            UnityContentGroupConfigVO vo = (UnityContentGroupConfigVO)ConfigVO;
            UnityContentManager contentManger = (UnityContentManager)ContentManager;
            
            GameObject groupRootGO = new GameObject($"[CL - {vo.ID}]");
            GameObject.DontDestroyOnLoad(groupRootGO);
            ContentGroupRoot = groupRootGO.transform;
            Transform parent = vo.GameObjectRoot == null
                ? contentManger.ContentRoot
                : vo.GameObjectRoot;
            ContentGroupRoot.SetParent(parent);
            ContentGroupRoot.localPosition = vo.LocalPosition;
            ContentGroupRoot.localRotation = Quaternion.identity;
            ContentGroupRoot.localScale = Vector3.one;
        }

        private void HandleOnLoadComplete(AbstractContentController contentController)
        {
            AbstractUnityContent content = (AbstractUnityContent)ActiveContentController.Content;
            Transform transform = content.transform;
            transform.SetParent(ContentGroupRoot);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
        }
    }
}

