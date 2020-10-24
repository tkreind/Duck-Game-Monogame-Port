// Decompiled with JetBrains decompiler
// Type: DuckGame.WindowFrame
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class WindowFrame : Thing
  {
    public float high;
    private bool floor;

    public WindowFrame(float xpos, float ypos, bool f)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("windowFrame");
      this.center = new Vec2(5f, 26f);
      this.depth = new Depth(-0.95f);
      this._editorCanModify = false;
      this.floor = f;
      if (!this.floor)
        return;
      this.graphic.angleDegrees = -90f;
    }

    public override void Draw()
    {
      this.graphic.depth = this.depth;
      if (this.floor)
      {
        Graphics.Draw(this.graphic, this.x + 14f, this.y + 5f, new Rectangle(0.0f, (float) (this.graphic.height - 2), (float) this.graphic.width, 2f));
        Graphics.Draw(this.graphic, this.x + 14f - this.high, this.y + 5f, new Rectangle(0.0f, 0.0f, (float) this.graphic.width, 3f));
      }
      else
      {
        Graphics.Draw(this.graphic, this.x - 5f, this.y + 6f, new Rectangle(0.0f, (float) (this.graphic.height - 2), (float) this.graphic.width, 2f));
        Graphics.Draw(this.graphic, this.x - 5f, this.y + 6f - this.high, new Rectangle(0.0f, 0.0f, (float) this.graphic.width, 3f));
      }
    }
  }
}
