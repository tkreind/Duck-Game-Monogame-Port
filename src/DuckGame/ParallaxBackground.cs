// Decompiled with JetBrains decompiler
// Type: DuckGame.ParallaxBackground
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace DuckGame
{
  public class ParallaxBackground : Thing
  {
    private List<int> _indexes = new List<int>();
    public float FUCKINGYOFFSET;
    public Color color = Color.White;
    private Sprite _sprite;
    private Dictionary<int, ParallaxZone> _zones = new Dictionary<int, ParallaxZone>();
    private int _hRepeat = 1;
    public float xmove;
    public Rectangle scissor;
    public bool restrictBottom = true;

    public ParallaxBackground(string image, float vx, float vdepth, int hRepeat = 1)
      : base()
    {
      this._sprite = new Sprite(image);
      this.graphic = this._sprite;
      this.x = vx;
      this.depth = (Depth) vdepth;
      this.layer = Layer.Parallax;
      this._hRepeat = hRepeat;
      this._opaque = true;
      for (int index = 0; index < 256; ++index)
        this._indexes.Add((int) Thing.GetGlobalIndex());
    }

    public ParallaxBackground(Texture2D t)
      : base()
    {
      this._sprite = new Sprite((Tex2D) t);
      this.graphic = this._sprite;
      this.x = 0.0f;
      this.depth = (Depth) 0.0f;
      this.layer = Layer.Parallax;
      this._hRepeat = 3;
      this._opaque = true;
      for (int index = 0; index < 256; ++index)
        this._indexes.Add((int) Thing.GetGlobalIndex());
    }

    public void AddZone(int yPos, float distance, float speed, bool moving = false, bool vis = true) => this._zones[yPos] = new ParallaxZone(distance, speed, moving, vis);

    public void AddZoneSprite(Sprite s, int yPos, float distance, float speed, bool moving = false)
    {
      if (!this._zones.ContainsKey(yPos))
        this._zones[yPos] = new ParallaxZone(distance, speed, moving, false);
      this._zones[yPos].AddSprite(s);
    }

    public void AddZoneThing(Thing s, int yPos, float distance, float speed, bool moving = false)
    {
      this._zones[yPos] = new ParallaxZone(distance, speed, moving, false);
      this._zones[yPos].AddThing(s);
    }

    public override void Initialize()
    {
    }

    public override void Update()
    {
      foreach (KeyValuePair<int, ParallaxZone> zone in this._zones)
        zone.Value.Update(this.xmove);
    }

    public override void Draw()
    {
      if ((double) this.scissor.width != 0.0)
        this.layer.scissor = this.scissor;
      if ((double) this.position.y > 0.0)
        this.position.y = 0.0f;
      if (this.restrictBottom && (double) this.position.y + (double) this._sprite.texture.height < (double) Layer.Parallax.camera.bottom)
        this.position.y = Layer.Parallax.camera.bottom - (float) this._sprite.texture.height;
      for (int index = 0; index < this._hRepeat; ++index)
      {
        for (int key = 0; key < this.graphic.height / 8; ++key)
        {
          if (this._zones.ContainsKey(key))
          {
            ParallaxZone zone = this._zones[key];
            if (index == 0)
              zone.RenderSprites(this.position);
            if (zone.visible)
            {
              float num = zone.scroll % (float) this.graphic.width;
              this._sprite.texture.currentObjectIndex = this._indexes[index + key * this._hRepeat];
              DuckGame.Graphics.Draw(this._sprite.texture, this.position + new Vec2(0.0f, this.FUCKINGYOFFSET) + new Vec2((num - (float) this.graphic.width + (float) (index * this.graphic.width)) * this.scale.x, (float) (key * 8) * this.scale.y), new Rectangle?(new Rectangle(0.0f, (float) (key * 8), (float) this.graphic.width, 8f)), this.color, 0.0f, new Vec2(), new Vec2(this.scale.x, this.scale.y), SpriteEffects.None, this.depth);
            }
          }
        }
      }
    }
  }
}
