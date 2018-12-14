#if UNITY_EDITOR && CustomAssets
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Askowl;
using CustomAsset.Mutable;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using AudioClips = CustomAsset.Constant.AudioClips;

/// <a href="">Unity Test Runner PlayMode tests</a> //#TBD#// <inheritdoc />
public class CustomAssetTests : PlayModeTests {
  private Text  results;
  private Float currentFloat;

  private IEnumerator Setup() {
    yield return LoadScene("Askowl-CustomAssets-Examples");

    results      = Component<Text>("Canvas/Results Panel/Text");
    currentFloat = FindObject<Float>("SampleFloatVariable");
  }

  /// <a href="">Have the test scene open and press a button to display data in the results panel</a> //#TBD#//
  [UnityTest] public IEnumerator AccessCustomAssets() {
    yield return Setup();

    yield return PushButton("CustomAssetGet");

    CheckPattern(new Regex(
                   @"^currentFloat asset is \d+(\.\d+)?\ninteger asset is \d+\nstr asset is\s*.*\nboolean asset is (True|False)\nlarger asset is \d+ / \d+(\.\d+)? / three$"),
                 results.text);
  }

  /// <a href="">Make sure we can update the contents of custom assets in memory</a> //#TBD#//
  [UnityTest, Timeout(10000)] public IEnumerator UpdateCustomAssets() {
    yield return Setup();

    currentFloat.Set(1);
    yield return PushButton("CustomAssetSet");

    const int count = 0;

    while (Math.Abs(currentFloat) > 0.1f) {
      Assert.Less(count, 200);
      yield return null;
    }
  }

  /// <a href="">Check for picking elements from a set sequentially</a> //#TBD#//
  [UnityTest] public IEnumerator TestSetPickerCyclic() {
    yield return Setup();

    SetPickerSample picker = FindObject<SetPickerSample>("SampleSetPicker");
    AudioClip[]     clips  = new AudioClip[6];

    for (int i = 0; i < clips.Length; i++) clips[i] = picker.Value.Pick();

    for (int i = 0; i < 3; i++) Assert.AreEqual(clips[i], clips[i + 3]);
  }

  /// <a href="">Check for picking elements from a set randomly, but restricted to new ones until a cycle is exhausted</a> //#TBD#//
  [UnityTest] public IEnumerator TestSetPickerExhaustive() {
    yield return Setup();

    var picker = AudioClips.Instance("SampleAudioClips");
    Assert.IsNotNull(picker);
    AudioClip[] clips = new AudioClip[6];

    for (int i = 0; i < clips.Length; i++) clips[i] = picker.Picker.Pick();

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

  /// <a href="">Check that we can transfer a CustomAsset change event to a UnityEvent</a> //#TBD#//
  [UnityTest, Timeout(10000)] public IEnumerator TestUnityEvent() {
    yield return Setup();

    Trigger     trigger     = FindObject<Trigger>("SampleUnityEventTrigger");
    AudioSource audioSource = Component<AudioSource>("Canvas/UnityEvent/UnityEventListener");

    trigger.Fire();
    while (!audioSource.isPlaying) yield return null;
    while (audioSource.isPlaying) yield return null;
  }

  /// <a href="">React to a CustomAsset change event directly in a MonoBehaviour</a> //#TBD#//
  [UnityTest] public IEnumerator TestDirectEvent() {
    yield return Setup();

    Trigger trigger = FindObject<Trigger>("SampleDirectEventTrigger");

    trigger.Fire();
    yield return null;

    CheckPattern(new Regex(@"Direct Event heard at \d\d/\d\d/\d\d\d\d \d\d?:\d\d:\d\d"), results.text);
  }

  /// <a href="">Have a CustomAsset.Trigger change event set of an animation</a> //#TBD#//
  [UnityTest, Timeout(10000)] public IEnumerator TestAnimationTriggerEvent() {
    yield return Setup();

    Trigger trigger = FindObject<Trigger>("SampleAnimatorTriggerEventTrigger");
    trigger.Fire();
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

  /// <a href="">Have a CustomAsset.Float change event change the colours on the results button</a> //#TBD#//
  [UnityTest] public IEnumerator TestFloatAsset() {
    yield return Setup();

    Slider slider = Component<Slider>("Canvas/Float Asset/Slider");
    slider.value = 0.8f;
    yield return CheckButtonAnimation();
  }

  private string ResultsButtonText => Component<Text>("CustomAssetEventListeners/Canvas/Button/Text").text;

  /// <a href="">Have a CustomAsset.String change event change the contents of the results button</a> //#TBD#//
  [UnityTest] public IEnumerator TestStringAsset() {
    yield return Setup();

    Assert.AreNotEqual("The rain in spain", ResultsButtonText);
    InputField inputField = Component<InputField>("Canvas/String Asset/InputField");
//    inputField.text = " ";
//    yield return null;
    inputField.text = "The rain in spain";
    yield return null;
    Assert.AreEqual("The rain in spain", ResultsButtonText);
  }

  /// <a href="">Have a CustomAsset.Integer change event the contents of the results button</a> //#TBD#//
  [UnityTest] public IEnumerator TestIntegerAsset() {
    yield return Setup();

    Slider slider = Component<Slider>("Canvas/Integer Asset/Slider");
    slider.value = 5;
    slider.value = 6;
    yield return null;

    int buttonValue = int.Parse(ResultsButtonText);
    Assert.AreEqual(expected: 6, actual: buttonValue);
  }

  /// <a href="">Have a CustomAsset.Boolean change event the contents of the results button</a> //#TBD#//
  [UnityTest] public IEnumerator TestBooleanAsset() {
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

  /// <a href="">Make sure quotes are retrieved as expected - exhaustive random</a> //#TBD#//
  [UnityTest, Timeout(10000)] public IEnumerator TestQuotes() {
    yield return Setup();

    var quotes = new Dictionary<string, int>();
    for (int i = 0; i < 20; i++) {
      yield return PushButton("Show Quote");

      if (!quotes.ContainsKey(results.text)) quotes[results.text] = 0;
      quotes[results.text] += 1;
    }

    Assert.GreaterOrEqual(quotes.Count, 10);
  }

  /// <a href="">Does Base.Instance return the same named item each time?</a> //#TBD#//
  [UnityTest] public IEnumerator TestInstance() {
    yield return Setup();

    yield return PushButton("Check Instance");

    CheckPattern(new Regex(@"^.* SampleFloatVariable as SampleFloatVariable\n.* 1234 .* 1234$"), results.text);
  }

  /// <a href="">Make sure that larger objects can be manipulated and tested for equality</a> //#TBD#//
  [UnityTest] public IEnumerator TestCompoundSetters() {
    var zero       = new CustomAssetsExample.LargerAssetContents {I = 0, F = 0, S = ""};
    var largeAsset = LargerAssetSample.New("LargerAssetSample");
    largeAsset.Set(zero);

    Assert.AreEqual(largeAsset.Contents, zero);

    var setTo = new CustomAssetsExample.LargerAssetContents {I = 123, F = 4.56f, S = "789"};
    largeAsset.Set(setTo);
    Assert.AreNotEqual(largeAsset.Contents, zero);
    Assert.AreEqual(largeAsset.Contents, setTo);
    yield return null;
  }
}
#endif