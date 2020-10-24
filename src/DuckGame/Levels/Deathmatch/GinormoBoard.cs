// Decompiled with JetBrains decompiler
// Type: DuckGame.GinormoBoard
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;

namespace DuckGame
{
  public class GinormoBoard : Thing
  {
    private Sprite _board;
    private Sprite _boardTop;
    private Sprite _boardBottom;
    private GinormoScreen _screen;
    private SpriteMap _lighting;
    private bool _activated;
    private BoardMode _mode;
    private Vec2 _pos;
    private Layer boardLightingLayer;
    private Layer overlayLayer;
    public static Layer boardLayer;

    public bool activated => this._activated;

    public GinormoBoard(float xpos, float ypos, BoardMode mode)
      : base(xpos, ypos)
    {
      this._board = new Sprite("rockThrow/boardMiddle");
      this._board.center = new Vec2((float) (this._board.w / 2), (float) (this._board.h / 2 - 30));
      this._lighting = new SpriteMap("rockThrow/lighting", 191, 23);
      this._lighting.frame = 1;
      this.boardLightingLayer = new Layer("LIGHTING", -85);
      this.boardLightingLayer.blend = new BlendState()
      {
        ColorSourceBlend = Blend.Zero,
        ColorDestinationBlend = Blend.SourceColor,
        ColorBlendFunction = BlendFunction.Add,
        AlphaSourceBlend = Blend.Zero,
        AlphaDestinationBlend = Blend.SourceColor,
        AlphaBlendFunction = BlendFunction.Add
      };
      Layer.Add(this.boardLightingLayer);
      BoardLighting boardLighting = new BoardLighting(this.x + 0.5f, this.y - 125f);
      boardLighting.layer = this.boardLightingLayer;
      Level.Add((Thing) boardLighting);
      if (RockWeather.weather == Weather.Snowing)
      {
        this._boardTop = new Sprite("rockThrow/boardTopSnow");
        this._boardBottom = new Sprite("rockThrow/boardBottomSnow");
      }
      else
      {
        this._boardTop = new Sprite("rockThrow/boardTop");
        this._boardBottom = new Sprite("rockThrow/boardBottom");
      }
      this._boardTop.center = new Vec2((float) (this._boardTop.w / 2), (float) (this._boardTop.h / 2 - 30));
      this._boardBottom.center = new Vec2((float) (this._boardBottom.w / 2), (float) (this._boardBottom.h / 2 - 30));
      this.layer = Layer.Background;
      this._pos = new Vec2(xpos, ypos);
      this._mode = mode;
      GinormoBoard.boardLayer = new Layer("BOARD", -85, targetLayer: true, targetSize: new Vec2(185f, 103f));
      GinormoBoard.boardLayer.camera = new Camera(0.0f, 0.0f, 185f, 103f);
      GinormoBoard.boardLayer.targetOnly = true;
      GinormoBoard.boardLayer.targetClearColor = new Color(0.05f, 0.05f, 0.05f);
      Layer.Add(GinormoBoard.boardLayer);
      this.overlayLayer = new Layer("OVERLAY", 10);
      Layer.Add(this.overlayLayer);
      GinormoOverlay ginormoOverlay = new GinormoOverlay(this.x - 182f, this.y - 65f);
      ginormoOverlay.z = -130f;
      ginormoOverlay.position = this.position;
      ginormoOverlay.layer = this.overlayLayer;
      Level.Add((Thing) ginormoOverlay);
    }

    public void Activate()
    {
      if (this._activated)
        return;
      this._screen = new GinormoScreen(0.0f, 0.0f, this._mode);
      Level.Add((Thing) this._screen);
      this._activated = true;
    }

    public override void Draw()
    {
      this.boardLightingLayer.perspective = true;
      this.boardLightingLayer.projection = Layer.Background.projection;
      this.boardLightingLayer.view = Layer.Background.view;
      this.overlayLayer.perspective = true;
      this.overlayLayer.projection = Layer.Game.projection;
      this.overlayLayer.view = Layer.Game.view;
      this.overlayLayer.camera = Layer.Game.camera;
      this.boardLightingLayer.colorAdd = new Vec3(1f - RockWeather.lightOpacity);
      this._lighting.frame = (double) RockWeather.lightOpacity <= 0.00999999977648258 ? 0 : 1;
      this._board.depth = this.depth;
      DuckGame.Graphics.Draw(this._board, this.x, this.y - 12f);
      DuckGame.Graphics.Draw(this._boardBottom, this.x, this.y + 58f);
      DuckGame.Graphics.Draw(this._boardTop, this.x, this.y - 68f);
      if (!RockScoreboard._sunEnabled)
        return;
      DuckGame.Graphics.Draw((Sprite) this._lighting, this.x - 95f, this.y - 67f);
    }
  }
}
