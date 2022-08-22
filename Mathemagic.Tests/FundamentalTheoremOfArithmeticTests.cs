using Mathemagic.Integers;
using NUnit.Framework;

namespace Mathemagic.Tests;

[TestFixture]
public class FundamentalTheoremOfArithmeticTests {

  [TestCase(0,0,0)]
  [TestCase(0,48,48)]
  [TestCase(18,0,18)]
  [TestCase(48,18,6)]
  [TestCase(18,48,6)]
  [TestCase(48,19,1)]
  [TestCase(48,48,48)]
  [TestCase(-48,18,6)]
  [TestCase(48,-18,6)]
  [TestCase(-48,-18,6)]
  public void GCDTest(int a, int b, int gcd) {
    Assert.That(a.GCD(b), Is.EqualTo(gcd));
  }

  [TestCase(0,0,0)]
  [TestCase(0,7,7)]
  [TestCase(0,-16,16)]
  [TestCase(211,0,211)]
  [TestCase(-12,0,12)]
  [TestCase(6,10,30)]
  [TestCase(-6,10,30)]
  [TestCase(6,-10,30)]
  [TestCase(-6,-10,30)]
  [TestCase(8,10,40)]
  [TestCase(11,17,187)]
  public void LCMTest(int a, int b, int lcm) {
    Assert.That(a.LCM(b), Is.EqualTo(lcm));
  }

}