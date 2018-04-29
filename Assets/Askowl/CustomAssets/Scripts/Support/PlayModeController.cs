using System.Collections;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Askowl {
  /// <summary>
  /// Helper code to control a running application with the goal of preparing for live testing.
  /// </summary>
  public class PlayModeController {
    /// <summary>
    /// Current scene as seen on the screen
    /// </summary>
    [UsedImplicitly] protected Scene Scene = default(Scene);

    /// <summary>
    /// Load scene by name. The scene must be registered in the build for this to be successful
    /// </summary>
    /// <param name="name">Name of scene</param>
    /// <returns>Enumerator that will allow a delay until the scene loading is complete</returns>
    [UsedImplicitly]
    protected virtual IEnumerator LoadScene(string name) {
      Log("Loading scene {0}", name);
      var handle = SceneManager.LoadSceneAsync(sceneName: name, mode: LoadSceneMode.Single);
      while (!handle.isDone) yield return null;

      Scene = SceneManager.GetActiveScene();
    }

    /// <summary>
    /// Given a reference to an active UI Button, provide the same functionality as a player pressing it.
    /// </summary>
    /// <param name="button">Reference to `Button` instance</param>
    /// <returns>Waits one update cycle so that button actions get a chance to start</returns>
    protected static IEnumerator PushButton(Button button) {
      button.Select();
      button.onClick.Invoke();
      yield return null;
    }

    /// <summary>
    /// Given the name of an active UI Button, provide the same functionality as a player pressing it.
    /// </summary>
    /// <param name="name">Name of button in the project hierarchy</param>
    /// <returns>Waits one update cycle so that button actions get a chance to start</returns>
    [UsedImplicitly]
    protected virtual IEnumerator PushButton(string name) {
      yield return PushButton(Objects.Component<Button>(name));
    }

    /// <summary>
    /// Shortcut to display formatted log messages to the console.
    /// </summary>
    /// <param name="format">Format string - or simple string if no parameters</param>
    /// <param name="parameters">List of parameters to fill the format</param>
    [UsedImplicitly]
    protected void Log(string format, params object[] parameters) {
      Debug.LogFormat(format, parameters);
    }
  }
}