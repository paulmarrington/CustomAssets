// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href=""></a> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Change Over Time")]
  public class ChangeOverTime : Trigger {
    [SerializeField] private Float targetForChange = default;
    [SerializeField] private float amountToChange  = 0;
    [SerializeField] private float overSeconds     = 1;
    [SerializeField] private int   stepsPerSecond  = 10;
    [SerializeField] private bool  resetOnStart    = false;

    private Fiber change;

    /// <a href=""></a> //#TBD#//
    public void Fire(float changeAmount, float seconds) {
      base.Fire();
      if (resetOnStart) targetForChange.Value = targetForChange.Minimum;
      var steps                               = Math.Max((int) (stepsPerSecond * seconds), 1);
      var stepTime                            = seconds      / steps;
      var stepAmount                          = changeAmount / steps;

      //- we heal in steps, not quitting on full health because player may also be taking damage
      void step(Fiber fiber) => targetForChange.Set(targetForChange + stepAmount);

      void finish(Fiber fiber) => change = null;

      //- Fibers do not need to run from a MonoBehaviour
      change = Fiber.Start.Begin.Do(step).WaitFor(stepTime).Repeat(steps).Do(finish);
    }

    /// <a href=""></a> //#TBD#// <inheritdoc />
    public override void Fire() => Fire(amountToChange, overSeconds);

    /// <a href=""></a> //#TBD#//
    public void Abort() => change?.Exit();
  }
}