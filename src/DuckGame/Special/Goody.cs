// Decompiled with JetBrains decompiler
// Type: DuckGame.Goody
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public abstract class Goody : MaterialThing, ISequenceItem
  {
    public string collectSound = "goody";
    public bool hidden;

    public Goody(float xpos, float ypos, Sprite sprite)
      : base(xpos, ypos)
    {
      this.graphic = sprite;
      this.center = new Vec2((float) (sprite.w / 2), (float) (sprite.h / 2));
      this._collisionSize = new Vec2(10f, 10f);
      this.collisionOffset = new Vec2(-5f, -5f);
      this.sequence = new SequenceItem((Thing) this);
      this.sequence.type = SequenceItemType.Goody;
      this.enablePhysics = false;
    }

    public override void Initialize()
    {
      if (!(Level.current is Editor) && this.sequence.waitTillOrder && this.sequence.order != 0)
      {
        this.visible = false;
        this.hidden = true;
      }
      base.Initialize();
    }

    public override void OnSequenceActivate()
    {
      if (this._visibleInGame)
        this.visible = true;
      this.hidden = false;
      base.OnSequenceActivate();
    }

    public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
      if (this.hidden)
        return;
      switch (with)
      {
        case Duck _:
        case RagdollPart _:
        case TrappedDuck _:
          if (with.destroyed)
            break;
          this.visible = false;
          this.hidden = true;
          if (this.collectSound != null && this.collectSound != "")
            SFX.Play(this.collectSound, 0.8f);
          if (Level.current is Editor)
            break;
          this._sequence.Finished();
          if (!ChallengeLevel.running || !this.sequence.isValid)
            break;
          ++ChallengeLevel.goodiesGot;
          break;
      }
    }
  }
}
