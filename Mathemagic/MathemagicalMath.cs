using System;

namespace Mathemagic;

public static class MathemagicalMath {

  public static Mathemagical Sin(this Mathemagical a) {
    return Mathemagical.Chain(a, Math.Sin(a.Value), Cos);
  }

  public static Mathemagical Cos(this Mathemagical a) {
    return Mathemagical.Chain(a, Math.Cos(a.Value), x => -Sin(x));
  }

  public static Mathemagical Pow(this Mathemagical x, Mathemagical y) {
    if (x.IsZero) {
      return Math.Pow(0, y.Value);
    }
    if (y.IsZero) {
      return Mathemagical.One;
    }
    if (y.IsConstant) {
      return Mathemagical.Chain(x, Math.Pow(x.Value, y.Value), x => y.Value*Pow(x, y.Value - 1));
    }
    throw new NotImplementedException("something something logarithm");
  }
  
  public static Mathemagical Sqrt(this Mathemagical x) {
    return x.Pow(0.5);
  }
}