using UnityEngine;

namespace Anvil.Unity.Content
{
    /// <summary>
    /// A <see cref="UnityContentGroupConfigVO"/> for content groups where the loaded content
    /// has a <see cref="Canvas"/> component. Adds a <see cref="SortingOrder"/> that is applied
    /// to <see cref="Canvas.sortingOrder"/> when content loads.
    /// </summary>
    public class UnityCanvasContentGroupConfigVO : UnityContentGroupConfigVO
    {
        /// <summary>
        /// The sorting order applied to the <see cref="Canvas"/> component on loaded content.
        /// Maps directly to <see cref="Canvas.sortingOrder"/>.
        /// Higher values render on top. Defaults to 0.
        /// </summary>
        public readonly int SortingOrder;

        /// <summary>
        /// Constructs a new instance of the <see cref="UnityCanvasContentGroupConfigVO"/>.
        /// </summary>
        /// <param name="id"><inheritdoc cref="Anvil.CSharp.Content.ContentGroupConfigVO.ID"/></param>
        /// <param name="localPosition"><inheritdoc cref="UnityContentGroupConfigVO.LocalPosition"/></param>
        /// <param name="sortingOrder"><inheritdoc cref="SortingOrder"/></param>
        /// <param name="gameObjectRoot"><inheritdoc cref="UnityContentGroupConfigVO.GameObjectRoot"/></param>
        public UnityCanvasContentGroupConfigVO(string id, Vector3 localPosition, int sortingOrder = 0, Transform gameObjectRoot = null)
            : base(id, localPosition, gameObjectRoot)
        {
            SortingOrder = sortingOrder;
        }
    }
}