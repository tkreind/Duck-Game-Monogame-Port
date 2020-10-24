// Decompiled with JetBrains decompiler
// Type: DuckGame.TutorialSign02
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("details|tutorial")]
  public class TutorialSign02 : TutorialSign
  {
    public TutorialSign02(float xpos, float ypos)
      : base(xpos, ypos, "tutorial/slideUnder", "Slide Under")
    {
    }

    public override void Draw()
    {
      this.graphic.color = Color.White * 0.5f;
      base.Draw();
    }
  }
}
