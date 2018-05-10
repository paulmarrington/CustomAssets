using System;
using JetBrains.Annotations;
using UnityEngine;

namespace CustomAsset {
  /// <summary>
  /// Encapsulation for one key value pair so it displays well in the inspector
  /// </summary>
  /// <typeparam name="TK">Type for the key</typeparam>
  /// <typeparam name="TV">Type for the value</typeparam>
  [Serializable]
  // ReSharper disable once InconsistentNaming
  public class KVP<TK, TV> {
    // ReSharper disable MissingXmlDoc
    [SerializeField, UsedImplicitly] public TK Key;

    [SerializeField, UsedImplicitly] public TV Value;
    // ReSharper restore MissingXmlDoc
  }
}