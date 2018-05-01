// With thanks to Jason Weimann  -- jason@unity3d.college

using UnityEditor;
using UnityEngine;

namespace CustomAsset {
  /// <inheritdoc />
  /// <summary>
  /// Unity editor for AudioClip components. Adds a play button.
  /// </summary>
  [CustomEditor(typeof(AudioClips))]
  public class AudioClipsEditor : PreviewEditor<AudioSource> {
    /// <inheritdoc />
    protected override void Preview() { ((AudioClips) target).Play(Source); }
  }
}