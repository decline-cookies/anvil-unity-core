namespace Anvil.Unity.Attribute
{
	/// <summary>
	/// Used by <see cref="MonoBehaviourUtil.EnforceEditorExposedFieldsSet"/> to allow a field to be set to its default value
	/// </summary>
	public sealed class PermitDefaultValue : System.Attribute
	{
		public PermitDefaultValue() { }
	}
}

