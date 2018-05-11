namespace CustomAsset.Animation {
  using UnityEngine;

  /// <inheritdoc />
  /// <summary>
  /// Fire an animation trigger event when a custom asset value changes
  /// </summary>
  [RequireComponent(typeof(Animator))]
  public sealed class FloatListener : FloatListener<Animator> {
    [SerializeField] private string   parameterName;
    [SerializeField] private Animator animator;

    /// <inheritdoc />
    protected override void Change(float value) { animator.SetFloat(parameterName, value); }
  }
}