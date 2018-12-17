// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href=""></a> //#TBD#//
  public class TransformCustomAssetConnector : MonoBehaviour {
    /// <a href=""></a> //#TBD#//
    public void ScaleX(float value) =>
      transform.localScale = new Vector3(value, transform.localScale.y, transform.localScale.z);

    /// <a href=""></a> //#TBD#//
    public void ScaleY(float value) =>
      transform.localScale = new Vector3(transform.localScale.x, value, transform.localScale.z);

    /// <a href=""></a> //#TBD#//
    public void ScaleZ(float value) =>
      transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, value);

    /// <a href=""></a> //#TBD#//
    public void RotationX(float value) =>
      transform.localEulerAngles = new Vector3(value * 360, transform.localEulerAngles.y, transform.localEulerAngles.z);

    /// <a href=""></a> //#TBD#//
    public void RotationY(float value) =>
      transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, value * 360, transform.localEulerAngles.z);

    /// <a href=""></a> //#TBD#//
    public void RotationZ(float value) =>
      transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, value * 360);

    /// <a href=""></a> //#TBD#//
    public void PositionX(float value) =>
      transform.localPosition = new Vector3(value, transform.localPosition.y, transform.localPosition.z);

    /// <a href=""></a> //#TBD#//
    public void PositionY(float value) =>
      transform.localPosition = new Vector3(transform.localPosition.x, value, transform.localPosition.z);

    /// <a href=""></a> //#TBD#//
    public void PositionZ(float value) =>
      transform.localPosition = new Vector3(transform.localPosition.x, transform.localPosition.y, value);
  }
}