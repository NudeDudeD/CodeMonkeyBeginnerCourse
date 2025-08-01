using UnityEditor;
using UnityEngine.UIElements;
using UnityEditor.UIElements;

[CustomPropertyDrawer(typeof(ReactiveVariable<>))]
public class ReactableVariableDrawer : PropertyDrawer
{
    public override VisualElement CreatePropertyGUI(SerializedProperty property)
    {
        VisualElement container = new();
        PropertyField amountField = new(property.FindPropertyRelative("_value"), property.displayName);
        container.Add(amountField);

        return container;
    }
}