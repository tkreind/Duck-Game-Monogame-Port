// Decompiled with JetBrains decompiler
// Type: DuckGame.BackgroundIndustrial
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("background")]
  public class BackgroundIndustrial : BackgroundTile
  {
    public BackgroundIndustrial(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("industrialBackground", 16, 16);
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this._editorName = "Industrial";
    }
  }
}
