// Decompiled with JetBrains decompiler
// Type: DuckGame.WireFlipper
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff|wires")]
  [BaggedProperty("isOnlineCapable", true)]
  public class WireFlipper : Block, IWirePeripheral
  {
    private SpriteMap _sprite;

    public WireFlipper(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("wireFlipper", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-8f, -8f);
      this.collisionSize = new Vec2(16f, 16f);
      this.depth = (Depth) -0.5f;
      this._editorName = "Wire Flipper";
      this.thickness = 4f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.layer = Layer.Foreground;
      this._canFlip = true;
    }

    public override void Initialize() => base.Initialize();

    public override void Draw()
    {
      bool flipHorizontal = this.flipHorizontal;
      this._sprite.frame = this.offDir >= (sbyte) 0 ? 0 : 1;
      this.flipHorizontal = false;
      base.Draw();
      this.flipHorizontal = flipHorizontal;
    }

    public override void Update() => base.Update();

    public override void Terminate() => base.Terminate();

    public void Pulse(int type, WireTileset wire)
    {
      SFX.Play("click");
      if (this.flipHorizontal)
        wire.dullSignalLeft = true;
      else
        wire.dullSignalRight = true;
      this.flipHorizontal = !this.flipHorizontal;
    }
  }
}
