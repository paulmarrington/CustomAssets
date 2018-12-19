//- A game manager is a custom asset that provides the game logic for a single concern - in this case health.

using CustomAsset.Mutable;
using UnityEngine;

#if UNITY_EDITOR && CustomAssets

// ReSharper disable MissingXmlDoc

//- Once the asset is create it can be loaded and used by anyone as it has no external code dependencies. We are replacing the Health value from the previous video with one that does more
namespace Askowl.Transcripts {
  [CreateAssetMenu(menuName = "Managers/Health")]
  //- This custom asset replaces the generic Float version of health from the previous video
  public class Health : Float {
    //- Health will slowly increase over time
    [SerializeField] private float trickleChargePerSecond;

    protected override void OnEnable() {
      base.OnEnable();
      //- We can just update health every second. Float corrects for health over the maximum
      void trickleCharge(Fiber fiber) => Set(Value + trickleChargePerSecond);
      //- Fibers are efficient, only taking up time once a second to update health value
      Fiber.Start.Begin.WaitFor(seconds: 1).Do(trickleCharge).Again.Finish();
    }
  }
}

#endif