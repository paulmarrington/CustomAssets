using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Concrete Dictionary Custom Asset with a string key pointing to an integer value
  /// </summary>
  [CreateAssetMenu(menuName = "Custom Assets/Dictionary/String Key, Float Value")]
  public class StringFloatDict : Dict<string, float> { }
}