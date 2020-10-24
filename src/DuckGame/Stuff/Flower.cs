// Decompiled with JetBrains decompiler
// Type: DuckGame.Flower
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [EditorGroup("stuff|props")]
  [BaggedProperty("isInDemo", true)]
  public class Flower : Holdable
  {
    public StateBinding _netQuackBinding = (StateBinding) new NetSoundBinding(nameof (_netQuack));
    public NetSoundEffect _netQuack = new NetSoundEffect(new string[1]
    {
      "happyQuack01"
    });
    public bool _picked;

    public Flower(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("flower");
      this.center = new Vec2(8f, 12f);
      this.collisionOffset = new Vec2(-3f, -12f);
      this.collisionSize = new Vec2(6f, 14f);
      this._holdOffset = new Vec2(-2f, 2f);
      this.depth = (Depth) -0.5f;
      this.weight = 1f;
      this.flammable = 0.3f;
      this.hugWalls = WallHug.Floor;
    }

    protected override bool OnDestroy(DestroyType type = null) => false;

    public override void Update()
    {
      if ((double) Math.Abs(this.hSpeed) > 0.200000002980232 || !this._picked && this.owner != null)
        this._picked = true;
      if (this._picked)
      {
        if (this.owner != null)
        {
          this.center = new Vec2(8f, 12f);
          this.collisionOffset = new Vec2(-3f, -12f);
          this.collisionSize = new Vec2(6f, 14f);
          this.angleDegrees = 0.0f;
          this.graphic.flipH = this.offDir < (sbyte) 0;
        }
        else
        {
          this.center = new Vec2(8f, 8f);
          this.collisionOffset = new Vec2(-7f, -5f);
          this.collisionSize = new Vec2(14f, 6f);
          this.angleDegrees = 90f;
          this.graphic.flipH = true;
          this.depth = (Depth) 0.4f;
        }
      }
      base.Update();
    }

    public override void OnPressAction()
    {
      if (Network.isActive)
      {
        if (this.isServerForObject)
          this._netQuack.Play(pit: Rando.Float(-0.1f, 0.1f));
      }
      else
        SFX.Play("happyQuack01", pitch: Rando.Float(-0.1f, 0.1f));
      if (this.owner == null)
        return;
      (this.owner as Duck).quack = 20;
    }

    public override void OnReleaseAction()
    {
      if (this.owner == null)
        return;
      (this.owner as Duck).quack = 0;
    }

    public override void Draw() => base.Draw();
  }
}
