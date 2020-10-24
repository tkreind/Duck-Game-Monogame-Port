// Decompiled with JetBrains decompiler
// Type: DuckGame.UnlockableHats
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class UnlockableHats : Unlockable
  {
    private List<Team> _teams;
    private DuckPersona _persona;

    public UnlockableHats(
      string identifier,
      List<Team> t,
      Func<bool> condition,
      string nam,
      string desc,
      string achieve = "")
      : base(identifier, condition, nam, desc, achieve)
    {
      this._teams = t;
      this._showScreen = true;
      this._persona = Persona.all.ElementAt<DuckPersona>(Rando.Int(3));
    }

    public override void Initialize()
    {
      foreach (Team team in this._teams)
      {
        if (team != null)
          team.locked = true;
      }
    }

    protected override void Unlock()
    {
      foreach (Team team in this._teams)
      {
        if (team != null)
          team.locked = false;
      }
    }

    protected override void Lock()
    {
      foreach (Team team in this._teams)
      {
        if (team != null)
          team.locked = true;
      }
    }

    public override void Draw(float x, float y, Depth depth)
    {
      y -= 9f;
      float num1 = 0.0f;
      if (this._teams.Count == 3)
        num1 = 18f;
      int num2 = 0;
      foreach (Team team in this._teams)
      {
        if (team != null)
        {
          float num3 = x;
          float y1 = y + 8f;
          this._persona.sprite.depth = depth;
          this._persona.sprite.color = Color.White;
          Graphics.Draw(this._persona.sprite, 0, num3 - num1 + (float) (num2 * 18), y1);
          this._persona.armSprite.frame = this._persona.sprite.imageIndex;
          this._persona.armSprite.scale = new Vec2(1f, 1f);
          this._persona.armSprite.depth = depth + 4;
          Graphics.Draw((Sprite) this._persona.armSprite, (float) ((double) num3 - (double) num1 + (double) (num2 * 18) - 3.0), y1 + 6f);
          Vec2 hatPoint = DuckRig.GetHatPoint(this._persona.sprite.imageIndex);
          team.hat.depth = depth + 2;
          team.hat.center = new Vec2(16f, 16f) + team.hatOffset;
          Graphics.Draw(team.hat, team.hat.frame, num3 - num1 + (float) (num2 * 18) + hatPoint.x, y1 + hatPoint.y);
        }
        ++num2;
      }
    }
  }
}
