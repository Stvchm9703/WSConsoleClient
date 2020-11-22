using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Websocket.Client;
// using WSConsoleClient.Parser;
namespace WSConsoleClient {
  public class WSConnecter {
    private WebsocketClient ws_client;
    ManualResetEvent ExitEvent = new ManualResetEvent(false);
    List<Task> msg_stack;
    public string ConnectedUrl { get { return this.ws_client.Url.ToString(); } }

    public WSConnecter(string url) {
      try {

        this.ws_client = new WebsocketClient(new Uri(url));

        this.ws_client.ReconnectTimeout = TimeSpan.FromSeconds(30);
        this.ws_client.Start().Wait();

      } catch (Exception e) {
        Console.WriteLine(e);
      }
    }

    #region  control for ws-cli
    public void Connect() {
      if (this.ws_client == null)throw new Exception("No Session Connection");
      try {
        this.ws_client.Start();
      } catch (Exception e) {
        throw e;
      }
    }
    public void Disconect() {
      if (this.ws_client == null)throw new Exception("No Session Connection");
      try {
        this.ws_client.Dispose();
      } catch (Exception e) {
        throw e;
      }
    }

    #endregion

    #region  message session
    public void Info(object incomes) {
      if (this.ws_client == null) {
        throw new Exception("No Session Connection");
      }
      try {
        msg_stack.Add(
          Task.Run(() => this.ws_client.Send(
            Parser.ToMessage(MessageType.INFO, incomes)
          ))
        );

      } catch (Exception e) {
        throw e;
      }
    }

    public void Warn(object incomes) {
      if (this.ws_client == null) {
        throw new Exception("No Session Connection");
      }
      try {
        this.ws_client.Send(
          Parser.ToMessage(MessageType.WARN, incomes)
        );
      } catch (Exception e) {
        throw e;
      }
    }

    /// <summary>
    /// ERROR Message : default 
    ///   default mode, with the exception stack trace
    /// </summary>
    /// <param name="incomes"></param>
    public void Error(object incomes) {
      if (this.ws_client == null) {
        throw new Exception("No Session Connection");
      }
      try {
        if (incomes is Exception) {
          this.ws_client.Send(
            Parser.ToMessage(MessageType.ERROR, (Exception)incomes)
          );
        } else {
          this.ws_client.Send(
            Parser.ToMessage(MessageType.ERROR, incomes)
          );
        }
      } catch (Exception e) {
        throw e;
      }
    }

    public void Debug(object incomes) {
      if (this.ws_client == null) {
        throw new Exception("No Session Connection");
      }
      try {
        if (incomes is Exception) {
          this.ws_client.Send(
            Parser.ToMessage(MessageType.DEBUG, (Exception)incomes)
          );
        } else {
          this.ws_client.Send(
            Parser.ToMessage(MessageType.DEBUG, incomes)
          );
        }
      } catch (Exception e) {
        throw e;
      }
    }

    #endregion
  }
}