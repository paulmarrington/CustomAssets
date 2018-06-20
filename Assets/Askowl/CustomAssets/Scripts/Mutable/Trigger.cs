// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using System;
using Askowl;
using CustomAsset.Mutable;

namespace CustomAsset.Mutable {
  using UnityEngine;

  /// <inheritdoc cref="HasEmitter" />
  /// <summary>
  /// Dynamic custom asset without any values. Use it to trigger and listen to events.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#trigger">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Trigger")]
  public class Trigger : ScriptableObject, HasEmitter {
    public new static Trigger Instance(string name) {
      Trigger[] instances = Objects.Find<Trigger>(name);

      Trigger instance = (instances.Length > 0) ? instances[0] : Resources.Load<Trigger>(name);
      if (instance != null) return instance;

      instance      = CreateInstance<Trigger>();
      instance.name = Guid.NewGuid().ToString();
      return instance;
    }

    private Emitter emitter = new Emitter();

    /// <inheritdoc />
    public Emitter Emitter { get { return emitter; } }

    /// <summary>
    /// Call to fire off a Changed event, since we have no data to change...
    /// </summary>
    public void Fire() { emitter.Fire(); }
  }
}