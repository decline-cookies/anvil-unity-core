using Anvil.CSharp.DelayedExecution;
using UnityEditor;

namespace Anvil.UnityEditor.DelayedExecution
{
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
