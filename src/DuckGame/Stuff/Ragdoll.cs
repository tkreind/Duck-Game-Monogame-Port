// Decompiled with JetBrains decompiler
// Type: DuckGame.Ragdoll
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  [BaggedProperty("canSpawn", false)]
  public class Ragdoll : Thing
  {
    public StateBinding _positionBinding = (StateBinding) new InterpolatedVec2Binding("position");
    public StateBinding _part1Binding = new StateBinding(nameof (part1));
    public StateBinding _part2Binding = new StateBinding(nameof (part2));
    public StateBinding _part3Binding = new StateBinding(nameof (part3));
    public StateBinding _physicsStateBinding = (StateBinding) new StateFlagBinding(new string[4]
    {
      nameof (solid),
      nameof (enablePhysics),
      nameof (active),
      nameof (visible)
    });
    public StateBinding _makeActiveBinding = new StateBinding(nameof (_makeActive));
    private bool _zekeBear;
    public RagdollPart _part1;
    public RagdollPart _part2;
    public RagdollPart _part3;
    private Duck _theDuck;
    protected Thing _zapper;
    private float _zap;
    public DuckPersona persona;
    public bool _slide;
    public float _timeSinceNudge;
    public float partSep = 6f;
    public int npi;
    private bool _didSmoke;
    public bool jetting;
    private bool _wasZapping;
    public bool _makeActive;

    public Thing holdingOwner
    {
      get
      {
        if (this._part1 != null && this._part1.owner != null)
          return this._part1.owner;
        if (this._part2 != null && this._part2.owner != null)
          return this._part2.owner;
        return this._part3 != null && this._part3.owner != null ? this._part3.owner : (Thing) null;
      }
    }

    public void MakeZekeBear()
    {
      if (this._part1 != null)
        this._part1.MakeZekeBear();
      if (this._part2 != null)
        this._part2.MakeZekeBear();
      if (this._part3 != null)
        this._part3.MakeZekeBear();
      this._zekeBear = true;
    }

    public RagdollPart part1
    {
      get => this._part1;
      set
      {
        this._part1 = value;
        if (this._part1 == null)
          return;
        this._part1.doll = this;
        this._part1.part = 0;
      }
    }

    public RagdollPart part2
    {
      get => this._part2;
      set
      {
        this._part2 = value;
        if (this._part2 == null)
          return;
        this._part2.doll = this;
        this._part2.part = 2;
      }
    }

    public RagdollPart part3
    {
      get => this._part3;
      set
      {
        this._part3 = value;
        if (this._part3 == null)
          return;
        this._part3.doll = this;
        this._part3.part = 1;
      }
    }

    public Duck _duck
    {
      get => this._theDuck;
      set => this._theDuck = value;
    }

    public void Zap(Thing zapper)
    {
      this._zapper = zapper;
      this._zap = 1f;
    }

    public override bool visible
    {
      get => base.visible;
      set
      {
        if (!this.visible && value)
        {
          this._makeActive = false;
          if (this._part1 != null)
          {
            this._part1.owner = (Thing) null;
            this._part1.framesSinceGrounded = (byte) 99;
          }
          if (this._part2 != null)
          {
            this._part2.owner = (Thing) null;
            this._part2.framesSinceGrounded = (byte) 99;
          }
          if (this._part3 != null)
          {
            this._part3.owner = (Thing) null;
            this._part3.framesSinceGrounded = (byte) 99;
          }
        }
        base.visible = value;
        if (this._part1 != null)
          this._part1.visible = value;
        if (this._part2 != null)
          this._part2.visible = value;
        if (this._part3 == null)
          return;
        this._part3.visible = value;
      }
    }

    public override bool active
    {
      get => base.active;
      set
      {
        base.active = value;
        if (this._part1 != null)
          this._part1.active = value;
        if (this._part2 != null)
          this._part2.active = value;
        if (this._part3 == null)
          return;
        this._part3.active = value;
      }
    }

    public override bool enablePhysics
    {
      get => base.enablePhysics;
      set
      {
        base.enablePhysics = value;
        if (this._part1 != null)
          this._part1.enablePhysics = value;
        if (this._part2 != null)
          this._part2.enablePhysics = value;
        if (this._part3 == null)
          return;
        this._part3.enablePhysics = value;
      }
    }

    public override bool solid
    {
      get => base.solid;
      set
      {
        base.solid = value;
        if (this._part1 != null)
          this._part1.solid = value;
        if (this._part2 != null)
          this._part2.solid = value;
        if (this._part3 == null)
          return;
        this._part3.solid = value;
      }
    }

    public override NetworkConnection connection
    {
      get => base.connection;
      set
      {
        base.connection = value;
        if (this._part1 != null)
          this._part1.connection = value;
        if (this._part2 != null)
          this._part2.connection = value;
        if (this._part3 == null)
          return;
        this._part3.connection = value;
      }
    }

    public override NetIndex8 authority
    {
      get => base.authority;
      set
      {
        base.authority = value;
        if (this._part1 != null)
          this._part1.authority = value;
        if (this._part2 != null)
          this._part2.authority = value;
        if (this._part3 == null)
          return;
        this._part3.authority = value;
      }
    }

    public Duck captureDuck
    {
      get => this._duck;
      set
      {
        this._duck = value;
        if (this._duck == null)
          return;
        if (this._part1 != null)
          this._part1.part = 0;
        if (this._part2 != null)
          this._part2.part = 2;
        if (this._part3 == null)
          return;
        this._part3.part = 1;
      }
    }

    public Ragdoll(
      float xpos,
      float ypos,
      Duck who,
      bool slide,
      float degrees,
      int off,
      Vec2 v,
      DuckPersona p = null)
      : base(xpos, ypos)
    {
      this._duck = who;
      this._slide = slide;
      this.offDir = (sbyte) off;
      this.angleDegrees = degrees;
      this.velocity = v;
      this.persona = p;
    }

    public bool PartHeld() => this._part1 != null && this._part2 != null && this._part3 != null && (this._part1.owner != null || this._part2.owner != null || this._part3.owner != null);

    public Profile PartHeldProfile()
    {
      if (this._part1 == null || this._part2 == null || this._part3 == null)
        return (Profile) null;
      if (this._part1.duck != null)
        return this._part1.duck.profile;
      if (this._part2.duck != null)
        return this._part2.duck.profile;
      return this._part3.duck != null ? this._part3.duck.profile : (Profile) null;
    }

    public void SortOutParts(
      float xpos,
      float ypos,
      Duck who,
      bool slide,
      float degrees,
      int off,
      Vec2 v)
    {
      this._duck = who;
      this._slide = slide;
      this.offDir = (sbyte) off;
      this.angleDegrees = degrees;
      this.velocity = v;
      this._makeActive = false;
      this.RunInit();
    }

    public void Organize()
    {
      Vec2 vec = Maths.AngleToVec(this.angle);
      if (this._part1 == null)
      {
        this._part1 = new RagdollPart(this.x - vec.x * this.partSep, this.y - vec.y * this.partSep, 0, this._duck != null ? this._duck.persona : this.persona, (int) this.offDir, this);
        if (Network.isActive && !GhostManager.inGhostLoop)
        {
          GhostManager.context.MakeGhost((Thing) this._part1);
          this._part1.ghostObject.ForceInitialize();
        }
        this._part2 = new RagdollPart(this.x, this.y, 2, this._duck != null ? this._duck.persona : this.persona, (int) this.offDir, this);
        if (Network.isActive && !GhostManager.inGhostLoop)
        {
          GhostManager.context.MakeGhost((Thing) this._part2);
          this._part2.ghostObject.ForceInitialize();
        }
        this._part3 = new RagdollPart(this.x + vec.x * this.partSep, this.y + vec.y * this.partSep, 1, this._duck != null ? this._duck.persona : this.persona, (int) this.offDir, this);
        if (Network.isActive && !GhostManager.inGhostLoop)
        {
          GhostManager.context.MakeGhost((Thing) this._part3);
          this._part3.ghostObject.ForceInitialize();
        }
        Level.Add((Thing) this._part1);
        Level.Add((Thing) this._part2);
        Level.Add((Thing) this._part3);
      }
      else
      {
        this._part1.SortOutDetails(this.x - vec.x * this.partSep, this.y - vec.y * this.partSep, 0, this._duck != null ? this._duck.persona : this.persona, (int) this.offDir, this);
        this._part2.SortOutDetails(this.x, this.y, 2, this._duck != null ? this._duck.persona : this.persona, (int) this.offDir, this);
        this._part3.SortOutDetails(this.x + vec.x * this.partSep, this.y + vec.y * this.partSep, 1, this._duck != null ? this._duck.persona : this.persona, (int) this.offDir, this);
      }
      this._part1.joint = this._part2;
      this._part3.joint = this._part2;
      this._part1.connect = this._part3;
      this._part3.connect = this._part1;
      this._part1.framesSinceGrounded = (byte) 99;
      this._part2.framesSinceGrounded = (byte) 99;
      this._part3.framesSinceGrounded = (byte) 99;
      if (this._duck == null)
        return;
      this._duck.ReturnItemToWorld((Thing) this._part1);
      this._duck.ReturnItemToWorld((Thing) this._part2);
      this._duck.ReturnItemToWorld((Thing) this._part3);
      this._part3.depth = new Depth(this._duck.depth.value);
      this._part1.depth = this._part3.depth - 1;
    }

    public override void Initialize() => base.Initialize();

    public void RunInit()
    {
      this.Organize();
      if (Network.isActive && !GhostManager.inGhostLoop)
      {
        GhostManager.context.MakeGhost((Thing) this);
        this.ghostObject.ForceInitialize();
      }
      if ((double) Math.Abs(this.hSpeed) < 0.200000002980232)
        this.hSpeed = NetRand.Float(0.3f, 1f) * ((double) NetRand.Float(1f) >= 0.5 ? 1f : -1f);
      float num1 = this._slide ? 1f : 1.05f;
      float num2 = this._slide ? 1f : 0.95f;
      this._part1.hSpeed = this.hSpeed * num1;
      this._part1.vSpeed = this.vSpeed;
      this._part2.hSpeed = this.hSpeed;
      this._part2.vSpeed = this.vSpeed;
      this._part3.hSpeed = this.hSpeed * num2;
      this._part3.vSpeed = this.vSpeed;
      this._part1.enablePhysics = false;
      this._part2.enablePhysics = false;
      this._part3.enablePhysics = false;
      this._part1.Update();
      this._part2.Update();
      this._part3.Update();
      this._part1.enablePhysics = true;
      this._part2.enablePhysics = true;
      this._part3.enablePhysics = true;
      if (this._duck == null || !this._duck.onFire)
        return;
      this._part2.Burn(this._part2.position, this._duck.lastBurnedBy);
    }

    public void Unragdoll()
    {
      bool flag = this._duck.HasEquipment(typeof (FancyShoes));
      this._duck.visible = true;
      if (Network.isActive)
        this._duck.position = this._part2._lastReasonablePosition;
      else
        this._duck.position = this._part2.position;
      if (!flag)
        this._duck.position.y -= 20f;
      this._duck.hSpeed = this._part2.hSpeed;
      this._duck.immobilized = false;
      this._duck.enablePhysics = true;
      this._duck._jumpValid = 0;
      this._makeActive = false;
      this._part2.ReturnItemToWorld((Thing) this._duck);
      if (Network.isActive)
      {
        this.active = false;
        this.visible = false;
        this.owner = (Thing) null;
        if ((double) this.y > -1000.0)
        {
          this.y = -9999f;
          this._part1.y = -9999f;
          this._part2.y = -9999f;
          this._part3.y = -9999f;
        }
        this._duck.Fondle((Thing) this);
      }
      else
        Level.Remove((Thing) this);
      this._duck.ragdoll = (Ragdoll) null;
      if (!flag)
        this._duck.vSpeed = -2f;
      else
        this._duck.vSpeed = this._part2.vSpeed;
    }

    public void Shot(Bullet bullet)
    {
      if (this._duck == null || this._duck.dead)
        return;
      this._duck.position = this._part2.position;
      this._duck.Kill((DestroyType) new DTShot(bullet));
      this._duck.y -= 5000f;
    }

    public void Killed(DestroyType t)
    {
      if (this._duck == null || this._duck.dead || t == null)
        return;
      this._duck.position = this._part2.position;
      this._duck.Destroy(t);
      this._duck.y -= 5000f;
    }

    public void LitOnFire(Thing litBy)
    {
      if (this._duck == null || this._duck.onFire)
        return;
      this._duck.Burn(this.position, litBy);
    }

    public override void Terminate()
    {
      if (this._part1 == null || this._part2 == null || this._part3 == null)
        return;
      Level.Remove((Thing) this._part1);
      Level.Remove((Thing) this._part2);
      Level.Remove((Thing) this._part3);
      if (Level.current is Editor || this._didSmoke || this._duck != null && this._duck.HasEquipment(typeof (FancyShoes)))
        return;
      Level.Add((Thing) SmallSmoke.New(this._part1.x, this._part1.y));
      Level.Add((Thing) SmallSmoke.New(this._part2.x, this._part2.y));
      Level.Add((Thing) SmallSmoke.New(this._part3.x, this._part3.y));
      this._didSmoke = true;
    }

    public void Solve(PhysicsObject body1, PhysicsObject body2, float dist)
    {
      float num1 = dist;
      Vec2 vec2_1 = body2.position - body1.position;
      float num2 = vec2_1.length;
      if ((double) num2 < 9.99999974737875E-05)
        num2 = 0.0001f;
      Vec2 vec2_2 = vec2_1 * (1f / num2);
      Vec2 vec2_3 = new Vec2(body1.hSpeed, body1.vSpeed);
      Vec2 vec2_4 = new Vec2(body2.hSpeed, body2.vSpeed);
      float num3 = Vec2.Dot(vec2_4 - vec2_3, vec2_2);
      float num4 = num2 - num1;
      float num5 = 2.1f;
      float num6 = 2.1f;
      if (body1 == this.part1 && this.jetting)
        num5 = 6f;
      else if (body2 == this.part1 && this.jetting)
        num6 = 6f;
      float num7 = (num3 + num4) / (num5 + num6);
      Vec2 vec2_5 = vec2_2 * num7;
      Vec2 vec2_6 = vec2_3 + vec2_5 * num5;
      Vec2 vec2_7 = vec2_4 - vec2_5 * num6;
      if (body1.owner == null)
      {
        body1.hSpeed = vec2_6.x;
        body1.vSpeed = vec2_6.y;
      }
      if (body2.owner != null)
        return;
      body2.hSpeed = vec2_7.x;
      body2.vSpeed = vec2_7.y;
    }

    public override bool ShouldUpdate() => false;

    public void ProcessInput(InputProfile input)
    {
    }

    public float SpecialSolve(PhysicsObject b1, PhysicsObject b2, float dist)
    {
      Thing thing1 = b1.owner != null ? b1.owner : (Thing) b1;
      Thing thing2 = b2.owner != null ? b2.owner : (Thing) b2;
      float num1 = dist;
      Vec2 vec2_1 = b2.position - b1.position;
      float num2 = vec2_1.length;
      if ((double) num2 < 9.99999974737875E-05)
        num2 = 0.0001f;
      if ((double) num2 < (double) num1)
        return 0.0f;
      Vec2 vec2_2 = vec2_1 * (1f / num2);
      Vec2 vec2_3 = new Vec2(thing1.hSpeed, thing1.vSpeed);
      Vec2 vec2_4 = new Vec2(thing2.hSpeed, thing2.vSpeed);
      float num3 = Vec2.Dot(vec2_4 - vec2_3, vec2_2);
      float num4 = num2 - num1;
      float num5 = 2.5f;
      float num6 = 2.1f;
      if (thing1 is ChainLink && !(thing2 is ChainLink))
      {
        num5 = 10f;
        num6 = 0.0f;
      }
      else if (thing2 is ChainLink && !(thing1 is ChainLink))
      {
        num5 = 0.0f;
        num6 = 10f;
      }
      else if (thing1 is ChainLink && thing2 is ChainLink)
      {
        num5 = 10f;
        num6 = 10f;
      }
      if (thing1 is RagdollPart)
        num5 = !this._zekeBear ? 10f : 4f;
      else if (thing2 is RagdollPart)
        num6 = !this._zekeBear ? 10f : 4f;
      float num7 = (num3 + num4) / (num5 + num6);
      Vec2 vec2_5 = vec2_2 * num7;
      Vec2 vec2_6 = vec2_3 + vec2_5 * num5;
      vec2_4 -= vec2_5 * num6;
      thing1.hSpeed = vec2_6.x;
      thing1.vSpeed = vec2_6.y;
      thing2.hSpeed = vec2_4.x;
      thing2.vSpeed = vec2_4.y;
      if (thing1 is ChainLink && (double) (thing2.position - thing1.position).length > (double) num1 * 12.0)
        thing1.position = this.position;
      if (thing2 is ChainLink && (double) (thing2.position - thing1.position).length > (double) num1 * 12.0)
        thing2.position = this.position;
      return num7;
    }

    public bool makeActive
    {
      get => this._makeActive;
      set => this._makeActive = value;
    }

    public override void Update()
    {
      if (this.removeFromLevel || (double) this.y > (double) Level.activeLevel.lowestPoint + 200.0)
        return;
      this._timeSinceNudge += 0.07f;
      if (this._part1 == null || this._part2 == null || this._part3 == null)
        return;
      if ((double) this._zap > 0.0)
      {
        this._part1.vSpeed += Rando.Float(-1f, 0.5f);
        this._part1.hSpeed += Rando.Float(-0.5f, 0.5f);
        this._part2.vSpeed += Rando.Float(-1f, 0.5f);
        this._part2.hSpeed += Rando.Float(-0.5f, 0.5f);
        this._part3.vSpeed += Rando.Float(-1f, 0.5f);
        this._part3.hSpeed += Rando.Float(-0.5f, 0.5f);
        this._part1.x += (float) Rando.Int(-2, 2);
        this._part1.y += (float) Rando.Int(-2, 2);
        this._part2.x += (float) Rando.Int(-2, 2);
        this._part2.y += (float) Rando.Int(-2, 2);
        this._part3.x += (float) Rando.Int(-2, 2);
        this._part3.y += (float) Rando.Int(-2, 2);
        this._zap -= 0.05f;
        this._wasZapping = true;
      }
      else if (this._wasZapping)
      {
        this._wasZapping = false;
        if (this.captureDuck != null)
        {
          if (this.captureDuck.dead)
          {
            this.captureDuck.Ressurect();
            return;
          }
          this.captureDuck.Kill((DestroyType) new DTElectrocute(this._zapper));
        }
      }
      if (this.captureDuck != null && this.captureDuck.isServerForObject)
      {
        if (this.captureDuck.inputProfile.Pressed("JUMP") && this.captureDuck.HasEquipment(typeof (Jetpack)))
          this.captureDuck.GetEquipment(typeof (Jetpack)).PressAction();
        if (this.captureDuck.inputProfile.Released("JUMP") && this.captureDuck.HasEquipment(typeof (Jetpack)))
          this.captureDuck.GetEquipment(typeof (Jetpack)).ReleaseAction();
      }
      this.partSep = 6f;
      if (this._zekeBear)
        this.partSep = 4f;
      if ((double) (this._part1.position - this._part3.position).length > (double) this.partSep * 5.0)
      {
        if (this._part1.owner != null)
          this._part2.position = this._part3.position = this._part1.position;
        else if (this._part3.owner != null)
          this._part1.position = this._part2.position = this._part3.position;
        else
          this._part1.position = this._part3.position = this._part2.position;
        this._part1.vSpeed = this._part2.vSpeed = this._part3.vSpeed = 0.0f;
        this._part1.hSpeed = this._part2.hSpeed = this._part3.hSpeed = 0.0f;
        this.Solve((PhysicsObject) this._part1, (PhysicsObject) this._part2, this.partSep);
        this.Solve((PhysicsObject) this._part2, (PhysicsObject) this._part3, this.partSep);
        this.Solve((PhysicsObject) this._part1, (PhysicsObject) this._part3, this.partSep * 2f);
      }
      this.Solve((PhysicsObject) this._part1, (PhysicsObject) this._part2, this.partSep);
      this.Solve((PhysicsObject) this._part2, (PhysicsObject) this._part3, this.partSep);
      this.Solve((PhysicsObject) this._part1, (PhysicsObject) this._part3, this.partSep * 2f);
      if (this._part1.owner is Duck && this._part3.owner is Duck)
      {
        double num1 = (double) this.SpecialSolve((PhysicsObject) this._part3, (PhysicsObject) (this._part1.owner as Duck), 16f);
        double num2 = (double) this.SpecialSolve((PhysicsObject) this._part1, (PhysicsObject) (this._part3.owner as Duck), 16f);
      }
      this.position = (this._part1.position + this._part2.position + this._part3.position) / 3f;
      if (this._duck == null || (double) this._zap > 0.0)
        return;
      if (this._duck.eyesClosed)
        this._part1.frame = 20;
      if (this.isServerForObject)
      {
        this.UpdateInput();
        if ((double) this.y > (double) Level.activeLevel.lowestPoint + 100.0 && !this._duck.dead)
        {
          this._duck.Kill((DestroyType) new DTFall());
          ++this._duck.profile.stats.fallDeaths;
        }
        this.jetting = false;
      }
      this._duck.y = this._part2.y - 9999f;
      this._duck.x = this._part2.x;
    }

    public override void Draw()
    {
      if (this.captureDuck == null)
        return;
      this.captureDuck.DrawIcon();
    }

    public bool TryingToControl()
    {
      if (this.captureDuck == null)
        return false;
      return this.captureDuck.inputProfile.Pressed("LEFT") || this.captureDuck.inputProfile.Pressed("RIGHT") || (this.captureDuck.inputProfile.Pressed("UP") || this.captureDuck.inputProfile.Pressed("RAGDOLL")) || this.captureDuck.inputProfile.Pressed("JUMP");
    }

    public void UpdateInput()
    {
      if (!this._duck.dead)
      {
        if (this._duck.HasEquipment(typeof (FancyShoes)) && !this.jetting)
        {
          if (this.captureDuck.inputProfile.Pressed("RIGHT"))
          {
            Vec2 vec2_1 = (this._part1.position - this._part2.position).Rotate(1.570796f, Vec2.Zero);
            RagdollPart part1 = this.part1;
            part1.velocity = part1.velocity + vec2_1 * 0.2f;
            Vec2 vec2_2 = (this._part3.position - this._part2.position).Rotate(1.570796f, Vec2.Zero);
            RagdollPart part3 = this.part3;
            part3.velocity = part3.velocity + vec2_2 * 0.2f;
          }
          else if (this.captureDuck.inputProfile.Pressed("LEFT"))
          {
            Vec2 vec2_1 = (this._part1.position - this._part2.position).Rotate(1.570796f, Vec2.Zero);
            RagdollPart part1 = this.part1;
            part1.velocity = part1.velocity + vec2_1 * -0.2f;
            Vec2 vec2_2 = (this._part3.position - this._part2.position).Rotate(1.570796f, Vec2.Zero);
            RagdollPart part3 = this.part3;
            part3.velocity = part3.velocity + vec2_2 * -0.2f;
          }
        }
        else if ((double) this._timeSinceNudge > 1.0 && !this.jetting)
        {
          if (this.captureDuck.inputProfile.Pressed("LEFT"))
          {
            this._part1.vSpeed += NetRand.Float(-2f, 2f);
            this._part3.vSpeed += NetRand.Float(-2f, 2f);
            this._part2.hSpeed += NetRand.Float(-2f, -1.2f);
            this._part2.vSpeed -= NetRand.Float(1f, 1.5f);
            this._timeSinceNudge = 0.0f;
          }
          else if (this.captureDuck.inputProfile.Pressed("RIGHT"))
          {
            this._part1.vSpeed += NetRand.Float(-2f, 2f);
            this._part3.vSpeed += NetRand.Float(-2f, 2f);
            this._part2.hSpeed += NetRand.Float(1.2f, 2f);
            this._part2.vSpeed -= NetRand.Float(1f, 1.5f);
            this._timeSinceNudge = 0.0f;
          }
          else if (this.captureDuck.inputProfile.Pressed("UP"))
          {
            this._part1.vSpeed += NetRand.Float(-2f, 1f);
            this._part3.vSpeed += NetRand.Float(-2f, 1f);
            this._part2.vSpeed -= NetRand.Float(1.5f, 2f);
            this._timeSinceNudge = 0.0f;
          }
        }
      }
      bool flag = false;
      if (this.captureDuck.HasEquipment(typeof (FancyShoes)) && (double) Math.Abs(this._part1.x - this._part3.x) < 9.0 && (double) this._part1.y < (double) this._part3.y)
        flag = true;
      if (this._duck.dead || !this.captureDuck.inputProfile.Pressed("RAGDOLL") && !this.captureDuck.inputProfile.Pressed("JUMP") || (double) this._part2.totalImpactPower >= 1.0 || this._part1.framesSinceGrounded >= (byte) 5 && this._part2.framesSinceGrounded >= (byte) 5 && (this._part3.framesSinceGrounded >= (byte) 5 && !this._part1.doFloat) && (!this.part2.doFloat && !this._part3.doFloat && !flag) || (this._part1.owner != null || this._part2.owner != null || this._part3.owner != null))
        return;
      this._makeActive = true;
    }

    public void UpdateUnragdolling()
    {
      if (!this.isServerForObject || this.captureDuck == null || !this._makeActive)
        return;
      this.Unragdoll();
    }
  }
}
