//- A game manager is a custom asset that provides the game logic for a single concern - in this case health.

using System;
using Askowl;
using CustomAsset;
using CustomAsset.Mutable;
using UnityEngine;

#if UNITY_EDITOR && CustomAssets

// ReSharper disable MissingXmlDoc

//- Once the asset is create it can be loaded and used by anyone as it has no external code dependencies.
[CreateAssetMenu(menuName = "Managers/Health")]
public class HealthManager : Manager {
  [SerializeField] private Float health;
  [SerializeField] private Float maximumHealth;

  private void OnEnable() {
    void healthChange() {
      var value = health.Value;
      if (value < 0) {
        health.Value = 0;
      }
      else if (value > maximumHealth) {
        health.Value = maximumHealth;
      }
    }

    health.Emitter.Subscribe(healthChange);
  }

  //- In this game healing takes time - quickly for a potion and slowly for a passive effect
  public void HealPlayer(float healAmount, float overSeconds) {
    var steps      = Math.Min((int) (10 * overSeconds), 1);
    var stepTime   = overSeconds / steps;
    var stepAmount = healAmount  / steps;

    //- we heal in steps, not quitting on full health because player may also be taking damage
    void heal(Fiber fiber) => health.Value = Math.Min(health.Value + stepAmount, maximumHealth.Value);

    //- Fibers do not need to run from a MonoBehaviour
    Fiber.Start.Begin.Do(heal).WaitFor(stepTime).Repeat(steps);
  }

  //- Poison is like heal in reverse. In the real world I would refactor to use common code. This one is a stub for later implementation
  public void PoisonPlayer(float poisonAmount, float overSeconds) {
    Log.Error("Not Implemented");
  }
}

#endif