// Decompiled with JetBrains decompiler
// Type: DuckGame.PineTreeSnowTileset
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks|snow")]
  public class PineTreeSnowTileset : PineTree
  {
    private SpriteMap _snowFall;
    private bool didChange;
    private float snowWait = 1f;

    public PineTreeSnowTileset(float x, float y)
      : base(x, y, "pineTilesetSnow")
    {
      this._editorName = "Pine Snow";
      this.physicsMaterial = PhysicsMaterial.Wood;
      this.verticalWidth = 14f;
      this.verticalWidthThick = 15f;
      this.horizontalHeight = 8f;
      this._tileset = "pineTileset";
      this.depth = (Depth) -0.55f;
      this._snowFall = new SpriteMap("snowFall", 8, 24);
      this._snowFall.AddAnimation("fall", (float) (0.200000002980232 + (double) Rando.Float(0.1f)), false, 0, 1, 2, 3, 4);
      this._snowFall.AddAnimation("idle", 0.4f, false, new int[1]);
      this._snowFall.SetAnimation("idle");
      this._snowFall.center = new Vec2(4f, 0.0f);
      this.snowWait = Rando.Float(4f);
    }

    public override void KnockOffSnow(Vec2 dir, bool vertShake)
    {
      this.iterated = true;
      if (!this.knocked || vertShake)
      {
        bool knocked = this.knocked;
        this.knocked = true;
        PineTree pineTree1 = (PineTree) Level.CheckPoint<PineTreeSnowTileset>(this.x - 8f, this.y, (Thing) this);
        PineTree pineTree2 = (PineTree) Level.CheckPoint<PineTreeSnowTileset>(this.x + 8f, this.y, (Thing) this);
        if (pineTree1 != null && !pineTree1.iterated && (!pineTree1.knocked || vertShake))
          pineTree1.KnockOffSnow(dir, vertShake);
        if (pineTree2 != null && !pineTree2.iterated && (!pineTree2.knocked || vertShake))
          pineTree2.KnockOffSnow(dir, vertShake);
        if (!knocked)
        {
          for (int index = 0; index < 2; ++index)
            Level.Add((Thing) new SnowFallParticle(this.x + Rando.Float(-4f, 4f), this.y + Rando.Float(-4f, 4f), dir * Rando.Float(1f) + new Vec2(Rando.Float(-0.1f, -0.1f), Rando.Float(-0.1f, -0.1f) - Rando.Float(0.1f, 0.3f))));
        }
      }
      if (this._snowFall.currentAnimation == "idle")
        this._snowFall.SetAnimation("fall");
      if (vertShake)
        this._vertPush = 0.5f;
      this.knocked = true;
      this.iterated = false;
    }

    public override void Update()
    {
      if (!this.edge && !this.didChange)
      {
        this.snowWait -= 0.01f;
        if ((double) this.snowWait <= 0.0)
        {
          this.snowWait = Rando.Float(2f, 3f);
          if ((double) Rando.Float(1f) > 0.920000016689301)
            Level.Add((Thing) new SnowFallParticle(this.x + Rando.Float(-4f, 4f), this.y + Rando.Float(-4f, 4f), new Vec2(0.0f, 0.0f)));
        }
      }
      base.Update();
    }

    public override void Draw()
    {
      if (!this.edge && this._snowFall.currentAnimation != "idle" && !this._snowFall.finished)
      {
        this._snowFall.depth = (Depth) -0.1f;
        this._snowFall.scale = new Vec2(1f, (float) ((double) this._snowFall.frame / 5.0 * 0.400000005960464 + 0.200000002980232));
        this._snowFall.alpha = (float) (1.0 - (double) this._snowFall.frame / 5.0 * 1.0);
        Graphics.Draw((Sprite) this._snowFall, this.x, (float) ((double) this.y - 7.0 + (double) this._snowFall.frame / 5.0 * 3.0));
      }
      if (this._snowFall.currentAnimation != "idle" && (this.edge || this._snowFall.frame == 1 && !this.didChange))
      {
        this.didChange = true;
        this._sprite = new SpriteMap("pineTileset", 8, 16);
        this._sprite.frame = (this.graphic as SpriteMap).frame;
        this.graphic = (Sprite) this._sprite;
      }
      base.Draw();
    }
  }
}
