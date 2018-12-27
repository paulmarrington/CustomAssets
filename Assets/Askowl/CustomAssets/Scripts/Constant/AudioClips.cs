// Copyright 2018 (C) paul@marrington.net http://www.askowl.net/unity-packages

using System;
using Askowl;
using UnityEngine;

namespace CustomAsset.Constant {
  /// <a href="http://bit.ly/2RcqaCt">Support class for audio clip playing</a> <inheritdoc />
  [Serializable] public sealed class AudioClipSet : Set<AudioClip> {
    [SerializeField, Header("Audio")]     private Range volume   = new Range(1, 1);
    [SerializeField, RangeBounds(0, 2)]   private Range pitch    = new Range(1, 2);
    [SerializeField, RangeBounds(0, 999)] private Range distance = new Range(1, 999);

    /// <a href="http://bit.ly/2RcqaCt">Find an AudioSource to use for playing the sounds</a>
    public void Play(UnityEngine.GameObject gameObject) => Play(gameObject.GetComponent<AudioSource>());

    /// <a href="http://bit.ly/2RcqaCt">Play a random, exhaustive random or sequential sound - with random variations of volume, pitch and distance heard</a>
    public void Play(AudioSource source) {
      source.clip        = Pick();
      source.pitch       = pitch.Pick();
      source.volume      = volume.Pick();
      source.minDistance = distance.Min;
      source.maxDistance = distance.Max;
      source.Play();
    }
  }

  /// <a href="http://bit.ly/2RcqaCt">Create an asset to store a list of sounds and play one randomly or cyclically</a>
  [CreateAssetMenu(menuName = "Custom Assets/Constant/Audio Clips", fileName = "AudioClips")]
  public sealed class AudioClips : OfType<AudioClipSet> {
    /// <a href="http://bit.ly/2RcqaCt">Retrieve a reference to a named AudioClips asset</a>
    public new static AudioClips Instance(string name) => Instance<AudioClips>(name);

    /// <a href="http://bit.ly/2RcqaCt">Audio Clip Player <see cref="AudioClipSet"/></a>
    public AudioClipSet Picker => Value;
  }
}