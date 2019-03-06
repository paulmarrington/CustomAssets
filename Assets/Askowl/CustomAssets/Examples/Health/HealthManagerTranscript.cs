//- A game manager is a custom asset that provides the game logic for a single concern - in this case health.

using System;
using CustomAsset;
using CustomAsset.Mutable;
using UnityEngine;

#if !ExcludeAskowlTests

// ReSharper disable MissingXmlDoc

//- Once the asset is create it can be loaded and used by code as it has no external code dependencies.
namespace Askowl.CustomAssets.Transcripts {
  //- Custom assets must be created to physical files in the project. We could have more than one.
  [CreateAssetMenu(menuName = "Managers/Health"), Serializable]
  public class HealthManagerTranscript : Manager {
    //- The field we are managing
    [SerializeField] private Float health = default;
    //- Health will slowly increase over time. A permanent ability could change this value
    [SerializeField] private Float trickleChargePerSecond = default;

    //- We use Initialise rather than OnEnable because we can't create a GameObject for Fibers in OnEnable. Initialise is called by a Managers MonoBehaviour or by PlayModeTests.AssetLoad during testing.
    protected override void Initialise() {
      base.Initialise();
      //- We can just update health every second. The Float custom asset corrects for health over the maximum
      void trickleCharge(Fiber fiber) => health.Value += trickleChargePerSecond;
      //- Fibers are efficient, only taking up time once a second to update health value
      Fiber.Start.Begin.WaitFor(seconds: 1.0f).Do(trickleCharge).Again.Finish();
    }
    //- Consider using the ChangeOverTime custom asset for updating health with potions or med-packs. Perhaps a large potion recharges to a higher value but more slowly.
  }
}

#endif