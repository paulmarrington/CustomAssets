/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

using System;

namespace CustomAsset {
  using JetBrains.Annotations;
  using UnityEngine;

  public abstract class OfType<T> : ScriptableObject {
#if UNITY_EDITOR
    [Multiline] public string Description = "";
#endif

    [Tooltip("Optional Channel to make variable reactive"), SerializeField]
    private Events.Channel onChange;

    [SerializeField] private T value;

    public T Value {
      get { return value; }
      set {
        this.value = value;
        Changed();
      }
    }

    public Action Changed { get; private set; }

    private void OnEnable() {
      if (onChange != null) {
        Changed = () => onChange.Trigger();
      } else {
        Changed = () => { };
      }
    }

    public static implicit operator T([NotNull] OfType<T> t) { return t.value; }
  }
}