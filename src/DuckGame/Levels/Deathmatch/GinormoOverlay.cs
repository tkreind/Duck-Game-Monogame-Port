// Decompiled with JetBrains decompiler
// Type: DuckGame.GinormoOverlay
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class GinormoOverlay : Thing
  {
    private Sprite _targetSprite;
    private Material _screenMaterial;
    private Tex2D _overlaySprite;

    public GinormoOverlay(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.depth = new Depth(0.9f);
      this.graphic = new Sprite("rockThrow/boardOverlay");
    }

    public override void Initialize()
    {
      this._overlaySprite = Content.Load<Tex2D>("rockThrow/boardOverlayLarge");
      this._targetSprite = new Sprite(GinormoBoard.boardLayer.target, 0.0f, 0.0f);
      this._screenMaterial = new Material("Shaders/lcdNoBlur");
      this._screenMaterial.SetValue("screenWidth", 185f);
      this._screenMaterial.SetValue("screenHeight", 103f);
      base.Initialize();
    }

    public override void Draw()
    {
      if (!RockScoreboard.drawingNormalTarget || !RockScoreboard.drawingNormalTarget)
        return;
      Material material = DuckGame.Graphics.material;
      DuckGame.Graphics.material = this._screenMaterial;
      DuckGame.Graphics.device.Textures[1] = (Texture) (Texture2D) this._overlaySprite;
      DuckGame.Graphics.device.SamplerStates[1] = SamplerState.LinearClamp;
      this._targetSprite.depth = new Depth(0.9f);
      DuckGame.Graphics.Draw(this._targetSprite, this.x - 92f, this.y - 33f);
      DuckGame.Graphics.material = material;
    }
  }
}
