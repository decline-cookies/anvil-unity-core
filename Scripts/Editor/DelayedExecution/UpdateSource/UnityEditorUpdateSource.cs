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
        public UnityEditorUpdateSource()
        {
            EditorApplication.update += EditorApplication_Update;
        }

        protected override void DisposeSelf()
        {
            EditorApplication.update -= EditorApplication_Update;

            base.DisposeSelf();
        }

        private void EditorApplication_Update()
        {
            DispatchOnUpdateEvent();
        }
    }
}
