#if UNITY_EDITOR && CustomAssets
using System;
using Askowl;
using CustomAsset.Constant;
using UnityEngine;

/// <inheritdoc />
/// <summary>
/// Satisfy Unity Inspector
/// </summary>
[Serializable]
public class AudioClipSet : Set<AudioClip> { }

/// <inheritdoc />
/// <summary>
/// Example showing how to use SelectAsset to choose randomly between one of
/// three audio clips.
/// </summary>
[CreateAssetMenu(menuName = "Examples/SetPicker", fileName = "SetPickerSample")]
public sealed class SetPickerSample : OfType<AudioClipSet> {
  /// <summary>
  /// Play is linked to a button on the test site. SelectAsset.Pick() chooses
  /// from the list of audio clips. Whether it will be a random, cyclic or exhaustive choice
  /// will depend on settings within the asset.
  /// </summary>
  public void Play() { AudioSource.PlayClipAtPoint(clip: Value.Pick(), position: Vector3.zero); }
}
#endif