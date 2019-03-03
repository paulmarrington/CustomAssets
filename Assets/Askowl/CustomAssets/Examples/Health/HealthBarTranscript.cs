//- We are going to create a code-free health resource for your game. First let's create a test scene. [[Folder Examples/Editor/Example. Context Create/Scene. Name Health Double-click]]
//- Next create a custom asset to store character health. [[Folder Examples/Editor/Example. Context Create/Custom Assets/Mutable/Float. Name Health.]]
//- For convenience we will start with full health [[Health Value: 1]]
//- Now for the visual. We will start by creating a canvas [[Create/UI/Canvas, context/Create Empty Child/named Bounds]]
//- ... and an empty game object called HealthBar [[Create/Create Empty Child/named Health]]
//- Let's create an image for the background and make it red [[Set X/Y Anchors to Min 0, Max 1, Change X pivot to 0]]
//- Duplicate for the foreground, but make it green.
//- The foreground is the only active component. We are going to reduce the scale so that the background shows through. Since transforms don't expose their data, we use a connector. [[Drag connector into Inspector]]
//- And we need a float driver to change the scale when our health custom asset changes [[drag Float-Driver into the inspector]]
//- This is where we hook them up [[drop health asset into driver and set component to ScaleX]]
//- We created the health scene for two reasons - so that we can tweak our health bar before adding it to our project and so we can add manual and automatic testing. We need to add a component to drive the health bar. Fortunately the Unity UI has a slider that works a treat. [[Bounds//context//UI//Slider]]
//- It is as simple as pie to hook in our custom asset [[On Value Change/+/Health custom asset/Float.Value]]
//- Let's run it up and see if I have made any mistakes

//- To finish up, we will write a quick Unity test that can be automated if you wish. It will be run in the Unity Test Runner and drives our visual test slider directly.

#if AskowlTests
using System.Collections;
using UnityEditor;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using UnityEngine.UI;

// ReSharper disable MissingXmlDoc

namespace Askowl.Transcripts {
  public class HealthBarTranscript : PlayModeTests {
    private static string scenePath = "Health";

    //- PlayModeTests provides a support function to make sure the scene is in the Build Settings so it can be run
    #if UNITY_EDITOR
    [InitializeOnLoadMethod] private static void AddSceneToBuildSettings() => AddSceneToBuildSettings(scenePath);
    #endif

    //- We will only need a single method to test the integrity of the health function.
    [UnityTest] public IEnumerator HeathBarTestsWithEnumeratorPasses() {
      yield return LoadScene(scenePath);
      //- We will need a reference to the slider for control and the foreground to check the results
      var slider     = Component<Slider>("Testing Slider");
      var foreground = Component<RectTransform>("Foreground");
      //- Set the slider to match the health starting value
      slider.value = foreground.localScale.x;

      //- local function to set and check a health value. Note the wait. It could be one frame, but I have made it longer so we can see the test happening.
      IEnumerator setAndCheck(float health) {
        slider.value = health;
        yield return new WaitForSeconds(0.1f);
        var scale = foreground.localScale.x;
        Assert.AreApproximatelyEqual(health, scale, 0.01f);
      }

      //- Check bounds
      yield return setAndCheck(0);
      yield return setAndCheck(1);
      //- Ramp up and make sure all matches
      for (float health = 0; health <= 1; health += 0.05f) yield return setAndCheck(health);
      //- Now let's do some random ones in case change of direction can be a problem.
      for (int i = 0; i < 20; i++) yield return setAndCheck(Random.Range(0f, 1f));
    }
  }
}
#endif
//- I guess you will want to see the test running...