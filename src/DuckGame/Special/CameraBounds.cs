// Decompiled with JetBrains decompiler
// Type: DuckGame.CameraBounds
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("special")]
  public class CameraBounds : Thing
  {
    public EditorProperty<int> wide;
    public EditorProperty<int> high;

    public CameraBounds()
      : base()
    {
      this.graphic = new Sprite("swirl");
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this._canFlip = false;
      this._visibleInGame = false;
      this.wide = new EditorProperty<int>(320, (Thing) this, 60f, 1920f, 1f);
      this.high = new EditorProperty<int>(320, (Thing) this, 60f, 1920f, 1f);
    }

    public override void Draw()
    {
      base.Draw();
      if (!(Level.current is Editor))
        return;
      float num1 = (float) this.wide.value;
      float num2 = (float) this.high.value;
      Graphics.DrawRect(this.position + new Vec2((float) (-(double) num1 / 2.0), (float) (-(double) num2 / 2.0)), this.position + new Vec2(num1 / 2f, num2 / 2f), Color.Blue * 0.5f, new Depth(1f), false);
    }
  }
}
