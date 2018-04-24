#if UNITY_EDITOR
using CustomAsset;
using JetBrains.Annotations;
using UnityEngine;
using Random = System.Random;

/// <summary>
/// Example showing how to use SelectAsset to choose randomly between one of
/// three audio clips.
/// </summary>
[CreateAssetMenu(menuName = "Examples/SelectAsset", fileName = "SelectAssetSample", order = 1)]
public sealed class SelectorSample : Selector<AudioClip> {
  /// <summary>
  /// Play is linked to a button on the test site. SelectAsset.Pick() chooses
  /// from the list of audio clips. Whether it will be a random, cyclic or exhaustive choice
  /// will depend on settings within the asset.
  /// </summary>
  [UsedImplicitly]
  public void Play() { AudioSource.PlayClipAtPoint(clip: Pick(), position: Vector3.zero); }
}
#endif