using Askowl;
using UnityEngine;

namespace CustomAsset.Mutable {
  /// <a href="">Add this component to any game object and have it poll one or more custom assets. Add more than one copy if you need different polling rates for different custom assets</a> //#TBD#//
  public class Polling : MonoBehaviour {
    [SerializeField] private float         secondsDelayAtStart     = 5;
    [SerializeField] private float         updateIntervalInSeconds = 1;
    [SerializeField] private WithEmitter[] componentsToPoll        = default;

    private void Awake() => DontDestroyOnLoad(gameObject);

    private void Start() {
      void poll(Fiber fiber) {
        if (!isActiveAndEnabled) return;
        for (var i = 0; i < componentsToPoll.Length; i++) componentsToPoll[i].Poll();
      }

      Fiber.Start.WaitFor(secondsDelayAtStart).Begin.Do(poll).WaitFor(updateIntervalInSeconds).Again.Finish();
    }
  }
}