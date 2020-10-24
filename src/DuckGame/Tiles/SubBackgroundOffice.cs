// Decompiled with JetBrains decompiler
// Type: DuckGame.SubBackgroundOffice
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class SubBackgroundOffice : SubBackgroundTile
  {
    public SubBackgroundOffice(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("officeSubBackground", 32, 32, true);
      this._opacityFromGraphic = true;
      this.center = new Vec2(24f, 16f);
      this.collisionSize = new Vec2(32f, 32f);
      this.collisionOffset = new Vec2(-16f, -16f);
    }
  }
}
