// Decompiled with JetBrains decompiler
// Type: DuckGame.ATGrenade
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class ATGrenade : AmmoType
  {
    public ATGrenade()
    {
      this.accuracy = 1f;
      this.penetration = 0.35f;
      this.bulletSpeed = 9f;
      this.rangeVariation = 0.0f;
      this.speedVariation = 0.0f;
      this.range = 2000f;
      this.rebound = true;
      this.affectedByGravity = true;
      this.deadly = false;
      this.weight = 5f;
      this.bulletThickness = 2f;
      this.bulletColor = Color.White;
      this.bulletType = typeof (GrenadeBullet);
      this.immediatelyDeadly = true;
      this.sprite = new Sprite("launcherGrenade");
      this.sprite.CenterOrigin();
    }

    public override void PopShell(float x, float y, int dir)
    {
      PistolShell pistolShell = new PistolShell(x, y);
      pistolShell.hSpeed = (float) dir * (1.5f + Rando.Float(1f));
      Level.Add((Thing) pistolShell);
    }
  }
}
