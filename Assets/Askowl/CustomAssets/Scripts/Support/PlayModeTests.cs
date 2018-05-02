﻿using System.Collections;
using System.Text.RegularExpressions;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine.UI;
using Object = UnityEngine.Object;

namespace CustomAsset {
  /// <summary>
  /// Base class for PlayMode Unity tests. Provides explicit `Setup` and `Teardown` functions.
  /// </summary>
  public class PlayModeTests : PlayModeController {
    [OneTimeTearDown]
    // ReSharper disable once MissingXmlDoc
    public void OneTimeTearDown() { Log("Tests complete"); }

    [UsedImplicitly]
    protected override IEnumerator LoadScene(string name) {
      yield return base.LoadScene(name);

      Assert.AreEqual(name, Scene.name);
    }

    /// <summary>
    /// Override for Objects.Component to check the result for compliance. Finds an active GameObject in the scene and retrieved a component by type from it.
    /// </summary>
    /// <param name="name">Name of GameObject holding required component</param>
    /// <typeparam name="T">Type of component to retrieve</typeparam>
    /// <returns></returns>
    protected static T Component<T>(string name) {
      T component = Objects.Component<T>(name);
      Assert.AreNotEqual(component, default(T), "No GameObject named {0}", name);
      return component;
    }

    /// <summary>
    /// Override for Objects.Find to check the result for compliance. Finds an active object. Used to retrieve custom assets that have been loaded into the scene elsewhere.
    /// </summary>
    /// <param name="name">Name of object to find</param>
    /// <typeparam name="T">Type of object to find</typeparam>
    /// <returns></returns>
    protected static T FindObject<T>(string name) where T : Object {
      T gameObject = Objects.Find<T>(name);
      Assert.NotNull(gameObject, "No active Objects called '{0}'", name);
      return gameObject;
    }

    /// <summary>
    /// Override for Objects.Find to check the result for compliance. Finds an active object. Used to retrieve custom assets that have been loaded into the scene elsewhere.
    /// </summary>
    /// <typeparam name="T">Type of object to find</typeparam>
    /// <returns></returns>
    protected static T FindObject<T>() where T : Object { return FindObject<T>(typeof(T).Name); }

    protected override IEnumerator PushButton(string name) {
      yield return PushButton(Component<Button>(name));
    }

    /// <summary>
    /// Given text extracted from the running game, check it against a regular expression and freak out if it doesn't match
    /// </summary>
    /// <param name="pattern">String representation of a regular expression</param>
    /// <param name="against">String to check against the regular expression</param>
    protected static void CheckPattern(string pattern, string against) {
      Regex           regex   = new Regex(pattern);
      MatchCollection matches = regex.Matches(against);
      Assert.AreEqual(1, matches.Count, "Pattern\n======{0}======Text======{1}", pattern, against);
    }
  }
}