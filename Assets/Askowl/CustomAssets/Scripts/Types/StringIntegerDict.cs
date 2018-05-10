using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Concrete Dictionary Custom Asset with a string key pointing to an integer value
  /// </summary>
  [CreateAssetMenu(menuName = "Custom Assets/Dictionary/String Key, Integer Value")]
  public class StringIntegerDict : Dict<string, int> { }
}