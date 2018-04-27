using System;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Linq;
using System.Text.RegularExpressions;
using CustomAsset;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/// <summary>
/// Base class for PlayMode Unity tests. Provides explicit `Setup` and `Teardown` functions.
/// </summary>
public class PlayModeTests {
  protected Scene Scene = default(Scene);

  [OneTimeSetUp]
  public void OneTimeSetUp() { }

  [OneTimeTearDown]
  public void OneTimeTearDown() { Log("Tests complete"); }

  [UsedImplicitly]
  protected IEnumerator LoadScene(string name) {
    Log("Loading scene {0}", name);
    yield return loadSceneAndWait(name);

    Scene = SceneManager.GetActiveScene();
    Assert.AreEqual(name, Scene.name);
  }

  private IEnumerator loadSceneAndWait(string name) {
    var handle = SceneManager.LoadSceneAsync(sceneName: name, mode: LoadSceneMode.Single);
    while (!handle.isDone) yield return null;
  }

  protected T Component<T>(string name) {
    GameObject gameObject = GameObject.Find(name);
    Assert.IsNotNull(gameObject, "No GameObject named {0}", name);
    return gameObject.GetComponent<T>();
  }

  protected T FindObject<T>(string name) where T : Object {
    T[] all = Resources.FindObjectsOfTypeAll<T>();

    foreach (T t in all.Where(t => t.name == name)) return t;

    Assert.Fail("No active Objects called '{0}'", name);
    return null;
  }

  protected IEnumerator PushButton(string name) {
    var button = Component<Button>(name);
    Assert.IsNotNull(button, "No button named {0}", name);
    button.Select();
    button.onClick.Invoke();
    yield return null;
  }

  protected void CheckPattern(string pattern, string against) {
    Regex           regex   = new Regex(pattern);
    MatchCollection matches = regex.Matches(against);
    Assert.AreEqual(1, matches.Count, "Pattern\n======{0}======Text======{1}", pattern, against);
  }

  [UsedImplicitly]
  protected void Log(string format, params object[] parameters) {
    Debug.LogFormat(format, parameters);
  }
}

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
}