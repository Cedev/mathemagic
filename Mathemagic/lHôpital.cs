using System;

namespace Mathemagic;


public static class lHôpital {

  public static double Divide(Mathemagical f, Mathemagical g) {
    Console.WriteLine("lHôpital divide");

    var variables = f.Derivatives.Keys.Intersect(g.Derivatives.Keys).ToArray();

    if (variables.Length == 0) {
      throw new Exception("f and g share no partial derivatives");
    }

    var firstNonZeroPartialOfG = new int[variables.Length];

    int variableIndex = 0;
    foreach (var variable in variables) {
      int purePartialIndex = 1;
      var purePartial = g[variable];
      while (true) {
        Console.WriteLine(string.Format("g{0}{1} {2}", variableIndex, purePartialIndex, purePartial.Value));
        if (purePartial.IsZero) {
          throw new Exception("g has no non-zero partial derivative in an index");
        }
        if (purePartial.Value != 0) {
          Console.WriteLine(string.Format("g{0}{1} Non-zero {2}", variableIndex, purePartialIndex, purePartial.Value));
          firstNonZeroPartialOfG[variableIndex] = purePartialIndex;
          break;
        }
        purePartialIndex++;
        purePartial = purePartial[variable];
      }
      variableIndex++;
    }

    Console.WriteLine("[" + string.Join("][", firstNonZeroPartialOfG) + "]");

    var someFpartial = f;
    var someGpartial = g;
    for (int somePartialIndex = 0; somePartialIndex < firstNonZeroPartialOfG[0]; somePartialIndex++) {
      someFpartial = someFpartial[variables[0]];
      someGpartial = someGpartial[variables[0]];
      Console.WriteLine(string.Format("f {0} g {1}", someFpartial.Value, someGpartial.Value));
    }
    var lambda = someFpartial.Value/someGpartial.Value;

    var allNonZero = AllPartialsZero(g, 0, variables, firstNonZeroPartialOfG, true, true);
    if (!allNonZero) {
      throw new Exception("g has a non-zero mixed partial under the simplex");
    }
    
    // tests about a simplicial polynomial, f partials equaling lambda partials everywhere

    return lambda;
  }

  public static bool AllPartialsZero(Mathemagical partial, int index, Variable[] variables, int[] simplex, bool first, bool last) {
    if (!(index < variables.Length)) {
      if (first || last) {
        return true;
      }
      if (partial.Value != 0) {
        // Console.WriteLine(new String(' ', 3*index + 1) + "x");
        return false;
      }
      return true;
    }

    var variable = variables[index];
    
    var indexedPartial = partial;
    for (var i=0; i <= simplex[index]; i++) {
      //Console.WriteLine(new String(' ', 3*index + 1) +  i.ToString());
      var atFirst = first && i == 0;
      var atLast = last && i == simplex[index];
      if (!AllPartialsZero(indexedPartial, index+1, variables, simplex, atFirst, atLast)) {
        return false;
      }
      indexedPartial = indexedPartial[variable];
    }

    return true;
  }

  
}