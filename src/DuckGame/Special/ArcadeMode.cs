// Decompiled with JetBrains decompiler
// Type: DuckGame.ArcadeMode
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("special")]
  [BaggedProperty("isOnlineCapable", false)]
  public class ArcadeMode : Thing
  {
    public ArcadeMode()
      : base()
    {
      this.graphic = new Sprite("arcadeIcon");
      this.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(16f, 16f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.depth = (Depth) 0.9f;
      this.layer = Layer.Foreground;
      this._visibleInGame = false;
      this._editorName = "Arcade";
      this._canFlip = false;
      this._canHaveChance = false;
    }

    public override void Initialize()
    {
    }

    public override void Update()
    {
    }
  }
}
