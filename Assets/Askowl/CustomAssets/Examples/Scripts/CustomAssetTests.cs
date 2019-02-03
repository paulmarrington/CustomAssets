#if UNITY_EDITOR && CustomAssets
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Askowl;
using CustomAsset.Mutable;
using NUnit.Framework;
using UnityEditor;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;
using AudioClips = CustomAsset.Constant.AudioClips;

// ReSharper disable MissingXmlDoc
namespace Askowl.Examples {
  public class CustomAssetTests : PlayModeTests {
    private Text  results;
    private Float currentFloat;

    private static string scenePath = "Askowl-CustomAssets-Examples";

    #if UNITY_EDITOR
    [InitializeOnLoadMethod] private static void AddSceneToBuildSettings() => AddSceneToBuildSettings(scenePath);
    #endif

    private IEnumerator Setup() {
      yield return LoadScene(scenePath);

      results      = Component<Text>("Canvas/Results Panel/Text");
      currentFloat = FindObject<Float>("SampleFloatVariable");
    }

    [UnityTest] public IEnumerator AccessCustomAssets() {
      yield return Setup();

      yield return PushButton("CustomAssetGet");

      CheckPattern(
        new Regex(
          @"^currentFloat asset is \d+(\.\d+)?\ninteger asset is \d+\nstr asset is\s*.*\nboolean asset is (True|False)\nlarger asset is \d+ / \d+(\.\d+)? / three$")
       ,
        results.text);
    }

    [UnityTest, Timeout(10000)] public IEnumerator UpdateCustomAssets() {
      yield return Setup();

      currentFloat.Set(0.4f);
      yield return PushButton("CustomAssetSet");

      int count = 0;

      while (Math.Abs(currentFloat) > 0.1f) {
        Assert.Less(count++, 200);
        yield return null;
      }

      Assert.Greater(count, 1);
    }

    [UnityTest] public IEnumerator TestSetPickerCyclic() {
      yield return Setup();

      SetPickerSample picker = FindObject<SetPickerSample>("SampleSetPicker");
      AudioClip[]     clips  = new AudioClip[6];

      for (int i = 0; i < clips.Length; i++) clips[i] = picker.Value.Pick();

      for (int i = 0; i < 3; i++) Assert.AreEqual(clips[i], clips[i + 3]);
    }

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

    [UnityTest, Timeout(10000)] public IEnumerator TestUnityEvent() {
      yield return Setup();

      Trigger     trigger     = FindObject<Trigger>("SampleUnityEventTrigger");
      AudioSource audioSource = Component<AudioSource>("Canvas/UnityEvent/UnityEventListener");

      trigger.Fire();
      while (!audioSource.isPlaying) yield return null;
      while (audioSource.isPlaying) yield return null;
    }

    [UnityTest] public IEnumerator TestDirectEvent() {
      yield return Setup();

      Trigger trigger = FindObject<Trigger>("SampleDirectEventTrigger");

      trigger.Fire();
      yield return null;

      CheckPattern(new Regex(@"Direct Event heard at \d\d?/\d\d?/\d\d\d\d \d\d?:\d\d:\d\d"), results.text);
    }

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

    [UnityTest] public IEnumerator TestFloatAsset() {
      yield return Setup();

      Slider slider = Component<Slider>("Canvas/Float Asset/Slider");
      slider.value = 0.8f;
      yield return CheckButtonAnimation();
    }

    private string ResultsButtonText => Component<Text>("CustomAssetEventListeners/Canvas/Button/Text").text;

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

    [UnityTest] public IEnumerator TestIntegerAsset() {
      yield return Setup();

      Slider slider = Component<Slider>("Canvas/Integer Asset/Slider");
      slider.value = 5;
      // ReSharper disable once Unity.InefficientPropertyAccess
      slider.value = 6;
      yield return null;

      int buttonValue = int.Parse(ResultsButtonText);
      Assert.AreEqual(expected: 6, actual: buttonValue);
    }

    [UnityTest] public IEnumerator TestBooleanAsset() {
      yield return Setup();

      Toggle toggle = Component<Toggle>("Canvas/Boolean Asset/Toggle");
      toggle.isOn = false;
      yield return null;

      // ReSharper disable once Unity.InefficientPropertyAccess
      toggle.isOn = true;
      yield return null;

      Assert.AreEqual("True", ResultsButtonText);

      toggle.isOn = false;
      yield return null;

      Assert.AreEqual("False", ResultsButtonText);
    }

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

    [UnityTest] public IEnumerator TestInstance() {
      yield return Setup();

      yield return PushButton("Check Instance");

      CheckPattern(new Regex(@"^.* SampleFloatVariable as SampleFloatVariable\n.* 0.1234 .* 0.1234$"), results.text);
    }

    [UnityTest] public IEnumerator TestCompoundSetters() {
      var zero       = new CustomAssetsExample.LargerAssetContents {I = 0, f = 0, s = ""};
      var largeAsset = LargerAssetSample.New("LargerAssetSample");
      largeAsset.Value = zero;

      Assert.AreEqual(largeAsset.Contents, zero);

      var setTo = new CustomAssetsExample.LargerAssetContents {I = 123, f = 4.56f, s = "789"};
      largeAsset.Value = setTo;
      Assert.AreNotEqual(largeAsset.Contents, zero);
      Assert.AreEqual(largeAsset.Contents, setTo);
      yield return null;
    }

    [UnityTest] public IEnumerator ChangeOverTime() {
      yield return Setup();

      currentFloat.Set(0);
      yield return PushButton("Change Over Time");

      int count = 0;

      while (Math.Abs(currentFloat) < 0.99f) {
        Assert.Less(count++, 501);
        yield return null;
      }

      Debug.Log($"*** ChangeOverTime '{count}'"); //#DM#//
      Assert.Greater(count, 60);
    }
  }
}
#endif