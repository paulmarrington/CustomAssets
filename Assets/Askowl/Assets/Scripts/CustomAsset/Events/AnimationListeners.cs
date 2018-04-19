namespace Events.Animation {
  using UnityEngine;
  using CustomAsset;

  public sealed class TriggerListener : BaseListener {
    [SerializeField] private string   parameterName;
    [SerializeField] private Animator animator;

    public override void OnTriggered(Listener listener) { animator.SetTrigger(parameterName); }
  }

  public sealed class BooleanListener : BaseListener<Boolean, bool> {
    protected override void Change(bool value) { Component.SetBool(ParameterName, value); }
  }

  public sealed class FloatListener : BaseListener<Float, float> {
    protected override void Change(float value) { Component.SetFloat(ParameterName, value); }
  }

  public sealed class IntegerListener : BaseListener<Integer, int> {
    protected override void Change(int value) { Component.SetFloat(ParameterName, value); }
  }

  public abstract class BaseListener<TA, TD> : ComponentListener<Animator, TA, TD>
    where TA : CustomAsset<TD> {
    [SerializeField] protected string ParameterName;
  }
}