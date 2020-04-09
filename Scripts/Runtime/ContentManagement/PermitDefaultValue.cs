using System;

namespace Anvil.Unity.Content
{
	/// <summary>
	/// Used by <see cref="AbstractContent"/> to allow a field to be set to its default value
	/// </summary>
	public sealed class PermitDefaultValue : Attribute
	{
		public PermitDefaultValue() { }
	}
}

