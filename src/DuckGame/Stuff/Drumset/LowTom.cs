// Decompiled with JetBrains decompiler
// Type: DuckGame.LowTom
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class LowTom : Drum
  {
    private Sprite _stand;

    public LowTom(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("drumset/largeTom");
      this.center = new Vec2((float) (this.graphic.w / 2), (float) (this.graphic.h / 2));
      this._stand = new Sprite("drumset/lowTomStand");
      this._stand.center = new Vec2((float) (this._stand.w / 2), 0.0f);
      this._sound = "lowTom";
    }

    public override void Draw()
    {
      base.Draw();
      this._stand.depth = this.depth - 1;
      Graphics.Draw(this._stand, this.x, this.y + 3f);
    }
  }
}
