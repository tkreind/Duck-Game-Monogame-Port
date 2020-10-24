// Decompiled with JetBrains decompiler
// Type: DuckGame.UnlockableDevtimes
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Linq;

namespace DuckGame
{
  public class UnlockableDevtimes : Unlockable
  {
    private DuckPersona _persona;

    public UnlockableDevtimes(string identifier, Func<bool> condition, string nam, string desc)
      : base(identifier, condition, nam, desc)
    {
      this._persona = Persona.all.ElementAt<DuckPersona>(Rando.Int(3));
      this._showScreen = true;
    }

    public override void Initialize()
    {
    }

    protected override void Unlock()
    {
    }

    protected override void Lock()
    {
    }

    public override void Draw(float x, float y, Depth depth)
    {
    }
  }
}
