// Decompiled with JetBrains decompiler
// Type: DuckGame.TutorialGunJump
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("details|tutorial")]
  public class TutorialGunJump : TutorialSign
  {
    public TutorialGunJump(float xpos, float ypos)
      : base(xpos, ypos, "tutorial/gunjump", "Gun Jump")
    {
    }

    public override void Draw()
    {
      Color color = new Color((int) sbyte.MaxValue, (int) sbyte.MaxValue, (int) sbyte.MaxValue);
      Graphics.DrawString("@SHOOT@", new Vec2(this.x - 16f, this.y + 8f), Color.White * 0.5f);
      Graphics.DrawString("@JUMP@", new Vec2(this.x - 39f, this.y + 8f), Color.White * 0.5f);
      this.depth = (Depth) 0.99f;
      this.graphic.color = color;
      base.Draw();
    }
  }
}
