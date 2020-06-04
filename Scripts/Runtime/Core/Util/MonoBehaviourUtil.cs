using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace Anvil.Unity.Core
{
    /// <summary>
    /// A collection of Utils to work with <see cref="MonoBehaviour"/>s
    /// </summary>
    public static class MonoBehaviourUtil
    {
        /// <summary>
        /// Logs errors for all editor exposed field reference types that haven't been set.
        /// </summary>
        /// <param name="target">The <see cref="MonoBehaviour"/> to analyse.</param>
        public static void EnforceEditorExposedFieldReferencesSet(MonoBehaviour target)
        {
            Type type = target.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            
            IEnumerable<FieldInfo> unsetSerializedFieldReferences = fields.Where(
                (field) =>
                    IsEditorExposedReferenceField(field)
                    && !field.IsDefined(typeof(PermitUnsetReference))
                    && IsNullOrUnityNull(field.GetValue(target))
                );

            foreach (FieldInfo field in unsetSerializedFieldReferences)
            {
                Debug.LogError($"[{type.Name}] An editor exposed field reference has not been set and is currently null. Either set it or mark the field exempt with [{nameof(PermitUnsetReference)}] attribute. GameObject: {target.gameObject.name} Field: {type.Name}.{field.Name}", target);
            }
        }

        /// <summary>
        /// Returns true if the provided field is an exposed reference in the UnityEditor's default inspector and property drawers
        /// </summary>
        /// <param name="field">The field to evaluate</param>
        /// <returns>true if the provided field is an exposed reference in the UnityEditor's default inspector and property drawers</returns>
        private static bool IsEditorExposedReferenceField(FieldInfo field)
        {
            return (field.IsPublic || field.IsDefined(typeof(SerializeField)))
                   && field.FieldType.IsSubclassOf(typeof(UnityEngine.Object));
        }

        /// <summary>
        /// Returns true if the object provided is null or has been set by Unity to the placeholder null object
        /// </summary>
        /// <remarks>
        /// https://answers.unity.com/questions/939119/using-reflection-sometimes-a-field-is-null-and-som.html
        /// </remarks>

        /// <param name="obj">The object to evaluate</param>
        /// <returns>true if the object provided is null or set to the Unity placeholder null object.</returns>
        private static bool IsNullOrUnityNull(object obj)
        {
            return obj == null || obj.Equals(null);
        }
    }
}
