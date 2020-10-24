﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.SnowDrift
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("details")]
  public class SnowDrift : MaterialThing
  {
    public EditorProperty<int> style;
    public bool kill;
    private bool melt;

    public override void EditorPropertyChanged(object property)
    {
      if ((int) this.style == -1)
        (this.graphic as SpriteMap).frame = Rando.Int(3);
      else
        (this.graphic as SpriteMap).frame = this.style.value;
    }

    public SnowDrift(float xpos, float ypos, int dir)
      : base(xpos, ypos)
    {
      this.style = new EditorProperty<int>(-1, (Thing) this, -1f, 3f, 1f);
      this.graphic = (Sprite) new SpriteMap("drifts", 16, 17);
      if ((int) this.style == -1)
        (this.graphic as SpriteMap).frame = Rando.Int(3);
      this.hugWalls = WallHug.Floor;
      this.center = new Vec2(8f, 14f);
      this.collisionSize = new Vec2(14f, 4f);
      this.collisionOffset = new Vec2(-7f, -2f);
      this.layer = Layer.Blocks;
      this.depth = new Depth(0.5f);
    }

    public override void Update()
    {
      if (this.kill)
      {
        this.alpha -= 0.012f;
        this.yscale -= 0.15f;
        this.xscale += 0.12f;
        this.y += 0.4f;
      }
      if (this.melt)
      {
        this.alpha -= 0.0036f;
        this.yscale -= 0.045f;
        this.xscale += 0.036f;
        this.y += 0.15f;
      }
      if ((double) this.yscale < 0.0)
        Level.Remove((Thing) this);
      base.Update();
    }

    public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
      if (!this.kill && (double) with.impactPowerV > 2.0)
      {
        float num1 = with.impactPowerV;
        float num2 = with.impactDirectionH;
        if ((double) num1 > 6.0)
          num1 = 6f;
        if ((double) num2 > 6.0)
          num2 = 6f;
        for (int index = 0; index < 20; ++index)
        {
          float num3 = 1f;
          if (index < 10)
            num3 = 0.7f;
          Level.Add((Thing) new SnowFallParticle(this.x + Rando.Float(-8f, 8f), this.y + Rando.Float(-6f, 0.0f), new Vec2((float) ((double) num2 * (double) num3 * 0.100000001490116) + Rando.Float((float) (-0.200000002980232 * ((double) num1 * (double) num3)), (float) (0.200000002980232 * ((double) num1 * (double) num3))), (float) (-(double) Rando.Float(0.8f, 1.5f) * ((double) num1 * (double) num3 * 0.150000005960464))), index < 6));
        }
        this.kill = true;
      }
      base.OnSoftImpact(with, from);
    }

    public override void HeatUp(Vec2 location)
    {
      this.melt = true;
      base.HeatUp(location);
    }

    public override void Draw()
    {
      this.graphic.flipH = this.flipHorizontal;
      base.Draw();
    }
  }
}
