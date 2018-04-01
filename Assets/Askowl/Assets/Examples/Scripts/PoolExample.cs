using System.Collections;
using Askowl;
using UnityEngine;
using UnityEngine.Assertions;

public sealed class PoolExample : MonoBehaviour {
  private void  Awake() { StartCoroutine(PoolTest()); }
  private float frequency = 0.01f;

  IEnumerator PoolTest() {
    yield return new WaitForSeconds(0.5f);

    Assert.IsNotNull(Pool.PoolFor("PoolSamplePrefab"));
    Assert.IsNotNull(Pool.PoolFor("PoolPrefabScriptSample"));

    PoolPrefabScriptSample[] prefab1 = new PoolPrefabScriptSample[21];
    Assert.AreEqual(0, Pool.PoolFor("PoolSamplePrefab").Count);

    for (int i = 0; i < 21; i++) {
      prefab1[i] =
        Pool.Acquire<PoolPrefabScriptSample>("PoolSamplePrefab",
                                             parent: FindObjectOfType<Canvas>().transform);

      yield return new WaitForSeconds(frequency);

      prefab1[i].MaxCount = i;
    }

    Assert.AreEqual(expected: 0, actual: Pool.PoolFor("PoolSamplePrefab").Count);

    PoolPrefabScriptSample[] prefab2 = new PoolPrefabScriptSample[21];

    for (int i = 0; i < 21; i++) {
      prefab2[i] = Pool.Acquire<PoolPrefabScriptSample>();
      yield return new WaitForSeconds(frequency);

      prefab2[i].MaxCount = 100 + i;
    }

    Assert.AreEqual(expected: 0, actual: Pool.PoolFor("PoolSamplePrefab").Count);
    Assert.AreEqual(expected: 0, actual: Pool.PoolFor("PoolPrefabScriptSample").Count);

    for (int i = 0; i < 7; i++) {
      prefab1[i].gameObject.SetActive(false);
      yield return new WaitForSeconds(frequency);
    }

    for (int i = 10; i < 19; i++) {
      prefab2[i].gameObject.SetActive(false);
      yield return new WaitForSeconds(frequency);
    }

    Assert.AreEqual(expected: 7, actual: Pool.PoolFor("PoolSamplePrefab").Count);
    Assert.AreEqual(expected: 9, actual: Pool.PoolFor("PoolPrefabScriptSample").Count);

    Assert.IsNotNull(Pool.PoolFor("Scene GameObject"));

    GameObject[] scenes = new GameObject[21];

    for (int i = 0; i < 21; i++) {
      scenes[i] = Pool.Acquire("Scene GameObject");
      yield return new WaitForSeconds(frequency);
    }

    Assert.AreEqual(expected: 0, actual: Pool.PoolFor("Scene GameObject").Count);

    for (int i = 15; i < 19; i++) {
      scenes[i].gameObject.SetActive(false);
      yield return new WaitForSeconds(frequency);
    }

    Assert.AreEqual(expected: 4, actual: Pool.PoolFor("Scene GameObject").Count);
  }
}