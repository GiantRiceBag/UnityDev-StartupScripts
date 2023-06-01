using UnityEngine;
using UnityEditor;
using System;

[AttributeUsage(AttributeTargets.Property|AttributeTargets.Class|AttributeTargets.Struct|AttributeTargets.Enum|AttributeTargets.Field)]
public class ConditionalHideAttribute : PropertyAttribute
{
    public string sourceField = string.Empty;
    public bool inverse = false;
    public string enumValue = string.Empty;

    public ConditionalHideAttribute(string sourceField,bool inverse = false)
    {
        this.sourceField = sourceField;
        this.inverse = inverse;
    }
    public ConditionalHideAttribute(string sourceField, string enumValue,bool inverse = false)
    {
        this.sourceField = sourceField;
        this.enumValue = enumValue;
        this.inverse = inverse;
    }
}

[CustomPropertyDrawer(typeof(ConditionalHideAttribute))]
public class ConditionalDrawer : PropertyDrawer
{
    bool GetConditionalHideResult(SerializedProperty property)
    {
        ConditionalHideAttribute attribute = this.attribute as ConditionalHideAttribute;
        var sourceField = property.serializedObject.FindProperty(attribute.sourceField);

        if (sourceField is not null)
        {
            if (sourceField.propertyType == SerializedPropertyType.Boolean)
            {
                return attribute.inverse ? !sourceField.boolValue : sourceField.boolValue;
            }
            else if(sourceField.propertyType == SerializedPropertyType.Enum)
            {
                return attribute.inverse ? !(sourceField.enumNames[sourceField.enumValueIndex] == attribute.enumValue) : (sourceField.enumNames[sourceField.enumValueIndex] == attribute.enumValue);
            }
        }
        return false;
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        return base.GetPropertyHeight(property, label) * (GetConditionalHideResult(property)?1f:0f);
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        ConditionalHideAttribute cdhAttri = this.attribute as ConditionalHideAttribute;
        var sourceField = property.serializedObject.FindProperty(cdhAttri.sourceField);
        
        if (GetConditionalHideResult(property))
        {
            EditorGUI.PropertyField(position, property, label);
        }
        else
            EditorGUI.LabelField(position, new GUIContent($"Unable to find SourceField {cdhAttri.sourceField}"));

    }
}