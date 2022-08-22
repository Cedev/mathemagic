using NUnit.Framework;
using Mathemagic;

namespace Mathemagic.Tests;


[TestFixture]
public class IndeterminateFormTests {
  [Test]
  public void XoverXTest() {
    var (ix, x) = Mathemagical.Variable(0);
    var one = x/x;
    Assert.That(one.Value, Is.EqualTo(1));
  }

  
  [Test]
  public void SimpleQuadratic() {
    var (ix, x) = Mathemagical.Variable(3);
    var six = (x*x-9)/(x - 3);
    Assert.That(six.Value, Is.EqualTo(6));
  }

  
  [Test]
  public void HopelessSqrtTest() {
    var (ix, x) = Mathemagical.Variable(0);
    var y = (3 - (x*x + 9).Sqrt())/x.Pow(2);

    Assert.That(y.Value, Is.EqualTo(-1/6.0).Within(1).Ulps);
  }

  [Test]
  public void HopefullSqrtTest() {
    var (ix, x) = Mathemagical.Variable(0);
    var conj = 3 + (x*x + 9).Sqrt();
    var y = (3 - (x*x + 9).Sqrt())*conj/(x.Pow(2)*conj);

    Assert.That(y.Value, Is.EqualTo(-1/6.0).Within(1).Ulps);
  }
  
  [Test]
  public void ReciprocalSincTest() {
    var (ix, x) = Mathemagical.Variable(0);

    var form = x/x.Sin();

    Assert.That(form.Value, Is.EqualTo(1));

  }

  [Test]
  [Ignore("TODO Requires comparing lambdas to see if the limit exists")]
  public void RSumCosTest() {
    var (ix, x) = Mathemagical.Variable(0);
    var (iy, y) = Mathemagical.Variable(0);

    var form = (x*x + y*y)/(x.Cos() - y.Cos());

    Assert.That(form.Value, Is.NaN);

  }

  [Test]
  [Ignore("Requires a change of coordinates")]
  public void NoChanceTest() {
    var (ix, x) = Mathemagical.Variable(0);
    var (iy, y) = Mathemagical.Variable(0);

    var completelyFucked = x.Pow(5)*y/(x.Pow(6) + x*x*y*y + y.Pow(6));
    
    Assert.That(completelyFucked.Value, Is.Not.NaN);
  }

  [Test]
  [Ignore("TODO requires propogating NaNs")]
  public void DegenerateArcTest() {
    var (ix, x) = Mathemagical.Variable(0);
    var l = 10.0;
    var t = 0.3;
    var r = l/x.Sin();
    TestContext.Out.WriteLine(r[ix].Value);
    Assert.That(r[ix].Value, Is.Not.NaN);
    TestContext.Out.WriteLine(r[ix][ix].Value);
    TestContext.Out.WriteLine(r[ix][ix][ix].Value);

    var xt = r*(t*x).Sin();

    Assert.That(xt.Value, Is.EqualTo(t*l).Within(1).Ulps);
  }

  [Test]
  public void DegenerateArcArtfulTest() {
    var (ix, x) = Mathemagical.Variable(0);
    var l = 10.0;
    var t = 0.3;

    var xt = l*(t*x).Sin()/x.Sin();

    Assert.That(xt.Value, Is.EqualTo(t*l).Within(1).Ulps);
  }
}