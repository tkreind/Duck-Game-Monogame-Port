// Decompiled with JetBrains decompiler
// Type: DuckGame.VerticalDoor
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff")]
  public class VerticalDoor : Block, IPlatform
  {
    private SpriteMap _sprite;
    private Sprite _bottom;
    private Sprite _top;
    private float _open;
    private float _desiredOpen;
    private bool _opened;
    private Vec2 _topLeft;
    private Vec2 _topRight;
    private Vec2 _bottomLeft;
    private Vec2 _bottomRight;
    private bool _cornerInit;

    public VerticalDoor(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("verticalDoor", 16, 32);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 24f);
      this.collisionSize = new Vec2(6f, 32f);
      this.collisionOffset = new Vec2(-3f, -24f);
      this.depth = new Depth(-0.5f);
      this._editorName = "Vertical Door";
      this.thickness = 3f;
      this.physicsMaterial = PhysicsMaterial.Metal;
      this._bottom = new Sprite("verticalDoorBottom");
      this._bottom.CenterOrigin();
      this._top = new Sprite("verticalDoorTop");
      this._top.CenterOrigin();
    }

    public override void Update()
    {
      if (!this._cornerInit)
      {
        this._topLeft = this.topLeft;
        this._topRight = this.topRight;
        this._bottomLeft = this.bottomLeft;
        this._bottomRight = this.bottomRight;
        this._cornerInit = true;
      }
      if (Level.CheckRect<Duck>(this._topLeft - new Vec2(18f, 0.0f), this._bottomRight + new Vec2(18f, 0.0f)) != null)
        this._desiredOpen = 1f;
      else if (Level.CheckRect<PhysicsObject>(new Vec2(this.x - 4f, this.y - 24f), new Vec2(this.x + 4f, this.y + 8f)) == null)
        this._desiredOpen = 0.0f;
      if ((double) this._desiredOpen > 0.5 && !this._opened)
      {
        this._opened = true;
        SFX.Play("slideDoorOpen", 0.6f);
      }
      if ((double) this._desiredOpen < 0.5 && this._opened)
      {
        this._opened = false;
        SFX.Play("slideDoorClose", 0.6f);
      }
      this._open = Maths.LerpTowards(this._open, this._desiredOpen, 0.15f);
      this._sprite.frame = (int) ((double) this._open * 32.0);
      this._collisionSize.y = (float) ((1.0 - (double) this._open) * 32.0);
    }

    public override void Draw()
    {
      base.Draw();
      this._top.depth = this.depth + 1;
      this._bottom.depth = this.depth + 1;
      Graphics.Draw(this._top, this.x, this.y - 27f);
      Graphics.Draw(this._bottom, this.x, this.y + 5f);
    }
  }
}
