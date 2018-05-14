using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Set or enum of strings - used to create custom asset
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#stringset">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/String Set")]
  public class StringSet : Set<string> { }
}