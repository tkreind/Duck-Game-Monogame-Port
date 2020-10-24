// Decompiled with JetBrains decompiler
// Type: DuckGame.ChatMessage
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ChatMessage
  {
    public Profile who;
    public string text;
    public ushort index;
    public float timeout = 10f;
    public float alpha = 1f;
    public float slide;
    public float scale = 1f;

    public ChatMessage(Profile w, string t, ushort idx)
    {
      this.who = w;
      this.text = t;
      this.index = idx;
    }
  }
}
