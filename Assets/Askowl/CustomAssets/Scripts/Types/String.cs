using JetBrains.Annotations;

namespace CustomAsset {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// CustomAsset that contains a string. Events are triggered every time the string changes
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#primitive-custom-assets">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/String")]
  public sealed class String : OfType<string> { }
}