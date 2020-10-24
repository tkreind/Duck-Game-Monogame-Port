// Decompiled with JetBrains decompiler
// Type: DuckGame.PyramidBLight
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  [EditorGroup("details|pyramid")]
  [BaggedProperty("isInDemo", true)]
  public class PyramidBLight : Thing
  {
    private SpriteThing _shade;
    private List<LightOccluder> _occluders = new List<LightOccluder>();
    private SpriteMap _sprite;

    public PyramidBLight(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("pyramidBackgroundLight", 14, 12);
      this._sprite.AddAnimation("go", 0.2f, true, 0, 1, 2, 3, 4);
      this._sprite.SetAnimation("go");
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(7f, 8f);
      this._collisionSize = new Vec2(8f, 8f);
      this._collisionOffset = new Vec2(-4f, -4f);
      this.depth = (Depth) -0.9f;
      this.alpha = 0.7f;
      this.layer = Layer.Game;
      this.placementLayerOverride = Layer.Blocks;
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      Level.Add((Thing) new PointLight(this.x, this.y - 1f, new Color(byte.MaxValue, byte.MaxValue, byte.MaxValue), 100f, strangeFalloff: true));
    }
  }
}
