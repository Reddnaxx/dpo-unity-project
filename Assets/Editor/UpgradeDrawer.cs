#if UNITY_EDITOR
using UnityEditor;
using UnityEngine;
using _00_Scripts.Game.Items;

[CustomPropertyDrawer(typeof(Upgrade))]
public class UpgradeDrawer : PropertyDrawer
{
  public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
  {
    // находим поля внутри Upgrade
    var typeProp  = property.FindPropertyRelative("type");
    var valueProp = property.FindPropertyRelative("value");

    // оставшееся пространство делим на два: enum и число
    float fieldWidth = position.width / 2 - 5;
    var typeRect = new Rect(position.x, position.y, fieldWidth, position.height);
    var valueRect = new Rect(typeRect.xMax + 10, position.y, fieldWidth, position.height);

    // рисуем поля inline
    EditorGUI.PropertyField(typeRect, typeProp,  GUIContent.none);
    EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
  }

  public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
  {
    return EditorGUIUtility.singleLineHeight;
  }
}
#endif
