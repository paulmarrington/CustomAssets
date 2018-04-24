namespace CustomAsset.Animation {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Fire an animation trigger event when a custom asset value changes
  /// </summary>
  public sealed class BooleanListener : BooleanListener<Animator> {
    [SerializeField] private string   parameterName;
    [SerializeField] private Animator animator;

    protected override void Change(bool value) { animator.SetBool(parameterName, value); }
  }
}