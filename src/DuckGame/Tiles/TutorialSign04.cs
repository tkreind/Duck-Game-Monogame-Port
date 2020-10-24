// Decompiled with JetBrains decompiler
// Type: DuckGame.TutorialSign04
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("details|tutorial")]
  public class TutorialSign04 : TutorialSign
  {
    public TutorialSign04(float xpos, float ypos)
      : base(xpos, ypos, "tutorial/fallThrough", "Fall Through")
    {
    }

    public override void Draw()
    {
      Graphics.DrawString("@JUMP@", new Vec2(this.x + 7f, this.y - 30f), Color.White * 0.5f);
      Graphics.DrawString("@DOWN@", new Vec2(this.x - 18f, this.y - 30f), Color.White * 0.5f);
      this.graphic.color = Color.White * 0.5f;
      base.Draw();
    }
  }
}
