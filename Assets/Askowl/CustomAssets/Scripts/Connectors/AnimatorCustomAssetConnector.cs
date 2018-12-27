// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEditor;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2RhNC0Z">Drive an animation directly from data custom assets</a>
  public class AnimatorCustomAssetConnector : MonoBehaviour {
    /// <a href="http://bit.ly/2RhNC0Z">for <see cref="NamedTriggerDriver"/></a>
    public void SetTrigger(string parameterName) => animator.SetTrigger(parameterName);

    /// <a href="http://bit.ly/2RhNC0Z"><see cref="NamedIntegerDriver"/></a>
    public void SetInteger(string parameterName, int value) => animator.SetInteger(parameterName, value);

    /// <a href="http://bit.ly/2RhNC0Z"><see cref="NamedFloatDriver"/></a>
    public void SetFloat(string parameterName, float value) => animator.SetFloat(parameterName, value);

    /// <a href="http://bit.ly/2RhNC0Z"><see cref="NamedBooleanDriver"/></a>
    public void SetBoolean(string parameterName, bool value) => animator.SetBool(parameterName, value);

    private Animator animator;

    private void Awake() => animator = GetComponent<Animator>();

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/Animator Connector")]
    private static void AddConnector() =>
      Selection.activeTransform.gameObject.AddComponent<AnimatorCustomAssetConnector>();
    #endif
  }
}