using JetBrains.Annotations;
using UnityEngine;

namespace Askowl {
  [CreateAssetMenu(menuName = "Custom Assets/Sound Clips", fileName = "Clips", order = 1)]
  public sealed class Clips : AssetSelector<AudioClip> {
    [UsedImplicitly]
    public void Play() {
      AudioSource.PlayClipAtPoint(clip: Pick(), position: new Vector3(x: 0, y: 0, z: 0));
    }
  }
}