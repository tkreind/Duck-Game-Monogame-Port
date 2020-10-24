// Decompiled with JetBrains decompiler
// Type: DuckGame.Duck
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class Duck : PhysicsObject, IReflect, ITakeInput, IAmADuck
  {
    public const int BackpackDepth = -15;
    public const int BackWingDepth = -10;
    public const int ClothingDepth = 2;
    public const int HoldingDepth = 8;
    public const int WingDepth = 10;
    public StateBinding _profileIndexBinding = new StateBinding(GhostPriority.Normal, nameof (netProfileIndex), 2);
    public StateBinding _disarmIndexBinding = new StateBinding(GhostPriority.Normal, nameof (disarmIndex), 3);
    public StateBinding _animationIndexBinding = new StateBinding(GhostPriority.Normal, nameof (netAnimationIndex), 4);
    public StateBinding _holdObjectBinding = new StateBinding(GhostPriority.High, nameof (holdObject));
    public StateBinding _ragdollBinding = new StateBinding(GhostPriority.High, nameof (ragdoll));
    public StateBinding _trappedBinding = new StateBinding(GhostPriority.High, nameof (_trapped));
    public StateBinding _cookedBinding = new StateBinding(GhostPriority.High, nameof (_cooked));
    public StateBinding _quackBinding = new StateBinding(GhostPriority.High, nameof (quack), 5);
    public StateBinding _quackPitchBinding = new StateBinding(GhostPriority.High, nameof (quackPitch));
    public StateBinding _trappedInstanceBinding = new StateBinding(nameof (_trappedInstance));
    public StateBinding _ragdollInstanceBinding = new StateBinding(nameof (_ragdollInstance));
    public StateBinding _cookedInstanceBinding = new StateBinding(nameof (_cookedInstance));
    public StateBinding _duckStateBinding = (StateBinding) new StateFlagBinding(GhostPriority.High, new string[11]
    {
      "crouch",
      "sliding",
      nameof (jumping),
      nameof (_hovering),
      nameof (immobilized),
      nameof (_canFire),
      nameof (afk),
      nameof (listening),
      nameof (chatting),
      nameof (beammode),
      nameof (eyesClosed)
    });
    public StateBinding _netQuackBinding = (StateBinding) new NetSoundBinding(GhostPriority.High, nameof (_netQuack));
    public StateBinding _netSwearBinding = (StateBinding) new NetSoundBinding(nameof (_netSwear));
    public StateBinding _netScreamBinding = (StateBinding) new NetSoundBinding(nameof (_netScream));
    public StateBinding _netJumpBinding = (StateBinding) new NetSoundBinding(GhostPriority.Normal, nameof (_netJump));
    public StateBinding _netDisarmBinding = (StateBinding) new NetSoundBinding(nameof (_netDisarm));
    public StateBinding _netTinyMotionBinding = (StateBinding) new NetSoundBinding(nameof (_netTinyMotion));
    public StateBinding _conversionResistanceBinding = new StateBinding(nameof (conversionResistance), 8);
    public StateBinding _listenTimeBinding = new StateBinding(nameof (listenTime));
    private bool forceDead;
    public bool afk = true;
    private byte _quackPitch;
    public NetSoundEffect _netQuack = new NetSoundEffect(new string[1]
    {
      nameof (quack)
    });
    public NetSoundEffect _netJump = new NetSoundEffect(new string[1]
    {
      "jump"
    })
    {
      volume = 0.5f
    };
    public NetSoundEffect _netDisarm = new NetSoundEffect(new string[1]
    {
      "disarm"
    })
    {
      volume = 0.3f
    };
    public NetSoundEffect _netTinyMotion = new NetSoundEffect(new string[1]
    {
      "tinyMotion"
    });
    public NetSoundEffect _netSwear = new NetSoundEffect(new List<string>()
    {
      "cutOffQuack",
      "cutOffQuack2"
    }, new List<string>() { "quackBleep" });
    public NetSoundEffect _netScream = new NetSoundEffect(new string[3]
    {
      "quackYell01",
      "quackYell02",
      "quackYell03"
    });
    public byte disarmIndex = 6;
    private byte _realIndex;
    private bool _netProfileInit;
    private bool _assignedIndex;
    private Sprite _shield;
    public int ctfTeamIndex;
    public bool forceMindControl;
    public SpriteMap _sprite;
    public SpriteMap _spriteArms;
    public SpriteMap _spriteQuack;
    public SpriteMap _spriteControlled;
    public InputProfile _mindControl;
    public Duck controlledBy;
    private bool _derpMindControl = true;
    protected DuckSkeleton _skeleton = new DuckSkeleton();
    public float slideBuildup;
    public int listenTime;
    public bool listening;
    public bool immobilized;
    public bool beammode;
    public bool jumping;
    public bool doThrow;
    public bool swinging;
    public bool holdObstructed;
    private bool _checkingTeam;
    private bool _checkingPersona;
    public bool isGrabbedByMagnet;
    public bool isRockThrowDuck;
    public bool _hovering;
    private bool _closingEyes;
    public bool _canFire = true;
    public float crippleTimer;
    public float jumpCharge;
    protected float armOffY;
    protected float armOffX;
    protected float centerOffset;
    protected float holdOffX;
    protected float holdOffY;
    protected float holdAngleOff;
    protected float aRadius = 4f;
    protected float dRadius = 4f;
    protected bool reverseThrow;
    protected float kick;
    public float unfocus = 1f;
    protected bool _isGhost;
    private bool _eyesClosed;
    private SinWave _arrowBob = (SinWave) 0.2f;
    private bool _remoteControl;
    private float _ghostTimer = 1f;
    private int _lives;
    private float _runMax = 3.1f;
    private bool _moveLock;
    private InputProfile _inputProfile = InputProfile.Get("SinglePlayer");
    private InputProfile _virtualInput;
    protected Profile _profile;
    protected Sprite _swirl;
    private float _swirlSpin;
    private bool _resetAction;
    protected SpriteMap _bionicArm;
    protected FeatherVolume _featherVolume;
    public List<Equipment> _equipment = new List<Equipment>();
    public int quack;
    public bool quackStart;
    private bool didHat;
    public TrappedDuck _trappedInstance;
    private Ragdoll _ins;
    public CookedDuck _cookedInstance;
    private SpriteMap _lagTurtle;
    private float _duckWidth = 1f;
    private float _duckHeight = 1f;
    private string _collisionMode = "normal";
    private Profile _disarmedBy;
    private DateTime _disarmedAt = DateTime.Now;
    private DateTime _timeSinceFuneralPerformed = DateTime.MinValue;
    private DateTime _timeSinceDuckLayedToRest = DateTime.MinValue;
    private Thing _holdingAtDisarm;
    private int _coolnessThisFrame;
    public Vec2 respawnPos;
    public float respawnTime;
    public Holdable _lastHoldItem;
    public byte _timeSinceThrow;
    public bool killingNet;
    public bool isKillMessage;
    private bool _killed;
    public Profile killedByProfile;
    public int framesSinceKilled;
    public float killMultiplier;
    private List<Thing> added = new List<Thing>();
    public DuckAI ai;
    private bool _crouchLock;
    public byte _flapFrame;
    public int _jumpValid;
    public int _groundValid;
    private TrappedDuck _trappedProp;
    public Profile trappedBy;
    private int tryGrabFrames;
    private bool _heldLeft;
    private bool _heldRight;
    private bool _updatedAnimation;
    private float disarmIndexCooldown;
    private int _disarmWait;
    public int _disarmDisable;
    public int _timeSinceChainKill;
    public int framesSinceJump;
    private ATShotgun shotty = new ATShotgun();
    public bool forceFire;
    public float jumpSpeed;
    public float removeBody = 1f;
    public int clientFrame;
    private bool _canWallJump;
    public bool localDuck = true;
    private Ragdoll _currentRagdoll;
    private int _wallJump;
    private bool _rightJump;
    private bool _leftJump;
    private int _atWallFrames;
    private bool leftWall;
    private bool rightWall;
    private bool atWall;
    private int _walkTime;
    private int _walkCount;
    private bool _nextTrigger;
    public bool grappleMul;
    public float grappleMultiplier = 1f;
    public bool onWall;
    private float maxrun;
    public bool pickedHat;
    public Vine _vine;
    public bool _double;
    public float _shieldCharge;
    public int skipPlatFrames;
    private bool didFireSlide;
    public static float JumpSpeed = -4.9f;
    private int bulletIndex;
    private bool vineRelease;
    private Vec2 prevCamPosition;
    private Thing _followPart;
    private int wait;
    private float lastCalc;
    private bool firstCalc = true;
    private CoolnessPlus plus;
    private float _bubbleWait;
    private static int _framesSinceInput = 0;
    public bool chatting;
    private Vec2 _lastGoodPosition;
    private byte _prevDisarmIndex = 6;
    public bool manualQuackPitch;
    private int killedWait;
    private bool _renderingDuck;
    private RenderTarget2D _offSide;
    public float _burnTime = 1f;
    public CookedDuck _cooked;
    private Sound _sizzle;
    private float _handHeat;
    private Duck _converted;
    public int conversionResistance = 100;
    public bool isConversionMessage;
    private bool _gripped;
    public bool localSpawnVisible = true;

    public override bool destroyed => this._destroyed || this.forceDead;

    public byte quackPitch
    {
      get => this._quackPitch;
      set => this._quackPitch = value;
    }

    public byte spriteFrame
    {
      get => this._sprite == null ? (byte) 0 : (byte) this._sprite._frame;
      set
      {
        if (this._sprite == null)
          return;
        this._sprite._frame = (int) value;
      }
    }

    public byte spriteImageIndex
    {
      get => this._sprite == null ? (byte) 0 : (byte) this._sprite._imageIndex;
      set
      {
        if (this._sprite == null)
          return;
        this._sprite._imageIndex = (int) value;
      }
    }

    public float spriteSpeed
    {
      get => this._sprite == null ? 0.0f : this._sprite._speed;
      set
      {
        if (this._sprite == null)
          return;
        this._sprite._speed = value;
      }
    }

    public float spriteInc
    {
      get => this._sprite == null ? 0.0f : this._sprite._frameInc;
      set
      {
        if (this._sprite == null)
          return;
        this._sprite._frameInc = value;
      }
    }

    public byte netAnimationIndex
    {
      get => this._sprite == null ? (byte) 0 : (byte) this._sprite.animationIndex;
      set
      {
        if (this._sprite == null || this._sprite.animationIndex == (int) value)
          return;
        this._sprite.animationIndex = (int) value;
      }
    }

    private byte _netProfileIndex
    {
      get => this._realIndex;
      set => this._realIndex = value;
    }

    public byte netProfileIndex
    {
      get => this._netProfileIndex;
      set
      {
        if ((int) this._netProfileIndex == (int) value && this._netProfileInit)
          return;
        DevConsole.Log(DCSection.General, "Assigning net profile index (" + (object) value + ")(" + (object) Profiles.all.Count<Profile>() + ")");
        this._netProfileIndex = value;
        Profile profile = Profiles.all.ElementAt<Profile>((int) this._netProfileIndex);
        if (Network.isClient && Level.current is TeamSelect2)
          (Level.current as TeamSelect2).OpenDoor((int) this._netProfileIndex, this);
        this.profile = profile;
        if (profile.team == null)
          profile.team = Teams.all[(int) this._netProfileIndex];
        this.InitProfile();
        this._netProfileInit = true;
        this._assignedIndex = true;
      }
    }

    public Hat hat => this.GetEquipment(typeof (Hat)) as Hat;

    public InputProfile mindControl
    {
      get => this._mindControl;
      set
      {
        if (value == null && this._mindControl != null && (this.profile.localPlayer || this.forceMindControl))
        {
          if (this.holdObject != null)
            Thing.Fondle((Thing) this.holdObject, DuckNetwork.localConnection);
          foreach (Thing t in this._equipment)
            Thing.Fondle(t, DuckNetwork.localConnection);
          Thing.Fondle((Thing) this._ragdollInstance, DuckNetwork.localConnection);
          Thing.Fondle((Thing) this._cookedInstance, DuckNetwork.localConnection);
          Thing.Fondle((Thing) this._trappedInstance, DuckNetwork.localConnection);
        }
        this._mindControl = value;
      }
    }

    public bool derpMindControl
    {
      get => this._derpMindControl;
      set => this._derpMindControl = value;
    }

    public DuckSkeleton skeleton
    {
      get
      {
        this.UpdateSkeleton();
        return this._skeleton;
      }
    }

    public bool dead
    {
      get => this.destroyed;
      set => this._destroyed = value;
    }

    public bool inNet => this._trapped != null;

    public Team team
    {
      get
      {
        if (this.profile == null)
          return (Team) null;
        if (this._checkingTeam || this._converted == null)
          return this.profile.team;
        this._checkingTeam = true;
        Team team = this._converted.team;
        this._checkingTeam = false;
        return team;
      }
    }

    public DuckPersona persona
    {
      get
      {
        if (this.profile == null)
          return (DuckPersona) null;
        if (this._checkingPersona || this._converted == null)
          return this.profile.persona;
        this._checkingPersona = true;
        DuckPersona persona = this._converted.persona;
        this._checkingPersona = false;
        return persona;
      }
    }

    public override bool CanBeControlled() => this.mindControl != null || this.isGrabbedByMagnet || (this.listening || this.dead) || this.wasSuperFondled > 0;

    public void CancelFlapping() => this._hovering = false;

    public bool IsNetworkDuck() => !this.isRockThrowDuck && Network.isClient;

    public bool closingEyes
    {
      get => this._closingEyes;
      set => this._closingEyes = value;
    }

    public bool canFire
    {
      get => this._canFire;
      set => this._canFire = value;
    }

    public bool CanMove() => !this.immobilized && (double) this.crippleTimer <= 0.0 && (!this.inNet && !this.swinging) && (!this.dead && !this.listening && (Level.current.simulatePhysics && !this._closingEyes)) && this.ragdoll == null;

    public static Duck Get(int index)
    {
      foreach (Thing thing in Level.current.things[typeof (Duck)])
      {
        Duck duck = thing as Duck;
        if (Persona.Number(duck.profile.persona) == index)
          return duck;
      }
      return (Duck) null;
    }

    public bool isGhost
    {
      get => this._isGhost;
      set => this._isGhost = value;
    }

    public bool eyesClosed
    {
      get => this._eyesClosed;
      set => this._eyesClosed = value;
    }

    public bool remoteControl
    {
      get => this._remoteControl;
      set => this._remoteControl = value;
    }

    public override bool action => !this._resetAction && (this.CanMove() || this.ragdoll != null && !this.dead && this.HasEquipment(typeof (FancyShoes)) || this._remoteControl) && this.inputProfile.Down("SHOOT") && this._canFire;

    public Vec2 armPosition => this.position + this.armOffset;

    public Vec2 armOffset
    {
      get
      {
        Vec2 vec2 = Vec2.Zero;
        if (this.gun != null)
          vec2 = -this.gun.barrelVector * this.kick;
        return new Vec2(this.armOffX * this.xscale + vec2.x, this.armOffY * this.yscale + vec2.y);
      }
    }

    public Vec2 armPositionNoKick => this.position + this.armOffsetNoKick;

    public Vec2 armOffsetNoKick => new Vec2(this.armOffX * this.xscale, this.armOffY * this.yscale);

    public Vec2 HoldOffset(Vec2 pos)
    {
      Vec2 vec2 = pos + new Vec2(this.holdOffX, this.holdOffY);
      vec2 = vec2.Rotate(this.holdAngle, new Vec2(0.0f, 0.0f));
      return this.position + (vec2 + this.armOffset);
    }

    public float holdAngle => this.holdObject != null ? this.holdObject.handAngle + this.holdAngleOff : this.holdAngleOff;

    public override Holdable holdObject
    {
      get => base.holdObject;
      set
      {
        if (value != this.holdObject && this.holdObject != null)
        {
          this._lastHoldItem = this.holdObject;
          this._timeSinceThrow = (byte) 0;
        }
        base.holdObject = value;
      }
    }

    public int lives
    {
      get => this._lives;
      set => this._lives = value;
    }

    public float holdingWeight => this.holdObject == null ? 0.0f : this.holdObject.weight;

    public override float weight
    {
      get => (float) ((double) this._weight + (double) this.holdingWeight * 0.400000005960464 + (this.sliding || this.crouch ? 16.0 : 0.0));
      set => this._weight = value;
    }

    public float runMax
    {
      get => this._runMax;
      set => this._runMax = value;
    }

    public bool moveLock
    {
      get => this._moveLock;
      set => this._moveLock = value;
    }

    public InputProfile inputProfile
    {
      get
      {
        if (this._mindControl != null)
          return this._mindControl;
        if (this._virtualInput != null)
          return this._virtualInput;
        return this._profile != null ? this._profile.inputProfile : this._inputProfile;
      }
    }

    public Profile profile
    {
      get => this._profile;
      set
      {
        this._profile = value;
        if (!Network.isActive || this._profile == null)
          return;
        if (this._profile.localPlayer)
          Thing.Fondle((Thing) this, DuckNetwork.localConnection);
        else
          this.connection = this._profile.connection;
      }
    }

    public override NetworkConnection connection
    {
      get => base.connection;
      set
      {
        if (this._profile != null)
        {
          if (this._profile.localPlayer && !this.CanBeControlled())
          {
            if (this.connection == DuckNetwork.localConnection)
              return;
            base.connection = DuckNetwork.localConnection;
            Duck duck = this;
            duck.authority = duck.authority + 1;
          }
          else
            base.connection = value;
        }
        else
          base.connection = value;
      }
    }

    public bool resetAction
    {
      get => this._resetAction;
      set => this._resetAction = value;
    }

    public virtual void InitProfile()
    {
      this._profile.duck = this;
      this._sprite = this.profile.persona.sprite.CloneMap();
      this._spriteArms = this.profile.persona.armSprite.CloneMap();
      this._spriteQuack = this.profile.persona.quackSprite.CloneMap();
      this._spriteControlled = this.profile.persona.controlledSprite.CloneMap();
      this._swirl = new Sprite("swirl");
      this._swirl.CenterOrigin();
      this._swirl.scale = new Vec2(0.75f, 0.75f);
      this._bionicArm = new SpriteMap("bionicArm", 32, 32);
      this._bionicArm.CenterOrigin();
      if (!this.didHat && (Network.isServer || RockScoreboard.initializingDucks))
      {
        if (this.profile.team != null && this.profile.team.hasHat)
        {
          Hat hat = (Hat) new TeamHat(0.0f, 0.0f, this.team);
          if (RockScoreboard.initializingDucks)
            hat.IgnoreNetworkSync();
          Level.Add((Thing) hat);
          this.Equip((Equipment) hat, false, true);
        }
        this.didHat = true;
      }
      this.graphic = (Sprite) this._sprite;
    }

    public Ragdoll _ragdollInstance
    {
      get => this._ins;
      set => this._ins = value;
    }

    public Duck(float xval, float yval, Profile pro)
      : base(xval, yval)
    {
      this._featherVolume = new FeatherVolume(this);
      this._featherVolume.anchor = (Anchor) (Thing) this;
      this.duck = true;
      this.profile = pro;
      if (this.profile != null)
        this.InitProfile();
      this.centerx = 16f;
      this.centery = 16f;
      this.friction = 0.25f;
      this.vMax = 8f;
      this.hMax = 12f;
      this._lagTurtle = new SpriteMap("lagturtle", 16, 16);
      this._lagTurtle.CenterOrigin();
      this.physicsMaterial = PhysicsMaterial.Duck;
      this.collideSounds.Add("land", ImpactedFrom.Bottom);
      this._impactThreshold = 1.3f;
      this._impactVolume = 0.4f;
      this.SetCollisionMode("normal");
      this._shield = new Sprite("sheeld");
      this._shield.CenterOrigin();
      this.flammable = 1f;
      this.thickness = 0.5f;
    }

    public override void Terminate()
    {
      if (Level.current.camera is FollowCam)
        (Level.current.camera as FollowCam).Remove((Thing) this);
      Level.Remove((Thing) this._featherVolume);
      if (!Network.isActive)
        return;
      Level.Remove((Thing) this._ragdollInstance);
      Level.Remove((Thing) this._trappedInstance);
      Level.Remove((Thing) this._cookedInstance);
    }

    public float duckWidth
    {
      get => this._duckWidth;
      set
      {
        this._duckWidth = value;
        this.xscale = this._duckWidth;
      }
    }

    public float duckHeight
    {
      get => this._duckHeight;
      set
      {
        this._duckHeight = value;
        this.yscale = this._duckHeight;
      }
    }

    public float duckSize
    {
      get => this._duckHeight;
      set => this.duckWidth = this.duckHeight = value;
    }

    private void SetCollisionMode(string mode)
    {
      this._collisionMode = mode;
      if (mode == "normal")
      {
        this.collisionSize = new Vec2(8f * this.duckWidth, 22f * this.duckHeight);
        this.collisionOffset = new Vec2(-4f * this.duckWidth, -7f * this.duckHeight);
        this._featherVolume.collisionSize = new Vec2(12f * this.duckWidth, 26f * this.duckHeight);
        this._featherVolume.collisionOffset = new Vec2(-6f * this.duckWidth, -9f * this.duckHeight);
      }
      else if (mode == "slide")
      {
        this.collisionSize = new Vec2(8f * this.duckWidth, 11f * this.duckHeight);
        this.collisionOffset = new Vec2(-4f * this.duckWidth, 4f * this.duckHeight);
        this._featherVolume.collisionSize = new Vec2(12f * this.duckWidth, 15f * this.duckHeight);
        this._featherVolume.collisionOffset = new Vec2(-6f * this.duckWidth, 2f * this.duckHeight);
      }
      else if (mode == "crouch")
      {
        this.collisionSize = new Vec2(8f * this.duckWidth, 16f * this.duckHeight);
        this.collisionOffset = new Vec2(-4f * this.duckWidth, -1f * this.duckHeight);
        this._featherVolume.collisionSize = new Vec2(12f * this.duckWidth, 20f * this.duckHeight);
        this._featherVolume.collisionOffset = new Vec2(-6f * this.duckWidth, -3f * this.duckHeight);
      }
      else
      {
        if (!(mode == "netted"))
          return;
        this.collisionSize = new Vec2(16f * this.duckWidth, 17f * this.duckHeight);
        this.collisionOffset = new Vec2(-8f * this.duckWidth, -9f * this.duckHeight);
        this._featherVolume.collisionSize = new Vec2(18f * this.duckWidth, 19f * this.duckHeight);
        this._featherVolume.collisionOffset = new Vec2(-9f * this.duckWidth, -10f * this.duckHeight);
      }
    }

    public void KnockOffEquipment(Equipment e, bool ting = true, Bullet b = null)
    {
      if (!this._equipment.Contains(e))
        return;
      e.UnEquip();
      if (ting && !Network.isActive)
        SFX.Play("ting2");
      this._equipment.Remove(e);
      e.Destroy((DestroyType) new DTImpact((Thing) null));
      e.solid = false;
      if (b != null)
      {
        e.hSpeed = b.travelDirNormalized.x;
        e.vSpeed = -2f;
        this.hSpeed += b.travelDirNormalized.x * (b.ammo.impactPower + 1f);
        this.vSpeed += b.travelDirNormalized.y * (b.ammo.impactPower + 1f);
        --this.vSpeed;
      }
      else
      {
        e.hSpeed = (float) -this.offDir * 2f;
        e.vSpeed = -2f;
      }
      this.ReturnItemToWorld((Thing) e);
    }

    public override void ReturnItemToWorld(Thing t)
    {
      Vec2 position = this.position;
      if (this.sliding)
        position.y += 10f;
      else if (this.crouch)
        position.y += 8f;
      Block block1 = Level.CheckLine<Block>(position, position + new Vec2(16f, 0.0f));
      if (block1 != null && block1.solid && (double) t.right > (double) block1.left)
        t.right = block1.left;
      Block block2 = Level.CheckLine<Block>(position, position - new Vec2(16f, 0.0f));
      if (block2 != null && block2.solid && (double) t.left < (double) block2.right)
        t.left = block2.right;
      Block block3 = Level.CheckLine<Block>(position, position + new Vec2(0.0f, -16f));
      if (block3 != null && block3.solid && (double) t.top < (double) block3.bottom)
        t.top = block3.bottom;
      Block block4 = Level.CheckLine<Block>(position, position + new Vec2(0.0f, 16f));
      if (block4 == null || !block4.solid || (double) t.bottom <= (double) block4.top)
        return;
      t.bottom = block4.top;
    }

    public void Unequip(Equipment e, bool forceNetwork = false)
    {
      if (!this.isServerForObject && !forceNetwork || !this._equipment.Contains(e))
        return;
      this.Fondle((Thing) e);
      e.UnEquip();
      this._equipment.Remove(e);
      this.ReturnItemToWorld((Thing) e);
    }

    public bool HasJumpModEquipment()
    {
      foreach (Equipment equipment in this._equipment)
      {
        if (equipment.jumpMod)
          return true;
      }
      return false;
    }

    public Equipment GetEquipment(System.Type t)
    {
      foreach (Equipment equipment in this._equipment)
      {
        if (equipment.GetAllTypes().Contains(t))
          return equipment;
      }
      return (Equipment) null;
    }

    public void Equip(Equipment e, bool makeSound = true, bool forceNetwork = false)
    {
      if (!this.isServerForObject && !forceNetwork)
        return;
      List<System.Type> allTypesFiltered = e.GetAllTypesFiltered(typeof (Equipment));
      if (allTypesFiltered.Contains(typeof (ITeleport)))
        allTypesFiltered.Remove(typeof (ITeleport));
      foreach (System.Type t in allTypesFiltered)
      {
        if (!t.IsInterface)
        {
          Equipment equipment = this.GetEquipment(t);
          if (equipment == null && e.GetType() == typeof (Jetpack))
            equipment = this.GetEquipment(typeof (Grapple));
          else if (equipment == null && e.GetType() == typeof (Grapple))
            equipment = this.GetEquipment(typeof (Jetpack));
          if (equipment != null)
          {
            this._equipment.Remove(equipment);
            this.Fondle((Thing) equipment);
            equipment.vSpeed = -2f;
            equipment.hSpeed = (float) this.offDir * 3f;
            equipment.UnEquip();
            this.ReturnItemToWorld((Thing) equipment);
          }
        }
      }
      if (e is TeamHat)
      {
        TeamHat teamHat = e as TeamHat;
        if (this.profile != null && teamHat.team != this.profile.team && !teamHat.hasBeenStolen)
        {
          ++Global.data.hatsStolen;
          teamHat.hasBeenStolen = true;
        }
      }
      this.Fondle((Thing) e);
      this._equipment.Add(e);
      e.Equip(this);
      if (!makeSound)
        e._prevEquipped = true;
      else
        e.equipIndex += 1;
    }

    public List<Equipment> GetArmor()
    {
      List<Equipment> equipmentList = new List<Equipment>();
      foreach (Equipment equipment in this._equipment)
      {
        if (equipment.isArmor)
          equipmentList.Add(equipment);
      }
      return equipmentList;
    }

    public bool ExtendsTo(Thing t)
    {
      if (this.ragdoll == null)
        return false;
      return t == this.ragdoll.part1 || t == this.ragdoll.part2 || t == this.ragdoll.part3;
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      if (this._trapped != null || this._trappedInstance != null && this._trappedInstance.visible || (this.ragdoll != null || this._ragdollInstance != null && this._ragdollInstance.visible))
        return false;
      if (bullet.isLocal)
        this.Kill((DestroyType) new DTShot(bullet));
      return base.Hit(bullet, hitPos);
    }

    public override bool Destroy(DestroyType type = null) => this.Kill(type);

    public void AddCoolness(int amount)
    {
      if ((double) Highlights.highlightRatingMultiplier == 0.0)
        return;
      this.profile.stats.coolness += amount;
      this._coolnessThisFrame += amount;
      if (Recorder.currentRecording == null)
        return;
      Recorder.currentRecording.LogCoolness(Math.Abs(amount));
    }

    public virtual bool Kill(DestroyType type = null)
    {
      if (this._killed)
        return true;
      this.forceDead = true;
      this._killed = true;
      int num1 = 10;
      if (type is DTFall)
      {
        Vec2 edgePos = this.GetEdgePos();
        Vec2 normalized = (edgePos - this.GetPos()).normalized;
        for (int index = 0; index < 8; ++index)
        {
          Feather feather = Feather.New(edgePos.x - normalized.x * 16f, edgePos.y - normalized.y * 16f, this.persona);
          feather.hSpeed += normalized.x * 1f;
          feather.vSpeed += normalized.y * 1f;
          Level.Add((Thing) feather);
        }
      }
      if (!GameMode.firstDead)
      {
        Party.AddDrink(this.profile, 1);
        if ((double) Rando.Float(1f) > 0.800000011920929)
          Party.AddRandomPerk(this.profile);
        GameMode.firstDead = true;
      }
      if ((double) Rando.Float(1f) > 0.970000028610229)
      {
        Party.AddRandomPerk(this.profile);
        Party.AddDrink(this.profile, 1);
      }
      if (Recorder.currentRecording != null)
        Recorder.currentRecording.LogDeath();
      this._destroyed = true;
      if (this._isGhost)
        return false;
      Main.SpecialCode = "ENTERED KILL";
      this.swinging = false;
      foreach (Equipment equipment in this._equipment)
      {
        if (equipment != null)
        {
          equipment.sleeping = false;
          equipment.owner = (Thing) null;
          if (!this.isKillMessage)
            Thing.Fondle((Thing) equipment, DuckNetwork.localConnection);
          equipment.hSpeed = this.hSpeed - (1f + NetRand.Float(2f));
          equipment.vSpeed = this.vSpeed - NetRand.Float(1.5f);
          this.ReturnItemToWorld((Thing) equipment);
          equipment.UnEquip();
        }
      }
      this._equipment.Clear();
      Profile profile = type.responsibleProfile;
      bool flag = false;
      if (this._trapped != null)
      {
        if (type is DTFall)
        {
          profile = this.trappedBy;
          if (this._trapped.prevOwner is Duck prevOwner)
            prevOwner.AddCoolness(1);
        }
        if (!this.killingNet)
        {
          this.killingNet = true;
          this._trapped.Destroy(type);
        }
        flag = true;
      }
      if (type is DTIncinerate)
        num1 -= 3;
      if (profile != null && profile.localPlayer)
        this.killedByProfile = profile;
      this.OnKill(type);
      Main.SpecialCode = "ON KILL";
      Holdable holdObject = this.holdObject;
      this.ThrowItem(false);
      if (holdObject != null)
      {
        holdObject.hSpeed *= 0.3f;
        if (type is DTImpale)
          holdObject.vSpeed = holdObject.hSpeed = 0.0f;
        if (!this.isKillMessage)
          Thing.Fondle((Thing) holdObject, DuckNetwork.localConnection);
      }
      Main.SpecialCode = "DROPPED ITEM";
      this.depth = new Depth(0.3f);
      if (profile != null)
      {
        if (profile == this.profile)
          ++profile.stats.suicides;
        else
          ++profile.stats.kills;
      }
      if (!Network.isActive && !(Level.current is ChallengeLevel))
      {
        int num2 = 0;
        int num3 = 0;
        System.Type killThingType = type.killThingType;
        Thing thing = type.thing;
        if (thing is Bullet bullet)
        {
          if ((double) bullet.travelTime > 0.5)
            ++num3;
          if ((double) bullet.bulletDistance > 300.0)
            ++num3;
          if (bullet.didPenetrate)
            ++num3;
          thing = bullet.firedFrom;
        }
        Main.SpecialCode = "KILL BULLET LOGIC";
        Event.Log((Event) new KillEvent(profile, this.profile, killThingType));
        this.profile.stats.LogKill(profile);
        if (killThingType != (System.Type) null)
        {
          if (killThingType == typeof (Mine))
          {
            --num2;
            num1 += 5;
          }
          if (killThingType == typeof (HugeLaser))
          {
            --num2;
            if (profile != null)
              ++num3;
          }
          if (killThingType == typeof (SuicidePistol))
          {
            --num2;
            if (profile != null && !profile.duck.dead)
              ++num3;
          }
        }
        Main.SpecialCode = "KILL WEAPON LOGIC";
        if (profile != null)
        {
          if (profile == this.profile)
          {
            num3 -= 2;
            if (killThingType == typeof (Grenade))
              --num3;
            if (holdObject == thing)
              --num3;
            if (killThingType == typeof (QuadLaser))
              --num3;
            Party.AddDrink(this.profile, 2);
            if ((double) Rando.Float(1f) > 0.899999976158142)
              Party.AddRandomPerk(this.profile);
            Main.SpecialCode = "SUICIDE";
          }
          else
          {
            ++num3;
            if (killThingType == typeof (QuadLaser) && type is DTIncinerate)
            {
              float num4 = 1f + Math.Min(((type as DTIncinerate).thing as QuadLaserBullet).timeAlive / 5f, 2f);
              num3 += (int) (1.0 * (double) num4);
            }
            if ((DateTime.Now - profile.stats.lastKillTime).TotalSeconds < 2.0)
              ++num3;
            // remove this garbage
            //if (bullet != null && (double) Math.Abs(bullet.travelDirNormalized.y) > 0.300000011920929)
            //  ++num3;
            Main.SpecialCode = "KILLED BY 01";
            profile.stats.lastKillTime = DateTime.Now;
            if (thing is Grenade)
            {
              ++num3;
              Grenade grenade = thing as Grenade;
              if ((double) grenade.cookTimeOnThrow < 0.5 && grenade.cookThrower != null)
                grenade.cookThrower.AddCoolness(1);
            }
            Main.SpecialCode = "KILLED BY 02";
            if ((double) Math.Abs(profile.duck.hSpeed) + (double) Math.Abs(profile.duck.vSpeed) + (double) Math.Abs(this.hSpeed) + (double) Math.Abs(this.vSpeed) > 20.0)
              ++num3;
            if (this._holdingAtDisarm != null && this._disarmedBy == profile && (DateTime.Now - this._disarmedAt).TotalSeconds < 3.0)
            {
              if (profile.duck.holdObject == this._holdingAtDisarm)
              {
                num3 += 4;
                num2 -= 2;
              }
              else
                ++num3;
            }
            Main.SpecialCode = "KILLED BY 03";
            if (profile.duck.dead)
            {
              ++num3;
              ++profile.stats.killsFromTheGrave;
            }
            if (type is DTShot && holdObject == null)
            {
              ++profile.stats.unarmedDucksShot;
            }
            else
            {
              switch (holdObject)
              {
                case PlasmaBlaster _:
                  ++num3;
                  break;
                case Saxaphone _:
                case Trombone _:
                case DrumSet _:
                  --num3;
                  ++num2;
                  Party.AddDrink(profile, 1);
                  break;
                case Flower _:
                  num3 -= 2;
                  num2 += 2;
                  Party.AddDrink(profile, 1);
                  break;
              }
            }
            Main.SpecialCode = "KILLED BY 04";
            if (killThingType != (System.Type) null)
            {
              if (killThingType == typeof (SledgeHammer) || killThingType == typeof (DuelingPistol))
                ++num3;
              if (thing is Sword && thing.owner != null && (thing as Sword).jabStance)
                ++num3;
            }
            if (flag && type is DTFall)
              ++num3;
            Main.SpecialCode = "KILLED BY 05";
            if (type is DTCrush)
            {
              if (thing is PhysicsObject)
              {
                double totalSeconds = (DateTime.Now - (thing as PhysicsObject).lastGrounded).TotalSeconds;
                num3 += 1 + (int) Math.Floor((DateTime.Now - (thing as PhysicsObject).lastGrounded).TotalSeconds * 6.0);
                if (Recorder.currentRecording != null)
                  Recorder.currentRecording.LogAction(14);
                Party.AddDrink(this.profile, 1);
                int num4 = num1 + 5;
                if ((double) Rando.Float(1f) > 0.800000011920929)
                  Party.AddRandomPerk(this.profile);
              }
              else
                ++num3;
            }
          }
          Main.SpecialCode = "KILLED BY 06";
          if (profile.duck.team == this.team && profile != this.profile)
          {
            num3 -= 2;
            Party.AddDrink(profile, 1);
          }
          if ((DateTime.Now - this._timeSinceDuckLayedToRest).TotalSeconds < 3.0)
            --num3;
          if ((DateTime.Now - this._timeSinceFuneralPerformed).TotalSeconds < 3.0)
            num3 -= 2;
        }
        Main.SpecialCode = "AFTER KILLED BY";
        if (this.controlledBy != null && this.controlledBy.profile != null)
        {
          this.controlledBy.profile.stats.coolness += Math.Abs(num2);
          if (num2 > 0)
            num2 = 0;
        }
        int amount1 = num3 + 1;
        int amount2 = num2 - 1;
        if (profile != null)
          amount1 *= (int) Math.Ceiling(1.0 + (double) profile.duck.killMultiplier);
        profile?.duck.AddCoolness(amount1);
        Main.SpecialCode = "ADD COOLNESS";
        this.AddCoolness(amount2);
        if (profile != null)
          ++profile.duck.killMultiplier;
      }
      if ((double) Highlights.highlightRatingMultiplier != 0.0)
        ++this.profile.stats.timesKilled;
      if (this.profile.connection == DuckNetwork.localConnection)
        ++DuckNetwork.deaths;
      if (!this.isKillMessage)
      {
        if (this.profile.connection != DuckNetwork.localConnection)
          ++DuckNetwork.kills;
        if (TeamSelect2.Enabled("CORPSEBLOW"))
        {
          Grenade grenade = new Grenade(this.x, this.y);
          grenade.hSpeed = this.hSpeed + Rando.Float(-2f, 2f);
          grenade.vSpeed = this.vSpeed - Rando.Float(1f, 2.5f);
          Level.Add((Thing) grenade);
          grenade.PressAction();
        }
        Thing.SuperFondle((Thing) this, DuckNetwork.localConnection);
        if (this._trappedInstance != null)
          Thing.SuperFondle((Thing) this._trappedInstance, DuckNetwork.localConnection);
        if (this.holdObject != null)
          Thing.SuperFondle((Thing) this.holdObject, DuckNetwork.localConnection);
        if ((double) this.y < -999.0)
        {
          Vec2 position = this.position;
          this.position = this._lastGoodPosition;
          this.GoRagdoll();
          this.position = position;
        }
        else
          this.GoRagdoll();
      }
      else if (type is DTCrush)
        this.MakeStars();
      if (Network.isActive && this.ragdoll != null && !this.isKillMessage)
        Thing.SuperFondle((Thing) this.ragdoll, DuckNetwork.localConnection);
      if (Network.isActive && !this.isKillMessage)
        Send.Message((NetMessage) new NMKillDuck(this.profile.networkIndex, type is DTCrush, type is DTIncinerate));
      if (!(this is TargetDuck))
        Global.Kill(this, type);
      return true;
    }

    public override void Zap(Thing zapper)
    {
      this.GoRagdoll();
      if (this.ragdoll != null)
        this.ragdoll.Zap(zapper);
      base.Zap(zapper);
    }

    public override void Removed()
    {
      if (Network.isServer)
      {
        if (this._ragdollInstance != null)
        {
          Thing.Fondle((Thing) this._ragdollInstance, DuckNetwork.localConnection);
          Level.Remove((Thing) this._ragdollInstance);
        }
        if (this._trappedInstance != null)
        {
          Thing.Fondle((Thing) this._trappedInstance, DuckNetwork.localConnection);
          Level.Remove((Thing) this._trappedInstance);
        }
        if (this._cookedInstance != null)
        {
          Thing.Fondle((Thing) this._cookedInstance, DuckNetwork.localConnection);
          Level.Remove((Thing) this._cookedInstance);
        }
      }
      base.Removed();
    }

    private void OnKill(DestroyType type = null)
    {
      SFX.Play("death");
      SFX.Play("pierce");
      if (!(Level.current is ChallengeLevel))
        Global.data.kills += 1;
      if (Network.isActive)
      {
        for (int index = 0; index < 8; ++index)
          Level.Add((Thing) Feather.New(this.cameraPosition.x, this.cameraPosition.y, this.persona));
      }
      else
      {
        for (int index = 0; index < 8; ++index)
          Level.Add((Thing) Feather.New(this.cameraPosition.x, this.cameraPosition.y, this.persona));
      }
      this._remoteControl = false;
      switch (type)
      {
        case DTShot dtShot:
          if (dtShot.bullet != null)
          {
            this.hSpeed = dtShot.bullet.travelDirNormalized.x * (dtShot.bullet.ammo.impactPower + 1f);
            this.vSpeed = dtShot.bullet.travelDirNormalized.y * (dtShot.bullet.ammo.impactPower + 1f);
          }
          this.vSpeed -= 3f;
          break;
        case DTIncinerate _:
          if (this.ragdoll != null)
          {
            this.position = this.ragdoll.position;
            if (Network.isActive)
              this.ragdoll.Unragdoll();
            else
              Level.Remove((Thing) this.ragdoll);
            this.vSpeed = -2f;
          }
          if (Network.isActive)
          {
            this._cooked = this._cookedInstance;
            if (this._cookedInstance != null)
            {
              this._cookedInstance.active = true;
              this._cookedInstance.visible = true;
              this._cookedInstance.solid = true;
              this._cookedInstance.enablePhysics = true;
              this._cookedInstance.x = this.x;
              this._cookedInstance.y = this.y;
              this._cookedInstance.owner = (Thing) null;
              Thing.Fondle((Thing) this._cookedInstance, DuckNetwork.localConnection);
              this.ReturnItemToWorld((Thing) this._cooked);
              this._cooked.vSpeed = this.vSpeed;
              this._cooked.hSpeed = this.hSpeed;
            }
          }
          else
          {
            this._cooked = new CookedDuck(this.x, this.y);
            this.ReturnItemToWorld((Thing) this._cooked);
            this._cooked.vSpeed = this.vSpeed;
            this._cooked.hSpeed = this.hSpeed;
            Level.Add((Thing) this._cooked);
          }
          this.OnTeleport();
          SFX.Play("ignite", pitch: (Rando.Float(0.3f) - 0.3f));
          this.y -= 25000f;
          break;
      }
    }

    public bool crouchLock => this._crouchLock;

    public TrappedDuck _trapped
    {
      get => this._trappedProp;
      set => this._trappedProp = value;
    }

    public virtual void Netted(Net n)
    {
      if (Network.isActive && this._trappedInstance != null && this._trappedInstance.visible || (Network.isActive && this._trappedInstance == null || this._trapped != null))
        return;
      if (Network.isActive)
      {
        this._trapped = this._trappedInstance;
        this._trappedInstance.active = true;
        this._trappedInstance.visible = true;
        this._trappedInstance.solid = true;
        this._trappedInstance.enablePhysics = true;
        this._trappedInstance.x = this.x;
        this._trappedInstance.y = this.y;
        this._trappedInstance.owner = (Thing) null;
        this._trappedInstance.InitializeStuff();
        n.Fondle((Thing) this._trappedInstance);
        n.Fondle((Thing) this);
      }
      else
      {
        this._trapped = new TrappedDuck(this.x, this.y, this);
        Level.Add((Thing) this._trapped);
      }
      this.ReturnItemToWorld((Thing) this._trapped);
      this.OnTeleport();
      if (this.holdObject != null)
        n.Fondle((Thing) this.holdObject);
      this.ThrowItem(false);
      Level.Remove((Thing) n);
      ++this.profile.stats.timesNetted;
      this._trapped.clip.Add((MaterialThing) this);
      this._trapped.clip.Add((MaterialThing) n);
      this._trapped.hSpeed = this.hSpeed + n.hSpeed * 0.4f;
      this._trapped.vSpeed = (float) ((double) this.vSpeed + (double) n.vSpeed - 1.0);
      if ((double) this._trapped.hSpeed > 6.0)
        this._trapped.hSpeed = 6f;
      if ((double) this._trapped.hSpeed < -6.0)
        this._trapped.hSpeed = -6f;
      if (n.onFire)
        this.Burn(n.position, (Thing) n);
      if (n.responsibleProfile == null)
        return;
      this.trappedBy = n.responsibleProfile;
      n.responsibleProfile.duck.AddCoolness(1);
      Event.Log((Event) new NettedEvent(n.responsibleProfile, this.profile));
    }

    private void UpdateQuack()
    {
      if (this.dead)
        return;
      if (this.inputProfile.Pressed("QUACK"))
      {
        if (Network.isActive)
          this._netQuack.Play(pit: this.inputProfile.leftTrigger);
        else if (this.GetEquipment(typeof (Hat)) is Hat equipment)
          equipment.Quack(1f, this.inputProfile.leftTrigger);
        else
          this._netQuack.Play(pit: this.inputProfile.leftTrigger);
        if (this.isServerForObject)
          ++Global.data.quacks.valueInt;
        ++this.profile.stats.quacks;
        this.quack = 20;
      }
      if (!this.inputProfile.Down("QUACK"))
        this.quack = Maths.CountDown(this.quack, 1, 0);
      if (!this.inputProfile.Released("QUACK"))
        return;
      this.quack = 0;
    }

    public bool HasEquipment(Equipment t) => this.HasEquipment(t.GetType());

    public bool HasEquipment(System.Type t)
    {
      foreach (Thing thing in this._equipment)
      {
        if (thing.GetAllTypesFiltered(typeof (Equipment)).Contains(t))
          return true;
      }
      return false;
    }

    public void ThrowItem(bool throwWithForce = true)
    {
      if (this.holdObject == null)
        return;
      this.Fondle((Thing) this.holdObject);
      this.holdObject.enablePhysics = true;
      this.holdObject.hSpeed = 0.0f;
      this.holdObject.vSpeed = 0.0f;
      this.holdObject.Thrown();
      this.holdObject.owner = (Thing) null;
      this.holdObject.clip.Add((MaterialThing) this);
      this.holdObject.lastGrounded = DateTime.Now;
      this.holdObstructed = false;
      if (this.holdObject is Mine && !(this.holdObject as Mine).pin && (!this.crouch || !this.grounded))
        (this.holdObject as Mine).Arm();
      if (!this.crouch)
      {
        float num1 = 1f;
        float num2 = 1f;
        if (this.inputProfile.Down("LEFT") || this.inputProfile.Down("RIGHT"))
          num1 = 2.5f;
        if ((double) num1 == 1.0 && this.inputProfile.Down("UP"))
        {
          this.holdObject.vSpeed -= 5f * this.holdWeightMultiplier;
        }
        else
        {
          float num3 = num1 * this.holdWeightMultiplier;
          if (this.inputProfile.Down("UP"))
            num2 = 2f;
          float num4 = num2 * this.holdWeightMultiplier;
          if (this.offDir > (sbyte) 0)
            this.holdObject.hSpeed += 3f * num3;
          else
            this.holdObject.hSpeed -= 3f * num3;
          if (this.reverseThrow)
            this.holdObject.hSpeed = -this.holdObject.hSpeed;
          this.holdObject.vSpeed -= 2f * num4;
        }
      }
      this.holdObject.ReturnToWorld();
      this.ReturnItemToWorld((Thing) this.holdObject);
      if (Recorder.currentRecording != null)
        Recorder.currentRecording.LogAction(2);
      this.holdObject.hSpeed += 0.3f * (float) this.offDir;
      this.holdObject.hSpeed *= this.holdObject.throwSpeedMultiplier;
      this.holdObject.solid = true;
      if (!throwWithForce)
        this.holdObject.hSpeed = this.holdObject.vSpeed = 0.0f;
      else if (Network.isActive)
      {
        if (this.isServerForObject)
          this._netTinyMotion.Play();
      }
      else
        SFX.Play("tinyMotion");
      this._lastHoldItem = this.holdObject;
      this._timeSinceThrow = (byte) 0;
      this.holdObject = (Holdable) null;
    }

    public void GiveHoldable(Holdable h)
    {
      if (this.holdObject == h)
        return;
      if (this.holdObject != null)
        this.ThrowItem(false);
      if (h == null)
        return;
      if (this.profile.localPlayer)
      {
        if (h is RagdollPart)
        {
          RagdollPart ragdollPart = h as RagdollPart;
          if (ragdollPart.doll != null)
          {
            ragdollPart.doll.connection = this.connection;
            Ragdoll doll = ragdollPart.doll;
            doll.authority = doll.authority + 15;
          }
        }
        else
        {
          h.connection = this.connection;
          Holdable holdable = h;
          holdable.authority = holdable.authority + 15;
        }
      }
      this.holdObject = h;
      this.holdObject.owner = (Thing) this;
      this.holdObject.solid = false;
      h.hSpeed = 0.0f;
      h.vSpeed = 0.0f;
      h.enablePhysics = false;
    }

    private void TryGrab()
    {
      foreach (Holdable h in (IEnumerable<Holdable>) Level.CheckCircleAll<Holdable>(new Vec2(this.x, this.y + 4f), 18f).OrderBy<Holdable, Holdable>((Func<Holdable, Holdable>) (h => h), (IComparer<Holdable>) new CompareHoldablePriorities(this)))
      {
        if (h.owner == null && h.canPickUp && (h != this._lastHoldItem || this._timeSinceThrow >= (byte) 30) && (h.active && Level.CheckLine<Block>(this.position, h.position) == null))
        {
          this.GiveHoldable(h);
          if (Network.isActive)
          {
            if (this.isServerForObject)
              this._netTinyMotion.Play();
          }
          else
            SFX.Play("tinyMotion");
          if (this.holdObject.disarmedFrom != this && (DateTime.Now - this.holdObject.disarmTime).TotalSeconds < 0.5)
            this.AddCoolness(2);
          this.tryGrabFrames = 0;
          break;
        }
      }
    }

    private void UpdateThrow()
    {
      if (!this.isServerForObject)
        return;
      bool flag = false;
      if (this.CanMove())
      {
        if (this.holdObject != null && this.inputProfile.Pressed("GRAB"))
          this.doThrow = true;
        if (!this._isGhost && this.inputProfile.Pressed("GRAB") && this.holdObject == null)
        {
          this.tryGrabFrames = 2;
          this.TryGrab();
        }
      }
      if (flag || !this.doThrow || this.holdObject == null)
        return;
      this.doThrow = false;
      this.ThrowItem();
    }

    private void UpdateAnimation()
    {
      this._updatedAnimation = true;
      if (this._hovering)
      {
        ++this._flapFrame;
        if (this._flapFrame > (byte) 8)
          this._flapFrame = (byte) 0;
      }
      this.UpdateCurrentAnimation();
    }

    private void UpdateCurrentAnimation()
    {
      if (this.dead && this._eyesClosed)
        this._sprite.currentAnimation = "dead";
      else if (this.inNet)
        this._sprite.currentAnimation = "netted";
      else if (this.listening)
        this._sprite.currentAnimation = "listening";
      else if (this.crouch)
      {
        this._sprite.currentAnimation = "crouch";
        if (!this.sliding)
          return;
        this._sprite.currentAnimation = "groundSlide";
      }
      else if (this.grounded)
      {
        if ((double) this.hSpeed > 0.0 && !this._gripped)
        {
          this._sprite.currentAnimation = "run";
          if (!this._heldLeft)
            return;
          this._sprite.currentAnimation = "slide";
        }
        else if ((double) this.hSpeed < 0.0 && !this._gripped)
        {
          this._sprite.currentAnimation = "run";
          if (!this._heldRight)
            return;
          this._sprite.currentAnimation = "slide";
        }
        else
          this._sprite.currentAnimation = "idle";
      }
      else
      {
        this._sprite.currentAnimation = "jump";
        this._sprite.speed = 0.0f;
        if ((double) this.vSpeed < 0.0 && !this._hovering)
          this._sprite.frame = 0;
        else
          this._sprite.frame = 2;
      }
    }

    private void UpdateBurning()
    {
      this.burnSpeed = 0.005f;
      if (this.onFire && !this.dead)
      {
        this.profile.stats.timeOnFire += Maths.IncFrameTimer();
        if (this.wallCollideLeft != null)
        {
          this.offDir = (sbyte) 1;
        }
        else
        {
          if (this.wallCollideRight == null)
            return;
          this.offDir = (sbyte) -1;
        }
      }
      else
      {
        if (this.onFire || this.dead)
          return;
        this.burnt -= 0.005f;
        if ((double) this.burnt >= 0.0)
          return;
        this.burnt = 0.0f;
      }
    }

    public void Ressurect()
    {
      this.dead = false;
      if (this.ragdoll != null)
        this.ragdoll.Unragdoll();
      this.unfocus = 1f;
      this._isGhost = false;
      this.Regenerate();
      this.immobilized = false;
      this._killed = false;
      this.crouch = false;
      this.sliding = false;
      this.active = true;
      this.forceDead = false;
      if (Level.current.camera is FollowCam)
        (Level.current.camera as FollowCam).Add((Thing) this);
      for (int index = 0; index < 14; ++index)
        Level.Add((Thing) new MusketSmoke(this.x - 5f + Rando.Float(10f), (float) ((double) this.y + 6.0 - 3.0 + (double) Rando.Float(6f) - (double) index * 1.0))
        {
          move = {
            x = (Rando.Float(0.4f) - 0.2f),
            y = (Rando.Float(0.4f) - 0.2f)
          }
        });
      this.vSpeed = -3f;
    }

    private void UpdateGhostStatus()
    {
      if (this.GetEquipment(typeof (GhostPack)) is GhostPack equipment && !this._isGhost)
      {
        this._equipment.Remove((Equipment) equipment);
        Level.Remove((Thing) equipment);
      }
            // TODO: else if (equipment == null && this._isGhost)
            else if (this._isGhost)
      {
        GhostPack ghostPack = new GhostPack(0.0f, 0.0f);
        this._equipment.Add((Equipment) ghostPack);
        ghostPack.Equip(this);
        Level.Add((Thing) ghostPack);
      }
      if (!this._isGhost)
        return;
      this._ghostTimer -= 23f / 1000f;
      if ((double) this._ghostTimer >= 0.0)
        return;
      this._ghostTimer = 1f;
      this._isGhost = false;
      this.Ressurect();
    }

    public void Swear()
    {
      if (Network.isActive)
      {
        if (this.isServerForObject)
          this._netSwear.Play();
      }
      else
      {
        float num = 0.0f;
        if (this.profile.team != null && this.profile.team.name == "Sailors")
          num += 0.1f;
        if ((double) Rando.Float(1f) < 0.0299999993294477 + (double) this.profile.funslider * 0.0450000017881393 + (double) num)
        {
          SFX.Play("quackBleep", 0.8f);
          Event.Log((Event) new SwearingEvent(this.profile, this.profile));
        }
        else if ((double) Rando.Float(1f) < 0.5)
          SFX.Play("cutOffQuack");
        else
          SFX.Play("cutOffQuack2");
      }
      this.quack = 10;
    }

    public void Scream()
    {
      if (Network.isActive)
      {
        if (this.isServerForObject)
          this._netScream.Play();
      }
      else if ((double) Rando.Float(1f) < 0.0299999993294477 + (double) this.profile.funslider * 0.0450000017881393)
      {
        SFX.Play("quackBleep", 0.9f);
        Event.Log((Event) new SwearingEvent(this.profile, this.profile));
      }
      else if ((double) Rando.Float(1f) < 0.5)
        SFX.Play("quackYell03");
      else if ((double) Rando.Float(1f) < 0.5)
        SFX.Play("quackYell02");
      else
        SFX.Play("quackYell01");
      this.quack = 10;
    }

    public void Disarm(Thing disarmedBy)
    {
      if (!this.isServerForObject)
        return;
      if (this.holdObject != null && (!Network.isActive || disarmedBy.isServerForObject))
        ++Global.data.disarms.valueInt;
      Profile responsibleProfile = disarmedBy?.responsibleProfile;
      if (responsibleProfile != null && this.holdObject != null)
      {
        this.disarmIndex = responsibleProfile.networkIndex;
        this.disarmIndexCooldown = 1f;
      }
      this._disarmedBy = responsibleProfile;
      this._disarmedAt = DateTime.Now;
      this._holdingAtDisarm = (Thing) this.holdObject;
      if (this.holdObject != null)
      {
        this.Fondle((Thing) this.holdObject);
        this.holdObject.disarmedFrom = this;
        this.holdObject.disarmTime = DateTime.Now;
        if (Network.isActive)
        {
          if (this.isServerForObject)
            this._netDisarm.Play();
        }
        else
          SFX.Play("disarm", 0.3f, Rando.Float(0.2f, 0.4f));
      }
      Event.Log((Event) new DisarmEvent(responsibleProfile, this.profile));
      this.ThrowItem();
      this.Swear();
    }

    public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
      Holdable holdable = with as Holdable;
      if (this._isGhost 
                || holdable != null && holdable.owner == this 
                || (with is FeatherVolume 
                || with == this._lastHoldItem && this._timeSinceThrow < (byte) 7) 
                || (with == this._trapped || with == this._trappedInstance 
                || this._disarmDisable > 0) 
                || with is RagdollPart 
                    && (with is RagdollPart ragdollPart 
                        && ragdollPart.doll != null 
                        && (ragdollPart.doll.captureDuck != null 
                        && ragdollPart.doll.captureDuck.killedByProfile == this.profile) 
                        && ragdollPart.doll.captureDuck.framesSinceKilled < 50
                            /*|| ragdollPart != null && ragdollPart.doll != null && (ragdollPart.doll.PartHeld() 
                            || this.holdObject is Chainsaw && this._timeSinceChainKill < 50) 
                            || (this.holdObject != null && this.holdObject is RagdollPart && ragdollPart.doll.holdingOwner == this 
                            || this.ragdoll != null && (with == this.ragdoll.part1 
                            || with == this.ragdoll.part2 
                            || with == this.ragdoll.part3)) 
                            || (ragdollPart.doll == null 
                            || ragdollPart.doll.captureDuck == this 
                            || this._timeSinceThrow < (byte) 15 && ragdollPart.doll != null && (ragdollPart.doll.part1 == this._lastHoldItem 
                            || ragdollPart.doll.part2 == this._lastHoldItem 
                            || ragdollPart.doll.part3 == this._lastHoldItem)))*/
                 || (this.dead || this.swinging || (!(with is PhysicsObject) || (double) with.totalImpactPower <= (double) with.weightMultiplierInv * 2.0))))
        return;
      if (with is Duck && (double) with.weight >= 5.0)
      {
        Duck duck = with as Duck;
        if (!duck.HasEquipment(typeof (Boots)) || duck.sliding || (from != ImpactedFrom.Top || (double) with.bottom - 5.0 >= (double) this.top) || (double) with.impactPowerV <= 2.0)
          return;
        this.vSpeed = with.impactDirectionV * 0.5f;
        with.vSpeed = (float) (-(double) with.vSpeed * 0.699999988079071);
        duck._groundValid = 7;
        if (with.isServerForObject)
          this.MakeStars();
        if (this.GetEquipment(typeof (Helmet)) is Helmet equipment)
        {
          SFX.Play("metalRebound");
          equipment.Crush();
        }
        else
        {
          if (!with.isServerForObject)
            return;
          ++Global.data.ducksCrushed.valueInt;
          this.Kill((DestroyType) new DTCrush(with as PhysicsObject));
        }
      }
      else if (with is Gun && with.dontCrush || !(with is Gun) && with.dontCrush)
      {
        if ((double) (with as Gun).alpha <= 0.990000009536743 || from != ImpactedFrom.Left && from != ImpactedFrom.Right || (Network.isActive || (double) with.impactPowerH <= 2.0) && (double) with.impactPowerH <= 3.0)
          return;
        this.hSpeed = with.impactDirectionH * 0.5f;
        with.hSpeed = -with.hSpeed * with.bouncy;
        if (!this.isServerForObject || Network.isActive && this._disarmWait != 0)
          return;
        this.Disarm((Thing) with);
        this._disarmWait = 5;
      }
      else if (from == ImpactedFrom.Top && (double) with.y < (double) this.y && ((double) with.vSpeed > 0.0 && (double) with.impactPowerV > 2.0) && (double) with.weight >= 5.0)
      {
        this.vSpeed = with.impactDirectionV * 0.5f;
        with.vSpeed = (float) (-(double) with.vSpeed * 0.5);
        if (with.isServerForObject)
          this.MakeStars();
        if (this.GetEquipment(typeof (Helmet)) is Helmet equipment)
        {
          SFX.Play("metalRebound");
          equipment.Crush();
        }
        else
        {
          if (!with.isServerForObject)
            return;
          this.Kill((DestroyType) new DTCrush(with as PhysicsObject));
          ++Global.data.ducksCrushed.valueInt;
        }
      }
      else if ((from == ImpactedFrom.Left || from == ImpactedFrom.Right) && (!Network.isActive && (double) with.impactPowerH > 2.0 || (double) with.impactPowerH > 3.0))
      {
        if (this.holdObject is SledgeHammer && with is RagdollPart || this.holdObject is Sword && (this.holdObject as Sword).crouchStance && (this.offDir < (sbyte) 0 && from == ImpactedFrom.Left || this.offDir > (sbyte) 0 && from == ImpactedFrom.Right))
          return;
        this.hSpeed = with.impactDirectionH * 0.5f;
        with.hSpeed = -with.hSpeed * with.bouncy;
        if (!this.isServerForObject || Network.isActive && this._disarmWait != 0)
          return;
        this.Disarm((Thing) with);
        this._disarmWait = 5;
      }
      else
      {
        if (from != ImpactedFrom.Bottom || (double) with.y <= (double) this.bottom || (double) with.impactPowerV <= 2.0)
          return;
        this.vSpeed = with.impactDirectionV * 0.5f;
        with.vSpeed = (float) (-(double) with.vSpeed * 0.5);
      }
    }

    public override void OnTeleport()
    {
      if (this.holdObject != null)
        this.holdObject.OnTeleport();
      foreach (Thing thing in this._equipment)
        thing.OnTeleport();
      if (this._vine == null)
        return;
      this._vine.Degrapple();
      this._vine = (Vine) null;
    }

    public void AdvanceServerTime(int frames)
    {
      while (frames > 0)
      {
        --frames;
        ++this.clientFrame;
        this.Update();
      }
    }

    public override void Initialize()
    {
      if (Level.current != null)
      {
        if (this.isServerForObject)
        {
          foreach (Equipper equipper in Level.current.things[typeof (Equipper)])
          {
            if (equipper.radius.value == 0 || (double) (this.position - equipper.position).length <= (double) equipper.radius.value)
            {
              Thing containedInstance = equipper.GetContainedInstance(this.position);
              if (containedInstance != null)
              {
                Level.Add(containedInstance);
                if (containedInstance is Equipment)
                  this.Equip(containedInstance as Equipment);
                else if (containedInstance is Holdable)
                  this.GiveHoldable(containedInstance as Holdable);
              }
            }
          }
        }
        Level.Add((Thing) this._featherVolume);
      }
      if (Network.isServer)
        this._netProfileIndex = (byte) DuckNetwork.IndexOf(this.profile);
      if (Network.isActive)
        this._netQuack.pitchBinding = new FieldBinding((object) this, "quackPitch");
      base.Initialize();
    }

    public override void NetworkUpdate()
    {
    }

    public Ragdoll ragdoll
    {
      get => this._currentRagdoll;
      set
      {
        this._currentRagdoll = value;
        if (this._currentRagdoll == null)
          return;
        this._currentRagdoll._duck = this;
      }
    }

    public void GoRagdoll()
    {
      if (Network.isActive && (this._ragdollInstance == null || this._ragdollInstance != null && this._ragdollInstance.visible || this._cookedInstance != null && this._cookedInstance.visible) || (this.ragdoll != null || this._cooked != null))
        return;
      this._hovering = false;
      float ypos = this.y + 4f;
      float degrees;
      if (this.sliding)
      {
        ypos += 6f;
        degrees = this.offDir >= (sbyte) 0 ? 0.0f : 180f;
      }
      else
        degrees = -90f;
      Vec2 v = new Vec2(this._hSpeed, this._vSpeed);
      this.hSpeed = 0.0f;
      this.vSpeed = 0.0f;
      if (Network.isActive)
      {
        this.ragdoll = this._ragdollInstance;
        this._ragdollInstance.active = true;
        this._ragdollInstance.visible = true;
        this._ragdollInstance.solid = true;
        this._ragdollInstance.enablePhysics = true;
        this._ragdollInstance.x = this.x;
        this._ragdollInstance.y = this.y;
        this._ragdollInstance.owner = (Thing) null;
        this._ragdollInstance.npi = (int) this.netProfileIndex;
        this._ragdollInstance.SortOutParts(this.x, ypos, this, this.sliding, degrees, (int) this.offDir, v);
        this.Fondle((Thing) this._ragdollInstance);
      }
      else
      {
        this.ragdoll = new Ragdoll(this.x, ypos, this, this.sliding, degrees, (int) this.offDir, v);
        Level.Add((Thing) this.ragdoll);
        this.ragdoll.RunInit();
      }
      if (this.ragdoll == null)
        return;
      this.ragdoll.connection = this.connection;
      this.ragdoll.part1.connection = this.connection;
      this.ragdoll.part2.connection = this.connection;
      this.ragdoll.part3.connection = this.connection;
      if (!this.HasEquipment(typeof (FancyShoes)))
      {
        Equipment equipment = this.GetEquipment(typeof (Hat));
        if (equipment != null)
        {
          this.Unequip(equipment);
          equipment.hSpeed = this.hSpeed * 1.2f;
          equipment.vSpeed = this.vSpeed - 2f;
        }
        this.ThrowItem(false);
      }
      this.OnTeleport();
      if ((double) this.y > -4000.0)
        this.y -= 5000f;
      this.sliding = false;
      this.crouch = false;
    }

    public virtual void UpdateSkeleton()
    {
      if (this.ragdoll != null)
      {
        if (this.ragdoll.part1 == null || this.ragdoll.part3 == null)
          return;
        this._skeleton.upperTorso.position = this.ragdoll.part1.Offset(new Vec2(0.0f, 7f));
        this._skeleton.upperTorso.orientation = this.ragdoll.part1.offDir > (sbyte) 0 ? -this.ragdoll.part1.angle : this.ragdoll.part1.angle;
        this._skeleton.lowerTorso.position = this.ragdoll.part3.Offset(new Vec2(5f, 11f));
        this._skeleton.lowerTorso.orientation = (this.ragdoll.part3.offDir > (sbyte) 0 ? -this.ragdoll.part3.angle : this.ragdoll.part3.angle) + Maths.DegToRad(180f);
        this._skeleton.head.position = this.ragdoll.part1.Offset(new Vec2(-2f, -6f));
        this._skeleton.head.orientation = this.ragdoll.part1.offDir > (sbyte) 0 ? -this.ragdoll.part1.angle : this.ragdoll.part1.angle;
      }
      else
      {
        if (this._sprite == null)
          return;
        this._skeleton.head.position = this.Offset(DuckRig.GetHatPoint(this._sprite.imageIndex));
        this._skeleton.upperTorso.position = this.Offset(DuckRig.GetChestPoint(this._sprite.imageIndex));
        this._skeleton.lowerTorso.position = this.position;
        if (this.sliding)
        {
          this._skeleton.head.orientation = Maths.DegToRad(90f);
          this._skeleton.upperTorso.orientation = Maths.DegToRad(90f);
          this._skeleton.lowerTorso.orientation = 0.0f;
        }
        else
        {
          this._skeleton.head.orientation = this.offDir < (sbyte) 0 ? this.angle : -this.angle;
          this._skeleton.upperTorso.orientation = this.offDir < (sbyte) 0 ? this.angle : -this.angle;
          this._skeleton.lowerTorso.orientation = this.offDir < (sbyte) 0 ? this.angle : -this.angle;
        }
        if (this._trapped == null)
          return;
        this._skeleton.head.orientation = 0.0f;
        this._skeleton.upperTorso.orientation = 0.0f;
        this._skeleton.lowerTorso.orientation = 0.0f;
        this._skeleton.head.position = this.Offset(DuckRig.GetHatPoint(this._sprite.imageIndex));
        this._skeleton.upperTorso.position = this.Offset(new Vec2(-1f, 2f));
        this._skeleton.lowerTorso.position = this.Offset(new Vec2(0.0f, -8f));
      }
    }

    public PhysicsSnapshotDuckProperties GetProperties() => new PhysicsSnapshotDuckProperties()
    {
      jumping = this.jumping
    };

    public void SetProperties(PhysicsSnapshotDuckProperties dat) => this.jumping = dat.jumping;

    public void UpdateMove()
    {
      ++this._timeSinceChainKill;
      this.weight = 5.3f;
      if (this.holdObject != null)
      {
        this.weight += Math.Max(0.0f, this.holdObject.weight - 5f);
        if (this.holdObject.destroyed)
          this.ThrowItem();
      }
      if (this.isServerForObject)
        this.UpdateQuack();
      if (this.inNet && this._trapped != null)
      {
        this.x = this._trapped.x;
        this.y = this._trapped.y;
        this.owner = (Thing) this._trapped;
        this.ThrowItem();
      }
      else
      {
        this.owner = (Thing) null;
        this.skipPlatFrames = Maths.CountDown(this.skipPlatFrames, 1, 0);
        this.crippleTimer = Maths.CountDown(this.crippleTimer, 0.1f);
        if (this.inputProfile.Pressed("JUMP"))
        {
          this._jumpValid = 4;
          if (!this.grounded && this.crouch)
            this.skipPlatFrames = 10;
        }
        else
          this._jumpValid = Maths.CountDown(this._jumpValid, 1, 0);
        this._skipPlatforms = false;
        if (this.inputProfile.Down("DOWN") && this.skipPlatFrames > 0)
          this._skipPlatforms = true;
        bool flag1 = this.grounded;
        if (!flag1 && this.HasEquipment(typeof (ChokeCollar)))
        {
          ChokeCollar equipment = this.GetEquipment(typeof (ChokeCollar)) as ChokeCollar;
          if (equipment.ball.grounded && (double) equipment.ball.bottom < (double) this.top && (double) this.vSpeed > -1.0)
            flag1 = true;
        }
        if (flag1)
        {
          this.framesSinceJump = 0;
          this._groundValid = 7;
          this._hovering = false;
          this._double = false;
        }
        else
        {
          this._groundValid = Maths.CountDown(this._groundValid, 1, 0);
          ++this.framesSinceJump;
        }
        if (this.mindControl != null)
          this.mindControl.UpdateExtraInput();
        this._heldLeft = false;
        this._heldRight = false;
        this._crouchLock = (this.crouch || this.sliding) && Level.CheckRect<Block>(new Vec2(this.x - 3f, this.y - 9f), new Vec2(this.x + 3f, this.y + 4f)) != null;
        float num1 = 0.55f * this.holdWeightMultiplier * this.grappleMultiplier;
        this.maxrun = this._runMax * this.holdWeightMultiplier;
        this.jumpSpeed = Duck.JumpSpeed;
        if (this._isGhost)
        {
          num1 *= 1.4f;
          this.maxrun *= 1.5f;
        }
        if (this.holdObject is TV && !(this.holdObject as TV)._ruined && !(this.holdObject as TV).channel)
        {
          num1 *= 1.4f;
          this.maxrun *= 1.5f;
        }
        if ((double) this.specialFrictionMod > 0.0)
          num1 *= Math.Min(this.specialFrictionMod * 2f, 1f);
        if (this.isServerForObject && (double) this.y > (double) Level.activeLevel.lowestPoint + 100.0 && !this.dead)
        {
          this.Kill((DestroyType) new DTFall());
          ++this.profile.stats.fallDeaths;
        }
        if (Network.isActive && this.ragdoll != null && (this.ragdoll.connection != DuckNetwork.localConnection && this.ragdoll.TryingToControl()) && !this.ragdoll.PartHeld())
          this.Fondle((Thing) this.ragdoll);
        if (!this.CanMove())
          return;
        if (!this._grounded)
          this.profile.stats.airTime += Maths.IncFrameTimer();
        if (this.isServerForObject && !this.sliding && this.inputProfile.Pressed("UP"))
        {
          Desk desk = Level.Nearest<Desk>(this.position);
          if (desk != null && ((double) (desk.position - this.position).length < 22.0 && Level.CheckLine<Block>(this.position, desk.position) == null))
          {
            this.Fondle((Thing) desk);
            desk.Flip(this.offDir < (sbyte) 0);
          }
        }
        float num2 = !this.inputProfile.Down("LEFT") ? Maths.NormalizeSection(Math.Abs(Math.Min(this.inputProfile.leftStick.x, 0.0f)), 0.2f, 0.9f) : 1f;
        float num3 = !this.inputProfile.Down("RIGHT") ? Maths.NormalizeSection(Math.Max(this.inputProfile.leftStick.x, 0.0f), 0.2f, 0.9f) : 1f;
        if ((double) num2 < 0.00999999977648258 && this.onFire && this.offDir == (sbyte) 1)
          num3 = 1f;
        if ((double) num3 < 0.00999999977648258 && this.onFire && this.offDir == (sbyte) -1)
          num2 = 1f;
        if (this.grappleMul)
        {
          num2 *= 1.5f;
          num3 *= 1.5f;
        }
        if (DevConsole.qwopMode && Level.current is GameLevel)
        {
          if ((double) num2 > 0.0)
            this.offDir = (sbyte) -1;
          else if ((double) num3 > 0.0)
            this.offDir = (sbyte) 1;
          if (this._walkTime == 0)
          {
            num3 = num2 = 0.0f;
          }
          else
          {
            if (this.offDir < (sbyte) 0)
              num2 = 1f;
            else
              num3 = 1f;
            --this._walkTime;
          }
          if (this._walkCount > 0)
            --this._walkCount;
          if (this.inputProfile.Pressed("LTRIGGER"))
          {
            if (this._walkCount > 0 && this._nextTrigger)
            {
              this.GoRagdoll();
              this._walkCount = 0;
            }
            else
            {
              this._walkCount += 20;
              if (DevConsole.rhythmMode && Level.current is GameLevel)
                this._walkTime += 20;
              else
                this._walkTime += 8;
              if (this._walkTime > 20)
                this._walkTime = 20;
              if (this._walkCount > 40)
                this._walkCount = 40;
              this._nextTrigger = true;
            }
          }
          else if (this.inputProfile.Pressed("RTRIGGER"))
          {
            if (this._walkCount > 0 && !this._nextTrigger)
            {
              this.GoRagdoll();
              this._walkCount = 0;
            }
            else
            {
              this._walkCount += 20;
              if (DevConsole.rhythmMode && Level.current is GameLevel)
                this._walkTime += 20;
              else
                this._walkTime += 8;
              if (this._walkTime > 20)
                this._walkTime = 20;
              if (this._walkCount > 40)
                this._walkCount = 40;
              this._nextTrigger = false;
            }
          }
        }
        bool flag2 = this._crouchLock && this.grounded && this.inputProfile.Pressed("ANY");
        if (flag2 && this.offDir == (sbyte) -1)
        {
          num2 = 1f;
          num3 = 0.0f;
        }
        if (flag2 && this.offDir == (sbyte) 1)
        {
          num3 = 1f;
          num2 = 0.0f;
        }
        if (this._leftJump)
          num2 = 0.0f;
        else if (this._rightJump)
          num3 = 0.0f;
        if (this._moveLock)
          return;
        bool flag3 = this.inputProfile.Down("STRAFE");
        if ((double) num2 > 0.00999999977648258 && (!this.crouch || flag2))
        {
          if ((double) this.hSpeed > -(double) this.maxrun * (double) num2)
          {
            this.hSpeed -= num1;
            if ((double) this.hSpeed < -(double) this.maxrun * (double) num2)
              this.hSpeed = -this.maxrun * num2;
          }
          this._heldLeft = true;
          if (!flag3 && !flag2)
            this.offDir = (sbyte) -1;
        }
        if ((double) num3 > 0.00999999977648258 && (!this.crouch || flag2))
        {
          if ((double) this.hSpeed < (double) this.maxrun * (double) num3)
          {
            this.hSpeed += num1;
            if ((double) this.hSpeed > (double) this.maxrun * (double) num3)
              this.hSpeed = this.maxrun * num3;
          }
          this._heldRight = true;
          if (!flag3 && !flag2)
            this.offDir = (sbyte) 1;
        }
        if (this.isServerForObject && flag3)
          Global.data.strafeDistance.valueFloat += Math.Abs(this.hSpeed) * 0.00015f;
        if (this._atWallFrames > 0)
        {
          --this._atWallFrames;
        }
        else
        {
          this.atWall = false;
          this.leftWall = false;
          this.rightWall = false;
        }
        this._canWallJump = this.GetEquipment(typeof (WallBoots)) != null;
        int num4 = 6;
        if (!this.grounded && this._canWallJump)
        {
          if (this.inputProfile.Down("LEFT") && Level.CheckLine<Block>(this.topLeft + new Vec2(0.0f, 4f), this.bottomLeft + new Vec2(-3f, -4f)) != null)
          {
            this.atWall = true;
            this.leftWall = true;
            this._atWallFrames = num4;
            if (!this.onWall)
            {
              this.onWall = true;
              SFX.Play("wallTouch", pitch: Rando.Float(-0.1f, 0.1f));
              for (int index = 0; index < 2; ++index)
              {
                Feather feather1 = Feather.New(this.x + (!this.leftWall ? -4f : 4f) + Rando.Float(-1f, 1f), this.y + Rando.Float(-4f, 4f), this.persona);
                Feather feather2 = feather1;
                feather2.velocity = feather2.velocity * 0.9f;
                if (this.leftWall)
                  feather1.hSpeed = Rando.Float(-1f, 2f);
                else
                  feather1.hSpeed = Rando.Float(-2f, 1f);
                feather1.vSpeed = Rando.Float(-2f, 1.5f);
                Level.Add((Thing) feather1);
              }
            }
          }
          else if (this.inputProfile.Down("RIGHT") && Level.CheckLine<Block>(this.topRight + new Vec2(3f, 4f), this.bottomRight + new Vec2(0.0f, -4f)) != null)
          {
            this.atWall = true;
            this.rightWall = true;
            this._atWallFrames = num4;
            if (!this.onWall)
            {
              this.onWall = true;
              SFX.Play("wallTouch", pitch: Rando.Float(-0.1f, 0.1f));
              for (int index = 0; index < 2; ++index)
              {
                Feather feather1 = Feather.New(this.x + (!this.leftWall ? -4f : 4f) + Rando.Float(-1f, 1f), this.y + Rando.Float(-4f, 4f), this.persona);
                feather1.vSpeed = Rando.Float(-2f, 1.5f);
                Feather feather2 = feather1;
                feather2.velocity = feather2.velocity * 0.9f;
                if (this.leftWall)
                  feather1.hSpeed = Rando.Float(-1f, 2f);
                else
                  feather1.hSpeed = Rando.Float(-2f, 1f);
                Level.Add((Thing) feather1);
              }
            }
          }
        }
        if (this.onWall && this._atWallFrames != num4)
        {
          SFX.Play("wallLeave", pitch: Rando.Float(-0.1f, 0.1f));
          for (int index = 0; index < 2; ++index)
          {
            Feather feather1 = Feather.New(this.x + (!this.leftWall ? -4f : 4f) + Rando.Float(-1f, 1f), this.y + Rando.Float(-4f, 4f), this.persona);
            feather1.vSpeed = Rando.Float(-2f, 1.5f);
            Feather feather2 = feather1;
            feather2.velocity = feather2.velocity * 0.9f;
            if (this.leftWall)
              feather1.hSpeed = Rando.Float(-1f, 2f);
            else
              feather1.hSpeed = Rando.Float(-2f, 1f);
            Level.Add((Thing) feather1);
          }
          this.onWall = false;
        }
        if ((this.leftWall || this.rightWall) && ((double) this.vSpeed > 1.0 && this._atWallFrames == num4))
          this.vSpeed = 0.5f;
        if (this._wallJump > 0)
          --this._wallJump;
        else
          this._rightJump = this._leftJump = false;
        bool flag4 = this._jumpValid > 0 && (this._groundValid > 0 && !this._crouchLock || this.atWall && this._wallJump == 0 || this.doFloat);
        if (this._double && !this.HasJumpModEquipment() && (!this._hovering && this.inputProfile.Pressed("JUMP")))
        {
          PhysicsRopeSection section = (PhysicsRopeSection) null;
          if (this._vine == null)
            section = Level.Nearest<PhysicsRopeSection>(this.x, this.y);
          if (section != null && (double) (this.position - section.position).length < 18.0)
          {
            this._vine = section.rope.LatchOn(section, this);
            this._double = false;
            flag4 = false;
            this._groundValid = 0;
          }
        }
        bool flag5 = false;
        if (flag4 && (double) Math.Abs(this.hSpeed) < 0.200000002980232 && (this.inputProfile.Down("DOWN") && (double) Math.Abs(this.hSpeed) < 0.200000002980232) && this.inputProfile.Down("DOWN"))
        {
          IEnumerable<IPlatform> source = Level.CheckLineAll<IPlatform>(this.bottomLeft + new Vec2(1f, 1f), this.bottomRight + new Vec2(-1f, 1f));
          if (source.FirstOrDefault<IPlatform>((Func<IPlatform, bool>) (p => p is Block)) == null)
          {
            foreach (IPlatform platform1 in source)
            {
              if (!(platform1 is Block))
              {
                if (platform1 is MaterialThing materialThing)
                {
                  this.clip.Add(materialThing);
                  IPlatform platform2 = Level.CheckPoint<IPlatform>(materialThing.topLeft + new Vec2(-2f, 2f));
                  if (platform2 != null && platform2 is MaterialThing && !(platform2 is Block))
                    this.clip.Add(platform2 as MaterialThing);
                  IPlatform platform3 = Level.CheckPoint<IPlatform>(materialThing.topRight + new Vec2(2f, 2f));
                  if (platform3 != null && platform3 is MaterialThing && !(platform3 is Block))
                    this.clip.Add(platform3 as MaterialThing);
                  flag4 = false;
                }
              }
              else
                break;
            }
          }
          if (!flag4)
          {
            ++this.y;
            this.vSpeed = 1f;
            this._groundValid = 0;
            this._hovering = false;
            this.jumping = true;
            flag5 = true;
          }
        }
        bool flag6 = false;
        if (!flag5)
        {
          if (this.inputProfile.Pressed("JUMP") && this.HasEquipment(typeof (Jetpack)) && (this._groundValid <= 0 || this.crouch || this.sliding))
          {
            this.GetEquipment(typeof (Jetpack)).PressAction();
            flag6 = true;
          }
          if (this.inputProfile.Down("JUMP") && this.HasEquipment(typeof (Jetpack)) && (this._groundValid <= 0 || this.crouch || this.sliding))
            flag6 = true;
          if (this.inputProfile.Released("JUMP") && this.HasEquipment(typeof (Jetpack)))
            this.GetEquipment(typeof (Jetpack)).ReleaseAction();
          if (this.inputProfile.Pressed("JUMP") && this.HasEquipment(typeof (Grapple)) && (!this.grounded && this._jumpValid <= 0) && this._groundValid <= 0)
            flag6 = true;
        }
        bool flag7 = flag4 && !flag6;
        bool flag8 = false;
        bool flag9 = false;
        bool flag10 = false;
        if (!flag7 && this._vine != null && this.inputProfile.Released("JUMP"))
        {
          this._vine.Degrapple();
          this._vine = (Vine) null;
          if (!this.inputProfile.Down("DOWN"))
          {
            flag7 = true;
            flag8 = true;
          }
          if (!this.inputProfile.Down("UP"))
            flag9 = true;
          flag10 = true;
        }
        if (flag7)
        {
          if (this.atWall)
          {
            this._wallJump = 8;
            if (this.leftWall)
            {
              this.hSpeed += 4f;
              this._leftJump = true;
            }
            else if (this.rightWall)
            {
              this.hSpeed -= 4f;
              this._rightJump = true;
            }
            this.vSpeed = this.jumpSpeed;
          }
          else
            this.vSpeed = this.jumpSpeed;
          this.jumping = true;
          this.sliding = false;
          if (Network.isActive)
          {
            if (this.isServerForObject)
              this._netJump.Play();
          }
          else
            SFX.Play("jump", 0.5f);
          this._groundValid = 0;
          this._hovering = false;
          this._jumpValid = 0;
          ++this.profile.stats.timesJumped;
          if (Recorder.currentRecording != null)
            Recorder.currentRecording.LogAction(6);
        }
        if (flag8)
        {
          this.jumping = false;
          if (flag9 && (double) this.vSpeed < 0.0)
            this.vSpeed *= 0.7f;
        }
        if (this.inputProfile.Released("JUMP"))
        {
          if (this.jumping)
          {
            this.jumping = false;
            if ((double) this.vSpeed < 0.0)
              this.vSpeed *= 0.5f;
          }
          this._hovering = false;
        }
        if (!flag7 && !this.HasJumpModEquipment() && this._groundValid <= 0)
        {
          bool flag11 = !this.crouch && (double) this.holdingWeight <= 5.0;
          if (!this._hovering && this.inputProfile.Pressed("JUMP"))
          {
            PhysicsRopeSection section = (PhysicsRopeSection) null;
            if (this._vine == null)
              section = Level.Nearest<PhysicsRopeSection>(this.x, this.y);
            if (section != null && (double) (this.position - section.position).length < 18.0)
            {
              this._vine = section.rope.LatchOn(section, this);
              this._double = false;
            }
            else if (this._vine == null && flag11)
            {
              this._hovering = true;
              this._flapFrame = (byte) 0;
            }
          }
          if (flag11 && this._hovering && (double) this.vSpeed >= 0.0)
          {
            if ((double) this.vSpeed > 1.0)
              this.vSpeed = 1f;
            this.vSpeed -= 0.15f;
          }
        }
        if (this.doFloat)
          this._hovering = false;
        if (this.isServerForObject)
        {
          if (this.inputProfile.Down("DOWN"))
          {
            this.crouch = true;
            if (this.grounded && (double) Math.Abs(this.hSpeed) > 1.0)
            {
              if (!this.sliding && (double) this.slideBuildup < -0.300000011920929)
              {
                this.slideBuildup = 0.4f;
                this.didFireSlide = true;
              }
              this.sliding = true;
            }
          }
          else if (!this._crouchLock)
          {
            this.crouch = false;
            this.sliding = false;
          }
          if (!this.sliding)
            this.didFireSlide = false;
          if ((double) this.slideBuildup > 0.0 || !this.sliding || !this.didFireSlide)
          {
            this.slideBuildup -= Maths.IncFrameTimer();
            if ((double) this.slideBuildup <= -0.600000023841858)
              this.slideBuildup = -0.6f;
          }
        }
        if (this.isServerForObject && !(this.holdObject is DrumSet) && (!(this.holdObject is Trumpet) && this.inputProfile.Pressed("RAGDOLL")) && !(Level.current is TitleScreen))
          this.GoRagdoll();
        if (this.isServerForObject && this.grounded && ((double) Math.Abs(this.vSpeed) + (double) Math.Abs(this.hSpeed) < 0.5 && !this._closingEyes) && (this.holdObject == null && this.inputProfile.Pressed("SHOOT")))
        {
          Ragdoll ragdoll = Level.Nearest<Ragdoll>(this.x, this.y, (Thing) this);
          if (ragdoll != null && ragdoll.active && ragdoll.visible && ((double) (ragdoll.position - this.position).length < 100.0 && ragdoll.captureDuck != null && (ragdoll.captureDuck.dead && !ragdoll.captureDuck._eyesClosed)) && (double) (ragdoll.part1.position - (this.position + new Vec2(0.0f, 8f))).length < 4.0)
          {
            Level.Add((Thing) new EyeCloseWing((double) ragdoll.part1.angle < 0.0 ? this.x - 4f : this.x - 11f, this.y + 7f, (double) ragdoll.part1.angle < 0.0 ? 1 : -1, this._spriteArms, this, ragdoll.captureDuck));
            this._closingEyes = true;
            ++this.profile.stats.respectGivenToDead;
            this.AddCoolness(1);
            this._timeSinceDuckLayedToRest = DateTime.Now;
            Flower flower = Level.Nearest<Flower>(this.x, this.y);
            if (flower != null && (double) (flower.position - this.position).length < 22.0)
            {
              this.Fondle((Thing) ragdoll);
              this.Fondle((Thing) ragdoll.captureDuck);
              ragdoll.captureDuck.LayToRest(this.profile);
              if (!Music.currentSong.Contains("MarchOfDuck"))
              {
                if (Network.isActive)
                  Send.Message((NetMessage) new NMPlayMusic("MarchOfDuck"));
                Music.Play("MarchOfDuck", false);
              }
            }
          }
        }
        if (this.inputProfile.Released("JUMP") || this.vineRelease)
        {
          this.vineRelease = false;
          if (!flag7 && this.holdObject is TV && (!(this.holdObject as TV)._ruined && (this.holdObject as TV).channel) && !this._double)
          {
            this._double = true;
            this._groundValid = 9999;
          }
        }
        if (!flag10)
          return;
        this.vineRelease = true;
      }
    }

    public override Vec2 cameraPosition
    {
      get
      {
        Vec2 zero = Vec2.Zero;
        Vec2 vec2 = this.ragdoll == null ? (this._cooked == null ? (this._trapped == null ? this.position : this._trapped.position) : this._cooked.position) : this.ragdoll.position;
        if ((double) vec2.y < -1000.0 || vec2 == Vec2.Zero)
          vec2 = this.prevCamPosition;
        else
          this.prevCamPosition = vec2;
        return vec2;
      }
    }

    public override Vec2 anchorPosition => this.cameraPosition;

    public Thing followPart => this._followPart == null ? (Thing) this : this._followPart;

    public void EmitBubbles(int num, float hVel)
    {
      if (!this.doFloat || this._curPuddle == null || (double) this.top + 2.0 <= (double) this._curPuddle.top)
        return;
      for (int index = 0; index < num; ++index)
        Level.Add((Thing) new TinyBubble(this.x + (float) ((this.offDir > (sbyte) 0 ? 6 : -6) * (this.sliding ? -1 : 1)) + Rando.Float(-1f, 1f), this.top + 7f + Rando.Float(-1f, 1f), Rando.Float(hVel) * (float) this.offDir, this._curPuddle.top + 7f));
    }

    public void MakeStars()
    {
      Level.Add((Thing) new DizzyStar(this.x + (float) ((int) this.offDir * -3), this.y - 9f, new Vec2(Rando.Float(-0.8f, -1.5f), Rando.Float(0.5f, -1f))));
      Level.Add((Thing) new DizzyStar(this.x + (float) ((int) this.offDir * -3), this.y - 9f, new Vec2(Rando.Float(-0.8f, -1.5f), Rando.Float(0.5f, -1f))));
      Level.Add((Thing) new DizzyStar(this.x + (float) ((int) this.offDir * -3), this.y - 9f, new Vec2(Rando.Float(0.8f, 1.5f), Rando.Float(0.5f, -1f))));
      Level.Add((Thing) new DizzyStar(this.x + (float) ((int) this.offDir * -3), this.y - 9f, new Vec2(Rando.Float(0.8f, 1.5f), Rando.Float(0.5f, -1f))));
      Level.Add((Thing) new DizzyStar(this.x + (float) ((int) this.offDir * -3), this.y - 9f, new Vec2(Rando.Float(-1.5f, 1.5f), Rando.Float(-0.5f, -1.1f))));
    }

    public override bool active
    {
      get => this._active;
      set => this._active = value;
    }

    public virtual void DuckUpdate()
    {
    }

    public override void SpecialNetworkUpdate()
    {
    }

    public override void Update()
    {
      if (this.killedByProfile != null)
        ++this.framesSinceKilled;
      int num = this.crouch ? 1 : 0;
      if (this._sprite == null)
        return;
      if (this.isServerForObject && this.inputProfile != null && this.inputProfile.CheckCode(Input.konamiCode))
        this.Kill((DestroyType) new DTFall());
      if (this._disarmWait > 0)
        --this._disarmWait;
      if (this._disarmDisable > 0)
        --this._disarmDisable;
      if ((double) this.killMultiplier > 0.0)
        this.killMultiplier -= 0.016f;
      else
        this.killMultiplier = 0.0f;
      if (this.isServerForObject && this.holdObject != null && this.holdObject.removeFromLevel)
        this.holdObject = (Holdable) null;
      if (Network.isActive)
      {
        if (this.isServerForObject)
        {
          if (this._assignedIndex)
          {
            this._assignedIndex = false;
            Thing.Fondle((Thing) this, DuckNetwork.localConnection);
            if (this.holdObject != null)
              Thing.Fondle((Thing) this.holdObject, DuckNetwork.localConnection);
            foreach (Equipment equipment in this._equipment)
            {
              if (equipment != null)
                Thing.Fondle((Thing) equipment, DuckNetwork.localConnection);
            }
          }
          this.chatting = DuckNetwork.core.enteringText;
          if (this.inputProfile != null && !this.manualQuackPitch)
            this.quackPitch = (byte) ((double) this.inputProfile.leftTrigger * (double) byte.MaxValue);
          ++Duck._framesSinceInput;
          if (this.inputProfile != null && this.inputProfile.Pressed("", true))
          {
            Duck._framesSinceInput = 0;
            this.afk = false;
          }
          if (Duck._framesSinceInput > 1200)
            this.afk = true;
        }
        else if (this.profile != null)
        {
          if (this.disarmIndex != (byte) 6 && (int) this.disarmIndex != (int) this._prevDisarmIndex && ((int) this._prevDisarmIndex == (int) this.profile.networkIndex || this._prevDisarmIndex == (byte) 6) && (this.disarmIndex >= (byte) 0 && this.disarmIndex < (byte) 4 && DuckNetwork.profiles[(int) this.disarmIndex].connection == DuckNetwork.localConnection))
            ++Global.data.disarms.valueInt;
          this._prevDisarmIndex = this.disarmIndex;
        }
        if (this.isServerForObject)
        {
          this.disarmIndexCooldown -= Maths.IncFrameTimer();
          if ((double) this.disarmIndexCooldown <= 0.0 && this.profile != null)
          {
            this.disarmIndexCooldown = 0.0f;
            this.disarmIndex = this.profile.networkIndex;
          }
        }
        if ((double) this.y > -999.0)
          this._lastGoodPosition = this.position;
        if (this._ragdollInstance != null)
          this._ragdollInstance.captureDuck = this;
        if (this._profile.localPlayer && !(this is RockThrowDuck))
        {
          if (this._trappedInstance == null)
          {
            this._trappedInstance = new TrappedDuck(this.x, this.y - 9999f, this);
            this._trappedInstance.active = false;
            this._trappedInstance.visible = false;
            this._trappedInstance.authority = (NetIndex8) 80;
            if (!GhostManager.inGhostLoop)
            {
              GhostManager.context.MakeGhost((Thing) this._trappedInstance);
              this._trappedInstance.ghostObject.ForceInitialize();
            }
            Level.Add((Thing) this._trappedInstance);
            if (this._profile.localPlayer)
              this.Fondle((Thing) this._trappedInstance);
          }
          if (this._cookedInstance == null)
          {
            this._cookedInstance = new CookedDuck(this.x, this.y - 9999f);
            this._cookedInstance.active = false;
            this._cookedInstance.visible = false;
            this._cookedInstance.authority = (NetIndex8) 80;
            if (!GhostManager.inGhostLoop)
            {
              GhostManager.context.MakeGhost((Thing) this._cookedInstance);
              this._cookedInstance.ghostObject.ForceInitialize();
            }
            Level.Add((Thing) this._cookedInstance);
            if (this._profile.localPlayer)
              this.Fondle((Thing) this._cookedInstance);
          }
          if (this._ragdollInstance == null)
          {
            this._ragdollInstance = new Ragdoll(this.x, this.y - 9999f, this, false, 0.0f, 0, Vec2.Zero);
            this._ragdollInstance.npi = (int) this.netProfileIndex;
            this._ragdollInstance.RunInit();
            this._ragdollInstance.active = false;
            this._ragdollInstance.visible = false;
            this._ragdollInstance.authority = (NetIndex8) 80;
            Level.Add((Thing) this._ragdollInstance);
            if (this._profile.localPlayer)
              this.Fondle((Thing) this._ragdollInstance);
          }
          if (this.connection != DuckNetwork.localConnection && !this.CanBeControlled())
          {
            Thing.Fondle((Thing) this, DuckNetwork.localConnection);
            Thing.Fondle((Thing) this.holdObject, DuckNetwork.localConnection);
            Thing.Fondle((Thing) this._trappedInstance, DuckNetwork.localConnection);
            Thing.Fondle((Thing) this._ragdollInstance, DuckNetwork.localConnection);
            Thing.Fondle((Thing) this._cookedInstance, DuckNetwork.localConnection);
            foreach (Thing t in this._equipment)
              Thing.Fondle(t, DuckNetwork.localConnection);
          }
          if (this._trappedInstance != null)
          {
            if (this._trappedInstance.visible || this._trappedInstance.owner != null)
            {
              this._trapped = this._trappedInstance;
            }
            else
            {
              this._trappedInstance.owner = (Thing) null;
              this._trapped = (TrappedDuck) null;
              this._trappedInstance.y = -9999f;
            }
          }
          if (this._ragdollInstance != null)
          {
            if (this._ragdollInstance.visible)
            {
              this.ragdoll = this._ragdollInstance;
            }
            else
            {
              this._ragdollInstance.visible = true;
              this._ragdollInstance.visible = false;
              this._ragdollInstance.part1.y = -9999f;
              this._ragdollInstance.part2.y = -9999f;
              this._ragdollInstance.part3.y = -9999f;
              this.ragdoll = (Ragdoll) null;
            }
          }
          if (this._cookedInstance != null)
          {
            if (this._cookedInstance.visible)
            {
              this._cooked = this._cookedInstance;
              if (this._ragdollInstance != null)
              {
                this._ragdollInstance.visible = false;
                this._ragdollInstance.active = false;
                this.ragdoll = (Ragdoll) null;
              }
            }
            else
            {
              this._cooked = (CookedDuck) null;
              this._cookedInstance.y = -9999f;
            }
          }
        }
      }
      if (this.profile != null && this.mindControl != null)
        this.profile.stats.timeUnderMindControl += Maths.IncFrameTimer();
      if (this.doFloat && this._curPuddle != null && (double) this.top + 2.0 > (double) this._curPuddle.top)
      {
        this._bubbleWait += Rando.Float(0.015f, 0.017f);
        if ((double) Rando.Float(1f) > 0.990000009536743)
          this._bubbleWait += 0.5f;
        if ((double) this._bubbleWait > 1.0)
        {
          this._bubbleWait = Rando.Float(0.2f);
          this.EmitBubbles(1, 1f);
        }
        if (!this.quackStart && this.quack > 0)
        {
          this.quackStart = true;
          this.EmitBubbles(Rando.Int(3, 6), 1.2f);
        }
      }
      if (this.quack <= 0)
        this.quackStart = false;
      ++this.wait;
      if (TeamSelect2.doCalc && this.wait > 10 && this.profile != null)
      {
        this.wait = 0;
        float profileScore = this.profile.endOfRoundStats.CalculateProfileScore();
        if (this.firstCalc)
        {
          this.firstCalc = false;
          this.lastCalc = profileScore;
        }
        if ((double) Math.Abs(this.lastCalc - profileScore) > 0.00499999988824129)
        {
          int c = (int) Math.Round(((double) profileScore - (double) this.lastCalc) / 0.00499999988824129);
          if (this.plus == null || this.plus.removeFromLevel)
          {
            this.plus = new CoolnessPlus(this.x, this.y, this, c);
            Level.Add((Thing) this.plus);
          }
          else
            this.plus.change = c;
        }
        this.lastCalc = profileScore;
      }
      this.grappleMultiplier = !this.grappleMul ? 1f : 1.5f;
      ++this._timeSinceThrow;
      if (this._timeSinceThrow > (byte) 30)
        this._timeSinceThrow = (byte) 30;
      if (this._resetAction && !this.inputProfile.Down("SHOOT"))
        this._resetAction = false;
      if (this._converted == null)
      {
        this._sprite.texture = this.profile.persona.sprite.texture;
        this._spriteArms.texture = this.profile.persona.armSprite.texture;
        this._spriteQuack.texture = this.profile.persona.quackSprite.texture;
        this._spriteControlled.texture = this.profile.persona.controlledSprite.texture;
      }
      else
      {
        this._sprite.texture = this._converted.profile.persona.sprite.texture;
        this._spriteArms.texture = this._converted.profile.persona.armSprite.texture;
        this._spriteQuack.texture = this._converted.profile.persona.quackSprite.texture;
        this._spriteControlled.texture = this._converted.profile.persona.controlledSprite.texture;
      }
      if (this.isServerForObject)
      {
        --this.listenTime;
        if (this.listenTime < 0)
          this.listenTime = 0;
        if (this.listening && this.listenTime <= 0)
          this.listening = false;
        if (!this.listening)
        {
          ++this.conversionResistance;
          if (this.conversionResistance > 100)
            this.conversionResistance = 100;
        }
      }
      this._coolnessThisFrame = 0;
      this.UpdateBurning();
      this.UpdateGhostStatus();
      if (this.dead)
      {
        this.immobilized = true;
        if ((double) this.unfocus > 0.0)
          this.unfocus -= 0.015f;
        else if ((double) this.unfocus > -1.0)
        {
          if (!this.grounded && this._lives > 0)
          {
            IEnumerable<Thing> thing = Level.current.things[typeof (SpawnPoint)];
            this.position = thing.ElementAt<Thing>(Rando.Int(thing.Count<Thing>() - 1)).position;
          }
          if (this._lives > 0)
          {
            --this._lives;
            this.unfocus = 1f;
            this._isGhost = true;
            this.Regenerate();
            this.immobilized = false;
            this.crouch = false;
            this.sliding = false;
          }
          else
          {
            this.unfocus = -1f;
            this.visible = false;
            if (!Network.isActive)
              this.active = false;
            if (Level.current.camera is FollowCam && !(Level.current is ChallengeLevel))
              (Level.current.camera as FollowCam).Remove((Thing) this);
            this.y -= 100000f;
          }
        }
        this.sliding = true;
        this.crouch = true;
      }
      else if (this.quack > 0)
        this.profile.stats.timeWithMouthOpen += Maths.IncFrameTimer();
      if (DevConsole.rhythmMode && Level.current is GameLevel && (this.inputProfile.Pressed("DOWN") || this.inputProfile.Pressed("JUMP") || (this.inputProfile.Pressed("SHOOT") || this.inputProfile.Pressed("QUACK")) || this.inputProfile.Pressed("GRAB")) && !RhythmMode.inTime)
        this.GoRagdoll();
      this.UpdateMove();
      if (this.ragdoll != null)
        this.ragdoll.UpdateUnragdolling();
      this.centerOffset = 8f;
      if (this.crouch)
        this.centerOffset = 24f;
      if (this.ragdoll == null && this.isServerForObject)
        base.Update();
      if ((double) this.kick > 0.0)
        this.kick -= 0.1f;
      else
        this.kick = 0.0f;
      this._sprite.speed = (float) (0.100000001490116 + (double) Math.Abs(this.hSpeed) / (double) this.maxrun * 0.100000001490116);
      this._sprite.flipH = this.offDir < (sbyte) 0;
      if (!this.swinging)
        this.UpdateAnimation();
      if (this._trapped != null)
        this.SetCollisionMode("netted");
      else if (this._sprite.currentAnimation == "run" || this._sprite.currentAnimation == "jump" || this._sprite.currentAnimation == "idle")
        this.SetCollisionMode("normal");
      else if (this._sprite.currentAnimation == "slide")
        this.SetCollisionMode("normal");
      else if (this._sprite.currentAnimation == "crouch" || this._sprite.currentAnimation == "listening")
        this.SetCollisionMode("crouch");
      else if (this._sprite.currentAnimation == "groundSlide" || this._sprite.currentAnimation == "dead")
        this.SetCollisionMode("slide");
      Holdable holdObject = this.holdObject;
      if (this.holdObject != null && this.isServerForObject && (this.ragdoll == null || !this.HasEquipment(typeof (FancyShoes))))
      {
        this.holdObject.isLocal = this.isLocal;
        this.holdObject.UpdateAction();
      }
      if (Network.isActive && this.holdObject != null && (this.holdObject.duck != this || !this.holdObject.active || !this.holdObject.visible) && this.isServerForObject)
        this.holdObject = (Holdable) null;
      if (this.tryGrabFrames > 0 && !this.inputProfile.Pressed("GRAB"))
      {
        --this.tryGrabFrames;
        this.TryGrab();
        if (this.holdObject != null)
          this.tryGrabFrames = 0;
      }
      else
        this.tryGrabFrames = 0;
      this.UpdateThrow();
      this.doThrow = false;
      this.reverseThrow = false;
      this.UpdateHoldPosition();
      if (!this.isServerForObject)
        base.Update();
      this.forceFire = false;
      if (this._offSide == null)
        this._offSide = new RenderTarget2D(32, 32);
      if (this.ShouldDrawIcon())
      {
        Viewport viewport = DuckGame.Graphics.viewport;
        DuckGame.Graphics.SetRenderTarget(this._offSide);
        DuckGame.Graphics.viewport = new Viewport(0, 0, this._offSide.width, this._offSide.height);
        DuckGame.Graphics.Clear(Color.CornflowerBlue);
        DuckGame.Graphics.ResetSpanAdjust();
        DuckGame.Graphics.ResetDepthBias();
        Camera camera = new Camera(0.0f, 0.0f, (float) (this._offSide.width / 2), (float) (this._offSide.height / 2));
        camera.center = this.position + new Vec2(0.0f, -2f);
        if (this.crouch)
          camera.centerY += 3f;
        if (this.sliding)
        {
          camera.centerY += 6f;
          camera.centerX -= (float) ((int) this.offDir * 7);
        }
        if (this.ragdoll != null)
          camera.center = this.ragdoll.part1.position;
        if (this._trapped != null)
          camera.center = this._trapped.position + new Vec2(0.0f, -5f);
        MTSpriteBatch mtSpriteBatch = new MTSpriteBatch(DuckGame.Graphics.device);
        this._renderingDuck = true;
        DuckGame.Graphics.screen.Begin(SpriteSortMode.BackToFront, BlendState.AlphaBlend, SamplerState.PointClamp, DepthStencilState.Default, RasterizerState.CullNone, (MTEffect) null, camera.getMatrix());
        if (this.ragdoll != null)
        {
          this.ragdoll.part1.Draw();
          this.ragdoll.part3.Draw();
        }
        else if (this._trapped != null)
          this._trapped.Draw();
        else
          this.Draw();
        DuckGame.Graphics.DrawRect(camera.center + new Vec2((float) (-this._offSide.width / 4), (float) (-this._offSide.width / 4)), camera.center + new Vec2((float) (this._offSide.width / 4), (float) (this._offSide.width / 4)), this.profile.persona.colorUsable, new Depth(1f), false);
        DuckGame.Graphics.screen.End();
        this._renderingDuck = false;
        DuckGame.Graphics.SetRenderTarget((RenderTarget2D) null);
        DuckGame.Graphics.viewport = viewport;
      }
      this._gripped = false;
    }

    public override Thing realObject => this._trapped != null ? (Thing) this._trapped : (Thing) this;

    protected override bool OnBurn(Vec2 firePosition, Thing litBy)
    {
      if (this.holdObject != null && this.holdObject.superNonFlammable)
      {
        this.holdObject.DoHeatUp(0.1f, firePosition);
        return false;
      }
      this._burnTime -= 0.02f;
      if (!this.onFire)
      {
        if (!this.dead)
        {
          if (Network.isActive)
            this.Scream();
          else
            SFX.Play("quackYell0" + Change.ToString((object) (Rando.Int(2) + 1)), pitch: (Rando.Float(0.3f) - 0.3f));
          SFX.Play("ignite", pitch: (Rando.Float(0.3f) - 0.3f));
          if ((double) Rando.Float(1f) < 0.100000001490116)
            this.AddCoolness(-1);
          Event.Log((Event) new LitOnFireEvent(litBy?.responsibleProfile, this.profile));
          ++this.profile.stats.timesLitOnFire;
          if (Recorder.currentRecording != null)
            Recorder.currentRecording.LogAction(9);
          if (this.ragdoll == null)
          {
            for (int index = 0; index < 5; ++index)
              Level.Add((Thing) SmallFire.New(Rando.Float(12f) - 6f, Rando.Float(16f) - 8f, 0.0f, 0.0f, stick: ((MaterialThing) this)));
          }
        }
        this.onFire = true;
      }
      return true;
    }

    public virtual void UpdateHoldPosition(bool updateLerp = true)
    {
      this.armOffY = 6f;
      this.armOffX = -3f * (float) this.offDir;
      if (this.holdObject != null)
      {
        this.armOffY = 6f;
        this.armOffX = -2f * (float) this.offDir;
      }
      this.holdOffX = 6f;
      this.holdOffY = -3f;
      if (this.holdObject != null)
      {
        if (this.holdObject.owner != this)
          return;
        if (!this.onFire && (double) this.holdObject.heat > 0.400000005960464 && this.holdObject.physicsMaterial == PhysicsMaterial.Metal)
        {
          if (this._sizzle == null)
            this._sizzle = SFX.Play("sizzle", 0.6f, looped: true);
          this._handHeat += 0.016f;
          if ((double) this._handHeat > 1.10000002384186)
          {
            this._sizzle.Stop();
            this.Scream();
            this.ThrowItem();
            this._handHeat = 0.0f;
          }
        }
        else
        {
          if (this._sizzle != null)
          {
            this._sizzle.Stop();
            this._sizzle = (Sound) null;
          }
          this._handHeat = 0.0f;
        }
        if (this._sprite.currentAnimation == "run")
        {
          if (this._sprite.frame == 1)
            ++this.holdOffY;
          else if (this._sprite.frame == 2)
          {
            ++this.holdOffY;
            --this.holdOffX;
          }
          else if (this._sprite.frame == 3)
          {
            ++this.holdOffY;
            this.holdOffX -= 2f;
          }
          else if (this._sprite.frame == 4)
          {
            ++this.holdOffY;
            --this.holdOffX;
          }
          else if (this._sprite.frame == 5)
            ++this.holdOffY;
        }
        else if (this._sprite.currentAnimation == "jump")
        {
          if (this._sprite.frame == 0)
            ++this.holdOffY;
          else if (this._sprite.frame == 2)
            --this.holdOffY;
        }
      }
      else
      {
        if (this._sizzle != null)
        {
          this._sizzle.Stop();
          this._sizzle = (Sound) null;
        }
        this._handHeat = 0.0f;
      }
      this.holdOffX *= (float) this.offDir;
      if (this.holdObject == null || this.ragdoll != null && this.HasEquipment(typeof (FancyShoes)))
        return;
      this._spriteArms.angle = this.holdAngle;
      this._bionicArm.angle = this.holdAngle;
      if (this.gun != null)
        this.kick = this.gun.kick * 5f;
      if (this.holdObject is DrumSet)
        this.position = this.holdObject.position + new Vec2(0.0f, -12f);
      else
        this.holdObject.position = this.armPositionNoKick + this.holdObject.holdOffset + new Vec2(this.holdOffX, this.holdOffY) + new Vec2((float) (2 * (int) this.offDir), 0.0f);
      this.holdObject.CheckIfHoldObstructed();
      if (!(this.holdObject is RagdollPart))
        this.holdObject.offDir = this.offDir;
      if (this._sprite.currentAnimation == "slide")
      {
        --this.holdOffY;
        ++this.holdOffX;
      }
      else if (this._sprite.currentAnimation == "crouch")
      {
        if (this.holdObject != null)
          this.armOffY += 4f;
      }
      else if ((this._sprite.currentAnimation == "groundSlide" || this._sprite.currentAnimation == "dead") && this.holdObject != null)
        this.armOffY += 6f;
      if (this.holdObject.canRaise && (this._hovering && this.holdObject.hoverRaise || (this.holdObstructed || this.holdObject.keepRaised)))
      {
        if (updateLerp)
          this.holdAngleOff = Maths.LerpTowards(this.holdAngleOff, -1.570796f * (float) this.offDir, this.holdObject.raiseSpeed * 2f);
        this.holdObject.raised = true;
      }
      else
      {
        if (updateLerp)
          this.holdAngleOff = Maths.LerpTowards(this.holdAngleOff, 0.0f, (float) ((double) this.holdObject.raiseSpeed * 2.0 * 2.0));
        if (this.holdObject.raised)
          this.holdObject.raised = false;
      }
      if (this.holdObject is DrumSet)
        return;
      this.holdObject.position = this.HoldOffset(this.holdObject.holdOffset);
      if (this.holdObject is RagdollPart)
        return;
      this.holdObject.angle = this.holdObject.handAngle + this.holdAngleOff;
    }

    public Duck converted => this._converted;

    public void ConvertDuck(Duck to)
    {
      if (this._converted != to && to != null && to.profile != null)
        ++to.profile.stats.conversions;
      this._converted = to;
      this._spriteArms = to._spriteArms.CloneMap();
      this._spriteControlled = to._spriteControlled.CloneMap();
      this._spriteQuack = to._spriteQuack.CloneMap();
      this._sprite = to._sprite.CloneMap();
      this.graphic = (Sprite) this._sprite;
      if (!this.isConversionMessage)
      {
        Equipment equipment = this.GetEquipment(typeof (TeamHat));
        if (equipment != null)
          this.Unequip(equipment);
        if (to.profile.team.hasHat)
        {
          Hat hat = (Hat) new TeamHat(0.0f, 0.0f, to.profile.team);
          Level.Add((Thing) hat);
          this.Equip((Equipment) hat, false);
        }
      }
      for (int index = 0; index < 3; ++index)
        Level.Add((Thing) new MusketSmoke(this.x - 5f + Rando.Float(10f), (float) ((double) this.y + 6.0 - 3.0 + (double) Rando.Float(6f) - (double) index * 1.0))
        {
          move = {
            x = (Rando.Float(0.4f) - 0.2f),
            y = (Rando.Float(0.4f) - 0.2f)
          }
        });
      this.listenTime = 0;
      this.listening = false;
      this.vSpeed -= 5f;
      SFX.Play("convert");
    }

    public void DoFuneralStuff()
    {
      Vec2 position = this.position;
      if (this.ragdoll != null)
        position = this.ragdoll.position;
      for (int index = 0; index < 3; ++index)
        Level.Add((Thing) new MusketSmoke(position.x - 5f + Rando.Float(10f), (float) ((double) position.y + 6.0 - 3.0 + (double) Rando.Float(6f) - (double) index * 1.0))
        {
          move = {
            x = (Rando.Float(0.4f) - 0.2f),
            y = (Rando.Float(0.4f) - 0.2f)
          }
        });
      this._timeSinceFuneralPerformed = DateTime.Now;
      SFX.Play("death");
      ++this.profile.stats.funeralsRecieved;
    }

    public void LayToRest(Profile whoDid)
    {
      Vec2 position = this.position;
      if (this.ragdoll != null)
        position = this.ragdoll.position;
      if (!this.isConversionMessage)
      {
        Tombstone tombstone = new Tombstone(position.x, position.y);
        Level.Add((Thing) tombstone);
        tombstone.vSpeed = -2.5f;
      }
      this.DoFuneralStuff();
      if (this.ragdoll != null)
      {
        this.ragdoll.y += 10000f;
        this.ragdoll.part1.y += 10000f;
        this.ragdoll.part2.y += 10000f;
        this.ragdoll.part3.y += 10000f;
      }
      this.y += 10000f;
      if (whoDid == null)
        return;
      ++whoDid.stats.funeralsPerformed;
      whoDid.duck.AddCoolness(2);
    }

    public bool gripped
    {
      get => this._gripped;
      set => this._gripped = value;
    }

    public void UpdateLerp()
    {
      if ((double) this.lerpSpeed == 0.0)
        return;
      Duck duck = this;
      duck.lerpPosition = duck.lerpPosition + this.lerpVector * this.lerpSpeed;
    }

    public bool IsQuacking()
    {
      if (this.quack > 0)
        return true;
      return this._mindControl != null && this._derpMindControl;
    }

    public void DrawHat()
    {
      if (this.hat == null)
        return;
      if (this._sprite != null)
        this.hat.alpha = this._sprite.alpha;
      this.hat.offDir = this.offDir;
      this.hat.depth = this.depth + this.hat.equippedDepth;
      this.hat.frame = this.quack > 0 || this._mindControl != null && this._derpMindControl ? 1 : 0;
      this.hat.angle = this.angle;
      this.hat.Draw();
    }

    public Vec2 GetPos()
    {
      Vec2 position = this.position;
      if (this.ragdoll != null)
        position = this.ragdoll.part1.position;
      else if (this._trapped != null)
        position = this._trapped.position;
      return position;
    }

    public Vec2 GetEdgePos()
    {
      Vec2 cameraPosition = this.cameraPosition;
      float num = 14f;
      if ((double) cameraPosition.x < (double) Level.current.camera.left + (double) num)
        cameraPosition.x = Level.current.camera.left + num;
      if ((double) cameraPosition.x > (double) Level.current.camera.right - (double) num)
        cameraPosition.x = Level.current.camera.right - num;
      if ((double) cameraPosition.y < (double) Level.current.camera.top + (double) num)
        cameraPosition.y = Level.current.camera.top + num;
      if ((double) cameraPosition.y > (double) Level.current.camera.bottom - (double) num)
        cameraPosition.y = Level.current.camera.bottom - num;
      return cameraPosition;
    }

    public bool ShouldDrawIcon()
    {
      Vec2 position = this.position;
      if (this.ragdoll != null)
      {
        if (this.ragdoll.part1 == null)
          return false;
        position = this.ragdoll.part1.position;
      }
      else if (this._trapped != null)
        position = this._trapped.position;
      if (Network.isActive && this._trapped != null && (this._trappedInstance != null && !this._trappedInstance.visible))
        position = this.position;
      if (Network.isActive && this.ragdoll != null && (this._ragdollInstance != null && !this._ragdollInstance.visible))
        position = this.position;
      if ((double) position.y < -600.0)
        return false;
      float num = -6f;
      if (this.level == null || this.level.camera == null || (this.dead || VirtualTransition.doingVirtualTransition) || (!(Level.current is GameLevel) || !Level.current.simulatePhysics))
        return false;
      return (double) position.x < (double) this.level.camera.left + (double) num || (double) position.x > (double) this.level.camera.right - (double) num || (double) position.y < (double) this.level.camera.top + (double) num || (double) position.y > (double) this.level.camera.bottom - (double) num;
    }

    public void DrawIcon()
    {
      if (this._offSide == null || this._renderingDuck || !this.ShouldDrawIcon())
        return;
      Vec2 position = this.position;
      if (this.ragdoll != null)
        position = this.ragdoll.part1.position;
      else if (this._trapped != null)
        position = this._trapped.position;
      Vec2 p2 = position;
      float num = 14f;
      if ((double) position.x < (double) Level.current.camera.left + (double) num)
        position.x = Level.current.camera.left + num;
      if ((double) position.x > (double) Level.current.camera.right - (double) num)
        position.x = Level.current.camera.right - num;
      if ((double) position.y < (double) Level.current.camera.top + (double) num)
        position.y = Level.current.camera.top + num;
      if ((double) position.y > (double) Level.current.camera.bottom - (double) num)
        position.y = Level.current.camera.bottom - num;
      DuckGame.Graphics.Draw((Tex2D) this._offSide, position, new Rectangle?(), Color.White, 0.0f, new Vec2((float) (this._offSide.width / 2), (float) (this._offSide.height / 2)), new Vec2(0.5f, 0.5f), SpriteEffects.None, (Depth) (0.9f + this.depth.span));
      int frame = this._sprite.frame;
      this._sprite.imageIndex = 21;
      float rad = Maths.DegToRad(Maths.PointDirection(position, p2));
      this._sprite.depth = new Depth(1f);
      this._sprite.angle = -rad;
      this._sprite.flipH = false;
      this._sprite.UpdateSpriteBox();
      this._sprite.position = new Vec2(position.x + (float) Math.Cos((double) rad) * 11f, position.y - (float) Math.Sin((double) rad) * 11f);
      this._sprite.DrawWithoutUpdate();
      this._sprite.angle = 0.0f;
      this._sprite.imageIndex = frame;
      this._sprite.UpdateSpriteBox();
    }

    public override void Draw()
    {
      if (this._sprite == null || !this.localSpawnVisible)
        return;
      if (this.inNet)
      {
        this.DrawIcon();
      }
      else
      {
        if (Network.isActive && (this._trappedInstance != null && this._trappedInstance.visible || this._ragdollInstance != null && this._ragdollInstance.visible || this._cookedInstance != null && this._cookedInstance.visible))
          return;
        this.DrawIcon();
        if (!this._updatedAnimation)
          this.UpdateAnimation();
        this.UpdateCurrentAnimation();
        this._updatedAnimation = false;
        this._sprite.UpdateFrame();
        this._sprite.flipH = this.offDir < (sbyte) 0;
        this._spriteArms.depth = this.depth + 10;
        this._bionicArm.depth = this.depth + 10;
        this.DrawAIPath();
        SpriteMap spriteQuack = this._spriteQuack;
        SpriteMap spriteControlled = this._spriteControlled;
        SpriteMap sprite = this._sprite;
        SpriteMap spriteArms = this._spriteArms;
        double num1 = this._isGhost ? 0.5 : 1.0;
        double alpha = (double) this.alpha;
        double num2;
        float num3 = (float) (num2 = num1 * alpha);
        spriteArms.alpha = (float) num2;
        double num4;
        float num5 = (float) (num4 = (double) num3);
        sprite.alpha = (float) num4;
        double num6;
        float num7 = (float) (num6 = (double) num5);
        spriteControlled.alpha = (float) num6;
        double num8 = (double) num7;
        spriteQuack.alpha = (float) num8;
        this._spriteQuack.flipH = this._spriteControlled.flipH = this._sprite.flipH;
        this._spriteControlled.depth = this.depth;
        this._sprite.depth = this.depth;
        this._spriteQuack.depth = this.depth;
        this._sprite.angle = this._spriteQuack.angle = this._spriteControlled.angle = this.angle;
        if (this.IsQuacking())
          DuckGame.Graphics.Draw(this._mindControl == null || !this._derpMindControl ? this._spriteQuack : this._spriteControlled, this._sprite.imageIndex, this.x, this.y, this.xscale, this.yscale);
        else
          DuckGame.Graphics.DrawWithoutUpdate(this._sprite, this.x, this.y, this.xscale, this.yscale);
        if (this._renderingDuck)
        {
          if (this.holdObject != null)
            this.holdObject.Draw();
          foreach (Thing thing in this._equipment)
            thing.Draw();
        }
        if (this._mindControl != null && this._derpMindControl || this.listening)
        {
          this._swirlSpin += 0.2f;
          this._swirl.angle = this._swirlSpin;
          DuckGame.Graphics.Draw(this._swirl, this.x, this.y - 12f);
        }
        this.DrawHat();
        Grapple equipment = this.GetEquipment(typeof (Grapple)) as Grapple;
        bool flag = equipment != null;
        int num9 = 0;
        if (equipment != null && equipment.hookInGun)
          num9 = 36;
        this._spriteArms.imageIndex = this._sprite.imageIndex;
        if (!this.inNet && !this._gripped && !this.listening)
        {
          Vec2 vec2 = Vec2.Zero;
          if (this.gun != null)
            vec2 = -this.gun.barrelVector * this.kick;
          float num10 = Math.Abs((float) (((double) this._flapFrame - 4.0) / 4.0)) - 0.1f;
          if (!this._hovering)
            num10 = 0.0f;
          this._spriteArms._frameInc = 0.0f;
          this._spriteArms.flipH = this._sprite.flipH;
          if (this.holdObject != null && !this.holdObject.ignoreHands)
          {
            if (!flag)
            {
              bool flipH = this._spriteArms.flipH;
              if (this.holdObject.handFlip)
                this._spriteArms.flipH = !this._spriteArms.flipH;
              DuckGame.Graphics.Draw(this._spriteArms, this._sprite.imageIndex + 18 + Maths.Int(this.action) * 18 * (this.holdObject.hasTrigger ? 1 : 0), this.armPosition.x + this.holdObject.handOffset.x * (float) this.offDir, this.armPosition.y + this.holdObject.handOffset.y, this._sprite.xscale, this._sprite.yscale);
              this._spriteArms._frameInc = 0.0f;
              this._spriteArms.flipH = flipH;
              if (this._sprite.currentAnimation == "jump")
              {
                this._spriteArms.angle = 0.0f;
                this._spriteArms.depth = this.depth + -10;
                DuckGame.Graphics.Draw(this._spriteArms, this._sprite.imageIndex + 5 + (int) Math.Round((double) num10 * 2.0), (float) ((double) this.x + (double) vec2.x + (double) (2 * (int) this.offDir) * (double) this.xscale), (float) ((double) this.y + (double) vec2.y + (double) this.armOffY * (double) this.yscale), -this._sprite.xscale, this._sprite.yscale, true);
                this._spriteArms.depth = this.depth + 10;
              }
            }
            else
            {
              this._bionicArm.flipH = this._sprite.flipH;
              if (this.holdObject.handFlip)
                this._bionicArm.flipH = !this._bionicArm.flipH;
              DuckGame.Graphics.Draw(this._bionicArm, this._sprite.imageIndex + 18 + num9, this.armPosition.x + this.holdObject.handOffset.x * (float) this.offDir, this.armPosition.y + this.holdObject.handOffset.y, this._sprite.xscale, this._sprite.yscale);
            }
          }
          else if (!this._closingEyes)
          {
            if (!flag)
            {
              this._spriteArms.angle = 0.0f;
              if (this._sprite.currentAnimation == "jump" && this._spriteArms.imageIndex == 9)
              {
                int num11 = 2;
                if (this.HasEquipment(typeof (ChestPlate)))
                  num11 = 3;
                this._spriteArms.depth = this.depth + 1;
                DuckGame.Graphics.Draw(this._spriteArms, this._spriteArms.imageIndex + 5 + (int) Math.Round((double) num10 * 2.0), (float) ((double) this.x + (double) vec2.x - (double) ((int) this.offDir * num11) * (double) this.xscale), (float) ((double) this.y + (double) vec2.y + (double) this.armOffY * (double) this.yscale), this._sprite.xscale, this._sprite.yscale, true);
                this._spriteArms.depth = this.depth + -10;
                if (this.holdObject == null || !this.holdObject.ignoreHands)
                {
                  this._spriteArms.imageIndex = 9;
                  DuckGame.Graphics.Draw(this._spriteArms, this._spriteArms.imageIndex + 5 + (int) Math.Round((double) num10 * 2.0), (float) ((double) this.x + (double) vec2.x + (double) (2 * (int) this.offDir) * (double) this.xscale), (float) ((double) this.y + (double) vec2.y + (double) this.armOffY * (double) this.yscale), -this._sprite.xscale, this._sprite.yscale, true);
                  this._spriteArms.depth = this.depth + 10;
                }
              }
              else
                DuckGame.Graphics.Draw(this._spriteArms, this._sprite.imageIndex, this.armPosition.x, this.armPosition.y, this._sprite.xscale, this._sprite.yscale);
            }
            else
            {
              this._bionicArm.angle = 0.0f;
              this._bionicArm.flipH = this._sprite.flipH;
              DuckGame.Graphics.Draw(this._bionicArm, this._sprite.imageIndex + num9, this.armPosition.x, this.armPosition.y, this._sprite.xscale, this._sprite.yscale);
            }
          }
        }
        Sprite graphic = this.graphic;
        this.graphic = (Sprite) null;
        base.Draw();
        this.graphic = graphic;
      }
    }

    private void DrawAIPath()
    {
      if (this.ai == null)
        return;
      this.ai.Draw();
    }
  }
}
