// Decompiled with JetBrains decompiler
// Type: DuckGame.OggSong
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework;
using System.IO;

namespace DuckGame
{
  public class OggSong
  {
    public static MemoryStream Load(string oggFile, bool localContent = true)
    {
      oggFile = oggFile.TrimStart('/');
      Stream input = !localContent ? (Stream) File.OpenRead(oggFile) : TitleContainer.OpenStream(oggFile);
      MemoryStream memoryStream = new MemoryStream();
      OggSong.CopyStream(input, (Stream) memoryStream);
      input.Close();
      return memoryStream;
    }

    public static void CopyStream(Stream input, Stream output)
    {
      byte[] buffer = new byte[16384];
      int count;
      while ((count = input.Read(buffer, 0, buffer.Length)) > 0)
        output.Write(buffer, 0, count);
    }
  }
}
