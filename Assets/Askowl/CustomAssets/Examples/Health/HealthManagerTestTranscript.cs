#if UNITY_EDITOR && CustomAssets
using UnityEngine.TestTools;

// ReSharper disable MissingXmlDoc

//- Testing a manager custom asset is simplified as it does not need any Unity features. It is game logic completely divorced from the visual.

namespace Askowl.Transcripts {
  public class HealthManagerTestTranscript {
    [UnityTest] public void HeathManager() { }
  }
}
#endif