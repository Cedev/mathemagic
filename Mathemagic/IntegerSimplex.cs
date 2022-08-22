using Mathemagic.Internal;

namespace Mathemagic.Integers;


// An integer simplex with vertices on the axis
public class IntegerSimplex {

  public IReadOnlyList<int> Vertices {get; private set;}

  private int[] basis;
  private int leastCommonBasis;

  
  public IntegerSimplex(IReadOnlyList<int> vertices) {
    Vertices = vertices;
    leastCommonBasis = vertices.LCM();
    basis = vertices.Select(x => leastCommonBasis/x).ToArray();
  }

  // Returns a negative number if the vertex is on the origin side
  // a positive number if it's on the other side
  // and 0 if it's on the simplex
  public int Compare(IReadOnlyList<int> vertex) {
    var distance = 0;
    for(int i = 0; i < basis.Length; i++) {
      distance += basis[i] * vertex[i];
    }
    return distance - leastCommonBasis;    
  }

  public bool Contains(IReadOnlyList<int> vertex) {
    return Compare(vertex) == 0;
  }

  // Visit every vertex less than or on the simplex
  public IEnumerable<R> Iterate<T,R>(T zero, Func<int, T, T> step, Func<T, bool, R> result) {
    return Iterate(zero, 0, 0, step, result).Iterate();
  }

  private IEnumerable<FreeEnumerable<R>> Iterate<T, R>( T start, int startDistance, int index, Func<int, T, T> step, Func<T, bool, R> result) {
    var distance = startDistance;
    var absBasis = Math.Abs(basis[index]);
    var point = start;
    while (distance <= leastCommonBasis) {
      if (index == basis.Length - 1) {
        yield return result(point, distance == leastCommonBasis);
      } else {
        yield return Iterate(point, distance, index+1, step, result).Itself();
      }
      // Go to the next point;
      point = step(index, point);
      distance += absBasis;
    }
  }

  public void Iterate<T>(T zero, Func<int, T, T> step, Action<T, bool> action) {
    Iterate(zero, 0, 0, step, action);
  }

  private void Iterate<T>(T start, int startDistance, int index, Func<int, T, T> step, Action<T, bool> action) {
    var distance = startDistance;
    var absBasis = Math.Abs(basis[index]);
    var point = start;
    while (distance <= leastCommonBasis) {
      if (index == basis.Length - 1) {
        action(point, distance == leastCommonBasis);
      } else {
        Iterate(point, distance, index+1, step, action);
      }
      // Go to the next point;
      point = step(index, point);
      distance += absBasis;
    }
  }

  public override string ToString() {
    return "IntegerSimplex [" + string.Join("][", Vertices) + "]";
  }
}