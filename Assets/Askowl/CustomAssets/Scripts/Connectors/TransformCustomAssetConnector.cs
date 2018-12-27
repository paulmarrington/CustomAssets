// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEditor;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QTHkWd">Adjust GameObject Transform properties directly from data custom assets</a>
  public class TransformCustomAssetConnector : MonoBehaviour {
    /// <a href="http://bit.ly/2QTHkWd">Adjust GameObject Transform properties directly from data custom assets</a>
    public void ScaleX(float value) {
      Vector3 localScale = transform.localScale;
      // ReSharper disable once Unity.InefficientPropertyAccess
      transform.localScale = new Vector3(value, localScale.y, localScale.z);
    }

    /// <a href="http://bit.ly/2QTHkWd">Adjust GameObject Transform properties directly from data custom assets</a>
    public void ScaleY(float value) {
      Vector3 localScale = transform.localScale;
      // ReSharper disable once Unity.InefficientPropertyAccess
      transform.localScale = new Vector3(localScale.x, value, localScale.z);
    }

    /// <a href="http://bit.ly/2QTHkWd">Adjust GameObject Transform properties directly from data custom assets</a>
    public void ScaleZ(float value) {
      Vector3 localScale = transform.localScale;
      // ReSharper disable once Unity.InefficientPropertyAccess
      transform.localScale = new Vector3(localScale.x, localScale.y, value);
    }

    /// <a href="http://bit.ly/2QTHkWd">Adjust GameObject Transform properties directly from data custom assets</a>
    public void RotationX(float value) {
      Vector3 localEulerAngles = transform.localEulerAngles;
      // ReSharper disable once Unity.InefficientPropertyAccess
      transform.localEulerAngles = new Vector3(value * 360, localEulerAngles.y, localEulerAngles.z);
    }

    /// <a href="http://bit.ly/2QTHkWd">Adjust GameObject Transform properties directly from data custom assets</a>
    public void RotationY(float value) {
      Vector3 localEulerAngles = transform.localEulerAngles;
      // ReSharper disable once Unity.InefficientPropertyAccess
      transform.localEulerAngles = new Vector3(localEulerAngles.x, value * 360, localEulerAngles.z);
    }

    /// <a href="http://bit.ly/2QTHkWd">Adjust GameObject Transform properties directly from data custom assets</a>
    public void RotationZ(float value) {
      Vector3 localEulerAngles = transform.localEulerAngles;
      // ReSharper disable once Unity.InefficientPropertyAccess
      transform.localEulerAngles = new Vector3(localEulerAngles.x, localEulerAngles.y, value * 360);
    }

    /// <a href="http://bit.ly/2QTHkWd">Adjust GameObject Transform properties directly from data custom assets</a>
    public void PositionX(float value) {
      Vector3 localPosition = transform.localPosition;
      // ReSharper disable once Unity.InefficientPropertyAccess
      transform.localPosition = new Vector3(value, localPosition.y, localPosition.z);
    }

    /// <a href="http://bit.ly/2QTHkWd">Adjust GameObject Transform properties directly from data custom assets</a>
    public void PositionY(float value) {
      Vector3 localPosition = transform.localPosition;
      // ReSharper disable once Unity.InefficientPropertyAccess
      transform.localPosition = new Vector3(localPosition.x, value, localPosition.z);
    }

    /// <a href="http://bit.ly/2QTHkWd">Adjust GameObject Transform properties directly from data custom assets</a>
    public void PositionZ(float value) {
      Vector3 localPosition = transform.localPosition;
      // ReSharper disable once Unity.InefficientPropertyAccess
      transform.localPosition = new Vector3(localPosition.x, localPosition.y, value);
    }

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/Transform Connector")]
    private static void AddConnector() =>
      Selection.activeTransform.gameObject.AddComponent<RectTransformCustomAssetConnector>();
    #endif
  }
}