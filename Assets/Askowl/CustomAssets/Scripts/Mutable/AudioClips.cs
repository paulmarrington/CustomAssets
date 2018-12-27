// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using CustomAsset.Constant;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="http://bit.ly/2RcqaCt">Create an asset to store a list of sounds and play one randomly or cyclically</a>
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Audio Clips", fileName = "AudioClips")]
  public class AudioClips : OfType<AudioClipSet> {
    /// <a href="http://bit.ly/2RcqaCt">Retrieve an asset of AudioClips</a>
    public static AudioClips Instance(string name) => Instance<AudioClips>(name);

    /// <a href="http://bit.ly/2RcqaCt">Audio Clip Picker <see cref="CustomAsset.Constant.AudioClipSet"/></a>
    public AudioClipSet Picker => Value;
  }
}