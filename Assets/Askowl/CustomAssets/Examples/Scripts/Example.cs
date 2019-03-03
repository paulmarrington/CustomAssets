#if UNITY_EDITOR && CustomAssets
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using UnityEngine.UI;

// ReSharper disable MissingXmlDoc

namespace Askowl.Example.HealthBarTranscript {
  public class HealthBarTranscript : PlayModeTests {
    private static string scenePath = "Health";
    #if UNITY_EDITOR

    [InitializeOnLoadMethod] private static void AddSceneToBuildSettings() => AddSceneToBuildSettings(scenePath);
    #endif
    [UnityTest] public IEnumerator HeathBarTestsWithEnumeratorPasses() {
      yield return LoadScene(scenePath);
      var slider     = Component<Slider>("Testing Slider");
      var foreground = Component<RectTransform>("Foreground");
      slider.value = foreground.localScale.x;
      IEnumerator setAndCheck(float health) {
        slider.value = health;
        yield return new WaitForSeconds(0.1f);
        var scale = foreground.localScale.x;
        Assert.AreApproximatelyEqual(health, scale);
      }
      yield return setAndCheck(0);
      yield return setAndCheck(1);
      for (float health = 0; health <= 1; health += 0.05f) yield return setAndCheck(health);
      for (int i = 0; i             < 20; i++) yield return setAndCheck(Random.Range(0f, 1f));
    }
  }
}
#endif