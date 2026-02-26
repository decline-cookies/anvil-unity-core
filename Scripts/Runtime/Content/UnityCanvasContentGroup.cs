using System;
using Anvil.CSharp.Content;
using UnityEngine;

namespace Anvil.Unity.Content
{
    /// <summary>
    /// A <see cref="UnityContentGroup"/> for content that uses a <see cref="Canvas"/>.
    /// Applies <see cref="UnityCanvasContentGroupConfigVO.SortingOrder"/> to the loaded content's
    /// <see cref="Canvas.sortingOrder"/> when content loads.
    /// </summary>
    public class UnityCanvasContentGroup : UnityContentGroup
    {
        /// <summary>
        /// Creates a new <see cref="UnityCanvasContentGroup"/> to be used with a <see cref="UnityContentManager"/>.
        /// </summary>
        /// <param name="contentManager">The <see cref="AbstractContentManager"/> that controls this Content Group.</param>
        /// <param name="configVO">The <see cref="UnityCanvasContentGroupConfigVO"/> to configure construction of this Content Group.</param>
        internal UnityCanvasContentGroup(AbstractContentManager contentManager, ContentGroupConfigVO configVO) : base(contentManager, configVO)
        {
        }

        protected override void Self_OnLoadComplete(AbstractContentController contentController)
        {
            base.Self_OnLoadComplete(contentController);

            AbstractUnityContent content = (AbstractUnityContent)ActiveContentController.Content;
            Canvas canvas = content.GetComponent<Canvas>();
            if (canvas == null)
            {
                throw new InvalidOperationException($"Content \"{content.name}\" loaded into a {nameof(UnityCanvasContentGroup)} but has no {nameof(Canvas)} component. Add a {nameof(Canvas)} to the content prefab or use {nameof(UnityContentGroup)} instead.");
            }

            UnityCanvasContentGroupConfigVO canvasConfigVO = (UnityCanvasContentGroupConfigVO)ConfigVO;
            canvas.sortingOrder = canvasConfigVO.SortingOrder;
        }
    }
}