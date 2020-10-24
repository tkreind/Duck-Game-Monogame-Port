// Decompiled with JetBrains decompiler
// Type: DuckGame.NMChatMessage
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NMChatMessage : NMDuckNetwork
  {
    public byte profileIndex;
    public ushort index;
    public string text = "";

    public NMChatMessage()
    {
    }

    public NMChatMessage(byte who, string t, ushort idx)
    {
      this.profileIndex = who;
      this.text = t;
      this.index = idx;
    }
  }
}
