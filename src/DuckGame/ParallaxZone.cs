// Decompiled with JetBrains decompiler
// Type: DuckGame.ParallaxZone
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class ParallaxZone
  {
    public float distance;
    public float speed;
    public float scroll;
    public bool moving;
    public bool visible = true;
    private List<Sprite> _sprites = new List<Sprite>();
    private List<Thing> _things = new List<Thing>();
    private int _ypos;

    public ParallaxZone(float d, float s, bool m, bool vis = true, int ypos = 0)
    {
      this.distance = d;
      this.speed = s;
      this.moving = m;
      this.visible = vis;
      this._ypos = 0;
    }

    public void Update(float mul)
    {
      if (this.moving)
        mul = 1f;
      this.scroll += (1f - this.distance) * this.speed * mul;
    }

    public void RenderSprites(Vec2 position)
    {
      float num = (float) (0.400000005960464 + (double) this._ypos * 0.00999999977648258);
      foreach (Sprite sprite1 in this._sprites)
      {
        Sprite sprite2 = sprite1;
        sprite2.position = sprite2.position + position;
        sprite1.position.x += this.scroll;
        if ((double) sprite1.position.x < -100.0)
          sprite1.position.x += 400f;
        if ((double) sprite1.position.x > 350.0)
          sprite1.position.x -= 400f;
        sprite1.depth = (Depth) num;
        Graphics.Draw(sprite1, sprite1.x, sprite1.y);
        num += 1f / 1000f;
        sprite1.position.x -= this.scroll;
        Sprite sprite3 = sprite1;
        sprite3.position = sprite3.position - position;
      }
      foreach (Thing thing1 in this._things)
      {
        Thing thing2 = thing1;
        thing2.position = thing2.position + position;
        thing1.position.x += this.scroll;
        if ((double) thing1.position.x < -100.0)
          thing1.position.x += 400f;
        if ((double) thing1.position.x > 350.0)
          thing1.position.x -= 400f;
        thing1.depth = (Depth) num;
        thing1.Update();
        thing1.Draw();
        thing1.position.x -= this.scroll;
        Thing thing3 = thing1;
        thing3.position = thing3.position - position;
      }
    }

    public void AddSprite(Sprite s) => this._sprites.Add(s);

    public void AddThing(Thing s) => this._things.Add(s);
  }
}
