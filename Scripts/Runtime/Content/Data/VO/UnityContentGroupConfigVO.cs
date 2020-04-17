using Anvil.CSharp.Content;
using UnityEngine;

namespace Anvil.Unity.Content
{
    public class UnityContentGroupConfigVO : AbstractContentGroupConfigVO
    {
        public readonly Vector3 LocalPosition;
        public readonly Transform GameObjectRoot;

        public UnityContentGroupConfigVO(string id, Vector3 localPosition, Transform gameObjectRoot = null)
            : base(id)
        {
            LocalPosition = localPosition;
            GameObjectRoot = gameObjectRoot;
        }
    }
}

