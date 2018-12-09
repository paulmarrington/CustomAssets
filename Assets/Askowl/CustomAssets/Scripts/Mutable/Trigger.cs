// With thanks to Ryan Hipple -- https://github.com/roboryantron/Unite2017

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Dynamic custom asset without any values. Use it to trigger and listen to events</a> //#TBD#// <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Trigger")]
  public class Trigger : WithEmitter {
    /// <a href="">Retrieve a loaded instance of a named trigger</a> //#TBD#//
    public static Trigger Instance(string name) {
      Trigger instance = Objects.Find<Trigger>(name);
      if (instance != null) return instance;

      instance      = CreateInstance<Trigger>();
      instance.name = Guid.NewGuid().ToString();
      return instance;
    }

    /// <a href="">Call to fire off a Changed event, since we have no data to change...</a> //#TBD#//
    public void Fire() => Emitter.Fire();
  }
}