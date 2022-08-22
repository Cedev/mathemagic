using System;
using System.Collections;
using System.Collections.Generic;

namespace Mathemagic.Internal;

// Free<IEnumerable<>>

public struct FreeEnumerable<T> : IEnumerable<T> {
  public bool IsPure;
  public T Pure;
  public IEnumerable<FreeEnumerable<T>> Free;

  IEnumerator IEnumerable.GetEnumerator() {
    return GetEnumerator();
  }

  public IEnumerator<T> GetEnumerator() {
    if (IsPure) {
      return FreeEnumerableExtensions.One(Pure).GetEnumerator();
    } else {
      return Free.Iterate().GetEnumerator();
    }
  }

  public static implicit operator FreeEnumerable<T>(T value) => new FreeEnumerable<T> {Pure = value, IsPure = true};

  public static implicit operator FreeEnumerable<T>(An<IEnumerable<FreeEnumerable<T>>> free) => new FreeEnumerable<T> {Free = free.Value};
}

// The identity functor for conversion operators
// because user-defined conversions to or from an interface are not allowed
public struct An<I> {
  public I Value;
}

public static class FreeEnumerableExtensions {

  public static An<I> Itself<I>(this I value) {
    return new An<I>() {Value = value};
  }

  public static An<I> AsAn<I>(this I value) {
    return new An<I>() {Value = value};
  }


  public static IEnumerable<T> One<T>(T value) {
    return new T[] {value};
  }
 
  public static IEnumerable<T> Iterate<T>(this IEnumerable<FreeEnumerable<T>> enumerable) {
    var stack = new Stack<IEnumerator<FreeEnumerable<T>>>();
    stack.Push(enumerable.GetEnumerator());

    while (stack.Any()) {
      var enumerator = stack.Pop();
      while (enumerator.MoveNext()) {
        if (enumerator.Current.IsPure) {
          yield return enumerator.Current.Pure;
        } else {
          var free = enumerator.Current.Free;
          stack.Push(enumerator);
          enumerator = free.GetEnumerator();
        }
      }
    }
  }
}

