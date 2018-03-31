using System.Collections;
using Askowl;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class PoolExample : MonoBehaviour {
  private void  Awake() { StartCoroutine(PoolTest()); }
  private float frequency = 0.01f;

  IEnumerator PoolTest() {
    yield return new WaitForSeconds(0.5f);

    Assert.IsNotNull(Pools.PoolFor("PoolSamplePrefab"));
    Assert.IsNotNull(Pools.PoolFor("PoolPrefabScriptSample"));

    PoolPrefabScriptSample[] prefab1 = new PoolPrefabScriptSample[21];
    Assert.AreEqual(0, Pools.PoolFor("PoolSamplePrefab").Count);

    for (int i = 0; i < 21; i++) {
      prefab1[i] = Pools.Get<PoolPrefabScriptSample>("PoolSamplePrefab");

      yield return new WaitForSeconds(frequency);

      prefab1[i].MaxCount = i;
    }

    Assert.AreEqual(expected: 0, actual: Pools.PoolFor("PoolSamplePrefab").Count);

    PoolPrefabScriptSample[] prefab2 = new PoolPrefabScriptSample[21];

    for (int i = 0; i < 21; i++) {
      prefab2[i] = Pools.Get<PoolPrefabScriptSample>();
      yield return new WaitForSeconds(frequency);

      prefab2[i].MaxCount = 100 + i;
    }

    Assert.AreEqual(expected: 0, actual: Pools.PoolFor("PoolSamplePrefab").Count);
    Assert.AreEqual(expected: 0, actual: Pools.PoolFor("PoolPrefabScriptSample").Count);

    for (int i = 0; i < 7; i++) {
      prefab1[i].gameObject.SetActive(false);
      yield return new WaitForSeconds(frequency);
    }

    for (int i = 10; i < 19; i++) {
      prefab2[i].gameObject.SetActive(false);
      yield return new WaitForSeconds(frequency);
    }

    Assert.AreEqual(expected: 7, actual: Pools.PoolFor("PoolSamplePrefab").Count);
    Assert.AreEqual(expected: 8, actual: Pools.PoolFor("PoolPrefabScriptSample").Count);

    Assert.IsNotNull(Pools.PoolFor("Scene GameObject"));

    GameObject[] scenes = new GameObject[21];

    for (int i = 0; i < 21; i++) {
      scenes[i] = Pools.Get("Scene GameObject");
      yield return new WaitForSeconds(frequency);
    }

    Assert.AreEqual(expected: 0, actual: Pools.PoolFor("Scene GameObject").Count);

    for (int i = 15; i < 19; i++) {
      scenes[i].gameObject.SetActive(false);
      yield return new WaitForSeconds(frequency);
    }

    Assert.AreEqual(expected: 4, actual: Pools.PoolFor("Scene GameObject").Count);
  }
}