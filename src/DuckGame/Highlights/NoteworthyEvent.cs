// Decompiled with JetBrains decompiler
// Type: DuckGame.NoteworthyEvent
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class NoteworthyEvent
  {
    public static string GoodKillDeathRatio = nameof (GoodKillDeathRatio);
    public static string BadKillDeathRatio = nameof (BadKillDeathRatio);
    public static string ManyFallDeaths = nameof (ManyFallDeaths);
    public string eventTag;
    public Profile who;
    public float quality;

    public NoteworthyEvent(string tag, Profile owner, float q)
    {
      this.eventTag = tag;
      this.who = owner;
      this.quality = q;
    }
  }
}
