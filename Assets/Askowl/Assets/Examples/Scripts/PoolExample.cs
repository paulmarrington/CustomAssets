using System.Collections;
using Askowl;
using UnityEngine.Assertions;
using UnityEngine.TestTools;
using UnityEngine.UI;

public sealed class PoolExample {
  [UnityTest]
  public IEnumerator PoolTestEnumerator() {
    Pool<Text> pool      = Pool<Text>.Instance;
    Text[]     instances = new Text[21];

    Assert.IsNotNull(pool);

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

    Text oneMore = pool.Get();
    Assert.AreEqual(expected: "PoolSample 20", actual: oneMore.name);
    Assert.AreEqual(expected: 30,              actual: pool.Depth);
  }
}