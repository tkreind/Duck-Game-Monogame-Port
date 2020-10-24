// Decompiled with JetBrains decompiler
// Type: DuckGame.FeatherVolume
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class FeatherVolume : MaterialThing
  {
    private Duck _duckOwner;

    public Duck duckOwner => this._duckOwner;

    public FeatherVolume(Duck duckOwner)
      : base(0.0f, 0.0f)
    {
      this.thickness = 0.1f;
      this._duckOwner = duckOwner;
      this._editorCanModify = false;
      this.ignoreCollisions = true;
      this.visible = false;
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      Gun owner = bullet.owner as Gun;
      if (bullet.owner != null && (bullet.owner == this._duckOwner || owner != null && owner.owner == this._duckOwner))
        return false;
      Feather feather = Feather.New(0.0f, 0.0f, this._duckOwner.persona);
      feather.hSpeed = (float) (-(double) bullet.travelDirNormalized.x * (1.0 + (double) Rando.Float(1f)));
      feather.vSpeed = -Rando.Float(2f);
      feather.position = hitPos;
      Level.Add((Thing) feather);
      return false;
    }

    public override void ExitHit(Bullet bullet, Vec2 exitPos)
    {
      Gun owner = bullet.owner as Gun;
      if (bullet.owner != null && (bullet.owner == this._duckOwner || owner != null && owner.owner == this._duckOwner))
        return;
      Feather feather = Feather.New(0.0f, 0.0f, this._duckOwner.persona);
      feather.hSpeed = (float) (-(double) bullet.travelDirNormalized.x * (1.0 + (double) Rando.Float(1f)));
      feather.vSpeed = -Rando.Float(2f);
      feather.position = exitPos;
      Level.Add((Thing) feather);
    }
  }
}
