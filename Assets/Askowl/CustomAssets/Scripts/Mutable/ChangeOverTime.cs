// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2CwSUh0">Change a float value in linear fashion over time</a> <inheritdoc />
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Change Over Time")]
  public class ChangeOverTime : Trigger {
    [SerializeField] private Float targetForChange = default;
    [SerializeField] private float amountToChange  = 0;
    [SerializeField] private float overSeconds     = 1;
    [SerializeField] private int   stepsPerSecond  = 10;
    [SerializeField] private bool  resetOnStart    = false;

    private Fiber change;
    private int   steps;
    private float stepTime, stepAmount;

    /// <a href="http://bit.ly/2CwSUh0">Start a fiber to change over non-default period</a>
    public void Fire(float changeAmount, float seconds) {
      base.Fire();
      if (resetOnStart) targetForChange.Value = targetForChange.Minimum;
      steps      = Math.Max((int) (stepsPerSecond * seconds), 1);
      stepTime   = seconds      / steps;
      stepAmount = changeAmount / steps;
      change.Go(); // Fibers do not need to run from a MonoBehaviour
    }

    /// <a href="http://bit.ly/2CwSUh0">Start a fiber to change over default period</a> <inheritdoc />
    public override void Fire() => Fire(amountToChange, overSeconds);

    /// <a href="http://bit.ly/2CwSUh0">Abort a long-running change loop</a>
    public void Abort() => change?.Exit();

    ///
    protected override void Initialise() {
      base.Initialise();
      void updateTargetValue(Fiber fiber) => targetForChange.Set(targetForChange + stepAmount);
      change = Fiber.Instance.Begin.Do(updateTargetValue).WaitFor(stepTime).Repeat(_ => steps);
    }
  }
}