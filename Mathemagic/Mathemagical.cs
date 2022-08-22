using Mathemagic.Internal;
using System.Collections.Immutable;

namespace Mathemagic;

public class Mathemagical
{
  public static Mathemagical Zero = new Mathemagical();
  public static Mathemagical One = new Mathemagical(1);
  public static Mathemagical NegativeOne = new Mathemagical(-1);

  public double Value {get; private set;}

  public bool IsZero {get; private set;}

  public bool IsConstant {get; private set;}

  public IReadOnlyDictionary<Variable, Lazy<Mathemagical>> Derivatives{get; private set;}

  public Mathemagical this[Variable i] {
    get {
      Lazy<Mathemagical> derivative = null;
      if (Derivatives.TryGetValue(i, out derivative)) {
        return derivative.Value;
      }
      return Zero;
    }
  }

  public Mathemagical() : this(0) {
  }

  public Mathemagical(double constant) {
    if (constant == 0) {
      IsZero = true;
    }
    IsConstant = true;
    Value = constant;
    Derivatives = ImmutableDictionary<Variable, Lazy<Mathemagical>>.Empty;
  }

  public Mathemagical(Variable variable, double value) {
    this.Value = value;
    this.Derivatives = new Dictionary<Variable, Lazy<Mathemagical>>() {
      {variable, Create.Lazy(() => One)}
    };
  }

  public Mathemagical(double value, IReadOnlyDictionary<Variable, Lazy<Mathemagical>> derivatives) {
    this.Value = value;
    this.Derivatives = derivatives;
  }
  
  public Mathemagical(double value, Func<Mathemagical, IReadOnlyDictionary<Variable, Lazy<Mathemagical>>> derivatives) {
    this.Value = value;
    this.Derivatives = derivatives(this);
  }

  public static Mathemagical Scalar(double value) {
    if (value == 0) {
      return Zero;
    }
    if (value == 1) {
      return One;
    }
    if (value == -1) {
      return NegativeOne;
    }
    return new Mathemagical(value);
  }

  public static implicit operator Mathemagical(double value) => Scalar(value);

  public static Mathemagical Lift(Mathemagical a, Mathemagical b, double value, Func<Mathemagical, Mathemagical, Mathemagical> derivative) {
      return new Mathemagical(value, (
        from variable in a.Derivatives.Keys.Union(b.Derivatives.Keys)
        select KeyValuePair.Create(variable, Create.Lazy(() => derivative(a[variable], b[variable])))
      ).ToDictionary());
  }
  
  public static Mathemagical Lift(Mathemagical a, double value, Func<Mathemagical, Mathemagical> derivative) {
      return new Mathemagical(value, (
        from variable in a.Derivatives.Keys
        select KeyValuePair.Create(variable, Create.Lazy(() => derivative(a[variable])))
      ).ToDictionary());
  }
  
  public static Mathemagical Chain(Mathemagical a, double value, Func<Mathemagical, Mathemagical> derivative) {
      return new Mathemagical(value, (
        from variable in a.Derivatives.Keys
        select KeyValuePair.Create(variable, Create.Lazy(() => a[variable] * derivative(a)))
      ).ToDictionary());
  }

  public static Mathemagical operator+ (Mathemagical a) {
    return a;
  }

  public static Mathemagical operator- (Mathemagical a) {
    if (a.IsZero) {
      return Zero;
    }
    return Chain(a, -a.Value, _ => NegativeOne);
  }

  public static Mathemagical operator+ (Mathemagical a, Mathemagical b) {
    if (a.IsZero) {
      return b;
    }
    if (b.IsZero) {
      return a;
    }
    return Lift(a, b, a.Value + b.Value, (da, db) => da + db);
  }

  public static Mathemagical operator- (Mathemagical a, Mathemagical b) {
    if (a.IsZero) {
      return -b;
    }
    if (b.IsZero) {
      return a;
    }
    return Lift(a, b, a.Value - b.Value, (da, db) => da - db);
  }
  
  public static Mathemagical operator* (Mathemagical a, Mathemagical b) {
    if (a.IsZero || b.IsZero) {
      return Zero;
    }
    return Lift(a, b, a.Value * b.Value, (da, db) => a*db + b*da);
  }

  public static Mathemagical operator/ (Mathemagical a, Mathemagical b) {
    Console.WriteLine(string.Format("divide {0}/{1}", a.Value, b.Value));
    if (a.IsZero && !b.IsZero) {
      return Zero;
    }
    double value;
    if (a.Value == 0 && b.Value == 0) {
      value = lHôpital.Divide(a, b);
    } else {
      value = a.Value / b.Value;
    }
    return Lift(a, b, value, (da, db) => (b*da - a*db)/(b*b) );
  }

  public static (Variable, Mathemagical) Variable(double value) {
    var variable = new Variable();
    return (variable, new Mathemagical(variable, value));
  }
}
