namespace CustomAsset {
  using UnityEngine;

  /// <summary>
  /// Float CustomAsset contains a float value which can be connected directly to OnValueChange
  /// callbacks in UI slider and scrollbar components. Connect it to event listeners to interact
  /// with components such as Animation, Text or Unity. Or add listeners to your own classes with
  /// Register(this).
  /// </summary>
  [CreateAssetMenu(menuName = "Custom Assets/Float")]
  public sealed class Float : OfType<float> { }
}