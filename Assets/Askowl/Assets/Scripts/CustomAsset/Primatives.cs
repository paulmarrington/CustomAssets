/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

namespace CustomAsset {
  using System;
  using JetBrains.Annotations;
  using UnityEngine;

  [Serializable]
  public class Primative<T> {
    [UsedImplicitly, SerializeField] private PrimativeAsset<T> variable;

    private bool isConstant;
    private T    constantValue;

    [UsedImplicitly]
    public T Value {
      get { return isConstant ? constantValue : variable.Value; }
      set {
        isConstant    = true;
        constantValue = value;
      }
    }

    public static implicit operator T([NotNull] Primative<T> t) { return t.Value; }
  }

  public class PrimativeAsset<T> : ScriptableObject {
#if UNITY_EDITOR
    [Multiline] public string Description = "";
#endif
    public T Value;

    public static implicit operator T([NotNull] PrimativeAsset<T> t) { return t.Value; }
  }
}