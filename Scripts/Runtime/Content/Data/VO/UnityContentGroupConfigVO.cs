using Anvil.CSharp.Content;
using UnityEngine;

namespace Anvil.Unity.Content
{
    /// <summary>
    /// A <see cref="ContentGroupConfigVO"/> specific to Unity Content Groups
    /// </summary>
    public class UnityContentGroupConfigVO : ContentGroupConfigVO
    {
        /// <summary>
        /// A local position to place the group <see cref="Transform"/> relative to its parent.
        /// </summary>
        public readonly Vector3 LocalPosition;

        /// <summary>
        /// A custom user supplied <see cref="GameObject"/> <see cref="Transform"/> to parent the
        /// <see cref="UnityContentGroup"/> to. If left null (the default), the <see cref="UnityContentGroup"/> will be parented
        /// to the <see cref="UnityContentManager"/>'s ContentRoot.
        /// </summary>
        public readonly Transform GameObjectRoot;

        /// <summary>
        /// Constructs a new instance of the <see cref="UnityContentGroupConfigVO"/> to use in construction of a <see cref="UnityContentGroup"/>
        /// </summary>
        /// <param name="id"><inheritdoc cref="ContentGroupConfigVO.ID"/></param>
        /// <param name="localPosition"><inheritdoc cref="LocalPosition"/></param>
        /// <param name="gameObjectRoot"><inheritdoc cref="GameObjectRoot"/></param>
        public UnityContentGroupConfigVO(string id, Vector3 localPosition, Transform gameObjectRoot = null)
            : base(id)
        {
            LocalPosition = localPosition;
            GameObjectRoot = gameObjectRoot;
        }
    }
}
