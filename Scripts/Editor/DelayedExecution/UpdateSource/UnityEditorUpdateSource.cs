using Anvil.CSharp.DelayedExecution;
using UnityEditor;

namespace Anvil.UnityEditor.DelayedExecution
{
    /// <summary>
    /// An update source that hooks into the <see cref="EditorApplication.update"/> event
    /// and provides it to a <see cref="UpdateHandle"/>
    /// </summary>
    public class UnityEditorUpdateSource : AbstractUpdateSource
    {
        protected override void Init()
        {
            EditorApplication.update += EditorApplication_update;
        }

        protected override void DisposeSelf()
        {
            EditorApplication.update -= EditorApplication_update;

            base.DisposeSelf();
        }

        private void EditorApplication_update()
        {
            DispatchOnUpdateEvent();
        }
    }
}
