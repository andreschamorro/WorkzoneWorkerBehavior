using System;
using System.Collections;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(FlaggerSystem.FlaggerList))]
public class FlaggerDrawer : PropertyDrawer
{
    private const float LINE_HEIGHT = 18;
    private const float SPACING = 4;

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        var x = position.x;
        var y = position.y;
        var inspectorWidth = position.width;

        var indent = EditorGUI.indentLevel;
        EditorGUI.indentLevel = 0;

        var flaggers = property.FindPropertyRelative("_flaggers");
        var titles = new string[] { "GameObject", "", "", "" };
        var props = new string[] { "transform", "^", "v", "-" };
        var widths = new float[] { .7f, .1f, .1f, .1f };
        var lineHeight = 18.0f;
        var changedLength = false;

        if (flaggers.arraySize > 0)
        {
            for (int i = 0; i < flaggers.arraySize; i++)
            {
                var item = flaggers.GetArrayElementAtIndex(i);

                float rowX = x;
                for (int n = 0; n < props.Length; ++n)
                {
                    float w = widths[n] * inspectorWidth;

                    Rect rect = new Rect(rowX, y, w, lineHeight);
                    rowX += w;

                    if (i == -1)
                    {
                        EditorGUI.LabelField(rect, titles[n]);
                    }
                    else
                    {
                        if (n == 0)
                        {
                            EditorGUI.ObjectField(rect, item.objectReferenceValue, typeof(GameObject), true);
                        }
                        else
                        {
                            if (GUI.Button(rect, props[n]))
                            {
                                switch (props[n])
                                {
                                    case "-":
                                        flaggers.DeleteArrayElementAtIndex(i);
                                        flaggers.DeleteArrayElementAtIndex(i);
                                        changedLength = true;
                                        break;
                                    case "v":
                                        if (i > 0)
                                        {
                                            flaggers.MoveArrayElement(i, i + 1);
                                        }

                                        break;
                                    case "^":
                                        if (i < flaggers.arraySize - 1)
                                        {
                                            flaggers.MoveArrayElement(i, i - 1);
                                        }

                                        break;
                                }
                            }
                        }
                    }
                }

                y += lineHeight + SPACING;
                if (changedLength)
                {
                    break;
                }
            }
        }
        else
        {
            var addButtonRect = new Rect((x + position.width) - widths[widths.Length - 1] * inspectorWidth, y, widths[widths.Length - 1] * inspectorWidth, lineHeight);
            if (GUI.Button(addButtonRect, "+"))
            {
                flaggers.InsertArrayElementAtIndex(flaggers.arraySize);
            }

            y += lineHeight + SPACING;
        }

        var addAllButtonRect = new Rect(x, y, inspectorWidth, lineHeight);
        if (GUI.Button(addAllButtonRect, "Assign using all child objects"))
        {
            var flaggsys = property.FindPropertyRelative("_flagsys").objectReferenceValue as FlaggerSystem;
            var children = new GameObject[flaggsys.transform.childCount];
            int n = 0;
            foreach (Transform child in flaggsys.transform)
            {
                children[n++] = child.gameObject;
            }

            Array.Sort(children, new TransformNameComparer());
            flaggsys._flaggerList._flaggers = new GameObject[children.Length];
            for (n = 0; n < children.Length; ++n)
            {
                flaggsys._flaggerList._flaggers[n] = children[n];
            }
        }

        y += lineHeight + SPACING;

        var renameButtonRect = new Rect(x, y, inspectorWidth, lineHeight);
        if (GUI.Button(renameButtonRect, "Auto Rename numerically from this order"))
        {
            var flaggsys = property.FindPropertyRelative("_flagsys").objectReferenceValue as FlaggerSystem;
            int n = 0;
            foreach (var child in flaggsys._flaggerList._flaggers)
            {
                child.name = "Flagger " + (n++).ToString("000");
            }
        }

        y += lineHeight + SPACING;

        EditorGUI.indentLevel = indent;
        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        var flaggers = property.FindPropertyRelative("_flaggers");
        const float lineAndSpace = LINE_HEIGHT + SPACING;
        return 40 + (flaggers.arraySize * lineAndSpace) + lineAndSpace;
    }

    private class TransformNameComparer : IComparer
    {
        public int Compare(object x, object y)
        {
            return String.Compare(((GameObject)x)?.name, ((GameObject)y)?.name, StringComparison.Ordinal);
        }
    }
}
