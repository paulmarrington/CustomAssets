#if UNITY_EDITOR && CustomAssets
using System;
using System.Collections;
using System.Linq;
using CustomAsset;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

/// <inheritdoc />
/// <summary>
/// Unity Test Runner PlayMode tests
/// </summary>
public class CustomAssetTests : PlayModeTests {
  private Text  results;
  private Float currentFloat;

  private IEnumerator Setup() {
    yield return LoadScene("Askowl-CustomAssets-Examples");

    results      = Component<Text>("Canvas/Results Panel/Text");
    currentFloat = FindObject<Float>("SampleFloatVariable");
  }

  /// <summary>
  /// Have the test scene open and press a button to display data in the results panel.
  /// </summary>
  /// <returns></returns>
  [UnityTest]
  public IEnumerator AccessCustomAssets() {
    yield return Setup();

    yield return PushButton("CustomAssetGet");

    CheckPattern(
      @"^currentFloat asset is 0\ninteger asset is 0\nstr asset is\s*\nboolean asset is True\nlarger asset is 1 / 5 / three$",
      results.text);
  }

  /// <summary>
  /// Make sure we can update the contents of custom assets in memory
  /// </summary>
  /// <returns></returns>
  [UnityTest]
  public IEnumerator UpdateCustomAssets() {
    yield return Setup();

    currentFloat.Value = 1;
    yield return PushButton("CustomAssetSet");

    const int count = 0;

    while (Math.Abs(currentFloat.Value) > 0.1f) {
      yield return null;

      Assert.Less(count, 200);
    }
  }

  /// <summary>
  /// Check for picking elements from a set sequentially
  /// </summary>
  /// <returns></returns>
  [UnityTest]
  public IEnumerator TestSetPickerCyclic() {
    yield return Setup();

    SetPickerSample picker = FindObject<SetPickerSample>("SampleSetPicker");
    AudioClip[]     clips  = new AudioClip[6];

    for (int i = 0; i < clips.Length; i++) clips[i] = picker.Pick();

    for (int i = 0; i < 3; i++) Assert.AreEqual(clips[i], clips[i + 3]);
  }

  /// <summary>
  /// Check for picking elements from a set randomly, but restricted to new ones until a cycle is exhausted
  /// </summary>
  /// <returns></returns>
  [UnityTest]
  public IEnumerator TestSetPickerExhaustive() {
    yield return Setup();

    AudioClips  picker = FindObject<AudioClips>();
    AudioClip[] clips  = new AudioClip[6];

    for (int i = 0; i < clips.Length; i++) clips[i] = picker.Pick();

    for (int i = 0; i < 3; i++) {
      for (int j = 0; j < 10; j++) {
        bool ok = false;

        for (int k = 3; k < 6; k++) {
          if ((i != k) && (clips[i] == clips[k])) {
            ok = true;
            break;
          }
        }

        Assert.IsTrue(ok);
      }
    }
  }

  /// <summary>
  /// Check that we can transfer a CustomAsset change event to a UnityEvent
  /// </summary>
  /// <returns></returns>
  [UnityTest, Timeout(10000)]
  public IEnumerator TestUnityEvent() {
    yield return Setup();

    Trigger     trigger     = FindObject<Trigger>("SampleUnityEventTrigger");
    AudioSource audioSource = Component<AudioSource>("Canvas/UnityEvent/UnityEventListener");

    trigger.Changed();
    while (!audioSource.isPlaying) yield return null;
    while (audioSource.isPlaying) yield return null;
  }

  /// <summary>
  /// React to a CustomAsset change event directly in a MonoBehaviour
  /// </summary>
  /// <returns></returns>
  [UnityTest]
  public IEnumerator TestDirectEvent() {
    yield return Setup();

    Trigger trigger = FindObject<Trigger>("SampleDirectEventTrigger");

    trigger.Changed();
    yield return null;

    CheckPattern(@"^Direct Event heard at \d\d/\d\d/\d\d\d\d \d\d:\d\d:\d\d", results.text);
  }

  /// <summary>
  /// Have a CustomAsset.Trigger change event set of an animation
  /// </summary>
  /// <returns></returns>
  [UnityTest, Timeout(10000)]
  public IEnumerator TestAnimationTriggerEvent() {
    yield return Setup();

    Trigger trigger = FindObject<Trigger>("SampleAnimatorTriggerEventTrigger");
    trigger.Changed();
    yield return CheckButtonAnimation();
  }

  private IEnumerator CheckButtonAnimation() {
    Button button = Component<Button>("CustomAssetEventListeners/Canvas/Button");

    float before = Time.realtimeSinceStartup;
    while (button.colors.normalColor.Equals(Color.white)) yield return null;
    while (!button.colors.normalColor.Equals(Color.white)) yield return null;

    float elapsed = Time.realtimeSinceStartup - before;
    Assert.Less(elapsed, 10.0f);
  }

  /// <summary>
  /// Have a CustomAsset.Float change event change the colours on the results button.
  /// </summary>
  /// <returns></returns>
  [UnityTest]
  public IEnumerator TestFloatAsset() {
    yield return Setup();

    Slider slider = Component<Slider>("Canvas/Float Asset/Slider");
    slider.value = 0.8f;
    yield return CheckButtonAnimation();
  }

  private string ResultsButtonText {
    get { return Component<Text>("CustomAssetEventListeners/Canvas/Button/Text").text; }
  }

  /// <summary>
  /// Have a CustomAsset.String change event change the contents of the results button
  /// </summary>
  /// <returns></returns>
  [UnityTest]
  public IEnumerator TestStringAsset() {
    yield return Setup();

    Assert.AreNotEqual("The rain in spain", ResultsButtonText);
    InputField inputField = Component<InputField>("Canvas/String Asset/InputField");
    inputField.text = "The rain in spain";
    Assert.AreEqual("The rain in spain", ResultsButtonText);
  }

  /// <summary>
  /// Have a CustomAsset.Integer change event the contents of the results button
  /// </summary>
  /// <returns></returns>
  [UnityTest]
  public IEnumerator TestIntegerAsset() {
    yield return Setup();

    Slider slider = Component<Slider>("Canvas/Integer Asset/Slider");
    slider.value = 0.77f;
    yield return null;

    int buttonValue = int.Parse(ResultsButtonText);
    Assert.GreaterOrEqual(buttonValue, 76);
    Assert.LessOrEqual(buttonValue, 78);
  }

  /// <summary>
  /// Have a CustomAsset.Boolean change event the contents of the results button
  /// </summary>
  /// <returns></returns>
  [UnityTest]
  public IEnumerator TestBooleanAsset() {
    yield return Setup();

    Toggle toggle = Component<Toggle>("Canvas/Boolean Asset/Toggle");
    toggle.isOn = false;
    yield return null;

    toggle.isOn = true;
    yield return null;

    Assert.AreEqual("True", ResultsButtonText);

    toggle.isOn = false;
    yield return null;

    Assert.AreEqual("False", ResultsButtonText);
  }

  [UnityTest, Timeout(10000)]
  public IEnumerator TestPooling() {
    yield return Setup();

    yield return PushButton("Show Pooling at Work");

    int                      length;
    PoolPrefabScriptSample[] all;

    do {
      yield return null;

      all    = Resources.FindObjectsOfTypeAll<PoolPrefabScriptSample>();
      length = all.Length;
    } while (length < 21);

    do {
      yield return null;

      all    = Resources.FindObjectsOfTypeAll<PoolPrefabScriptSample>();
      length = all.Count(one => !one.gameObject.activeSelf);
    } while (length < 16);
  }
}
#endif