// Decompiled with JetBrains decompiler
// Type: DuckGame.WireButton
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isInDemo", true)]
  [EditorGroup("stuff|wires")]
  public class WireButton : Block, IWirePeripheral
  {
    public EditorProperty<bool> offSignal = new EditorProperty<bool>(false);
    public EditorProperty<bool> releaseOnly = new EditorProperty<bool>(false);
    public EditorProperty<int> orientation = new EditorProperty<int>(0, max: 3f, increment: 1f);
    private WireButtonTop _top;
    private SpriteMap _sprite;
    private PhysicsObject prevO;

    public WireButton(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("wireButton", 16, 19);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 11f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.depth = (Depth) -0.5f;
      this._editorName = "Wire Button";
      this.thickness = 4f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.layer = Layer.Foreground;
    }

    public override void Initialize()
    {
      this.angleDegrees = (float) this.orientation.value * 90f;
      if (!(Level.current is Editor))
      {
        if (this.orientation.value == 0)
          this._top = new WireButtonTop(this.x, this.y - 9f, this, this.orientation.value);
        else if (this.orientation.value == 1)
          this._top = new WireButtonTop(this.x + 9f, this.y, this, this.orientation.value);
        else if (this.orientation.value == 2)
          this._top = new WireButtonTop(this.x, this.y + 9f, this, this.orientation.value);
        else if (this.orientation.value == 3)
          this._top = new WireButtonTop(this.x - 9f, this.y, this, this.orientation.value);
        Level.Add((Thing) this._top);
      }
      base.Initialize();
    }

    public override void Terminate()
    {
      Level.Remove((Thing) this._top);
      base.Terminate();
    }

    public void Pulse(int type, WireTileset wire)
    {
    }

    public void ButtonPressed(PhysicsObject t)
    {
      if (this._sprite.frame == 0)
      {
        SFX.Play("click");
        this._sprite.frame = 1;
        if (!this.releaseOnly.value && t.isServerForObject)
          Level.CheckRect<WireTileset>(this.topLeft + new Vec2(2f, 2f), this.bottomRight + new Vec2(-2f, -2f))?.Emit(type: (this.offSignal.value ? 1 : 0));
      }
      this.prevO = t;
    }

    public override void Update()
    {
      if (this._sprite.frame == 1)
      {
        PhysicsObject physicsObject = Level.CheckRect<PhysicsObject>(this._top.topLeft, this._top.bottomRight);
        if (physicsObject == null)
        {
          SFX.Play("click");
          this._sprite.frame = 0;
          if ((this.offSignal.value || this.releaseOnly.value) && (this.prevO == null || this.prevO.isServerForObject))
            Level.CheckRect<WireTileset>(this.topLeft + new Vec2(2f, 2f), this.bottomRight + new Vec2(-2f, -2f))?.Emit(type: (this.releaseOnly.value ? 0 : 2));
        }
        this.prevO = physicsObject;
      }
      base.Update();
    }

    public override void Draw()
    {
      this.angleDegrees = (float) this.orientation.value * 90f;
      base.Draw();
    }
  }
}
