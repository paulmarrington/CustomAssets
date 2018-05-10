using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Concrete Dictionary Custom Asset with a string key pointing to an string value
  /// </summary>
  [CreateAssetMenu(menuName = "Custom Assets/Dictionary/String Key, String Value")]
  public class StringStringDict : Dict<string, string> { }
}