#if UNITY_EDITOR && CustomAssets
using System;
using Askowl;
using CustomAsset.Constant;
using UnityEngine;

/// <a href="http://bit.ly/2RcqaCt">Satisfy Unity Inspector</a> <inheritdoc />
[Serializable] public class AudioClipSet : Set<AudioClip> { }

/// <a href="http://bit.ly/2RcqaCt">Example showing how to use SelectAsset to choose randomly between one of three audio clips</a> <inheritdoc />
[CreateAssetMenu(menuName = "Examples/SetPicker", fileName = "SetPickerSample")]
public sealed class SetPickerSample : OfType<AudioClipSet> {
  /// <a href="http://bit.ly/2RcqaCt">Play is linked to a button on the test site. SelectAsset.Pick() chooses from the list of audio clips. Whether it will be a random, cyclic or exhaustive choice will depend on settings within the asset</a>
  public void Play() => AudioSource.PlayClipAtPoint(clip: Value.Pick(), position: Vector3.zero);
}
#endif