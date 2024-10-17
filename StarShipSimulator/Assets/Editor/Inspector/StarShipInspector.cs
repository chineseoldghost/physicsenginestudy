using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(StarShip))] 
public class StarShipInspector : Editor
{
    	private StarShip script { get { return target as StarShip; } }

    float Engine0Radius =  1f;
    int Engine0RotOff = 2;
    float Engine1Radius =  3f;
    int Engine1RotOff = -9;
    float Engine2Radius =  4.0f;
    int Engine2RotOff = 20;
    float EngineYOffset = -0f;

    public override void OnInspectorGUI()
    {
        GUI.changed = false;

        DrawDefaultInspector();

      
        EditorGUILayout.BeginHorizontal();
        Engine0Radius = EditorGUILayout.FloatField("Engine0Radius:", Engine0Radius);
        Engine0RotOff = EditorGUILayout.IntField("Engine0RotOff:", Engine0RotOff);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        Engine1Radius = EditorGUILayout.FloatField("Engine1Radius:", Engine1Radius);
        Engine1RotOff = EditorGUILayout.IntField("Engine1RotOff:", Engine1RotOff);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        Engine2Radius = EditorGUILayout.FloatField("Engine2Radius:", Engine2Radius);
        Engine2RotOff = EditorGUILayout.IntField("Engine2RotOff:", Engine2RotOff);
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        EngineYOffset = EditorGUILayout.FloatField("EngineYOffset:", EngineYOffset); 
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("ResetEnginesPos",  GUILayout.Width(220)))
            ResetEnginesPos(); 
        EditorGUILayout.EndHorizontal();
 
         

        if (GUI.changed)
            EditorUtility.SetDirty(script);
    }

	void ResetEnginesPos()
    { 

        for(int i=0;i< 3;i++)
        {
            //script.mEngine_0[i] = GameObject.Find("Raptor 2 Engine.0_" + i);
            script.mEngine_0[i].transform.position = new Vector3(Mathf.Sin((Mathf.PI/1.5f) * i + Mathf.PI*Engine0RotOff / 180.0f) * Engine0Radius, -EngineYOffset, Mathf.Cos((Mathf.PI / 1.5f) * i + Mathf.PI * Engine0RotOff / 180.0f) * Engine0Radius);
        }

        for (int i = 0; i < 10; i++)
        {
            //script.mEngine_1[i] = GameObject.Find("Raptor 2 Engine.1_" + i);
            script.mEngine_1[i].transform.position = new Vector3(Mathf.Sin((Mathf.PI/5.0f)*i + Mathf.PI * Engine1RotOff / 180.0f) * Engine1Radius, -EngineYOffset, Mathf.Cos((Mathf.PI/5.0f)* i + Mathf.PI * Engine1RotOff / 180.0f) * Engine1Radius);
        }

        for (int i = 0; i < 20; i++)
        {
            //script.mEngine_2[i] = GameObject.Find("Raptor 2 Engine.2_" + i);
            script.mEngine_2[i].transform.position = new Vector3(Mathf.Sin((Mathf.PI / 10.0f) * i + Mathf.PI * Engine2RotOff / 180.0f) * Engine2Radius, -EngineYOffset, Mathf.Cos((Mathf.PI / 10.0f) * i + Mathf.PI * Engine2RotOff / 180.0f) * Engine2Radius);
        }
    }
}

 