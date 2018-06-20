// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc />
  /// <summary>
  /// Create an asset to store a list of sounds and play one randomly or cyclicly.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#audioclips">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Audio Clips", fileName = "AudioClips")]
  public sealed class AudioClips : OfType<CustomAsset.Constant.AudioClipSet> {
    /// <summary>
    /// Retrieve an asset of AudioClips
    /// </summary>
    /// <param name="name">with this name</param>
    public static AudioClips Instance(string name) { return Instance<AudioClips>(name); }

    /// <summary>
    /// Audio Clip Picker <see cref="CustomAsset.Constant.AudioClipSet"/>
    /// </summary>
    public CustomAsset.Constant.AudioClipSet Picker { get { return Value; } }
  }
}