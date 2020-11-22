using System;
using System.Diagnostics;
using Newtonsoft.Json;

namespace WSConsoleClient {
  public enum MessageType {
    DEBUG = 0, INFO = 1, WARN = 2, ERROR = 3, TRACE = 4,
  }

  public static class Parser {
    public static string GetTypeString(MessageType pt) {
      return (Enum.GetName(typeof(MessageType), pt)).ToLower();
    }
    public static string ToMessage(MessageType type, object targetObject) {
      return $"[{GetTypeString(type)}] { JsonConvert.SerializeObject(targetObject)}";
    }

    public static string ToMessage(MessageType type = MessageType.INFO, Exception except = null) {
      var info = new StackTrace(except);
      var return_str = "";
      return_str += JsonConvert.SerializeObject(except) + "\n";
      foreach (var frm in info.GetFrames()) {
        return_str += "\t\t" +
          frm.ToString() + "\n";
      }
      return $"[{GetTypeString(type)}] {return_str}";
    }
  }
}