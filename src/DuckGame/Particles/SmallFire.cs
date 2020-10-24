// Decompiled with JetBrains decompiler
// Type: DuckGame.SmallFire
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class SmallFire : PhysicsParticle, ITeleport
  {
    private static int kMaxObjects = 128;
    private static SmallFire[] _objects = new SmallFire[SmallFire.kMaxObjects];
    private static int _lastActiveObject = 0;
    public float waitToHurt;
    public Duck whoWait;
    private SpriteMap _sprite;
    private SpriteMap _airFire;
    private float _airFireScale;
    private float _spinSpeed;
    private bool _multiplied;
    private byte _groundLife = 125;
    private Vec2 _stickOffset;
    private MaterialThing _stick;
    private Thing _firedFrom;
    private static bool kAlternate = false;
    private bool _alternate;
    private bool _alternateb;
    public bool doFloat;
    private int _fireID;
    private bool _canMultiply = true;
    private bool didRemove;

    public static SmallFire New(
      float xpos,
      float ypos,
      float hspeed,
      float vspeed,
      bool shortLife = false,
      MaterialThing stick = null,
      bool canMultiply = true,
      Thing firedFrom = null,
      bool network = false)
    {
      SmallFire smallFire;
      if (Network.isActive)
        smallFire = new SmallFire();
      else if (SmallFire._objects[SmallFire._lastActiveObject] == null)
      {
        smallFire = new SmallFire();
        SmallFire._objects[SmallFire._lastActiveObject] = smallFire;
      }
      else
        smallFire = SmallFire._objects[SmallFire._lastActiveObject];
      SmallFire._lastActiveObject = (SmallFire._lastActiveObject + 1) % SmallFire.kMaxObjects;
      if (smallFire != null)
      {
        smallFire.ResetProperties();
        smallFire.Init(xpos, ypos, hspeed, vspeed, shortLife, stick, canMultiply);
        smallFire._sprite.globalIndex = (int) Thing.GetGlobalIndex();
        smallFire._airFire.globalIndex = (int) Thing.GetGlobalIndex();
        smallFire._firedFrom = firedFrom;
        smallFire.needsSynchronization = true;
        smallFire.isLocal = !network;
        if (Network.isActive && !network)
          GhostManager.context.particleManager.AddLocalParticle((PhysicsParticle) smallFire);
        if (float.IsNaN(smallFire.position.x) || float.IsNaN(smallFire.position.y))
        {
          smallFire.position.x = -9999f;
          smallFire.position.y = -9999f;
        }
      }
      return smallFire;
    }

    public override void NetSerialize(BitBuffer b)
    {
      if (this.stick != null && this.stick.ghostObject != null)
      {
        b.Write(true);
        b.Write((ushort) (int) this.stick.ghostObject.ghostObjectIndex);
        b.Write((sbyte) this.stickOffset.x);
        b.Write((sbyte) this.stickOffset.y);
      }
      else
      {
        b.Write(false);
        b.Write((short) this.x);
        b.Write((short) this.y);
      }
    }

    public override void NetDeserialize(BitBuffer d)
    {
      if (d.ReadBool())
      {
        GhostObject ghost = GhostManager.context.GetGhost((NetIndex16) (int) d.ReadUShort());
        if (ghost != null)
          this.stick = ghost.thing as MaterialThing;
        this.stickOffset = new Vec2((float) d.ReadSByte(), (float) d.ReadSByte());
        this.UpdateStick();
        this.hSpeed = 0.0f;
        this.vSpeed = 0.0f;
      }
      else
        this.netLerpPosition = new Vec2((float) d.ReadShort(), (float) d.ReadShort());
    }

    public byte groundLife
    {
      get => this._groundLife;
      set => this._groundLife = value;
    }

    public Vec2 stickOffset
    {
      get => this._stickOffset;
      set => this._stickOffset = value;
    }

    public MaterialThing stick
    {
      get => this._stick;
      set => this._stick = value;
    }

    public Thing firedFrom => this._firedFrom;

    public int fireID => this._fireID;

    private SmallFire()
      : base(0.0f, 0.0f)
    {
      this._bounceEfficiency = 0.2f;
      this._sprite = new SpriteMap("smallFire", 16, 16);
      this._sprite.AddAnimation("burn", (float) (0.200000002980232 + (double) Rando.Float(0.2f)), true, 0, 1, 2, 3, 4);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 14f);
      this._airFire = new SpriteMap("airFire", 16, 16);
      this._airFire.AddAnimation("burn", (float) (0.200000002980232 + (double) Rando.Float(0.2f)), true, 0, 1, 2, 1);
      this._airFire.center = new Vec2(8f, 8f);
      this._collisionSize = new Vec2(12f, 12f);
      this._collisionOffset = new Vec2(-6f, -6f);
    }

    private void Init(
      float xpos,
      float ypos,
      float hspeed,
      float vspeed,
      bool shortLife = false,
      MaterialThing stick = null,
      bool canMultiply = true)
    {
      if ((double) xpos == 0.0 && (double) ypos == 0.0 && stick == null)
      {
        xpos = -9999f;
        ypos = -9999f;
      }
      this.position.x = xpos;
      this.position.y = ypos;
      this._airFireScale = 0.0f;
      this._multiplied = false;
      this._groundLife = (byte) 125;
      this.doFloat = false;
      this.hSpeed = hspeed;
      this.vSpeed = vspeed;
      this._sprite.SetAnimation("burn");
      this._sprite.imageIndex = Rando.Int(4);
      this.xscale = this.yscale = 0.8f + Rando.Float(0.6f);
      this.angleDegrees = Rando.Float(20f) - 10f;
      this._airFire.SetAnimation("burn");
      this._airFire.imageIndex = Rando.Int(2);
      this._airFire.xscale = this._airFire.yscale = 0.0f;
      this._spinSpeed = 0.1f + Rando.Float(0.1f);
      this._airFire.color = Color.Orange * (0.8f + Rando.Float(0.2f));
      this._gravMult = 0.7f;
      this._sticky = 0.6f;
      this._life = 100f;
      if (Network.isActive)
        this._sticky = 0.0f;
      this._fireID = FireManager.GetFireID();
      this.needsSynchronization = true;
      if (shortLife)
        this._groundLife = (byte) 31;
      this.depth = (Depth) 0.6f;
      this._stick = stick;
      this._stickOffset = new Vec2(xpos, ypos);
      this.UpdateStick();
      this._alternate = SmallFire.kAlternate;
      SmallFire.kAlternate = !SmallFire.kAlternate;
      this._canMultiply = canMultiply;
    }

    public void UpdateStick()
    {
      if (this._stick == null)
        return;
      this.position = this._stick.Offset(this._stickOffset);
    }

    public void SuckLife(float l) => this._life -= l;

    public override void Removed()
    {
      if (Network.isActive && !this.didRemove && (this.isLocal && GhostManager.context != null))
      {
        this.didRemove = true;
        GhostManager.context.particleManager.RemoveParticle((PhysicsParticle) this);
      }
      base.Removed();
    }

    public override void Update()
    {
      if ((double) this.waitToHurt > 0.0)
        this.waitToHurt -= Maths.IncFrameTimer();
      else
        this.whoWait = (Duck) null;
      if (!this.isLocal)
      {
        if (this._stick != null)
          this.UpdateStick();
        else
          base.Update();
      }
      else
      {
        if ((double) this._airFireScale < 1.20000004768372)
          this._airFireScale += 0.15f;
        if (this._grounded && this._stick == null)
        {
          this._airFireScale -= 0.3f;
          if ((double) this._airFireScale < 0.899999976158142)
            this._airFireScale = 0.9f;
          this._spinSpeed -= 0.01f;
          if ((double) this._spinSpeed < 0.0500000007450581)
            this._spinSpeed = 0.05f;
        }
        if (this._grounded)
        {
          if (this._groundLife <= (byte) 0)
          {
            this.alpha -= 0.04f;
            if ((double) this.alpha < 0.0)
              Level.Remove((Thing) this);
          }
          else
            --this._groundLife;
        }
        this._airFire.xscale = this._airFire.yscale = this._airFireScale;
        this._airFire.depth = this.depth - 1;
        this._airFire.alpha = 0.5f;
        this._airFire.angle += this.hSpeed * this._spinSpeed;
        if (this.isLocal && this._canMultiply && (!this._multiplied && (double) Rando.Float(310f) < 1.0))
        {
          Level.Add((Thing) SmallFire.New(this.x, this.y, Rando.Float(1f) - 0.5f, (float) -(0.5 + (double) Rando.Float(0.5f))));
          this._multiplied = true;
        }
        if (this._stick == null)
        {
          base.Update();
        }
        else
        {
          this._grounded = true;
          if (this._stick.destroyed)
          {
            this._stick = (MaterialThing) null;
            this._grounded = false;
          }
          else
          {
            this.UpdateStick();
            this.stick.UpdateFirePosition(this);
            if (!this._stick.onFire || this._stick.removeFromLevel || (double) this._stick.alpha < 0.00999999977648258)
            {
              Level.Add((Thing) SmallSmoke.New(this.x, this.y));
              Level.Remove((Thing) this);
            }
          }
        }
        this._alternateb = !this._alternateb;
        if (!this._alternateb)
          return;
        this._alternate = !this._alternate;
      }
    }

    public override void Draw() => base.Draw();
  }
}
