// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href=""></a> //#TBD#//
  public class RectTransformCustomAssetConnector : TransformCustomAssetConnector {
    private RectTransform rectTransform;

    /// <a href=""></a> //#TBD#//
    public void Width(float value) => rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);

    /// <a href=""></a> //#TBD#//
    public void Left(float value) => PositionX(value);

    /// <a href=""></a> //#TBD#//
    public void Right(float value) =>
      rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value - transform.localPosition.x);

    /// <a href=""></a> //#TBD#//
    public void Height(float value) => rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);

    /// <a href=""></a> //#TBD#//
    public void Bottom(float value) => PositionY(value);

    /// <a href=""></a> //#TBD#//
    public void Top(float value) =>
      rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value - transform.localPosition.y);

    /// <a href=""></a> //#TBD#//
    public void AnchorMinX(float value) => rectTransform.anchorMin = new Vector2(value, rectTransform.anchorMin.y);

    /// <a href=""></a> //#TBD#//
    public void AnchorMaxX(float value) => rectTransform.anchorMax = new Vector2(value, rectTransform.anchorMax.y);

    /// <a href=""></a> //#TBD#//
    public void AnchorPivotX(float value) => rectTransform.pivot = new Vector2(value, rectTransform.pivot.y);

    /// <a href=""></a> //#TBD#//
    public void AnchorMinY(float value) => rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, value);

    /// <a href=""></a> //#TBD#//
    public void AnchorMaxY(float value) => rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, value);

    /// <a href=""></a> //#TBD#//
    public void AnchorPivotY(float value) => rectTransform.pivot = new Vector2(rectTransform.pivot.x, value);

    private void Awake() {
      rectTransform = GetComponent<RectTransform>();
    }
  }
}