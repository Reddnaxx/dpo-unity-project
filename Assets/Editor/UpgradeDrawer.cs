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
    var categoryProp = property.FindPropertyRelative("category");

    var statTypeProp = property.FindPropertyRelative("statType");
    var weaponTypeProp = property.FindPropertyRelative("weaponType");

    var valueProp = property.FindPropertyRelative("value");


    // оставшееся пространство делим на два: enum и число
    float fieldWidth = position.width / 3 - 5;
    var categoryRect = new Rect(position.x, position.y, fieldWidth, position.height);
    var weaponTypeRect = new Rect(categoryRect.xMax + 5, position.y, position.width / (3f / 2f) - 5, position.height);

    var typeRect = new Rect(categoryRect.xMax + 5, position.y, fieldWidth, position.height);
    var valueRect = new Rect(typeRect.xMax + 5, position.y, fieldWidth, position.height);

    // рисуем поля inline
    EditorGUI.PropertyField(categoryRect, categoryProp, GUIContent.none);

    if (categoryProp.enumValueIndex == (int)UpgradeCategory.Stat)
    {
      EditorGUI.PropertyField(typeRect, statTypeProp, GUIContent.none);
      EditorGUI.PropertyField(valueRect, valueProp, GUIContent.none);
    }
    else
    {
      EditorGUI.PropertyField(weaponTypeRect, weaponTypeProp, GUIContent.none);
    }
  }

  public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
  {
    return EditorGUIUtility.singleLineHeight;
  }
}
#endif
