// Decompiled with JetBrains decompiler
// Type: DuckGame.NMVersionMismatch
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMVersionMismatch : NMDuckNetwork
  {
    public byte byteCode;
    public string serverVersion;

    public NMVersionMismatch.Type GetCode() => (NMVersionMismatch.Type) this.byteCode;

    public NMVersionMismatch()
    {
    }

    public NMVersionMismatch(NMVersionMismatch.Type code, string ver)
    {
      this.byteCode = (byte) code;
      this.serverVersion = ver;
    }

    public enum Type
    {
      Match = -1, // 0xFFFFFFFF
      Older = 0,
      Newer = 1,
      Error = 2,
    }
  }
}
