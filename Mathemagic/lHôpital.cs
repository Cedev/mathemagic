using Mathemagic.Integers;
using System;


namespace Mathemagic;


public static class lHôpital {

  public static double Divide(Mathemagical f, Mathemagical g) {
    // l'Hopital's rule in more than one dimension
    // https://www.tandfonline.com/doi/full/10.1080/00029890.2020.1793635
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
        if (purePartial.IsZero) {
          throw new Exception("g has no non-zero partial derivative in an index");
        }
        if (purePartial.Value != 0) {
          firstNonZeroPartialOfG[variableIndex] = purePartialIndex;
          break;
        }
        purePartialIndex++;
        purePartial = purePartial[variable];
      }
      variableIndex++;
    }

    var simplex = new IntegerSimplex(firstNonZeroPartialOfG);
    Console.WriteLine(simplex);

    var lambdas = new List<double>();

    simplex.Iterate(
      ValueTuple.Create(f,g),
      (i, partials) => ValueTuple.Create(partials.Item1[variables[i]], partials.Item2[variables[i]]),
      (partials, onSimplex) => {
      if (onSimplex) {
        if (partials.Item2.Value == 0) {
          lambdas.Add(0);
        } else {
          var lambda = partials.Item1.Value/partials.Item2.Value;
          lambdas.Add(lambda);
        }
      } else {
        if (partials.Item2.Value != 0) {
          throw new Exception("g has a non-zero mixed partial under the simplex");
        }
      }
    });

    lambdas.Sort();
    Console.WriteLine("Lambda = [" + String.Join(", ", lambdas) + "]");

    // tests about a simplicial polynomial, f partials equaling lambda partials everywhere (all lambdas being the same)
    return lambdas[lambdas.Count/2];
  }
  
}