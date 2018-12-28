// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2RlCfFy">Dynamic custom asset without any values. Use it to trigger and listen to events</a> <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Trigger")]
  public class Trigger : WithEmitter {
    /// <a href="http://bit.ly/2RlCfFy">Retrieve a loaded instance of a named trigger</a>
    public static Trigger Instance(string name) {
      Trigger instance = Objects.Find<Trigger>(name);
      if (instance != null) return instance;

      instance      = CreateInstance<Trigger>();
      instance.name = Guid.NewGuid().ToString();
      return instance;
    }

    /// <a href="http://bit.ly/2RlCfFy">Call to fire off a Changed event, since we have no data to change...</a>
    public virtual void Fire() => Emitter.Fire();
  }
}