// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href=""></a> //#TBD#//
  public class AnimatorCustomAssetConnector : MonoBehaviour {
    /// <a href=""></a> //#TBD#//
    public void SetTrigger(string parameterName) => animator.SetTrigger(parameterName);

    /// <a href=""></a> //#TBD#//
    public void SetInteger(string parameterName, int value) => animator.SetInteger(parameterName, value);

    /// <a href=""></a> //#TBD#//
    public void SetFloat(string parameterName, float value) => animator.SetFloat(parameterName, value);

    /// <a href=""></a> //#TBD#//
    public void SetBoolean(string parameterName, bool value) => animator.SetBool(parameterName, value);

    private Animator animator;

    private void Awake() {
      animator = GetComponent<Animator>();
    }
  }
}