// Copyright 2019 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using CustomAsset;
using UnityEditor;
using UnityEngine;

namespace Askowl {
  public interface ICreate {
    void Create();
  }
  public abstract class AssetWizard : Manager, ICreate {
    protected static string       destination, type;
    private          List<string> sources = new List<string>();
    protected        string       Label   = "BuildCustomAsset";

    [SerializeField, Tooltip("Optional: Overrides default project directory")]
    protected Jit<string> selectedPathInProjectView = Jit<string>.Instance(getProjectFolder);
    private static readonly Func<bool, string> getProjectFolder = _ => AssetDb.ProjectFolder();

    protected void CreateAssets(string assetName, string assetType, string basePath) {
      name = assetName;
      type = assetType;
      SetDestination(basePath);
      sources.Clear();
      ProcessAllFiles(@"cs|txt");
      AssetDatabase.ImportAsset(
        Path.GetDirectoryName(destination)
      , ImportAssetOptions.ForceUpdate | ImportAssetOptions.ForceSynchronousImport |
        ImportAssetOptions.ImportRecursive);

      if (sources.Count > 0) {
        foreach (string source in sources) {
          var asset = AssetDb.Load<MonoScript>(source);
          AssetDb.Labels(asset, Label);
        }
        Debug.Log($"      Please wait while compiling scripts in `{destination}`");
      }
    }

    protected virtual void ProcessAllFiles(string textAssetTypes) {
      string[] guids                                  = AssetDatabase.FindAssets("", new[] {TemplatePath()});
      for (int i = 0; i < guids.Length; i++) guids[i] = AssetDatabase.GUIDToAssetPath(guids[i]);
      ProcessFiles(textAssetTypes, guids);
    }

    protected void ProcessFiles(string textAssetTypes, params string[] files) {
      Regex textAssetTypesRegex = new Regex($@"\.({textAssetTypes})$");
      using (var template = Template.Instance) {
        for (int i = 0; i < files.Length; i++) {
          if (File.Exists(files[i])) {
            var fileName = Path.GetFileName(files[i]) ?? "";
            var filePath = fileName.StartsWith(name) ? $"{destination}/{fileName}" : $"{destination}/{name}{fileName}";
            if (textAssetTypesRegex.IsMatch(files[i])) {
              if (File.Exists(filePath) && (filePath != files[i]))
                Fatal($"{filePath} already exists. Select a different name or directory");
              var text = FillTemplate(template, File.ReadAllText(files[i]));
              if (files[i].EndsWith(".cs") && text.Contains("  [CreateAssetMenu(")) sources.Add(filePath);
              File.WriteAllText(filePath, text);
            } else {
              if (File.Exists(filePath)) Fatal($"{filePath} already exists. Select a different name or directory");
              File.Copy(files[i], filePath);
            }
          }
        }
      }
    }

    protected virtual string FillTemplate(Template template, string text) => template.From(text).Result();

    protected virtual string TemplatePath() => throw new NotImplementedException();

    protected string AskForDestinationPath(string basePath) {
      destination = EditorUtility.SaveFilePanel($"Location for your new asset", basePath, "", "");
      name        = Path.GetFileNameWithoutExtension(destination) ?? "UNKNOWN";
      return destination;
    }

    private string SetDestination(string path) {
      if (!path.EndsWith(name)) path            = $"{path}/{name}";
      if (path.Contains("Assets/Askowl/")) path = path.Substring(path.IndexOf("Assets/", StringComparison.Ordinal));
      AssetDb.Instance.CreateFolders(path);
      return destination = path;
    }

    protected void Fatal(string msg) => throw new Exception(msg);

    protected string[] ToDefinitions(string text) => csRegex.Split(text);

    protected string ToTuple(string fields) {
      var pairs = ToDefinitions(fields);
      if (pairs.Length == 0) return null;
      if (pairs.Length == 1) return $"{pairs[0]} ";

      if ((pairs.Length < 4) || ((pairs.Length % 2) != 0))
        throw new Exception($"'{fields}' must be single type or pairs of (type, name)");

      builder.Clear().Append("(");
      builder.Append($"{pairs[0]} {pairs[1]}");
      for (int i = 2; i < pairs.Length; i += 2) builder.Append($",{pairs[i]} {pairs[i + 1]}");
      return builder.Append(") ").ToString();
    }
    private readonly        StringBuilder builder = new StringBuilder();
    private static readonly Regex         csRegex = new Regex(@"\s*;\s*|\s*,\s*|\s+", RegexOptions.Singleline);

    [CustomEditor(typeof(AssetWizard), true)] public class AssetWizardEditor : Editor {
      public override void OnInspectorGUI() {
        GUI.SetNextControlName("FirstWizardField");
        if (GUILayout.Button("Clear")) {
          ((AssetWizard) target).Clear();
          GUI.FocusControl(null);
        }
        serializedObject.Update();
        DrawDefaultInspector();
        if (GUILayout.Button("Create")) ((AssetWizard) target).Create();
        serializedObject.ApplyModifiedProperties();
      }
    }
    public virtual void Create() => throw new NotImplementedException();
    public virtual void Clear()  => throw new NotImplementedException();
  }
}