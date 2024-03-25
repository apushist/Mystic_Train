using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static UnityEngine.Rendering.DebugUI;


[CustomEditor(typeof(InteractiveZone))]
public class MyScriptEditor : Editor
{
    override public void OnInspectorGUI()
    {
        var myScript = target as InteractiveZone;

        myScript._destroyScriptAfterUse = EditorGUILayout.Toggle("destroyScriptAfterUse", myScript._destroyScriptAfterUse);
        myScript._destroyObjectAfterUse = EditorGUILayout.Toggle("destroyObjectAfterUse", myScript._destroyObjectAfterUse);
        myScript._attachedObjectToDestroy = (GameObject)EditorGUILayout.ObjectField("attachedObjectToDestroy", myScript._attachedObjectToDestroy, typeof(GameObject), true);

        myScript._currentInterType = (InteractionType)EditorGUILayout.EnumFlagsField(myScript._currentInterType);

        if (myScript._currentInterType == InteractionType.lockedDoor)
        {
            EditorGUI.indentLevel++;
            myScript._neededItem = (InventoryItem)EditorGUILayout.ObjectField("neededItem", myScript._neededItem, typeof(InventoryItem), true);
            myScript._attachedDoor = (Door)EditorGUILayout.ObjectField("attachedDoor", myScript._attachedDoor, typeof(Door), true);
            EditorGUI.indentLevel--;
        }
        if (myScript._currentInterType == InteractionType.puzzle)
        {
            EditorGUI.indentLevel++;
            myScript._puzzle = (PuzzleBase)EditorGUILayout.ObjectField("puzzle", myScript._puzzle, typeof(PuzzleBase), true);
            myScript._winItem = (InventoryItem)EditorGUILayout.ObjectField("winItem", myScript._winItem, typeof(InventoryItem), true);
            EditorGUI.indentLevel--;
        }
        if (myScript._currentInterType == InteractionType.lock3Item)
        {
            EditorGUI.indentLevel++;
            myScript._puzzle = (PuzzleBase)EditorGUILayout.ObjectField("puzzle", myScript._puzzle, typeof(PuzzleBase), true);
            myScript._winItem = (InventoryItem)EditorGUILayout.ObjectField("winItem", myScript._winItem, typeof(InventoryItem), true);

            SerializedProperty gameObjectsArray = serializedObject.FindProperty("_neededItem3");
            serializedObject.Update();
            EditorGUILayout.PropertyField(gameObjectsArray, new GUIContent("neededItems"));
            serializedObject.ApplyModifiedProperties();

            EditorGUI.indentLevel--;
        }

    }
}
