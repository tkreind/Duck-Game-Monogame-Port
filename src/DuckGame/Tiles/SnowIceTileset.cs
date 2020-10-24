// Decompiled with JetBrains decompiler
// Type: DuckGame.SnowIceTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks|snow")]
  [BaggedProperty("isInDemo", false)]
  public class SnowIceTileset : AutoBlock
  {
    protected string meltedTileset = "snowTileset";
    protected string frozenTileset = "snowIceTileset";
    private float melt;
    private bool melted;

    public SnowIceTileset(float x, float y, string tset = "snowIceTileset")
      : base(x, y, tset)
    {
      this._editorName = "Snow Ice";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidthThick = 15f;
      this.verticalWidth = 14f;
      this.horizontalHeight = 15f;
      this._impactThreshold = -1f;
      this.willHeat = true;
      this._tileset = "snowTileset";
      this.frozenTileset = tset;
    }

    public void Freeze()
    {
      if (Network.isActive)
        return;
      this.melted = false;
      this._sprite = new SpriteMap(this.frozenTileset, 16, 16);
      this._sprite.frame = (this.graphic as SpriteMap).frame;
      this.graphic = (Sprite) this._sprite;
      this.melt = 0.0f;
    }

    public override void HeatUp(Vec2 location)
    {
      if (!Network.isActive)
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

    public override void OnSolidImpact(MaterialThing with, ImpactedFrom from)
    {
      if (!this.melted && with is PhysicsObject)
      {
        (with as PhysicsObject).specialFrictionMod = 0.16f;
        (with as PhysicsObject).modFric = true;
      }
      base.OnSolidImpact(with, from);
    }
  }
}
