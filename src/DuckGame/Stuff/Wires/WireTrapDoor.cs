// Decompiled with JetBrains decompiler
// Type: DuckGame.WireTrapDoor
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", true)]
  [EditorGroup("stuff|wires")]
  public class WireTrapDoor : Block, IWirePeripheral
  {
    public StateBinding _openBinding = new StateBinding(nameof (_open));
    public bool _open;
    private Thing _shutter;
    private SpriteMap _sprite;
    private bool _lastFlip;
    public EditorProperty<bool> fallthrough;
    private bool _lastFallthrough = true;

    public override bool flipHorizontal
    {
      get => base.flipHorizontal;
      set
      {
        base.flipHorizontal = value;
        if (this.flipHorizontal)
          this.offDir = (sbyte) -1;
        else
          this.offDir = (sbyte) 1;
        if (!this._initialized)
          return;
        this.CreateShutter();
      }
    }

    public override void EditorPropertyChanged(object property)
    {
      if (!this._initialized)
        return;
      this.UpdateShutter();
    }

    private void UpdateShutter()
    {
      if (this._lastFallthrough != this.fallthrough.value)
      {
        Level.Remove(this._shutter);
        this._shutter = (Thing) null;
      }
      if (this._shutter == null)
        this.CreateShutter();
      this._lastFallthrough = this.fallthrough.value;
    }

    public WireTrapDoor(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.fallthrough = new EditorProperty<bool>(true, (Thing) this);
      this._sprite = new SpriteMap("wireBlock", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.depth = (Depth) -0.5f;
      this._editorName = "Wire Trapdoor";
      this.thickness = 4f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.layer = Layer.Foreground;
      this._canFlip = true;
    }

    public void CreateShutter()
    {
      if (this._shutter != null)
        Level.Remove(this._shutter);
      this._shutter = !this.fallthrough.value ? (Thing) new WireTrapDoorShutterSolid(this.x + (float) (4 * (int) this.offDir), this.y - 5f, this) : (Thing) new WireTrapDoorShutter(this.x + (float) (4 * (int) this.offDir), this.y - 5f, this);
      this._shutter.depth = this.depth + 5;
      this._shutter.offDir = this.offDir;
      Level.Add(this._shutter);
    }

    public override void Initialize()
    {
      this.CreateShutter();
      base.Initialize();
    }

    public override void Update()
    {
      this.UpdateShutter();
      if (this._open)
        this._shutter.angleDegrees = Lerp.Float(this._shutter.angleDegrees, 90f * (float) this.offDir, 10f);
      else
        this._shutter.angleDegrees = Lerp.Float(this._shutter.angleDegrees, 0.0f, 10f);
      base.Update();
    }

    public override void Terminate()
    {
      Level.Remove(this._shutter);
      base.Terminate();
    }

    public void Pulse(int type, WireTileset wire)
    {
      Thing.Fondle((Thing) this, DuckNetwork.localConnection);
      this._open = !this._open;
      SFX.Play("click");
    }
  }
}
