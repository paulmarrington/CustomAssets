namespace CustomAsset {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Integer CustomAsset contains an int value. Add listeners to your own classes with Register(this).
  /// </summary>
  /// <remarks><a href="http://customasset.marrington.net#primitive-custom-assets">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Integer")]
  public sealed class Integer : OfType<int> { }
}