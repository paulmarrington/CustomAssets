#if UNITY_EDITOR && CustomAssets
using System.Collections;
using CustomAsset;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.UI;

/// <inheritdoc />
/// <summary>
/// Monobehaviour to test pooling.
/// </summary>
public sealed class PoolExample : MonoBehaviour {
  /// <summary>
  /// Test pooling on the press of a button
  /// </summary>

  public void StartPoolTest() { StartCoroutine(PoolTest()); }

  private const float Frequency = 0.01f;

  private static IEnumerator PoolTest() {
    yield return new WaitForSeconds(0.5f);

    Assert.IsNotNull(Pool.PoolFor("PoolSamplePrefab"));
    Assert.IsNotNull(Pool.PoolFor("PoolPrefabResourceSample"));

    PoolPrefabScriptSample[] prefab1   = new PoolPrefabScriptSample[21];
    Pool.PoolQueue           poolQueue = Pool.PoolFor("PoolSamplePrefab");
    Assert.IsNotNull(poolQueue);
    Assert.AreEqual(0, poolQueue.Count);

    for (int i = 0; i < 21; i++) {
      prefab1[i] =
        Pool.Acquire<PoolPrefabScriptSample>("PoolSamplePrefab",
                                             parent: FindObjectOfType<Canvas>().transform,
                                             position: new Vector3(x: i * 60, y: i * 60));

      yield return new WaitForSeconds(Frequency);

      Assert.IsNotNull(prefab1[i]);
      prefab1[i].MaxCount = i;
    }

    poolQueue = Pool.PoolFor("PoolSamplePrefab");
    Assert.IsNotNull(poolQueue);
    Assert.AreEqual(0, poolQueue.Count);

    Text[] prefab2 = new Text[21];

    for (int i = 0; i < 21; i++) {
      prefab2[i] = Pool.Acquire<Text>("PoolPrefabResourceSample");
      yield return new WaitForSeconds(Frequency);

      Assert.IsNotNull(prefab2[i]);
      prefab2[i].gameObject.GetComponent<PoolPrefabScriptSample>().MaxCount = 100 + i;
    }

    poolQueue = Pool.PoolFor("PoolSamplePrefab");
    Assert.IsNotNull(poolQueue);
    Assert.AreEqual(expected: 0, actual: poolQueue.Count);
    poolQueue = Pool.PoolFor("PoolPrefabResourceSample");
    Assert.IsNotNull(poolQueue);
    Assert.AreEqual(expected: 0, actual: poolQueue.Count);

    for (int i = 0; i < 7; i++) {
      prefab1[i].gameObject.SetActive(false);
      yield return new WaitForSeconds(Frequency);
    }

    for (int i = 10; i < 19; i++) {
      prefab2[i].gameObject.SetActive(false);
      yield return new WaitForSeconds(Frequency);
    }

    poolQueue = Pool.PoolFor("PoolSamplePrefab");
    Assert.IsNotNull(poolQueue);
    Assert.AreEqual(expected: 7, actual: poolQueue.Count);
    poolQueue = Pool.PoolFor("PoolPrefabResourceSample");
    Assert.IsNotNull(poolQueue);
    Assert.AreEqual(expected: 9, actual: poolQueue.Count);

    Assert.IsNotNull(Pool.PoolFor("Scene GameObject for Pooling"));

    GameObject[] scenes = new GameObject[21];

    for (int i = 0; i < 21; i++) {
      scenes[i] = Pool.Acquire("Scene GameObject for Pooling");
      yield return new WaitForSeconds(Frequency);
    }

    poolQueue = Pool.PoolFor("Scene GameObject for Pooling");
    Assert.IsNotNull(poolQueue);
    Assert.AreEqual(expected: 0, actual: poolQueue.Count);

    for (int i = 15; i < 19; i++) {
      scenes[i].gameObject.SetActive(false);
      yield return new WaitForSeconds(Frequency);
    }

    poolQueue = Pool.PoolFor("Scene GameObject for Pooling");
    Assert.IsNotNull(poolQueue);
    Assert.AreEqual(expected: 4, actual: poolQueue.Count);

    yield return new WaitForSeconds(5);

    foreach (PoolPrefabScriptSample prefab in prefab1) {
      if (!prefab.gameObject.activeSelf) continue;

      prefab.gameObject.SetActive(false);
      yield return new WaitForSeconds(Frequency);
    }

    foreach (Text prefab in prefab2) {
      if (!prefab.gameObject.activeSelf) continue;

      prefab.gameObject.SetActive(false);
      yield return new WaitForSeconds(Frequency);
    }
  }
}
#endif