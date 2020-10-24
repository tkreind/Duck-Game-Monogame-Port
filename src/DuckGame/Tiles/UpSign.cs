// Decompiled with JetBrains decompiler
// Type: DuckGame.UpSign
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("details")]
  public class UpSign : Thing
  {
    private SpriteMap _sprite;

    public UpSign(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("upSign", 32, 32);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(16f, 24f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = (Depth) -0.5f;
      this._editorName = "Up Sign";
      this.hugWalls = WallHug.Floor;
    }

    public override void Draw()
    {
      this._sprite.frame = this.offDir > (sbyte) 0 ? 0 : 1;
      base.Draw();
    }
  }
}
