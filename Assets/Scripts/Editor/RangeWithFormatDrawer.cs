#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RangeWithFormatAttribute))]
public class RangeWithFormatDrawer : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        RangeWithFormatAttribute range = (RangeWithFormatAttribute)attribute;

        if (property.propertyType == SerializedPropertyType.Float)
        {
            // Get the current value
            float currentValue = property.floatValue;

            Rect labelRect = new Rect(position.x, position.y, EditorGUIUtility.labelWidth, position.height);
            Rect sliderRect = new Rect(position.x + EditorGUIUtility.labelWidth, position.y,
                                       position.width - EditorGUIUtility.labelWidth - 50, position.height);
            Rect valueRect = new Rect(position.x + position.width - 45, position.y, 45, position.height);

            // Draw label
            EditorGUI.LabelField(labelRect, label);

            // Draw slider
            float newValue = GUI.HorizontalSlider(sliderRect, currentValue, range.min, range.max);

            // Draw value field with formatted text (only 1 decimal)
            string formattedValue = newValue.ToString(range.format);
            GUI.Label(valueRect, formattedValue);

            // Apply the value
            if (newValue != currentValue)
            {
                property.floatValue = newValue;
            }
        }
        else
        {
            EditorGUI.PropertyField(position, property, label);
        }
    }
}
#endif
