// Decompiled with JetBrains decompiler
// Type: DuckGame.FunBeam
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [EditorGroup("stuff")]
  public class FunBeam : MaterialThing
  {
    private SpriteMap _beam;
    private Vec2 _prev = Vec2.Zero;
    private Vec2 _endPoint = Vec2.Zero;

    public FunBeam(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._beam = new SpriteMap("funBeam", 16, 16);
      this._beam.ClearAnimations();
      this._beam.AddAnimation("idle", 1f, true, 0, 1, 2, 3, 4, 5, 6, 7);
      this._beam.SetAnimation("idle");
      this._beam.speed = 0.2f;
      this._beam.alpha = 0.3f;
      this._beam.center = new Vec2(0.0f, 8f);
      this.graphic = new Sprite("funBeamer");
      this.center = new Vec2(9f, 8f);
      this.collisionOffset = new Vec2(-2f, -5f);
      this.collisionSize = new Vec2(4f, 10f);
      this.depth = (Depth) -0.5f;
      this._editorName = "Fun Beam";
      this.hugWalls = WallHug.Left;
    }

    public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
      if (!(with is Gun gun))
        return;
      switch (gun)
      {
        case Sword _:
          break;
        case SledgeHammer _:
          break;
        default:
          gun.PressAction();
          break;
      }
    }

    public override void Draw()
    {
      if (this._prev != this.position)
      {
        this._endPoint = Vec2.Zero;
        for (int index = 0; index < 32; ++index)
        {
          Thing thing = (Thing) Level.CheckLine<Block>(this.position + new Vec2((float) (4 + index * 16), 0.0f), this.position + new Vec2((float) ((index + 1) * 16 - 6), 0.0f));
          if (thing != null)
          {
            this._endPoint = new Vec2(thing.left - 2f, this.y);
            break;
          }
        }
        this._prev = this.position;
      }
      if (this._endPoint != Vec2.Zero)
      {
        this.graphic.flipH = true;
        this.graphic.depth = this.depth;
        Graphics.Draw(this.graphic, this._endPoint.x, this._endPoint.y);
        this.graphic.flipH = false;
        this._beam.depth = this.depth - 2;
        float x = this._endPoint.x - this.x;
        int num = (int) Math.Ceiling((double) x / 16.0);
        for (int index = 0; index < num; ++index)
        {
          this._beam.cutWidth = index != num - 1 ? 0 : 16 - (int) ((double) x % 16.0);
          Graphics.Draw((Sprite) this._beam, this.x + (float) (index * 16), this.y);
        }
        this.collisionOffset = new Vec2(-1f, -4f);
        this.collisionSize = new Vec2(x, 8f);
      }
      else
      {
        this.collisionOffset = new Vec2(-1f, -5f);
        this.collisionSize = new Vec2(4f, 10f);
      }
      base.Draw();
    }
  }
}
