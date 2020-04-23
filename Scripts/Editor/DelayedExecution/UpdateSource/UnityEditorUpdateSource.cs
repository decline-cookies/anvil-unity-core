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
            EditorApplication.update += HandleEditorApplicationUpdate;
        }

        protected override void DisposeSelf()
        {
            EditorApplication.update -= HandleEditorApplicationUpdate;

            base.DisposeSelf();
        }

        private void HandleEditorApplicationUpdate()
        {
            DispatchOnUpdateEvent();
        }
    }
}
