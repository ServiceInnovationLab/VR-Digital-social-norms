﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;

[ExecuteInEditMode]
public class SocialMediaScenarioPicker : Singleton<SocialMediaScenarioPicker>
{
    [SerializeField] SocialMediaScenario fallbackScenario;
    [SerializeField] bool canChangeScenario;

    [SerializeField] SocialMediaScenario[] scenariosInProject;

    public SocialMediaScenario CurrentScenario { get; set; }

    public SocialMediaScenario[] ScenariosInProject
    {
        get { return scenariosInProject; }
    }

    public List<string> CustomScenarios { get; private set; } = new List<string>();

    public static string CustomScenarioPath
    {
        get { return Path.Combine(Application.streamingAssetsPath, "Scenarios"); }
    }

    public bool CanChange
    {
        get { return canChangeScenario; }
    }

    private void Awake()
    {
#if UNITY_EDITOR
        if (!Application.isPlaying)
        {
          /*  var serializedObject = new UnityEditor.SerializedObject(this);

            var instances = EditorHelper.GetAllInstances<SocialMediaScenario>();
            var property = serializedObject.FindProperty(nameof(scenariosInProject));

            // Initial only update if needed. TODO: Need check contents too
         //   if (instances.Length == scenariosInProject.Length)
          //      return;

            serializedObject.Update();

            property.ClearArray();
            for (int i = 0; i < instances.Length; i++)
            {
                property.InsertArrayElementAtIndex(i);
                property.GetArrayElementAtIndex(i).objectReferenceValue = instances[i];
            }

            serializedObject.ApplyModifiedProperties();*/

            return;
        }
#endif

        if (Instance != this)
        {
            Instance.canChangeScenario = canChangeScenario;

            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        foreach (var file in Directory.EnumerateFiles(CustomScenarioPath, "*.xlsx", SearchOption.TopDirectoryOnly))
        {
            CustomScenarios.Add(file);
        }

        CurrentScenario = fallbackScenario;
    }

    public void LoadScenario(string path)
    {
        CurrentScenario = ScenarioSpreadsheetLoader.LoadSocialMediaScenario(path);
    }
}
