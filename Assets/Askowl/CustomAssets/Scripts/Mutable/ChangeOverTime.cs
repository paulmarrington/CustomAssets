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
    [SerializeField] private Range range           = new Range(0, 1);

    private Fiber change;

    /// <a href=""></a> //#TBD#//
    public void Start() => Fire();

    /// <a href=""></a> //#TBD#// <inheritdoc />
    public override void Fire() {
      base.Fire();
      var steps      = Math.Min((int) (stepsPerSecond * overSeconds), 1);
      var stepTime   = overSeconds    / steps;
      var stepAmount = amountToChange / steps;

      //- we heal in steps, not quitting on full health because player may also be taking damage
      void step(Fiber fiber) =>
        targetForChange.Value = Math.Max(range.Min, Math.Min(targetForChange.Value + stepAmount, range.Max));

      void finish(Fiber fiber) => change = null;

      //- Fibers do not need to run from a MonoBehaviour
      change = Fiber.Start.Begin.Do(step).WaitFor(stepTime).Repeat(steps).Do(finish);
    }

    /// <a href=""></a> //#TBD#//
    public void Abort() => change?.Exit();
  }
}