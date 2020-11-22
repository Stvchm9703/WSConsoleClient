using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Websocket.Client;
using WSConsoleClient;

namespace WSConsoleClient.ExampleNDryRunner {
  class Program {
    static void Main(string[] args) {

      ManualResetEvent ExitEvent = new ManualResetEvent(false);
      Console.WriteLine("Hello World!");
      // basic object parser
      var testOject = new {
        worker = "me"
      };
      var testString = Parser.ToMessage(MessageType.INFO, testOject);
      Console.WriteLine(testString);

      // Exception parser 
      var testExceptString = Parser.ToMessage(MessageType.INFO, new Exception("A Exception Occur"));

      // try the real one
      // try {
      //   var testCli = new WSConnecter("ws://127.0.0.1:9002");
      //   testCli.Info("test message in string");
      //   testCli.Info(new { message = "test message in string" });
      //   testCli.Error(new Exception("A Exception Occur"));
      //   testCli.Disconect();
      // } catch (Exception e) {
      //   Console.WriteLine(e);
      // }
      var url = "ws://127.0.0.1:9002";

      try {
        using(var ws_client = new WebsocketClient(new Uri(url))) {

          ws_client.ReconnectTimeout = TimeSpan.FromSeconds(30);
          ws_client.Start().Wait();
          Task.Run(() => {
            ws_client.Send(
              Parser.ToMessage(MessageType.INFO, "incomes single msg")
            );
            ExitEvent.WaitOne();
          });
          Task.Run(() => {
            ws_client.Send(
              Parser.ToMessage(MessageType.INFO, testString)
            );
            ExitEvent.WaitOne();
          });
          Task.Run(() => {
            ws_client.Send(
              Parser.ToMessage(MessageType.INFO, testExceptString)
            );
            ExitEvent.WaitOne();
          });
          ExitEvent.WaitOne();
          ws_client.Dispose();
        }
        ExitEvent.Dispose();
      } catch (Exception excep) {
        Console.WriteLine(excep);
      }
    }
  }
}