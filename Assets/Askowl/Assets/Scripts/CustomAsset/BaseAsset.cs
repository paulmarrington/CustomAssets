/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

namespace CustomAsset {
  using JetBrains.Annotations;
  using UnityEngine;

  public class BaseAsset<T> : ScriptableObject {
#if UNITY_EDITOR
    [Multiline] public string Description = "";
#endif
    public T Value;

    public static implicit operator T([NotNull] BaseAsset<T> t) { return t.Value; }
  }
}