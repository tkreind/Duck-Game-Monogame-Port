// Decompiled with JetBrains decompiler
// Type: DuckGame.LaserSpawner
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Xml.Linq;

namespace DuckGame
{
  [BaggedProperty("isInDemo", false)]
  [EditorGroup("spawns")]
  public class LaserSpawner : ItemSpawner
  {
    public float fireDirection;
    public float firePower = 1f;

    public float direction => this.fireDirection + (this.flipHorizontal ? 180f : 0.0f);

    public LaserSpawner(float xpos, float ypos, System.Type c = null)
      : base(xpos, ypos)
    {
      this.graphic = new Sprite("laserSpawner");
      this.center = new Vec2(8f, 8f);
      this.collisionSize = new Vec2(12f, 12f);
      this.collisionOffset = new Vec2(-6f, -6f);
      this.depth = new Depth(-0.6f);
      this.contains = c;
      this.hugWalls = WallHug.None;
      this._hasContainedItem = false;
      this._visibleInGame = false;
    }

    public override void Initialize()
    {
      if (!this.spawnOnStart)
        return;
      this._spawnWait = this.spawnTime;
    }

    public override void Update()
    {
      if (Level.current.simulatePhysics)
        this._spawnWait += 0.0166666f;
      if (Level.current.simulatePhysics && Network.isServer && (this._numSpawned < this.spawnNum || this.spawnNum == -1) && (double) this._spawnWait >= (double) this.spawnTime)
      {
        if ((double) this.initialDelay > 0.0)
        {
          this.initialDelay -= 0.0166666f;
        }
        else
        {
          Vec2 travel = Maths.AngleToVec(Maths.DegToRad(this.direction)) * this.firePower;
          Vec2 vec2 = this.position - travel.normalized * 16f;
          Level.Add((Thing) new QuadLaserBullet(vec2.x, vec2.y, travel));
          this._spawnWait = 0.0f;
          ++this._numSpawned;
        }
      }
      this.angleDegrees = -this.direction;
    }

    public override void Terminate()
    {
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = base.Serialize();
      binaryClassChunk.AddProperty("fireDirection", (object) this.fireDirection);
      binaryClassChunk.AddProperty("firePower", (object) this.firePower);
      return binaryClassChunk;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      base.Deserialize(node);
      this.fireDirection = node.GetProperty<float>("fireDirection");
      this.firePower = node.GetProperty<float>("firePower");
      return true;
    }

    public override XElement LegacySerialize()
    {
      XElement xelement = base.LegacySerialize();
      xelement.Add((object) new XElement((XName) "fireDirection", (object) Change.ToString((object) this.fireDirection)));
      xelement.Add((object) new XElement((XName) "firePower", (object) Change.ToString((object) this.firePower)));
      return xelement;
    }

    public override bool LegacyDeserialize(XElement node)
    {
      base.LegacyDeserialize(node);
      XElement xelement1 = node.Element((XName) "fireDirection");
      if (xelement1 != null)
        this.fireDirection = Convert.ToSingle(xelement1.Value);
      XElement xelement2 = node.Element((XName) "firePower");
      if (xelement2 != null)
        this.firePower = Convert.ToSingle(xelement2.Value);
      return true;
    }

    public override ContextMenu GetContextMenu()
    {
      EditorGroupMenu contextMenu = base.GetContextMenu() as EditorGroupMenu;
      contextMenu.AddItem((ContextMenu) new ContextSlider("Angle", (IContextListener) null, new FieldBinding((object) this, "fireDirection", max: 360f), 1f));
      contextMenu.AddItem((ContextMenu) new ContextSlider("Power", (IContextListener) null, new FieldBinding((object) this, "firePower", 1f, 20f)));
      return (ContextMenu) contextMenu;
    }

    public override void DrawHoverInfo() => Graphics.DrawLine(this.position, this.position + Maths.AngleToVec(Maths.DegToRad(this.direction)) * (this.firePower * 5f), Color.Red, 2f, new Depth(1f));

    public override void Draw()
    {
      this.angleDegrees = -this.direction;
      base.Draw();
    }
  }
}
