using NUnit.Framework;
using Mathemagic;

namespace Mathemagic.Tests;


[TestFixture]
public class DerivativeTests
{

  [Test]
  public void ZeroTest()  {
    var variable = new Variable();
    Assert.That(Mathemagical.Zero.Value, Is.EqualTo(0));
    Assert.That(Mathemagical.Zero[variable], Is.SameAs(Mathemagical.Zero));
  }
  
  [Test]
  public void ContantTest([Values(-12, -1, 0, 1,  0.00001)] double value)  {
    var variable = new Variable();
    var scalar = Mathemagical.Scalar(value);
    Assert.That(scalar.Value, Is.EqualTo(value));
    Assert.That(scalar[variable], Is.SameAs(Mathemagical.Zero));
  }

  [Test]
  public void VariableTest([Values(-1, 0, 1, 8)] double value) {
    var (variable, x) = Mathemagical.Variable(value);
    Assert.That(x.Value, Is.EqualTo(value));
    Assert.That(x[variable], Is.SameAs(Mathemagical.One));
  }

  [Test]
  public void SumTest() {
    var (ix, x) = Mathemagical.Variable(3);
    var (iy, y) = Mathemagical.Variable(7);
    var sum = 5*x + 4*y;

    Assert.That(sum.Value, Is.EqualTo(3*5+4*7));
    Assert.That(sum[ix].Value, Is.EqualTo(5));
    Assert.That(sum[iy].Value, Is.EqualTo(4));
  }
  
  [Test]
  public void SimpleQuadraticTest() {
    var (ix, x) = Mathemagical.Variable(3);
    var six = (x*x-9)[ix];
    Assert.That(six.Value, Is.EqualTo(6));
  }

  
  [Test]
  public void SimplePowTest() {
    var (ix, x) = Mathemagical.Variable(1);
    var y = x.Pow(3);
    Assert.That(y.Value, Is.EqualTo(1));
    Assert.That(y[ix].Value, Is.EqualTo(3));
    Assert.That(y[ix][ix].Value, Is.EqualTo(6));
    Assert.That(y[ix][ix][ix].Value, Is.EqualTo(6));
    Assert.That(y[ix][ix][ix][ix].Value, Is.EqualTo(0));
    Assert.That(y[ix][ix][ix][ix].IsZero, Is.True);
  }

  [Test]
  public void SqrtTest() {
    var (ix, x) = Mathemagical.Variable(16);
    var y = x.Sqrt()[ix];
    Assert.That(y.Value, Is.EqualTo(0.125));
  }

  
  [Test]
  public void HopefullSqrtNumeratorTest() {
    var (ix, x) = Mathemagical.Variable(0);
    var conj = 3 + (x*x + 9).Sqrt();
    var denominator = x.Pow(2)*conj;    

    Assert.That(denominator.Value, Is.EqualTo(0));
    Assert.That(denominator[ix].Value, Is.EqualTo(0));
    Assert.That(denominator[ix][ix].Value, Is.EqualTo(2*Math.Sqrt(9)+6));
  }

  [Test]
  public void HopefullSqrtSqrtTest() {
    var (ix, x) = Mathemagical.Variable(0);
    var conj = 3 + (x*x + 9).Sqrt();

    Assert.That(conj.Value, Is.EqualTo(6));
    Assert.That(conj[ix].Value, Is.EqualTo(0), "First derivative");
    Assert.That(conj[ix][ix].Value, Is.EqualTo(1/3.0), "Second derivative");
  }

  [Test]
  public void HopelessSqrtQuadraticTest() {
    var (ix, x) = Mathemagical.Variable(0);
    var y = x*x + 9;

    Assert.That(y.Value, Is.EqualTo(9));
    Assert.That(y[ix].Value, Is.EqualTo(0), "First derivative");
    Assert.That(y[ix][ix].Value, Is.EqualTo(2), "Second derivative");
  }

}
