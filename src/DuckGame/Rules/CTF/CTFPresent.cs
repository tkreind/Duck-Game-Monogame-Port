// Decompiled with JetBrains decompiler
// Type: DuckGame.CTFPresent
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class CTFPresent : Present
  {
    private SpriteMap _sprite;

    public CTFPresent(float xpos, float ypos, bool team)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("ctf/present", 18, 17);
      this._sprite.frame = team ? 0 : 1;
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(9f, 8f);
      this.collisionOffset = new Vec2(-9f, -6f);
      this.collisionSize = new Vec2(18f, 14f);
      this.weight = 7f;
      this.flammable = 0.8f;
    }

    public override void OnPressAction()
    {
      if (this.duck == null || this.duck.ctfTeamIndex == this._sprite.frame)
        return;
      base.OnPressAction();
    }

    protected override bool OnDestroy(DestroyType type = null)
    {
      if (type is DTIncinerate)
      {
        Level.Remove((Thing) this);
        Level.Add((Thing) SmallSmoke.New(this.x, this.y));
      }
      return false;
    }
  }
}
