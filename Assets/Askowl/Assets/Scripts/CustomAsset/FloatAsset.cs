namespace CustomAsset {
  using System;
  using UnityEngine;

  [Serializable]
  public sealed class Float : Primative<float> { }

  [CreateAssetMenu(menuName = "Custom Assets/Float")]
  public sealed class FloatAsset : PrimativeAsset<float> { }
}