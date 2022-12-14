using System.Text;

namespace Hot.Core.IO;

public sealed class VoidTextWriter : TextWriter
{
  public override void Write(char value) { }

  public override Encoding Encoding
  {
    get { return Encoding.Default; }
  }
}
