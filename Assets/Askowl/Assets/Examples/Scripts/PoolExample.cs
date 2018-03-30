using System.Collections;
using Askowl;
using JetBrains.Annotations;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using UnityEngine.UI;

public sealed class PoolExample {
  [UnityTest]
  public IEnumerator PoolTextTest() {
    Pool<Text> textPool = Pool<Text>.Instance;
    yield return PoolTest(textPool);
  }

  private static IEnumerator PoolTest<T>([NotNull] Pool<T> pool) where T : Component {
    Assert.IsNotNull(pool);
    T[] instances = new T[21];

    for (int i = 0; i < 10; i++) {
      instances[i] = pool.Get();
      Assert.AreEqual(expected: 10, actual: pool.Depth);
      yield return null;
    }

    for (int i = 10; i < 20; i++) {
      instances[i] = pool.Get();
      Assert.AreEqual(expected: 20, actual: pool.Depth);
      yield return null;
    }

    for (int i = 0; i < 12; i++) {
      pool.Return(clone: instances[i]);
    }

    Assert.AreEqual(expected: 20, actual: pool.Depth);

    for (int i = 0; i < 12; i++) {
      instances[i] = pool.Get();
      Assert.AreEqual(expected: 20, actual: pool.Depth);
      yield return null;
    }

    T oneMore = pool.Get();
    Assert.AreEqual(expected: "PoolSample 20", actual: oneMore.name);
    Assert.AreEqual(expected: 30,              actual: pool.Depth);
  }
}