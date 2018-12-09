using System.Collections;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Add this component to any game object and have it poll one or more custom assets. Add more than one copy if you need different polling rates for different custom assets</a> //#TBD#//
  public sealed class Polling : MonoBehaviour {
    [SerializeField] private float         secondsDelayAtStart     = 5;
    [SerializeField] private float         updateIntervalInSeconds = 1;
    [SerializeField] private WithEmitter[] componentsToPoll;

    private void Awake() => DontDestroyOnLoad(gameObject);

    private IEnumerator Start() {
      yield return new WaitForSecondsRealtime(secondsDelayAtStart);

      while (true) {
        yield return new WaitForSecondsRealtime(updateIntervalInSeconds);

        if (isActiveAndEnabled) {
          foreach (var component in componentsToPoll) component.Poll();
        }
      }
    }
  }
}