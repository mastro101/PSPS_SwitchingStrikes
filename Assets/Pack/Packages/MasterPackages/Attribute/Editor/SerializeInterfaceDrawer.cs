using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Drawer for the RequireInterface attribute.
/// </summary>
[CustomPropertyDrawer(typeof(SerializeInterfaceAttribute))]
public class SerializeInterfaceDrawer : PropertyDrawer
{
    /// <summary>
    /// Overrides GUI drawing for the attribute.
    /// </summary>
    /// <param name="position">Position.</param>
    /// <param name="property">Property.</param>
    /// <param name="label">Label.</param>
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        Object oldObject;

        // Check if this is reference type property.
        if (property.propertyType == SerializedPropertyType.ObjectReference)
        {
            // Get attribute parameters.
            var requiredAttribute = this.attribute as SerializeInterfaceAttribute;
            // Begin drawing property field.
            EditorGUI.BeginProperty(position, label, property);
            // Draw property field.
            oldObject = property.objectReferenceValue;
            Object obj = EditorGUI.ObjectField(position, label, property.objectReferenceValue, typeof(Object), true);
            property.objectReferenceValue = obj;
            if (obj is GameObject g)
            {
                var v = g.GetComponent(requiredAttribute.requiredType);
                if (v)
                    property.objectReferenceValue = obj;
                else
                    property.objectReferenceValue = oldObject;
            }
            // Finish drawing property field.
        }
        else
        {
            // If field is not reference, show error message.
            // Save previous color and change GUI to red.
            var previousColor = GUI.color;
            GUI.color = Color.red;
            // Display label with error message.
            EditorGUI.LabelField(position, label, new GUIContent("Property is not a reference type"));
            // Revert color change.
            GUI.color = previousColor;
        }
        EditorGUI.EndProperty();
    }
}