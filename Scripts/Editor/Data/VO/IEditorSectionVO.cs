namespace Anvil.UnityEditor.Data
{
    public interface IEditorSectionVO<T> : IEditorSectionVO
        where T : class, IEditorSectionVO
    {
        void CopyInto(T other);
    }

    public interface IEditorSectionVO
    {
        bool IsBeingEdited { get; set; }
    }
}
