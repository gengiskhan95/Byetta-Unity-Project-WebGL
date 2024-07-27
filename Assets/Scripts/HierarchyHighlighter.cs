using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

[InitializeOnLoad]
public static class HierarchyHighlighter
{
    private static HashSet<Type> myScriptTypes;

    static HierarchyHighlighter()
    {
        myScriptTypes = new HashSet<Type>();
        FindMyScripts();
        EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyGUI;
    }

    private static void FindMyScripts()
    {
        // Tüm Assembly'leri tarar
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            // Assembly içindeki tüm tipleri alýr
            foreach (var type in assembly.GetTypes())
            {
                // Tip bir MonoBehaviour ise ve script klasörünüzde ise
                if (type.IsSubclassOf(typeof(MonoBehaviour)) && type.Assembly == typeof(HierarchyHighlighter).Assembly)
                {
                    myScriptTypes.Add(type);
                }
            }
        }
    }

    private static void OnHierarchyGUI(int instanceID, Rect selectionRect)
    {
        GameObject obj = EditorUtility.InstanceIDToObject(instanceID) as GameObject;
        if (obj != null)
        {
            MonoBehaviour[] scripts = obj.GetComponents<MonoBehaviour>();
            var scriptNames = scripts.Where(script => script != null && myScriptTypes.Contains(script.GetType()))
                                     .Select(script => script.GetType().Name)
                                     .ToArray();

            if (scriptNames.Length > 0)
            {
                // Obje isminin geniþliðini hesapla
                GUIContent content = new GUIContent(obj.name);
                float nameWidth = EditorStyles.label.CalcSize(content).x;

                // Script isimlerini obje isminin yanýna parantez içinde yazdýr
                float additionalSpace = 25f; // Ekstra boþluk
                Rect labelRect = new Rect(selectionRect.x + nameWidth + additionalSpace, selectionRect.y, selectionRect.width - nameWidth - additionalSpace, selectionRect.height);
                EditorGUI.LabelField(labelRect, "(" + string.Join(", ", scriptNames) + ")", EditorStyles.miniLabel);
            }
        }
    }
}
