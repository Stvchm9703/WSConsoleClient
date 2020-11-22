using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using WSConsoleClient;
namespace WSConsoleClient.Test {
  [TestClass]
  public class ParserTest {
    [TestMethod]
    public void TestParser() {
      var expected = Parser.ToMessage(MessageType.INFO, new {
        worker = "me"
      });
      var actual = "[info] {\"worker\":\"me\"}";
      Console.WriteLine(expected);
      Assert.AreEqual(expected, actual);
    }

  }

}