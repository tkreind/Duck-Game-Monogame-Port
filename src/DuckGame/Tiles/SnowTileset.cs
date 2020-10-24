// Decompiled with JetBrains decompiler
// Type: DuckGame.SnowTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks|snow")]
  [BaggedProperty("isInDemo", false)]
  public class SnowTileset : AutoBlock
  {
    protected string meltedTileset = "snowTileset";
    protected string frozenTileset = "snowIceTileset";
    private float melt;
    private bool melted = true;
    private long lastHitFrame;

    public SnowTileset(float x, float y, string tset = "snowTileset")
      : base(x, y, tset)
    {
      this._editorName = "Snow";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidthThick = 15f;
      this.verticalWidth = 14f;
      this.horizontalHeight = 15f;
      this._tileset = "snowTileset";
      this.willHeat = true;
      this._impactThreshold = -1f;
      this.meltedTileset = tset;
    }

    public override void HeatUp(Vec2 location)
    {
      if (!Network.isActive && !this.melted)
      {
        this.melt += 0.05f;
        if ((double) this.melt > 1.0)
        {
          this.melted = true;
          this._sprite = new SpriteMap(this.meltedTileset, 16, 16);
          this._sprite.frame = (this.graphic as SpriteMap).frame;
          this.graphic = (Sprite) this._sprite;
        }
      }
      base.HeatUp(location);
    }

    public void Freeze()
    {
      if (Network.isActive)
        return;
      this.melted = false;
      this._sprite = new SpriteMap(this.frozenTileset, 16, 16);
      this._sprite.frame = (this.graphic as SpriteMap).frame;
      this.graphic = (Sprite) this._sprite;
      this.DoPositioning();
      this.melt = 0.0f;
    }

    public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
    {
      if (!this.melted)
      {
        if (with is PhysicsObject)
        {
          (with as PhysicsObject).specialFrictionMod = 0.16f;
          (with as PhysicsObject).modFric = true;
        }
      }
      else if (Graphics.frame - this.lastHitFrame > 3L && (double) with.totalImpactPower > 2.5 && (double) with.impactPowerV > 0.5)
      {
        int num = (int) ((double) with.totalImpactPower * 0.5);
        if (num > 6)
          num = 6;
        if (num < 3)
          num = 3;
        switch (from)
        {
          case ImpactedFrom.Left:
            for (int index = 0; index < num; ++index)
              Level.Add((Thing) new SnowFallParticle(this.right - Rando.Float(0.0f, 1f), with.y + Rando.Float(-6f, 6f), new Vec2(Rando.Float(0.3f, 1f), Rando.Float(-0.5f, 0.5f))));
            break;
          case ImpactedFrom.Right:
            for (int index = 0; index < num; ++index)
              Level.Add((Thing) new SnowFallParticle(this.left - Rando.Float(0.0f, 1f), with.y + Rando.Float(-6f, 6f), new Vec2(-Rando.Float(0.3f, 1f), Rando.Float(-0.5f, 0.5f))));
            break;
          case ImpactedFrom.Top:
            for (int index = 0; index < num; ++index)
              Level.Add((Thing) new SnowFallParticle(with.x + Rando.Float(-6f, 6f), this.bottom + Rando.Float(0.0f, 1f), new Vec2(Rando.Float(-0.5f, 0.5f), Rando.Float(0.3f, 1f))));
            break;
          case ImpactedFrom.Bottom:
            for (int index = 0; index < num; ++index)
              Level.Add((Thing) new SnowFallParticle(with.x + Rando.Float(-6f, 6f), this.top - Rando.Float(0.0f, 1f), new Vec2(Rando.Float(-0.5f, 0.5f), -Rando.Float(0.3f, 1f))));
            break;
        }
      }
      this.OnSoftImpact(with, from);
    }
  }
}
