namespace Anvil.Unity.Core
{
    /// <summary>
    /// Used by <see cref="MonoBehaviourUtil.EnforceEditorExposedFieldReferencesSet"/> to allow a field reference type to remain unset.
    /// </summary>
    public sealed class PermitUnsetReference : System.Attribute
    {
        public PermitUnsetReference() { }
    }
}
