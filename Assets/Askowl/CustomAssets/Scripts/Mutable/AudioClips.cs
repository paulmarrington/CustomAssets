// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using CustomAsset.Constant;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Create an asset to store a list of sounds and play one randomly or cyclically</a> //#TBD#//
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Audio Clips", fileName = "AudioClips")]
  public class AudioClips : OfType<AudioClipSet> {
    /// <a href="">Retrieve an asset of AudioClips</a> //#TBD#//
    public static AudioClips Instance(string name) => Instance<AudioClips>(name);

    /// <a href="">Audio Clip Picker <see cref="CustomAsset.Constant.AudioClipSet"/></a> //#TBD#//
    public AudioClipSet Picker => Value;
  }
}