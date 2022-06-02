using CliFx.Infrastructure;

namespace Feeder;

internal static class ConsoleWriterExtensions {
  internal static void WriteOutputVariable(this ConsoleWriter writer, string name, string value) {
    writer.WriteLine("::set-output name={0}::{1}", name, value);
  }

  internal static void WriteOutputVariable(this ConsoleWriter writer, string name, bool value) {
    writer.WriteOutputVariable(name, value.ToString().ToLower());
  }
}
