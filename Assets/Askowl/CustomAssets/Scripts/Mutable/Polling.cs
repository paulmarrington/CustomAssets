using System.Collections;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <inheritdoc />
  /// <summary>
  /// Add this component to any game object and have it poll one or more custom assets.
  /// Add more than one copy if you need different polling rates for different customa ssets.
  /// </summary>
  public sealed class Polling : MonoBehaviour {
    [SerializeField] private float         updateIntervalInSeconds;
    [SerializeField] private WithEmitter[] componentsToPoll;

    private bool running;

    /// <summary>
    /// View and control the running state of this component
    /// </summary>
    public bool Running {
      get { return running; }
      set {
        if (value && !running) StartPolling();
        running = value;
      }
    }

    private void Awake() { DontDestroyOnLoad(gameObject); }

    private void OnEnable() { StartPolling(); }

    /// <summary>
    /// Start a coroutine to poll a component on the given MonoBehaviour.
    /// </summary>
    private void StartPolling() {
      if (updateIntervalInSeconds > 0) StartCoroutine(StartPollingCoroutine());
    }

    private IEnumerator StartPollingCoroutine() {
      var interval = new WaitForSecondsRealtime(updateIntervalInSeconds);
      running = true;

      while (running) {
        foreach (var component in componentsToPoll) component.Poll();
        yield return interval;
      }
    }
  }
}