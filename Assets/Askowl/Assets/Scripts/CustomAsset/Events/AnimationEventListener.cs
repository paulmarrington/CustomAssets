namespace CustomAsset {
  using UnityEngine;

  public sealed class AnimationEventListener : BaseEventListener {
    private enum FieldTypes {
      Float,
      Int,
      Bool,
      Trigger
    };

    [SerializeField] private Animator   animator;
    [SerializeField] private string     parameterName;
    [SerializeField]         FieldTypes parameterType;

    public override void OnEventRaised(IEventListener _) { animator.SetTrigger(parameterName); }
  }
}