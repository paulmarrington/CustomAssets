/*
 * With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017
 */

using System;

namespace CustomAsset {
  using JetBrains.Annotations;
  using UnityEngine;

  public class CustomAsset<T> : ScriptableObject {
#if UNITY_EDITOR
    [Multiline] public string Description = "";
#endif
    [SerializeField] private T              value;
    [SerializeField] private Events.Channel onChange;

    public Action Changed { get; private set; }

    public T Value {
      get { return value; }
      set {
        this.value = value;
        Changed();
      }
    }

    private void OnEnable() {
      if (onChange != null) {
        Changed = () => onChange.Trigger();
      } else {
        Changed = () => { };
      }
    }

    public static implicit operator T([NotNull] CustomAsset<T> t) { return t.value; }
  }
}