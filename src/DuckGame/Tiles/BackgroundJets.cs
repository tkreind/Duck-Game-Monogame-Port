// Decompiled with JetBrains decompiler
// Type: DuckGame.BackgroundJets
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("details")]
  public class BackgroundJets : Thing
  {
    public SpriteMap _leftJet;
    public SpriteMap _rightJet;
    private bool _leftAlternate;
    private bool _rightAlternate = true;

    public BackgroundJets(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.graphic = (Sprite) new SpriteMap("levelJetIdle", 32, 13);
      this._leftJet = new SpriteMap("jet", 16, 16);
      this._leftJet.AddAnimation("idle", 0.4f, true, 0, 1, 2);
      this._leftJet.SetAnimation("idle");
      this._leftJet.center = new Vec2(8f, 0.0f);
      this._leftJet.alpha = 0.7f;
      this._rightJet = new SpriteMap("jet", 16, 16);
      this._rightJet.AddAnimation("idle", 0.4f, true, 1, 2, 0);
      this._rightJet.SetAnimation("idle");
      this._rightJet.center = new Vec2(8f, 0.0f);
      this._rightJet.alpha = 0.7f;
      this.center = new Vec2(16f, 8f);
      this._collisionSize = new Vec2(16f, 14f);
      this._collisionOffset = new Vec2(-8f, -8f);
      this.hugWalls = WallHug.Ceiling;
      this._canFlip = false;
    }

    public override void Update()
    {
      this._leftAlternate = !this._leftAlternate;
      this._rightAlternate = !this._rightAlternate;
    }

    public override void Draw()
    {
      base.Draw();
      Graphics.Draw((Sprite) this._leftJet, this.x - 8f, this.y + 5f);
      Graphics.Draw((Sprite) this._rightJet, this.x + 8f, this.y + 5f);
    }
  }
}
