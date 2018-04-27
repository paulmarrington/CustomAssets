#if UNITY_EDITOR && CustomAssets
using UnityEngine;

/// <summary>
/// Sample custom asset to test for more complex data
/// </summary>
[CreateAssetMenu(menuName = "Examples/LargerAssetSample")]
public class LargerAssetSample : CustomAsset.OfType<CustomAssetsExample.LargerAssetContents> { }
#endif