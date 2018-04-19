namespace Events {
  using UnityEngine;

  public sealed class AnimationListener : BaseListener {
    [SerializeField] private Animator animator;
    [SerializeField] private string   parameterName;

    public override void OnTriggered(Listener _) { animator.SetTrigger(parameterName); }
  }
}