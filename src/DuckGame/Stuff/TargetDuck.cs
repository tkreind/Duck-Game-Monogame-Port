// Decompiled with JetBrains decompiler
// Type: DuckGame.TargetDuck
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Linq;
using System.Xml.Linq;

namespace DuckGame
{
  [BaggedProperty("canSpawn", false)]
  [EditorGroup("stuff|props")]
  [BaggedProperty("isOnlineCapable", false)]
  public class TargetDuck : Duck, ISequenceItem
  {
    private new SpriteMap _sprite;
    private Sprite _base;
    private Sprite _woodWing;
    private bool _popup;
    private float _upSpeed;
    private bool _up;
    private TargetStance _stance;
    public System.Type contains;
    public bool chestPlate;
    public bool helmet;
    private float _timeCount;
    public EditorProperty<float> time = new EditorProperty<float>(0.0f, max: 30f, minSpecial: "INF");
    public EditorProperty<float> autofire = new EditorProperty<float>(0.0f, max: 100f, minSpecial: "INF");
    public EditorProperty<bool> random = new EditorProperty<bool>(false);
    public EditorProperty<int> maxrandom = new EditorProperty<int>(1, min: 1f, max: 32f, increment: 1f);
    public EditorProperty<bool> dropgun = new EditorProperty<bool>(true);
    private float _autoFireWait;
    private bool editorUpdate;
    private float _waitFire = 1f;
    private int _stanceSetting;

    public TargetDuck(float xpos, float ypos, TargetStance stance)
      : base(xpos, ypos, (Profile) null)
    {
      this._sprite = new SpriteMap("woodDuck", 32, 32);
      this._base = new Sprite("popupPad");
      this._woodWing = new Sprite("woodWing");
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(16f, 22f);
      this._stance = stance;
      this.UpdateCollision();
      this.physicsMaterial = PhysicsMaterial.Wood;
      this.thickness = 0.5f;
      this._hitPoints = this._maxHealth = 0.1f;
      this.hugWalls = WallHug.Floor;
      this._canHaveChance = false;
      this.sequence = new SequenceItem((Thing) this);
      this.sequence.type = SequenceItemType.Target;
    }

    public override void Initialize()
    {
      this._profile = Profiles.EnvironmentProfile;
      this.InitProfile();
      this._sprite = new SpriteMap("woodDuck", 32, 32);
      this._base = new Sprite("popupPad");
      this._woodWing = new Sprite("woodWing");
      this.graphic = (Sprite) this._sprite;
      if (!(Level.current is Editor))
      {
        if (this._stance != TargetStance.Fly)
          this.scale = new Vec2(1f, 0.0f);
        else
          this.scale = new Vec2(0.0f, 1f);
        ChallengeLevel.allTargetsShot = false;
        this._autoFireWait = this.autofire.value;
      }
      this.UpdateCollision();
    }

    public void SpawnHoldObject()
    {
      this._autoFireWait = (float) this.autofire;
      if (!(this.contains != (System.Type) null) || !(Editor.CreateThing(this.contains) is Holdable thing))
        return;
      Level.Add((Thing) thing);
      this.GiveThing(thing);
    }

    public override void ReturnItemToWorld(Thing t)
    {
      Vec2 p1 = this.position + new Vec2((float) ((int) this.offDir * 3), 0.0f);
      Block block1 = Level.CheckLine<Block>(p1, p1 + new Vec2(16f, 0.0f));
      if (block1 != null && block1.solid && (double) t.right > (double) block1.left)
        t.right = block1.left;
      Block block2 = Level.CheckLine<Block>(p1, p1 - new Vec2(16f, 0.0f));
      if (block2 != null && block2.solid && (double) t.left < (double) block2.right)
        t.left = block2.right;
      Block block3 = Level.CheckLine<Block>(p1, p1 + new Vec2(0.0f, -16f));
      if (block3 != null && block3.solid && (double) t.top < (double) block3.bottom)
        t.top = block3.bottom;
      Block block4 = Level.CheckLine<Block>(p1, p1 + new Vec2(0.0f, 16f));
      if (block4 == null || !block4.solid || (double) t.bottom <= (double) block4.top)
        return;
      t.bottom = block4.top;
    }

    public void UpdateCollision()
    {
      if (Level.current is Editor || Level.current == null || this._up && this._popup)
      {
        this.crouch = false;
        this.sliding = false;
        if (this._stance == TargetStance.Stand)
        {
          this._sprite.frame = 0;
          this._collisionOffset = new Vec2(-6f, -24f);
          this.collisionSize = new Vec2(12f, 24f);
          this.hugWalls = WallHug.Floor;
        }
        else if (this._stance == TargetStance.StandArmed)
        {
          this._sprite.frame = 1;
          this._collisionOffset = new Vec2(-6f, -23f);
          this.collisionSize = new Vec2(12f, 23f);
          this.hugWalls = WallHug.Floor;
        }
        else if (this._stance == TargetStance.Crouch)
        {
          this._sprite.frame = 2;
          this._collisionOffset = new Vec2(-6f, -18f);
          this.collisionSize = new Vec2(12f, 18f);
          this.crouch = true;
          this.hugWalls = WallHug.Floor;
        }
        else if (this._stance == TargetStance.Slide)
        {
          this._sprite.frame = 3;
          this._collisionOffset = new Vec2(-6f, -10f);
          this.collisionSize = new Vec2(12f, 10f);
          this.sliding = true;
          this.hugWalls = WallHug.Floor;
        }
        else if (this._stance == TargetStance.Fly)
        {
          this._sprite.frame = 4;
          this._collisionOffset = new Vec2(-8f, -24f);
          this.collisionSize = new Vec2(16f, 24f);
          this.hugWalls = WallHug.Left | WallHug.Right;
        }
      }
      else
      {
        this.hugWalls = WallHug.Floor;
        if (this._stance == TargetStance.Stand)
          this._sprite.frame = 0;
        else if (this._stance == TargetStance.StandArmed)
          this._sprite.frame = 1;
        else if (this._stance == TargetStance.Crouch)
          this._sprite.frame = 2;
        else if (this._stance == TargetStance.Slide)
          this._sprite.frame = 3;
        else if (this._stance == TargetStance.Fly)
        {
          this._sprite.frame = 4;
          this.hugWalls = WallHug.Left | WallHug.Right;
        }
        this._collisionOffset = new Vec2(-6000f, 0.0f);
        this.collisionSize = new Vec2(2f, 2f);
      }
      this._collisionOffset.y += 10f;
      --this._collisionSize.y;
      this._featherVolume.collisionSize = new Vec2(this.collisionSize.x + 2f, this.collisionSize.y + 2f);
      this._featherVolume.collisionOffset = new Vec2(this.collisionOffset.x - 1f, this.collisionOffset.y - 1f);
    }

    public override void OnSequenceActivate() => this._popup = true;

    public void PopDown()
    {
      this._popup = false;
      if (this.holdObject != null)
      {
        Level.Remove((Thing) this.holdObject);
        this.holdObject = (Holdable) null;
      }
      foreach (Equipment equipment in this._equipment)
      {
        if (equipment != null)
          Level.Remove((Thing) equipment);
      }
      this._equipment.Clear();
      this._sequence.Finished();
    }

    public override bool Kill(DestroyType type = null)
    {
      if (this._up && this._popup)
      {
        if (ChallengeLevel.running)
          ++ChallengeLevel.targetsShot;
        if (this.holdObject is Gun && !(bool) this.dropgun)
          (this.holdObject as Gun).ammo = 0;
        this.ThrowItem(false);
        foreach (Equipment equipment in this._equipment)
        {
          if (equipment != null)
          {
            equipment.owner = (Thing) null;
            equipment.hSpeed = Rando.Float(2f) - 1f;
            equipment.vSpeed = -Rando.Float(1.5f);
            this.ReturnItemToWorld((Thing) equipment);
            equipment.UnEquip();
          }
        }
        SFX.Play("ting", Rando.Float(0.7f, 0.8f), Rando.Float(-0.2f, 0.2f));
        if (type is DTShot)
          SFX.Play("targetRebound", Rando.Float(0.7f, 0.8f), Rando.Float(-0.2f, 0.2f));
        Vec2 vec2 = Vec2.Zero;
        if (type is DTShot)
          vec2 = (type as DTShot).bullet.travelDirNormalized;
        for (int index = 0; index < 4; ++index)
        {
          Thing thing = (Thing) WoodDebris.New(this.x - 8f + Rando.Float(16f), this.y - 20f + Rando.Float(16f));
          thing.hSpeed = (float) (((double) Rando.Float(1f) > 0.5 ? 1.0 : -1.0) * (double) Rando.Float(3f) + (double) Math.Sign(vec2.x) * 0.5);
          thing.vSpeed = -Rando.Float(1f);
          Level.Add(thing);
        }
        for (int index = 0; index < 2; ++index)
          Level.Add((Thing) Feather.New(this.x, this.y - 16f, this.persona));
        this.PopDown();
      }
      return false;
    }

    public override bool Hit(Bullet bullet, Vec2 hitPos) => this._up && this._popup && base.Hit(bullet, hitPos);

    public override void ExitHit(Bullet bullet, Vec2 hitPos)
    {
      if (!this._up || this._popup)
        return;
      for (int index = 0; index < 2; ++index)
      {
        Thing thing = (Thing) WoodDebris.New(hitPos.x, hitPos.y);
        thing.hSpeed = (float) (-(double) bullet.travelDirNormalized.x * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929));
        thing.vSpeed = (float) (-(double) bullet.travelDirNormalized.y * 2.0 * ((double) Rando.Float(1f) + 0.300000011920929)) - Rando.Float(2f);
        Level.Add(thing);
      }
    }

    public override bool Hurt(float points)
    {
      if (!this._popup || (double) this._maxHealth == 0.0)
        return false;
      this._hitPoints -= points;
      return true;
    }

    public void GiveThing(Holdable h)
    {
      this.holdObject = h;
      this.holdObject.owner = (Thing) this;
      this.holdObject.solid = false;
    }

    public override void UpdateSkeleton()
    {
      int num = 6;
      int frame = 0;
      if (this.sliding)
        frame = 12;
      else if (this.crouch)
        frame = 11;
      this._skeleton.head.position = this.Offset(DuckRig.GetHatPoint(frame) + new Vec2(0.0f, (float) -num));
      this._skeleton.upperTorso.position = this.Offset(DuckRig.GetChestPoint(frame) + new Vec2(0.0f, (float) -num));
      this._skeleton.lowerTorso.position = this.position + new Vec2(0.0f, (float) (10 - num));
      if (this.sliding)
      {
        this._skeleton.head.orientation = Maths.DegToRad(90f);
        this._skeleton.upperTorso.orientation = Maths.DegToRad(90f);
      }
      else
      {
        this._skeleton.head.orientation = 0.0f;
        this._skeleton.upperTorso.orientation = 0.0f;
      }
    }

    public override void UpdateHoldPosition(bool updateLerp = true)
    {
      if (this.holdObject == null)
        return;
      this.holdOffY = 0.0f;
      this.armOffY = 0.0f;
      if ((!this._up || !this._popup) && !this.editorUpdate)
        return;
      this.holdObject.UpdateAction();
      this.holdObject.position = this.armPosition + this.holdObject.holdOffset + new Vec2(this.holdOffX, this.holdOffY) + new Vec2((float) (2 * (int) this.offDir), 0.0f);
      this.holdObject.offDir = this.offDir;
      if (this._sprite.currentAnimation == "slide")
      {
        --this.holdOffY;
        ++this.holdOffX;
      }
      else if (this.crouch)
      {
        if (this.holdObject != null)
          this.armOffY += 4f;
      }
      else if (this.sliding && this.holdObject != null)
        this.armOffY += 6f;
      if (this._stance != TargetStance.Fly)
      {
        this.holdAngleOff = Maths.LerpTowards(this.holdAngleOff, 0.0f, 0.4f);
        if (this.holdObject.raised)
          this.holdObject.raised = false;
      }
      this.holdObject.position = this.HoldOffset(this.holdObject.holdOffset) + new Vec2((float) ((int) this.offDir * 3), 0.0f);
      this.holdObject.angle = this.holdObject.handAngle + this.holdAngleOff;
    }

    public override void DuckUpdate()
    {
    }

    public override void Update()
    {
      this.impacting.Clear();
      if (this._up && this._popup && this.holdObject is Gun)
      {
        Gun holdObject = this.holdObject as Gun;
        float num = 300f;
        if (holdObject.ammoType != null)
          num = holdObject.ammoType.range;
        Vec2 vec2 = this.holdObject.position + new Vec2((float) this.offDir * num, 0.0f);
        foreach (Duck duck in Level.current.things[typeof (Duck)].Where<Thing>((Func<Thing, bool>) (d => !(d is TargetDuck))))
        {
          if ((Collision.Line(this.holdObject.position + new Vec2(0.0f, -5f), vec2 + new Vec2(0.0f, -5f), duck.rectangle) || Collision.Line(this.holdObject.position + new Vec2(0.0f, 5f), vec2 + new Vec2(0.0f, 5f), duck.rectangle)) && Level.CheckLine<Block>(this.holdObject.position, duck.position) == null)
          {
            this._waitFire -= 0.03f;
            break;
          }
        }
        bool flag = false;
        if ((double) this._autoFireWait > 0.0)
        {
          this._autoFireWait -= Maths.IncFrameTimer();
          if ((double) this._autoFireWait <= 0.0)
            flag = true;
        }
        if ((double) this._waitFire <= 0.0 || flag)
        {
          holdObject.PressAction();
          this._waitFire = 1f;
        }
        if ((double) this._waitFire < 1.0)
          this._waitFire += 0.01f;
      }
      this.UpdateCollision();
      this.UpdateSkeleton();
      if ((double) this._hitPoints <= 0.0)
        this.Destroy((DestroyType) new DTCrush((PhysicsObject) null));
      if (!this._up)
      {
        this._timeCount = 0.0f;
        if (this._popup)
          this._upSpeed += 0.1f;
        if (this._stance != TargetStance.Fly)
        {
          this.yscale += this._upSpeed;
          if ((double) this.yscale < 1.0)
            return;
          this.yscale = 1f;
          this._upSpeed = 0.0f;
          this._up = true;
          SFX.Play("grappleHook", 0.7f, Rando.Float(-0.2f, 0.2f));
          Level.Add((Thing) SmallSmoke.New(this.x - 4f, this.y));
          Level.Add((Thing) SmallSmoke.New(this.x + 4f, this.y));
          this.SpawnHoldObject();
          if (this.helmet)
          {
            Helmet helmet = new Helmet(this.x, this.y);
            Level.Add((Thing) helmet);
            this.Equip((Equipment) helmet);
          }
          if (!this.chestPlate)
            return;
          ChestPlate chestPlate = new ChestPlate(this.x, this.y);
          Level.Add((Thing) chestPlate);
          this.Equip((Equipment) chestPlate);
        }
        else
        {
          this.xscale += this._upSpeed;
          if ((double) this.xscale < 1.0)
            return;
          this.xscale = 1f;
          this._upSpeed = 0.0f;
          this._up = true;
          SFX.Play("grappleHook", 0.7f, Rando.Float(-0.2f, 0.2f));
          Level.Add((Thing) SmallSmoke.New(this.x - 4f, this.y));
          Level.Add((Thing) SmallSmoke.New(this.x + 4f, this.y));
          if (this.helmet)
          {
            Helmet helmet = new Helmet(this.x, this.y);
            Level.Add((Thing) helmet);
            this.Equip((Equipment) helmet);
          }
          if (!this.chestPlate)
            return;
          ChestPlate chestPlate = new ChestPlate(this.x, this.y);
          Level.Add((Thing) chestPlate);
          this.Equip((Equipment) chestPlate);
        }
      }
      else
      {
        this._timeCount += Maths.IncFrameTimer();
        if (this._popup && (double) this.time.value != 0.0 && (double) this._timeCount >= (double) this.time.value)
        {
          SFX.Play("grappleHook", 0.2f, Rando.Float(-0.2f, 0.2f));
          this.PopDown();
        }
        else
        {
          if (!this._popup)
            this._upSpeed += 0.1f;
          if (this._stance != TargetStance.Fly)
          {
            this.yscale -= this._upSpeed;
            if ((double) this.yscale >= 0.0)
              return;
            this.yscale = 0.0f;
            this._upSpeed = 0.0f;
            this._up = false;
            SFX.Play("grappleHook", 0.2f, Rando.Float(-0.2f, 0.2f));
            Level.Add((Thing) SmallSmoke.New(this.x - 4f, this.y));
            Level.Add((Thing) SmallSmoke.New(this.x + 4f, this.y));
            this._hitPoints = this._maxHealth = 0.1f;
          }
          else
          {
            this.xscale -= this._upSpeed;
            if ((double) this.xscale >= 0.0)
              return;
            this.xscale = 0.0f;
            this._upSpeed = 0.0f;
            this._up = false;
            SFX.Play("grappleHook", 0.2f, Rando.Float(-0.2f, 0.2f));
            Level.Add((Thing) SmallSmoke.New(this.x - 4f, this.y));
            Level.Add((Thing) SmallSmoke.New(this.x + 4f, this.y));
            this._hitPoints = this._maxHealth = 0.1f;
          }
        }
      }
    }

    public override void Draw()
    {
      if (this.graphic == null)
        return;
      this.graphic.flipH = this.offDir <= (sbyte) 0;
      this.graphic.scale = this.scale;
      if (Level.current is Editor)
      {
        this.graphic.center = this.center;
        this.graphic.position = this.position;
      }
      else if (this._stance != TargetStance.Fly)
      {
        this.graphic.center = this.center + new Vec2(0.0f, 10f);
        this.graphic.position = this.position + new Vec2(0.0f, 10f);
      }
      else
      {
        this.graphic.center = this.center + new Vec2(-12f, 10f);
        this.graphic.position = this.position + new Vec2((float) (-12 * (int) this.offDir), 10f);
      }
      this.graphic.depth = this.depth;
      this.graphic.alpha = this.alpha;
      this.graphic.angle = this.angle;
      this.graphic.Draw();
      if (!this._popup || !this._up)
        return;
      this.DrawHat();
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = base.Serialize();
      binaryClassChunk.AddProperty("stanceSetting", (object) this.stanceSetting);
      binaryClassChunk.AddProperty("contains", this.contains != (System.Type) null ? (object) ModLoader.SmallTypeName(this.contains) : (object) "");
      binaryClassChunk.AddProperty("chestPlate", (object) this.chestPlate);
      binaryClassChunk.AddProperty("helmet", (object) this.helmet);
      return binaryClassChunk;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      base.Deserialize(node);
      this.stanceSetting = node.GetProperty<int>("stanceSetting");
      this.contains = Editor.GetType(node.GetProperty<string>("contains"));
      this.chestPlate = node.GetProperty<bool>("chestPlate");
      this.helmet = node.GetProperty<bool>("helmet");
      return true;
    }

    public override XElement LegacySerialize()
    {
      XElement xelement = base.LegacySerialize();
      xelement.Add((object) new XElement((XName) "stanceSetting", (object) Change.ToString((object) this.stanceSetting)));
      xelement.Add((object) new XElement((XName) "contains", this.contains != (System.Type) null ? (object) this.contains.AssemblyQualifiedName : (object) ""));
      xelement.Add((object) new XElement((XName) "chestPlate", (object) Change.ToString((object) this.chestPlate)));
      xelement.Add((object) new XElement((XName) "helmet", (object) Change.ToString((object) this.helmet)));
      return xelement;
    }

    public override bool LegacyDeserialize(XElement node)
    {
      base.LegacyDeserialize(node);
      XElement xelement1 = node.Element((XName) "stanceSetting");
      if (xelement1 != null)
        this.stanceSetting = Convert.ToInt32(xelement1.Value);
      XElement xelement2 = node.Element((XName) "contains");
      if (xelement2 != null)
        this.contains = Editor.GetType(xelement2.Value);
      XElement xelement3 = node.Element((XName) "chestPlate");
      if (xelement3 != null)
        this.chestPlate = Convert.ToBoolean(xelement3.Value);
      XElement xelement4 = node.Element((XName) "helmet");
      if (xelement4 != null)
        this.helmet = Convert.ToBoolean(xelement4.Value);
      return true;
    }

    public int stanceSetting
    {
      get => this._stanceSetting;
      set
      {
        this._stanceSetting = value;
        this._stance = (TargetStance) this._stanceSetting;
        this.UpdateCollision();
      }
    }

    public override string GetDetailsString()
    {
      string str = "NONE";
      if (this.contains != (System.Type) null)
        str = this.contains.Name;
      return base.GetDetailsString() + "Order: " + (object) this.sequence.order + "\nHolding: " + str;
    }

    public override void Netted(Net n)
    {
      base.Netted(n);
      this.y -= 10000f;
      this._trapped.infinite = true;
    }

    public override void EditorUpdate()
    {
      if (this.chestPlate && this.GetEquipment(typeof (ChestPlate)) == null)
        this.Equip((Equipment) new ChestPlate(0.0f, 0.0f), false);
      else if (!this.chestPlate)
      {
        Equipment equipment = this.GetEquipment(typeof (ChestPlate));
        if (equipment != null)
          this.Unequip(equipment);
      }
      if (this.helmet && this.GetEquipment(typeof (Helmet)) == null)
        this.Equip((Equipment) new Helmet(0.0f, 0.0f), false);
      else if (!this.helmet)
      {
        Equipment equipment = this.GetEquipment(typeof (Helmet));
        if (equipment != null)
          this.Unequip(equipment);
      }
      if (this.contains != (System.Type) null)
      {
        if (this.holdObject == null || this.holdObject.GetType() != this.contains)
          this.GiveHoldable(Editor.CreateThing(this.contains) as Holdable);
      }
      else
        this.holdObject = (Holdable) null;
      foreach (Thing thing in this._equipment)
        thing.DoUpdate();
      if (this.holdObject != null)
      {
        this.editorUpdate = true;
        this.UpdateHoldPosition(true);
        this.holdObject.DoUpdate();
        this.editorUpdate = false;
      }
      base.EditorUpdate();
    }

    public override void EditorRender()
    {
      foreach (Thing thing in this._equipment)
        thing.DoDraw();
      if (this.holdObject != null)
      {
        this.holdObject.depth = new Depth(0.9f);
        this.holdObject.DoDraw();
      }
      base.EditorRender();
    }

    public override ContextMenu GetContextMenu()
    {
      EditorGroupMenu contextMenu = base.GetContextMenu() as EditorGroupMenu;
      contextMenu.AddItem((ContextMenu) new ContextRadio("Stand", this.stanceSetting == 0, (object) 0, (IContextListener) null, new FieldBinding((object) this, "stanceSetting")));
      contextMenu.AddItem((ContextMenu) new ContextRadio("Crouch", this.stanceSetting == 2, (object) 2, (IContextListener) null, new FieldBinding((object) this, "stanceSetting")));
      contextMenu.AddItem((ContextMenu) new ContextRadio("Slide", this.stanceSetting == 3, (object) 3, (IContextListener) null, new FieldBinding((object) this, "stanceSetting")));
      contextMenu.AddItem((ContextMenu) new ContextRadio("Fly", this.stanceSetting == 4, (object) 4, (IContextListener) null, new FieldBinding((object) this, "stanceSetting")));
      contextMenu.AddItem((ContextMenu) new ContextCheckBox("Chest Plate", (IContextListener) null, new FieldBinding((object) this, "chestPlate")));
      contextMenu.AddItem((ContextMenu) new ContextCheckBox("Helmet", (IContextListener) null, new FieldBinding((object) this, "helmet")));
      FieldBinding radioBinding = new FieldBinding((object) this, "contains");
      EditorGroupMenu editorGroupMenu = new EditorGroupMenu((IContextListener) contextMenu);
      editorGroupMenu.InitializeGroups(new EditorGroup(typeof (PhysicsObject)), radioBinding);
      editorGroupMenu.text = "Holding";
      contextMenu.AddItem((ContextMenu) editorGroupMenu);
      return (ContextMenu) contextMenu;
    }
  }
}
