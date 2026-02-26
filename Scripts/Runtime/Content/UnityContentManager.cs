using System;
using System.Collections.Generic;
using Anvil.CSharp.Content;
using UnityEngine;

namespace Anvil.Unity.Content
{
    /// <summary>
    /// A concrete implementation of a <see cref="AbstractContentManager"/> specific to Unity
    /// </summary>
    public class UnityContentManager : AbstractContentManager
    {
        private static readonly Dictionary<Type, Func<AbstractContentManager, ContentGroupConfigVO, UnityContentGroup>> s_GroupFactoryLookup
            = new Dictionary<Type, Func<AbstractContentManager, ContentGroupConfigVO, UnityContentGroup>>
            {
                { typeof(UnityCanvasContentGroupConfigVO), (manager, configVO) => new UnityCanvasContentGroup(manager, configVO) },
                { typeof(UnityContentGroupConfigVO), (manager, configVO) => new UnityContentGroup(manager, configVO) }
            };

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
                UnityEngine.Object.Destroy(ContentRoot.gameObject);
            }

            base.DisposeSelf();
        }

        protected override AbstractContentGroup CreateGroup(ContentGroupConfigVO configVO)
        {
            Type configVOType = configVO.GetType();
            if (s_GroupFactoryLookup.TryGetValue(configVOType, out Func<AbstractContentManager, ContentGroupConfigVO, UnityContentGroup> factory))
            {
                return factory(this, configVO);
            }

            throw new InvalidOperationException($"No group factory registered for {nameof(ContentGroupConfigVO)} type \"{configVOType.Name}\". Register it in {nameof(s_GroupFactoryLookup)}.");
        }

        private void PreventDestroyOnLoad()
        {
            if (ContentRoot.parent == null)
            {
                UnityEngine.Object.DontDestroyOnLoad(ContentRoot);
            }
            else
            {
                Logger.Warning(
                    $"This {nameof(UnityContentManager)} may be destroyed on a scene change. ContentRoot: {ContentRoot.name}"
                    + $"\nThe {nameof(ContentRoot)} provided is not at the root of the hierarchy.");
            }
        }
    }
}