// With thanks to Jason Weimann  -- jason@unity3d.college

using System.Collections;
using System.Collections.Generic;
using CustomAsset;
using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  [CustomEditor(typeof(Clips))]
  public class ClipsEditor : Editor {
    private AudioSource audioSource;

    private void OnEnable() {
      GameObject gameObject = EditorUtility.CreateGameObjectWithHideFlags(
        "Audio Preview", HideFlags.HideAndDontSave, typeof(AudioSource));

      audioSource = gameObject.GetComponent<AudioSource>();
    }

    private void OnDisable() { DestroyImmediate(audioSource.gameObject); }

    public override void OnInspectorGUI() {
      DrawDefaultInspector();
      EditorGUI.BeginDisabledGroup(serializedObject.isEditingMultipleObjects);

      if (GUILayout.Button("Preview")) ((Clips) target).Play(audioSource);

      EditorGUI.EndDisabledGroup();
    }
  }
}