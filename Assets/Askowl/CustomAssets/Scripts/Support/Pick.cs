using JetBrains.Annotations;

namespace Askowl {
  /// <summary>
  /// Interface so that code can use a picker without know more about the source. A picker returns a value using source specific rules.
  /// </summary>
  /// <typeparam name="T">Type of result returned by the picker.</typeparam>
  /// <remarks><a href="http://customasset.marrington.net#pick">More...</a></remarks>
  public interface Pick<out T> {
    /// <summary>
    /// Method to call to return the selection
    /// </summary>
    [UsedImplicitly]
    T Pick();
  }
}