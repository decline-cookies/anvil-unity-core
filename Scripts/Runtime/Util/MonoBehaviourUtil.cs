using UnityEngine;
using System;
using System.Reflection;
using System.Collections.Generic;
using System.Linq;
using Anvil.Unity.Attribute;

namespace Anvil.Unity.Util
{
    /// <summary>
    /// A collection of Utils to work with <see cref="MonoBehaviour"/>s
    /// </summary>
    public static class MonoBehaviourUtil
    {
        /// <summary>
        /// Logs errors for all editor exposed fields that are currently set to the type's default value
        /// </summary>
        /// <param name="target">The <see cref="MonoBehaviour"/> to analyse.</param>
        public static void EnforceEditorExposedFieldsSet(MonoBehaviour target)
        {
            Type type = target.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            IEnumerable<FieldInfo> unsetSerializedFields = fields.Where(
                (field) =>
                    IsEditorExposedField(field)
                    && !field.IsDefined(typeof(PermitDefaultValue))
                    && field.GetValue(target) == field.FieldType.CreateDefaultValue()
                );

            foreach (FieldInfo field in unsetSerializedFields)
            {
                Debug.LogError($"[{type.Name}] An editor exposed field is set to its default value. Either set a value or mark the field exempt with [PermitDefault] attribute. Field: {type.Name}.{field.Name}, Value: {field.GetValue(target)}", target);
            }
        }

        /// <summary>
        /// Returns true if a field is exposed by the UnityEditor's default inspector and property drawers
        /// </summary>
        /// <param name="field">The field to evaluate</param>
        /// <returns>true if field is exposed by the UnityEditor's default inspector and property drawers</returns>
        public static bool IsEditorExposedField(FieldInfo field)
        {
            return field.IsPublic
                || field.IsDefined(typeof(SerializeField));
        }
    }
}

