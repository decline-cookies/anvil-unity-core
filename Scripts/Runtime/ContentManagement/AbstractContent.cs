using Anvil.Unity.Core;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Linq;
using UnityEngine;

namespace Anvil.Unity.Content
{
    public abstract class AbstractContent : AbstractAnvilMonoBehaviour
    {
        internal AbstractContentController ContentController { private get; set; }
        
        internal bool IsContentDisposing { get; private set; }

        protected virtual void Awake()
        {
#if DEBUG
            EnforceEditorExposedFieldsSet();
#endif
        }

        protected override void DisposeSelf()
        {
            if (IsContentDisposing)
            {
                return;
            }
            IsContentDisposing = true;

            if (ContentController != null && !ContentController.IsContentControllerDisposing)
            {
                ContentController.Dispose();
                ContentController = null;
            }
            
            base.DisposeSelf();
        }

        public T GetContentController<T>() where T : AbstractContentController
        {
            return (T)ContentController;
        }

        protected void EnforceEditorExposedFieldsSet()
        {
            Type type = this.GetType();
            FieldInfo[] fields = type.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            IEnumerable<FieldInfo> unsetSerializedFields = fields.Where(
                (field) =>
                    IsEditorExposed(field)
                    && !field.IsDefined(typeof(PermitDefaultValue))
                    && field.GetValue(this) == field.FieldType.CreateDefaultValue()
                );

            foreach (FieldInfo field in unsetSerializedFields)
            {
                Debug.LogError($"[{typeof(AbstractContent).Name}] An editor exposed field is set to its default value. Either set a value or mark the field exempt with [PermitDefault] attribute. Field: {type.Name}.{field.Name}, Value: {field.GetValue(this)}", this);
            }
        }

        private bool IsEditorExposed(FieldInfo field)
        {
            return field.IsPublic 
                || field.IsDefined(typeof(SerializeField));
        }
    }
}

