
namespace Mathemagic.Integers;

public static class MathI {

  public static int GCD(this int a, int b) {
    a = Math.Abs(a);
    b = Math.Abs(b);
    while (b != 0) {
      (a, b) = (b, a % b);
    }
    return a;
  }

  public static int GCD(this IEnumerable<int> xs) {
    return xs.Aggregate(0, GCD);
  }

  
  public static int LCM(this int a, int b) {
    if (a == 0) {
      return Math.Abs(b);
    }
    if (b == 0) {
      return Math.Abs(a);
    }
    return Math.Abs(a)*(Math.Abs(b)/GCD(a,b));
  }

  public static int LCM(this IEnumerable<int> xs) {
    return xs.Aggregate(0, LCM);
  }

}