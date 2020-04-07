using Anvil.CSharp.Data;
using UnityEngine;
using UnityEngine.SocialPlatforms;

namespace Anvil.Unity.Content
{
    public class ContentGroupConfigVO : AbstractAnvilVO
    {
        public string ID { get; private set; }
        public Vector3 LocalPosition { get; private set; }

        public Transform AlternativeGameObjectRoot { get; private set; }

        public ContentGroupConfigVO(string id, Vector3 localPosition, Transform alternativeGameObjectRoot = null)
        {
            ID = id;
            LocalPosition = localPosition;
            AlternativeGameObjectRoot = alternativeGameObjectRoot;
        }
    }
}

