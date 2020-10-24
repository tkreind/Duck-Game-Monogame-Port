// Decompiled with JetBrains decompiler
// Type: DuckGame.CookedDuck
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class CookedDuck : Holdable
  {
    private List<SpriteMap> _flavourLines = new List<SpriteMap>();

    public override bool visible
    {
      get => base.visible;
      set => base.visible = value;
    }

    public CookedDuck(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("cookedDuck");
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-6f, -4f);
      this.collisionSize = new Vec2(12f, 11f);
      this.depth = new Depth(-0.5f);
      this.thickness = 0.5f;
      this.weight = 5f;
      this.collideSounds.Add("rockHitGround2", ImpactedFrom.Bottom);
      this.collideSounds.Add("smallSplatLouder");
      for (int index = 0; index < 3; ++index)
      {
        SpriteMap spriteMap = new SpriteMap("barrelSmoke", 8, 8);
        spriteMap.AddAnimation("idle", 0.12f, true, 3, 4, 5, 6, 7, 8);
        spriteMap.SetAnimation("idle");
        spriteMap.frame = Rando.Int(5);
        spriteMap.center = new Vec2(1f, 8f);
        spriteMap.alpha = 0.5f;
        this._flavourLines.Add(spriteMap);
      }
    }

    protected override bool OnDestroy(DestroyType type = null) => false;

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      if (bullet.isLocal)
      {
        this.OnDestroy((DestroyType) new DTShot(bullet));
        CookedDuck cookedDuck = this;
        cookedDuck.velocity = cookedDuck.velocity + bullet.travelDirNormalized * 0.7f;
        this.vSpeed -= 0.5f;
      }
      SFX.Play("smallSplat", Rando.Float(0.8f, 1f), Rando.Float(-0.2f, 0.2f));
      Level.Add((Thing) new WetEnterEffect(hitPos.x, hitPos.y, -bullet.travelDirNormalized, (Thing) this));
      return base.Hit(bullet, hitPos);
    }

    public override void ExitHit(Bullet bullet, Vec2 exitPos) => Level.Add((Thing) new WetPierceEffect(exitPos.x, exitPos.y, bullet.travelDirNormalized, (Thing) this));

    public override void Update() => base.Update();

    public override void Draw()
    {
      base.Draw();
      for (int index = 0; index < 3; ++index)
      {
        this._flavourLines[index].depth = this.depth;
        Graphics.Draw((Sprite) this._flavourLines[index], this.x - 4f + (float) (index * 4), this.y - 3f);
      }
    }
  }
}
