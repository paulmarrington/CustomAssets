using System;
using System.Collections;
using Askowl;
using CustomAsset;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using UnityEngine.UI;

public class CustomAssetTests : PlayModeTests {
  private Text  results;
  private Float maxFloat, currentFloat;

  private IEnumerator Setup() {
    yield return LoadScene("Askowl-CustomAssets-Examples");

    results      = Component<Text>("Canvas/Results Panel/Text");
    currentFloat = FindObject<Float>("SampleFloatVariable");
    maxFloat     = FindObject<Float>("MaxFloatVariable");
  }

  [UnityTest]
  public IEnumerator AccessCustomAssets() {
    yield return Setup();

    yield return PushButton("CustomAssetGet");

    CheckPattern(
      @"^currentFloat asset is 0\ninteger asset is 0\nstr asset is\s*\nboolean asset is True\nlarger asset is 1 / 5 / three$",
      results.text);
  }

  [UnityTest]
  public IEnumerator UpdateCustomAssets() {
    yield return Setup();

    currentFloat.Value = 1;
    yield return PushButton("CustomAssetSet");

    int count = 0;

    while (Math.Abs(currentFloat.Value) > 0.1f) {
      yield return null;

      Assert.Less(count, 200);
    }
  }

  [UnityTest]
  public IEnumerator TestSetPickerCyclic() {
    yield return Setup();

    SetPickerSample picker = FindObject<SetPickerSample>("SampleSetPicker");
    AudioClip[]     clips  = new AudioClip[6];

    for (int i = 0; i < clips.Length; i++) clips[i] = picker.Pick();

    for (int i = 0; i < 3; i++) Assert.AreEqual(clips[i], clips[i + 3]);
  }

  [UnityTest]
  public IEnumerator TestSetPickerExhaustive() {
    yield return Setup();

    Clips       picker = FindObject<Clips>();
    AudioClip[] clips  = new AudioClip[6];

    for (int i = 0; i < clips.Length; i++) clips[i] = picker.Pick();

    for (int i = 0; i < 3; i++) {
      bool ok = false;

      for (int j = 4; j < 6; j++) {
        if (clips[i].Equals(clips[j])) {
          ok = true;
          break;
        }
      }
      Assert.IsTrue(ok);
    }
  }
}