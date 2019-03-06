//- A manager is game logic completely divorced from the visual. It still needs to run in a project if it uses Fibers.
//- First we create a new custom asset, setting the starting health to zero as we want to see it grow. [[context/Manager/Health]
//- While Fibers do not need to run from a MonoBehaviour, they still need a running scene - so we use UnityTest
#if !ExcludeAskowlTests
// ReSharper disable MissingXmlDoc
using System.Collections;
using CustomAsset;
using CustomAsset.Mutable;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;

namespace Askowl.CustomAssets.Transcripts {
  public class HealthManagerTestTranscript : PlayModeTests {
    //- The Timeout attribute uses Time.timeScale.
    [UnityTest, Timeout(100000)] public IEnumerator HealthManager() {
      //- Our health component will take 1,000 seconds to go from zero to full. Let's speed things up by 10x
      Time.timeScale = 10;
      try {
        //- We don't need a specific scene, just load the custom assets
        Manager.Load<HealthManagerTranscript>("HealthManager.asset");
        var health = Manager.Load<Float>("Health.asset");
        //- Set to 0 since once a custom asset is loaded in the editor it stays loaded
        health.Set(0);
        //- It looks like forever, but the Timeout attribute will cause a test failure after 10 seconds
        while (true) {
          //- This causes a 1/10th of a second delay due to the modified scale
          yield return new WaitForSeconds(1.0f);
          //- Leaving once the test passes is a successful result. This should take 1 second at the current time scale.
          if (health >= 0.01f) yield break;
        }
      } finally {
        //- reset the time scale so other tests aren't effected.
        Time.timeScale = 1;
      }
    }
  }
}
#endif
//- Let's run the test to see if it is all working