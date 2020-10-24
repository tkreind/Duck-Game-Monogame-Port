// Decompiled with JetBrains decompiler
// Type: DuckGame.TutorialSign01
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("details|tutorial")]
  public class TutorialSign01 : TutorialSign
  {
    public TutorialSign01(float xpos, float ypos)
      : base(xpos, ypos, "tutorial/groundPound", "Ground Pound")
    {
    }

    public override void Draw()
    {
      Graphics.DrawString("@JUMP@", new Vec2(this.x - 24f, this.y + 36f), Color.White * 0.5f);
      Graphics.DrawString("@DOWN@", new Vec2(this.x + 25f, this.y - 1f), Color.White * 0.5f);
      this.graphic.color = Color.White * 0.5f;
      base.Draw();
    }
  }
}
