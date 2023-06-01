using UnityEditor;
using UnityEngine;
using System;

public static class SerializedPropertyExtension
{
    public static object GetValue(this SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Generic:
                return property.objectReferenceValue;
            case SerializedPropertyType.Integer:
                return property.intValue;
            case SerializedPropertyType.Boolean:
                return property.boolValue;
            case SerializedPropertyType.Float:
                return property.floatValue;
            case SerializedPropertyType.String:
                return property.stringValue;
            case SerializedPropertyType.Color:
                return property.colorValue;
            case SerializedPropertyType.ObjectReference:
                return property.objectReferenceValue;
            case SerializedPropertyType.LayerMask:
                return property.intValue;
            case SerializedPropertyType.Enum:
                return property.enumNames[property.enumValueIndex];
            case SerializedPropertyType.Vector2:
                return property.vector2Value;
            case SerializedPropertyType.Vector3:
                return property.vector3Value;
            case SerializedPropertyType.Vector4:
                return property.vector4Value;
            case SerializedPropertyType.Rect:
                return property.rectValue;
            case SerializedPropertyType.ArraySize:
                return property.arraySize;
            case SerializedPropertyType.Character:
                return property.stringValue;
            case SerializedPropertyType.AnimationCurve:
                return property.animationCurveValue;
            case SerializedPropertyType.Bounds:
                return property.boundsValue;
            case SerializedPropertyType.Gradient:
                return property.floatValue;
            case SerializedPropertyType.Quaternion:
                return property.quaternionValue;
            case SerializedPropertyType.ExposedReference:
                return property.exposedReferenceValue;
            case SerializedPropertyType.FixedBufferSize:
                return property.fixedBufferSize;
            case SerializedPropertyType.Vector2Int:
                return property.vector2IntValue;
            case SerializedPropertyType.Vector3Int:
                return property.vector3IntValue;
            case SerializedPropertyType.RectInt:
                return property.rectIntValue; ;
            case SerializedPropertyType.BoundsInt:
                return property.boundsIntValue; ;
            case SerializedPropertyType.ManagedReference:
                return property.managedReferenceValue; ;
            case SerializedPropertyType.Hash128:
                return property.hash128Value; ;
            default:
                return null;
        }
    }

    public static Type GetType(this SerializedProperty property)
    {
        switch (property.propertyType)
        {
            case SerializedPropertyType.Generic:
                return typeof(object);
            case SerializedPropertyType.Integer:
                return property.type == "long" ? typeof(int) : typeof(long);
            case SerializedPropertyType.Boolean:
                return typeof(bool);
            case SerializedPropertyType.Float:
                return property.type == "double" ? typeof(double) : typeof(float);
            case SerializedPropertyType.String:
                return typeof(string);
            case SerializedPropertyType.Color:
                return typeof(Color);
            case SerializedPropertyType.ObjectReference:
                return typeof(UnityEngine.Object);
            case SerializedPropertyType.LayerMask:
                return typeof(LayerMask);
            case SerializedPropertyType.Enum:
                return typeof(Enum);
            case SerializedPropertyType.Vector2:
                return typeof(Vector2);
            case SerializedPropertyType.Vector3:
                return typeof(Vector3);
            case SerializedPropertyType.Vector4:
                return typeof(Vector4);
            case SerializedPropertyType.Rect:
                return typeof(Rect);
            case SerializedPropertyType.ArraySize:
                return typeof(int);
            case SerializedPropertyType.Character:
                return typeof(char);
            case SerializedPropertyType.AnimationCurve:
                return typeof(AnimationCurve);
            case SerializedPropertyType.Bounds:
                return typeof(Bounds);
            case SerializedPropertyType.Gradient:
                return typeof(Gradient);
            case SerializedPropertyType.Quaternion:
                return typeof(Quaternion);
            case SerializedPropertyType.ExposedReference:
                return typeof(UnityEngine.Object);
            case SerializedPropertyType.FixedBufferSize:
                return typeof(int);
            case SerializedPropertyType.Vector2Int:
                return typeof(Vector2Int);
            case SerializedPropertyType.Vector3Int:
                return typeof(Vector3Int);
            case SerializedPropertyType.RectInt:
                return typeof(RectInt);
            case SerializedPropertyType.BoundsInt:
                return typeof(BoundsInt);
            default:
                return typeof(object);
        }
    }
}