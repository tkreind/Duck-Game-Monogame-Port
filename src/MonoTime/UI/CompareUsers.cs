// Decompiled with JetBrains decompiler
// Type: DuckGame.CompareUsers
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class CompareUsers : IComparer<UIInviteUser>
  {
    public int Compare(UIInviteUser h1, UIInviteUser h2)
    {
      if (h1 == h2)
        return 0;
      int num = 0;
      if (h1.inDuckGame)
        num -= 4;
      else if (h1.inGame)
        ++num;
      if (h2.inDuckGame)
        num += 4;
      else if (h2.inGame)
        --num;
      if (h1.inMyLobby)
        --num;
      if (h2.inMyLobby)
        ++num;
      if (h1.triedInvite)
        num -= 8;
      if (h2.triedInvite)
        num += 8;
      if (h1.state == SteamUserState.Online)
        num -= 2;
      if (h2.state == SteamUserState.Online)
        num += 2;
      return num;
    }
  }
}
