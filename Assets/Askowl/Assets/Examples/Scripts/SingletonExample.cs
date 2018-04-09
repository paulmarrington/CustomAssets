#if UNITY_EDITOR
using System.Collections;
using UnityEngine.Assertions;
using UnityEngine.TestTools;

public sealed class SingletonExample {
  [UnityTest]
  public IEnumerator SingletonTestEnumerator() {
    for (int i = 0; i < 10; i++) {
      SingletonSample sample = SingletonSample.Instance;
      Assert.AreEqual(expected: i, actual: sample.value);
      sample.value += 1;
      yield return null;
    }
  }
}
#endif