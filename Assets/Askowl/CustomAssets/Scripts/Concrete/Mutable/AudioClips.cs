// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc />
  /// <summary>
  /// Create an asset to store a list of sounds and play one randomly or cyclicly.
  /// </summary>
  /// <remarks><a href="http://customassets.marrington.net#audioclips">More...</a></remarks>
  [CreateAssetMenu(menuName = "Custom Assets/Mutable/Audio Clips", fileName = "AudioClips")]
  public sealed class AudioClips : OfType<CustomAsset.Support.AudioClips> {
    /// <summary>
    /// Audio Clip Picker <see cref="CustomAsset.Support.AudioClips"/>
    /// </summary>
    public CustomAsset.Support.AudioClips Picker { get { return Value; } }
  }
}