//- A game manager is a custom asset that provides the game logic for a single concern - in this case health.

using System;
using Askowl;
using CustomAsset.Mutable;
using UnityEngine;

#if UNITY_EDITOR && CustomAssets

// ReSharper disable MissingXmlDoc

//- Once the asset is create it can be loaded and used by anyone as it has no external code dependencies. We are replacing the Health value from the previous video with one that does more
[CreateAssetMenu(menuName = "Managers/Health")]
public class Health : Float {
  protected override void OnEnable() {
    base.OnEnable();
    Emitter.Subscribe(HealthChange);
  }

  //- It turns out that a health manager is extremely simple. All we need do is make sure it stays within expected bounds
  private void HealthChange() {
    var value = Value;
    if (value      < 0) Set(0);
    else if (value > 1) Set(1);
  }
}

#endif