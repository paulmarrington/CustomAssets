using System;
using UnityEngine;
using UnityEngine.TestTools;
using System.Collections;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Object = UnityEngine.Object;

/// <summary>
/// Base class for PlayMode Unity tests. Provides explicit `Setup` and `Teardown` functions.
/// </summary>
public class PlayModeTests {
  protected class Objects<T> where T : Object {
    private T[]                   list;
    private Dictionary<string, T> dict = new Dictionary<string, T>();

    public Objects() { List = Resources.FindObjectsOfTypeAll<T>(); }

    public T[] List {
      set {
        foreach (T entry in (list = value)) dict[entry.name] = entry;
      }
    }

    public int Length { get { return list.Length; } }

    public bool Contains(string key) { return dict.ContainsKey(key); }

    public T this[int i] { get { return list[i]; } }

    public T this[string key] {
      get { return dict.ContainsKey(key) ? dict[key] : default(T); }
      set { dict[key] = value; }
    }
  }

  protected Scene               Scene = default(Scene);
  protected Objects<GameObject> GameObjects;

  private Scene firstScene;

  [OneTimeSetUp]
  public void OneTimeSetUp() { firstScene = SceneManager.GetActiveScene(); }

  [OneTimeTearDown]
  public void OneTimeTearDown() {
    Log("Tests complete, reloading {0}", firstScene.name);
    // ReSharper disable once IteratorMethodResultIsIgnored
    loadSceneAndWait(firstScene.name);
  }

  [UsedImplicitly]
  protected IEnumerator LoadScene(string name) {
    Log("Loading scene {0}...", name);
    yield return loadSceneAndWait(name);

    Scene = SceneManager.GetActiveScene();
    Assert.AreEqual(name, Scene.name);
    GameObjects = new Objects<GameObject>();
    Log("...Scene {0} loaded with {1} GameObjects", name, GameObjects.Length);
  }

  private IEnumerator loadSceneAndWait(string name) {
    var handle = SceneManager.LoadSceneAsync(sceneName: name, mode: LoadSceneMode.Single);
    while (!handle.isDone) yield return null;
  }

  protected T Component<T>(string name) {
    Assert.IsTrue(GameObjects.Contains(name));
    return GameObjects[name].GetComponent<T>();
  }

  protected IEnumerator PushButton(string name) {
    var button = Component<Button>(name);
    button.Select();
    button.onClick.Invoke();
    yield return null;
  }

  [UsedImplicitly]
  protected void Log(string format, params object[] parameters) {
    Debug.LogFormat(format, parameters);
  }
}

public class CustomAssetTests : PlayModeTests {
  [UnityTest]
  public IEnumerator AccessCustomAssets() {
    yield return LoadScene("Askowl-CustomAssets-Examples");
    yield return PushButton("CustomAssetGet");
  }
}