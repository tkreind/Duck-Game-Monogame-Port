// Decompiled with JetBrains decompiler
// Type: DuckGame.ArrowSign
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("details")]
  public class ArrowSign : Thing
  {
    private SpriteMap _sprite;

    public ArrowSign(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("directionSign", 32, 32);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(16f, 24f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = new Depth(-0.5f);
      this._editorName = "Arrow Sign";
      this.hugWalls = WallHug.Floor;
    }

    public override void Draw()
    {
      this._sprite.frame = this.offDir > (sbyte) 0 ? 1 : 0;
      base.Draw();
    }
  }
}
