// Decompiled with JetBrains decompiler
// Type: DuckGame.TutorialSign05
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("details|tutorial")]
  public class TutorialSign05 : TutorialSign
  {
    public TutorialSign05(float xpos, float ypos)
      : base(xpos, ypos, "tutorial/fly", "Fly")
    {
    }

    public override void Draw()
    {
      Color color = new Color((int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue);
      Graphics.DrawString("@JUMP@", new Vec2(this.x - 26f, this.y + 32f), Color.White * 0.5f);
      Graphics.DrawString("@JUMP@", new Vec2(this.x - 5f, this.y - 16f), Color.White * 0.5f);
      Graphics.DrawString("@JUMP@", new Vec2(this.x + 15f, this.y - 8f), Color.White * 0.5f);
      this.depth = new Depth(0.99f);
      this.graphic.color = color;
      base.Draw();
    }
  }
}
