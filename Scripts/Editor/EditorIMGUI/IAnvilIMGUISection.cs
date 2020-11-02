using System;

namespace Anvil.UnityEditor.IMGUI
{
    public interface IAnvilIMGUISection
    {
        event Action<string, bool> OnFocusControl;
    }
}
