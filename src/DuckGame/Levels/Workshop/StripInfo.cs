// Decompiled with JetBrains decompiler
// Type: DuckGame.StripInfo
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public struct StripInfo
  {
    public string header;
    public bool large;
    public int cardsVisible;
    public List<Card> cards;

    public StripInfo(bool l)
    {
      this.header = (string) null;
      this.large = l;
      this.cardsVisible = 3;
      this.cards = new List<Card>();
    }
  }
}
