// Decompiled with JetBrains decompiler
// Type: DuckGame.Door
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace DuckGame
{
  [EditorGroup("stuff")]
  public class Door : Block, IPlatform, IDontMove, ISequenceItem
  {
    public StateBinding _hitPointsBinding = new StateBinding("_hitPoints");
    public StateBinding _openBinding = new StateBinding(nameof (_open));
    public StateBinding _openForceBinding = new StateBinding(nameof (_openForce));
    public StateBinding _jiggleBinding = new StateBinding(nameof (_jiggle));
    public StateBinding _jamBinding = new StateBinding(nameof (_jam));
    public StateBinding _positionBinding = new StateBinding(nameof (netPosition));
    public StateBinding _damageMultiplierBinding = new StateBinding(nameof (damageMultiplier));
    public StateBinding _doorInstanceBinding = new StateBinding(nameof (_doorInstance));
    public StateBinding _doorStateBinding = (StateBinding) new StateFlagBinding(new string[4]
    {
      nameof (_didJiggle),
      nameof (_jammed),
      "_destroyed",
      nameof (locked)
    });
    public DoorOffHinges _doorInstance;
    public float damageMultiplier = 1f;
    protected SpriteMap _sprite;
    public bool landed = true;
    public bool locked;
    public bool _lockDoor;
    public float _open;
    public float _openForce;
    private Vec2 _topLeft;
    private Vec2 _topRight;
    private Vec2 _bottomLeft;
    private Vec2 _bottomRight;
    private bool _cornerInit;
    public bool _jammed;
    public float _jiggle;
    public bool _didJiggle;
    public SinWave _wave = (SinWave) 0.4f;
    public new bool _initialized;
    public float colWide = 6f;
    public float _jam = 1f;
    private Dictionary<Mine, float> _mines = new Dictionary<Mine, float>();
    private Sprite _lock;
    private bool _opened;
    private SpriteMap _key;
    private DoorFrame _frame;
    private bool _fucked;
    public EditorProperty<bool> objective;
    protected bool secondaryFrame;
    private bool _lockedSprite;
    public bool networkUnlockMessage;
    private bool didUnlock;
    private bool prevLocked;
    private List<Mine> _removeMines = new List<Mine>();

    public override Vec2 netPosition
    {
      get => this.position;
      set
      {
        if (!(this.position != value))
          return;
        this.position = value;
        if (this._frame != null)
          this._frame.position = this.position + new Vec2(0.0f, -1f);
        Level.current.things.quadTree.Remove((Thing) this);
        Level.current.things.quadTree.Add((Thing) this);
      }
    }

    public override void SetTranslation(Vec2 translation)
    {
      if (this._frame != null)
        this._frame.SetTranslation(translation);
      base.SetTranslation(translation);
    }

    public override void EditorPropertyChanged(object property) => this.sequence.isValid = this.objective.value;

    public Door(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.objective = new EditorProperty<bool>(false, (Thing) this);
      this._maxHealth = 50f;
      this._hitPoints = 50f;
      this._sprite = new SpriteMap("door", 32, 32);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(16f, 25f);
      this.collisionSize = new Vec2(6f, 32f);
      this.collisionOffset = new Vec2(-3f, -25f);
      this.depth = new Depth(-0.5f);
      this._editorName = nameof (Door);
      this.thickness = 2f;
      this._lock = new Sprite("lock");
      this._lock.CenterOrigin();
      this._impactThreshold = 0.0f;
      this._key = new SpriteMap("keyInDoor", 16, 16);
      this._key.center = new Vec2(2f, 8f);
      this._canFlip = false;
      this.sequence = new SequenceItem((Thing) this);
      this.sequence.type = SequenceItemType.Goody;
    }

    public override void Initialize()
    {
      this.sequence.isValid = this.objective.value;
      this._lockDoor = this.locked;
      if (this._lockDoor)
      {
        this._sprite = new SpriteMap("lockDoor", 32, 32);
        this.graphic = (Sprite) this._sprite;
        this._lockedSprite = true;
      }
      else
      {
        this._frame = new DoorFrame(this.x, this.y - 1f, this.secondaryFrame);
        Level.Add((Thing) this._frame);
        if (!Network.isActive || !Network.isServer)
          return;
        this._doorInstance = new DoorOffHinges(this.x, this.y - 8f, this.secondaryFrame);
        this._doorInstance.active = false;
        this._doorInstance.visible = false;
        this._doorInstance.solid = false;
        Level.Add((Thing) this._doorInstance);
      }
    }

    public override void Terminate()
    {
      if ((double) this._hitPoints > 5.0 && !Network.isActive)
      {
        Level.Remove((Thing) this._frame);
        this._frame = (DoorFrame) null;
      }
      base.Terminate();
    }

    protected override bool OnDestroy(DestroyType type = null)
    {
      if (this._lockDoor || this._destroyed)
        return false;
      this._hitPoints = 0.0f;
      Level.Remove((Thing) this);
      if (this.sequence != null && this.sequence.isValid)
      {
        this.sequence.Finished();
        if (ChallengeLevel.running)
          ++ChallengeLevel.goodiesGot;
      }
      DoorOffHinges doorOffHinges = (DoorOffHinges) null;
      if (Network.isActive)
      {
        if (this._doorInstance != null)
        {
          doorOffHinges = this._doorInstance;
          doorOffHinges.visible = true;
          doorOffHinges.active = true;
          doorOffHinges.solid = true;
          Thing.Fondle((Thing) this, DuckNetwork.localConnection);
          Thing.Fondle((Thing) doorOffHinges, DuckNetwork.localConnection);
        }
      }
      else
        doorOffHinges = new DoorOffHinges(this.x, this.y - 8f, this.secondaryFrame);
      if (doorOffHinges != null)
      {
        if (type is DTShot dtShot && dtShot.bullet != null)
        {
          doorOffHinges.hSpeed = dtShot.bullet.travelDirNormalized.x * 2f;
          doorOffHinges.vSpeed = (float) ((double) dtShot.bullet.travelDirNormalized.y * 2.0 - 1.0);
          doorOffHinges.offDir = (double) dtShot.bullet.travelDirNormalized.x > 0.0 ? (sbyte) 1 : (sbyte) -1;
        }
        else
        {
          doorOffHinges.hSpeed = (float) this.offDir * 2f;
          doorOffHinges.vSpeed = -2f;
          doorOffHinges.offDir = this.offDir;
        }
        if (!Network.isActive)
        {
          Level.Add((Thing) doorOffHinges);
          doorOffHinges.MakeEffects();
        }
      }
      return true;
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos)
    {
      if (bullet.isLocal)
        Thing.Fondle((Thing) this, DuckNetwork.localConnection);
      if ((double) this._hitPoints <= 0.0)
        return base.Hit(bullet, hitPos);
      hitPos -= bullet.travelDirNormalized;
      if (this.physicsMaterial == PhysicsMaterial.Wood)
      {
        for (int index = 0; (double) index < 1.0 + (double) this.damageMultiplier / 2.0; ++index)
        {
          Thing thing = (Thing) WoodDebris.New(hitPos.x, hitPos.y);
          thing.hSpeed = (float) (-(double) bullet.travelDirNormalized.x * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929));
          thing.vSpeed = (float) (-(double) bullet.travelDirNormalized.y * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929)) - Rando.Float(2f);
          Level.Add(thing);
        }
        SFX.Play("woodHit");
      }
      if (bullet.isLocal)
      {
        this._hitPoints -= this.damageMultiplier * 4f;
        ++this.damageMultiplier;
        if ((double) this._hitPoints <= 0.0 && !this.destroyed)
          this.Destroy((DestroyType) new DTShot(bullet));
      }
      return base.Hit(bullet, hitPos);
    }

    public override void ExitHit(Bullet bullet, Vec2 exitPos)
    {
      exitPos += bullet.travelDirNormalized;
      for (int index = 0; (double) index < 1.0 + (double) this.damageMultiplier / 2.0; ++index)
      {
        Thing thing = (Thing) WoodDebris.New(exitPos.x, exitPos.y);
        thing.hSpeed = (float) ((double) bullet.travelDirNormalized.x * 3.0 * ((double) Rando.Float(1f) + 0.300000011920929));
        thing.vSpeed = (float) ((double) bullet.travelDirNormalized.y * 3.0 * ((double) Rando.Float(1f) + 0.300000011920929) - ((double) Rando.Float(2f) - 1.0));
        Level.Add(thing);
      }
    }

    public override void OnSoftImpact(MaterialThing with, ImpactedFrom from)
    {
      if (with.isServerForObject && this.locked && with is Key)
      {
        if (Network.isActive)
        {
          with.Fondle((Thing) this);
          Send.Message((NetMessage) new NMUnlockDoor(this));
          this.networkUnlockMessage = true;
        }
        this.UnlockDoor(with as Key);
      }
      base.OnSoftImpact(with, from);
    }

    public void UnlockDoor(Key with)
    {
      if (!this.locked)
        return;
      this.locked = false;
      if (with.owner is Duck owner)
        owner.ThrowItem();
      Level.Remove((Thing) with);
      if (Network.isActive)
        return;
      this.DoUnlock(with.position);
    }

    public void DoUnlock(Vec2 keyPos)
    {
      SFX.Play("deedleBeep");
      Level.Add((Thing) SmallSmoke.New(keyPos.x, keyPos.y));
      for (int index = 0; index < 3; ++index)
        Level.Add((Thing) SmallSmoke.New(this.x + Rando.Float(-3f, 3f), this.y + Rando.Float(-3f, 3f)));
      this.didUnlock = true;
    }

    public override void Update()
    {
      if (!this._lockDoor && this.locked)
      {
        this._sprite = new SpriteMap("lockDoor", 32, 32);
        this.graphic = (Sprite) this._sprite;
        this._lockedSprite = true;
        this._lockDoor = true;
      }
      if (this.networkUnlockMessage)
        this.locked = false;
      if (Network.isActive && !this.locked && (this.prevLocked && !this.didUnlock))
        this.DoUnlock(this.position);
      this.prevLocked = this.locked;
      if (this._lockDoor)
      {
        this._hitPoints = 100f;
        this.physicsMaterial = PhysicsMaterial.Metal;
        this.thickness = 4f;
      }
      if (!this._fucked && (double) this._hitPoints < (double) this._maxHealth / 2.0)
      {
        this._sprite = new SpriteMap(this.secondaryFrame ? "flimsyDoorDamaged" : "doorFucked", 32, 32);
        this.graphic = (Sprite) this._sprite;
        this._fucked = true;
      }
      if (!this._cornerInit)
      {
        this._topLeft = this.topLeft;
        this._topRight = this.topRight;
        this._bottomLeft = this.bottomLeft;
        this._bottomRight = this.bottomRight;
        this._cornerInit = true;
      }
      base.Update();
      if ((double) this.damageMultiplier > 1.0)
        this.damageMultiplier -= 0.2f;
      else
        this.damageMultiplier = 1f;
      this._removeMines.Clear();
      foreach (KeyValuePair<Mine, float> mine in this._mines)
      {
        if ((double) mine.Value < 0.0 && (double) this._open > (double) mine.Value || (double) mine.Value >= 0.0 && (double) this._open < (double) mine.Value)
        {
          mine.Key.addWeight = 0.0f;
          this._removeMines.Add(mine.Key);
        }
        else
          mine.Key.addWeight = 3f;
      }
      foreach (Mine removeMine in this._removeMines)
        this._mines.Remove(removeMine);
      bool flag1 = false;
      PhysicsObject physicsObject1 = (PhysicsObject) null;
      if ((double) this._open < 0.899999976158142 && (double) this._open > -0.899999976158142)
      {
        bool flag2 = false;
        Thing thing = (Thing) Level.CheckRectAll<Duck>(this._topLeft - new Vec2(18f, 0.0f), this._bottomRight + new Vec2(18f, 0.0f)).FirstOrDefault<Duck>((Func<Duck, bool>) (d => !(d is TargetDuck)));
        if (thing == null)
        {
          thing = (Thing) Level.CheckRectAll<Duck>(this._topLeft - new Vec2(32f, 0.0f), this._bottomRight + new Vec2(32f, 0.0f)).FirstOrDefault<Duck>((Func<Duck, bool>) (d => !(d is TargetDuck) && (double) Math.Abs(d.hSpeed) > 4.0));
          flag2 = true;
        }
        if (thing != null)
        {
          (thing as Duck).Fondle((Thing) this);
          if ((double) thing.x < (double) this.x)
          {
            IEnumerable<PhysicsObject> physicsObjects = Level.CheckRectAll<PhysicsObject>(this._topRight, this._bottomRight + new Vec2(10f, 0.0f));
            bool flag3 = true;
            this._jam = 1f;
            foreach (PhysicsObject physicsObject2 in physicsObjects)
            {
              if (!(physicsObject2 is Duck) && (double) physicsObject2.weight > 3.0 && physicsObject2.owner == null && (!(physicsObject2 is Holdable) || (physicsObject2 as Holdable).hoverSpawner == null))
              {
                if (physicsObject2 is RagdollPart)
                {
                  this.Fondle((Thing) physicsObject2);
                  physicsObject2.hSpeed = 2f;
                }
                else
                {
                  float num = Maths.Clamp((float) (((double) physicsObject2.left - (double) this._bottomRight.x) / 14.0), 0.0f, 1f);
                  if ((double) num < 0.100000001490116)
                    num = 0.1f;
                  if ((double) this._jam > (double) num)
                  {
                    if ((double) this._open != 0.0 && physicsObject2 is Gun)
                    {
                      if (physicsObject2 is Mine key && !key.pin && !this._mines.ContainsKey(key))
                        this._mines[key] = this._open;
                    }
                    else
                    {
                      this._jam = num;
                      physicsObject1 = physicsObject2;
                    }
                  }
                }
              }
            }
            if (this.locked)
            {
              this._jam = 0.1f;
              if (!this._didJiggle)
              {
                this._jiggle = 1f;
                this._didJiggle = true;
              }
            }
            if (flag3)
            {
              if (flag2)
                this._openForce += 0.25f;
              else
                this._openForce += 0.08f;
            }
          }
          else
          {
            IEnumerable<PhysicsObject> physicsObjects = Level.CheckRectAll<PhysicsObject>(this._topLeft - new Vec2(10f, 0.0f), this._bottomLeft);
            bool flag3 = true;
            this._jam = -1f;
            foreach (PhysicsObject physicsObject2 in physicsObjects)
            {
              if (!(physicsObject2 is Duck) && (double) physicsObject2.weight > 3.0 && physicsObject2.owner == null && (!(physicsObject2 is Holdable) || (physicsObject2 as Holdable).hoverSpawner == null))
              {
                if (physicsObject2 is RagdollPart)
                {
                  this.Fondle((Thing) physicsObject2);
                  physicsObject2.hSpeed = -2f;
                }
                else
                {
                  float num = Maths.Clamp((float) (((double) physicsObject2.right - (double) this.left) / 14.0), -1f, 0.0f);
                  if ((double) num > -0.100000001490116)
                    num = -0.1f;
                  if ((double) this._jam < (double) num)
                  {
                    if ((double) this._open != 0.0 && physicsObject2 is Gun)
                    {
                      if (physicsObject2 is Mine key && !key.pin && !this._mines.ContainsKey(key))
                        this._mines[key] = this._open;
                    }
                    else
                    {
                      this._jam = num;
                      physicsObject1 = physicsObject2;
                    }
                  }
                }
              }
            }
            if (this.locked)
            {
              this._jam = -0.1f;
              if (!this._didJiggle)
              {
                this._jiggle = 1f;
                this._didJiggle = true;
              }
            }
            if (flag3)
            {
              if (flag2)
                this._openForce -= 0.25f;
              else
                this._openForce -= 0.08f;
            }
          }
        }
        else
          this._didJiggle = false;
      }
      foreach (PhysicsObject physicsObject2 in Level.CheckRectAll<PhysicsObject>(this._topLeft - new Vec2(18f, 0.0f), this._bottomRight + new Vec2(18f, 0.0f)))
      {
        if ((physicsObject2 is Duck || !this._jammed) && (!(physicsObject2 is Holdable) || physicsObject2 is Mine || (physicsObject2 as Holdable).canPickUp) && physicsObject2.solid)
        {
          if (!(physicsObject2 is Duck) && (double) this.weight < 3.0)
          {
            if ((double) this._open < -0.0)
            {
              this.Fondle((Thing) physicsObject2);
              physicsObject2.hSpeed = 3f;
            }
            else if ((double) this._open > 0.0)
            {
              this.Fondle((Thing) physicsObject2);
              physicsObject2.hSpeed = -3f;
            }
          }
          if ((double) this._open < -0.0 && physicsObject2 != null && (physicsObject2 is Duck || (double) physicsObject2.right > (double) this._topLeft.x - 10.0 && (double) physicsObject2.left < (double) this._topRight.x))
            flag1 = true;
          if ((double) this._open > 0.0 && physicsObject2 != null && (physicsObject2 is Duck || (double) physicsObject2.left < (double) this._topRight.x + 10.0 && (double) physicsObject2.right > (double) this._topLeft.x))
            flag1 = true;
        }
      }
      this._jiggle = Maths.CountDown(this._jiggle, 0.08f);
      if (!flag1)
      {
        if ((double) this._openForce > 1.0)
          this._openForce = 1f;
        if ((double) this._openForce < -1.0)
          this._openForce = -1f;
        if ((double) this._openForce > 0.0399999991059303)
          this._openForce -= 0.04f;
        else if ((double) this._openForce < -0.0399999991059303)
          this._openForce += 0.04f;
        else if ((double) this._openForce > -0.0599999986588955 && (double) this._openForce < 0.0599999986588955)
          this._openForce = 0.0f;
      }
      this._open += this._openForce;
      if ((double) Math.Abs(this._open) > 0.5 && !this._opened)
      {
        this._opened = true;
        SFX.Play("doorOpen", Rando.Float(0.8f, 0.9f), Rando.Float(-0.1f, 0.1f));
      }
      else if ((double) Math.Abs(this._open) < 0.100000001490116 && this._opened)
      {
        this._opened = false;
        SFX.Play("doorClose", Rando.Float(0.5f, 0.6f), Rando.Float(-0.1f, 0.1f));
      }
      if ((double) this._open > 1.0)
        this._open = 1f;
      if ((double) this._open < -1.0)
        this._open = -1f;
      if ((double) this._jam > 0.0 && (double) this._open > (double) this._jam)
      {
        if (!this._jammed)
        {
          SFX.Play("doorJam");
          this._jammed = true;
          if (physicsObject1 != null)
          {
            physicsObject1.hSpeed += 0.6f;
            this.Fondle((Thing) physicsObject1);
          }
        }
        this._open = this._jam;
        if ((double) this._openForce > 0.100000001490116)
          this._openForce = 0.1f;
      }
      if ((double) this._jam < 0.0 && (double) this._open < (double) this._jam)
      {
        if (!this._jammed)
        {
          SFX.Play("doorJam");
          this._jammed = true;
          if (physicsObject1 != null)
          {
            physicsObject1.hSpeed -= 0.6f;
            this.Fondle((Thing) physicsObject1);
          }
        }
        this._open = this._jam;
        if ((double) this._openForce < -0.100000001490116)
          this._openForce = -0.1f;
      }
      if ((double) this._open > 0.0)
      {
        this._sprite.flipH = false;
        this._sprite.frame = (int) ((double) this._open * 15.0);
      }
      else
      {
        this._sprite.flipH = true;
        this._sprite.frame = (int) ((double) Math.Abs(this._open) * 15.0);
      }
      if (this._sprite.frame > 9)
      {
        this.collisionSize = new Vec2(0.0f, 0.0f);
        this.solid = false;
        this.collisionOffset = new Vec2(0.0f, -999999f);
        this.depth = new Depth(-0.7f);
      }
      else
      {
        this.collisionSize = new Vec2(this.colWide, 32f);
        this.solid = true;
        this.collisionOffset = new Vec2((float) (-(double) this.colWide / 2.0), -24f);
        this.depth = new Depth(-0.5f);
      }
      if ((double) this._hitPoints <= 0.0 && !this._destroyed)
        this.Destroy((DestroyType) new DTImpact((Thing) this));
      if ((double) this._openForce == 0.0)
        this._open = Maths.LerpTowards(this._open, 0.0f, 0.1f);
      if ((double) this._open == 0.0)
        this._jammed = false;
      float num1 = (float) ((double) this._hitPoints / (double) this._maxHealth * 0.200000002980232 + 0.800000011920929);
      this._sprite.color = new Color(num1, num1, num1);
    }

    public override void Draw()
    {
      base.Draw();
      if (Level.current is Editor)
      {
        if (this.locked && !this._lockedSprite)
        {
          this._sprite = new SpriteMap("lockDoor", 32, 32);
          this.graphic = (Sprite) this._sprite;
          this._lockedSprite = true;
        }
        else if (!this.locked && this._lockedSprite)
        {
          this._sprite = new SpriteMap("door", 32, 32);
          this.graphic = (Sprite) this._sprite;
          this._lockedSprite = false;
        }
      }
      if (!this._lockDoor || this.locked)
        return;
      this._key.frame = this._sprite.frame;
      if (this._key.frame > 12)
        this._key.depth = this.depth - 1;
      else
        this._key.depth = this.depth + 1;
      this._key.flipH = this.graphic.flipH;
      Graphics.Draw((Sprite) this._key, this.x + this._open * 12f, this.y - 8f);
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = base.Serialize();
      binaryClassChunk.AddProperty("locked", (object) this.locked);
      return binaryClassChunk;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      base.Deserialize(node);
      this.locked = node.GetProperty<bool>("locked");
      return true;
    }

    public override XElement LegacySerialize()
    {
      XElement xelement = base.LegacySerialize();
      xelement.Add((object) new XElement((XName) "locked", (object) Change.ToString((object) this.locked)));
      return xelement;
    }

    public override bool LegacyDeserialize(XElement node)
    {
      base.LegacyDeserialize(node);
      XElement xelement = node.Element((XName) "locked");
      if (xelement != null)
        this.locked = Convert.ToBoolean(xelement.Value);
      return true;
    }

    public override ContextMenu GetContextMenu()
    {
      ContextMenu contextMenu = base.GetContextMenu();
      contextMenu.AddItem((ContextMenu) new ContextCheckBox("Locked", (IContextListener) null, new FieldBinding((object) this, "locked")));
      return contextMenu;
    }
  }
}
