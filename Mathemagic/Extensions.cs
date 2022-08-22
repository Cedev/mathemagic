namespace Mathemagic.Internal;

public static class Create {
  public static Lazy<T> Lazy<T>(Func<T> initializer) {
    return new Lazy<T>(initializer);
  }

  public static Dictionary<K, V> ToDictionary<K, V>(this IEnumerable<KeyValuePair<K,V>> values) {
    return new Dictionary<K, V>(values);
  }
  
}