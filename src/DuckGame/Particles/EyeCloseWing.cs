// Decompiled with JetBrains decompiler
// Type: DuckGame.EyeCloseWing
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class EyeCloseWing : Thing
  {
    private SpriteMap _sprite;
    private float _move;
    private int _dir;
    private Duck _closer;

    public EyeCloseWing(float xpos, float ypos, int dir, SpriteMap s, Duck own, Duck closer)
      : base(xpos, ypos)
    {
      this._sprite = s.CloneMap();
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this._dir = dir;
      this.depth = (Depth) 0.9f;
      if (this._dir < 0)
        this.angleDegrees = 70f;
      else
        this.angleDegrees = 120f;
      this.owner = (Thing) own;
      this._closer = closer;
      if (this._dir >= 0)
        return;
      this.x += 14f;
    }

    public override void Update()
    {
      float num = 0.3f;
      this.x += (float) this._dir * num;
      this._move += num;
      if (this._dir < 0)
        this.angleDegrees += 2f;
      else
        this.angleDegrees -= 2f;
      if ((double) this._move > 4.0)
        this._closer.eyesClosed = true;
      if ((double) this._move <= 8.0)
        return;
      Level.Remove((Thing) this);
      (this._owner as Duck).closingEyes = false;
    }

    public override void Draw()
    {
      int frame = this._sprite.frame;
      this._sprite.flipV = this._dir <= 0;
      this._sprite.flipH = false;
      this._sprite.frame = 18;
      base.Draw();
      this._sprite.frame = frame;
    }
  }
}
