using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;

[CustomEditor(typeof(NavMeshManager))]

public class NavMeshUtility : Editor {

    GUIStyle style = new GUIStyle();
    GUIStyle centered = new GUIStyle();

    bool stepOne;
    bool stepTwo;
    bool stepThree;
    bool allDone;

    [MenuItem("NavMesh Manager/Create New NavMesh Manager &n")]
    private static void CreateNavMeshManager()
    {
        GameObject navMeshManager = new GameObject("NavMesh Manager");

        navMeshManager.AddComponent<NavMeshManager>();
    }


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        style.richText = true;
        style.wordWrap = true;
        centered.richText = true;
        centered.alignment = TextAnchor.UpperCenter;

        NavMeshManager navMeshManager = (NavMeshManager) target;

        #region Step Display
        GUILayout.Label("<size=20>How To Use:</size>", style);
        GUILayout.Label("");
        GUILayout.Label("<size=17.5><color=black>Step 1: </color></size>", style);
        GUILayout.Label("");
        GUILayout.Label("<size=15>-Create two new layers named 'Walkable' and 'Non-Walkable'.</size>", style);
        GUILayout.Label("<size=15>(Edit>Project Settings>Tags and Layers)</size>", style);

        stepOne = EditorGUILayout.Toggle("Finished Step 1", stepOne);


        if (stepOne)
        {
            GUILayout.Label("<size=17.5><color=black>Step 2: </color></size>", style);
            GUILayout.Label("");
            GUILayout.Label("<size=15>-Add every GameObject that will be walked on to the 'Walkable' layer.</size>", style);
            GUILayout.Label("");
            GUILayout.Label("<size=15>-Add every GameObject that will be an obsticle or wall to the 'Non-Walkable' layer.</size>", style);

            stepTwo = EditorGUILayout.Toggle("Finished Step 2", stepTwo);

            if (stepTwo)
            {
                GUILayout.Label("<size=17.5><color=black>Step 3: </color></size>", style);
                GUILayout.Label("");
                GUILayout.Label("<size=15>-Press the button below to add every GameObject from Step 2 to Navigation.</size>", style);
                GUILayout.BeginHorizontal();

                if (GUILayout.Button("Sort Enviroment For NavMesh"))
                {
                    navMeshManager.Start();
                }

                GUILayout.EndHorizontal();

                stepThree = EditorGUILayout.Toggle("Finished Step 3", stepThree);

                if (stepThree)
                {
                    GUILayout.Label("<size=17.5><color=black>Step 4: </color></size>", style);
                    GUILayout.Label("");
                    GUILayout.Label("<size=15>-Open the Navigation window.</size>", style);
                    GUILayout.Label("<size=15>(Window>AI>Navigation.</size>", style);
                    GUILayout.Label("");
                    GUILayout.Label("<size=15>-Open the Bake tab, and press the Bake Button.</size>", style);

                    allDone = EditorGUILayout.Toggle("Finished Step 3", allDone);

                    if (allDone)
                    {
                        GUILayout.Label("");
                        GUILayout.Label("<size=17.5><color=black>Thank you for using the NavMesh Manager Tool!</color></size>", centered);
                        GUILayout.Label("<size=17.5><color=black>Created by Christopher Cornford</color></size>", centered);
                        GUILayout.Label("");
                    }
                }
            }
        }
        #endregion

    }
}

public class NavMeshManager :  MonoBehaviour
{
    public void Start()
    {
        GameObject[] tempArray = FindObjectsOfType(typeof(GameObject)) as GameObject[];

        SortEnviroment(tempArray);
    }

    public void SortEnviroment(GameObject[] tempArray)
    {
        foreach (GameObject navMeshObject in tempArray)
        {
            if (navMeshObject.layer == LayerMask.NameToLayer("Walkable"))
            {
                GameObjectUtility.SetStaticEditorFlags(navMeshObject, StaticEditorFlags.NavigationStatic);
                GameObjectUtility.SetNavMeshArea(navMeshObject, 0);
            }
            else if (navMeshObject.layer == LayerMask.NameToLayer("Non-Walkable"))
            {
                GameObjectUtility.SetStaticEditorFlags(navMeshObject, StaticEditorFlags.NavigationStatic);
                GameObjectUtility.SetNavMeshArea(navMeshObject, 1);
            }
        }
    }
}
