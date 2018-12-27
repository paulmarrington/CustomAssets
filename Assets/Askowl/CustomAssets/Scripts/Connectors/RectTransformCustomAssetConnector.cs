// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEditor;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
  public class RectTransformCustomAssetConnector : TransformCustomAssetConnector {
    private RectTransform rectTransform;

    /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
    public void Width(float value) => rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value);

    /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
    public void Left(float value) => PositionX(value);

    /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
    public void Right(float value) =>
      rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, value - transform.localPosition.x);

    /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
    public void Height(float value) => rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value);

    /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
    public void Bottom(float value) => PositionY(value);

    /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
    public void Top(float value) =>
      rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, value - transform.localPosition.y);

    /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
    public void AnchorMinX(float value) => rectTransform.anchorMin = new Vector2(value, rectTransform.anchorMin.y);

    /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
    public void AnchorMaxX(float value) => rectTransform.anchorMax = new Vector2(value, rectTransform.anchorMax.y);

    /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
    public void AnchorPivotX(float value) => rectTransform.pivot = new Vector2(value, rectTransform.pivot.y);

    /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
    public void AnchorMinY(float value) => rectTransform.anchorMin = new Vector2(rectTransform.anchorMin.x, value);

    /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
    public void AnchorMaxY(float value) => rectTransform.anchorMax = new Vector2(rectTransform.anchorMax.x, value);

    /// <a href="http://bit.ly/2QP2IM3">Adjust RectTransform properties directly from data custom assets</a>
    public void AnchorPivotY(float value) => rectTransform.pivot = new Vector2(rectTransform.pivot.x, value);

    private void Awake() => rectTransform = GetComponent<RectTransform>();

    #if UNITY_EDITOR
    [MenuItem("Component/CustomAssets/RectTransform Connector")]
    private static void AddConnector() =>
      Selection.activeTransform.gameObject.AddComponent<RectTransformCustomAssetConnector>();
    #endif
  }
}