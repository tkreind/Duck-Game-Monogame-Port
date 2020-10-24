// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckPersona
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DuckPersona
  {
    private Vec3 _color;
    private SpriteMap _skipSprite;
    private SpriteMap _arrowSprite;
    private SpriteMap _fingerPositionSprite;
    private SpriteMap _featherSprite;
    private SpriteMap _crowdSprite;
    private SpriteMap _sprite;
    private SpriteMap _armSprite;
    private SpriteMap _quackSprite;
    private SpriteMap _controlledSprite;
    private SpriteMap _defaultHead;

    public Vec3 color
    {
      get => this._color;
      set => this._color = value;
    }

    public Color colorUsable => new Color((byte) this._color.x, (byte) this._color.y, (byte) this._color.z);

    public SpriteMap skipSprite
    {
      get => this._skipSprite;
      set => this._skipSprite = value;
    }

    public SpriteMap arrowSprite
    {
      get => this._arrowSprite;
      set => this._arrowSprite = value;
    }

    public SpriteMap fingerPositionSprite
    {
      get => this._fingerPositionSprite;
      set => this._fingerPositionSprite = value;
    }

    public SpriteMap featherSprite
    {
      get => this._featherSprite;
      set => this._featherSprite = value;
    }

    public SpriteMap crowdSprite
    {
      get => this._crowdSprite;
      set => this._crowdSprite = value;
    }

    public SpriteMap sprite
    {
      get => this._sprite;
      set => this._sprite = value;
    }

    public SpriteMap armSprite
    {
      get => this._armSprite;
      set => this._armSprite = value;
    }

    public SpriteMap quackSprite
    {
      get => this._quackSprite;
      set => this._quackSprite = value;
    }

    public SpriteMap controlledSprite
    {
      get => this._controlledSprite;
      set => this._controlledSprite = value;
    }

    public SpriteMap defaultHead
    {
      get => this._defaultHead;
      set => this._defaultHead = value;
    }

    public DuckPersona(Vec3 varCol)
    {
      this._color = varCol;
      this._skipSprite = new SpriteMap(DuckGame.Graphics.Recolor(Content.Load<Tex2D>("skipSign"), this._color), 52, 18);
      this._skipSprite.center = new Vec2((float) (this._skipSprite.width - 3), 15f);
      this._arrowSprite = new SpriteMap(DuckGame.Graphics.Recolor(Content.Load<Tex2D>("startArrow"), this._color), 24, 16);
      this._arrowSprite.CenterOrigin();
      this._sprite = new SpriteMap(DuckGame.Graphics.Recolor(Content.Load<Tex2D>("duck"), this._color), 32, 32);
      this._sprite.CenterOrigin();
      this._crowdSprite = new SpriteMap(DuckGame.Graphics.Recolor(Content.Load<Tex2D>("seatDuck"), this._color), 19, 23);
      this._crowdSprite.CenterOrigin();
      this._sprite.ClearAnimations();
      this._sprite.AddAnimation("idle", 1f, true, new int[1]);
      this._sprite.AddAnimation("run", 1f, true, 1, 2, 3, 4, 5, 6);
      this._sprite.AddAnimation("jump", 1f, true, 7, 8, 9);
      this._sprite.AddAnimation("slide", 1f, true, 10);
      this._sprite.AddAnimation("crouch", 1f, true, 11);
      this._sprite.AddAnimation("groundSlide", 1f, true, 12);
      this._sprite.AddAnimation("dead", 1f, true, 13);
      this._sprite.AddAnimation("netted", 1f, true, 14);
      this._sprite.AddAnimation("listening", 1f, true, 16);
      this._sprite.SetAnimation("idle");
      this._featherSprite = new SpriteMap(DuckGame.Graphics.Recolor(Content.Load<Tex2D>("feather"), this._color), 12, 4);
      this._featherSprite.speed = 0.3f;
      this._featherSprite.AddAnimation("feather", 1f, true, 0, 1, 2, 3);
      this._fingerPositionSprite = new SpriteMap(DuckGame.Graphics.Recolor(Content.Load<Tex2D>("fingerPositions"), this._color), 16, 12);
      this._fingerPositionSprite.CenterOrigin();
      this._quackSprite = new SpriteMap(DuckGame.Graphics.Recolor(Content.Load<Tex2D>("quackduck"), this._color), 32, 32);
      this._quackSprite.CenterOrigin();
      this._armSprite = new SpriteMap(DuckGame.Graphics.Recolor(Content.Load<Tex2D>("duckArms"), this._color), 16, 16);
      this._armSprite.CenterOrigin();
      this._controlledSprite = new SpriteMap(DuckGame.Graphics.Recolor(Content.Load<Tex2D>("controlledDuck"), this._color), 32, 32);
      this._controlledSprite.CenterOrigin();
      this._defaultHead = new SpriteMap(DuckGame.Graphics.Recolor(Content.Load<Tex2D>("hats/default"), this._color), 32, 32);
      this._defaultHead.CenterOrigin();
    }

    public void Update()
    {
      if (!(this._sprite.texture.nativeObject as Microsoft.Xna.Framework.Graphics.RenderTarget2D).IsContentLost && !(this._quackSprite.texture.nativeObject as Microsoft.Xna.Framework.Graphics.RenderTarget2D).IsContentLost && !(this._armSprite.texture.nativeObject as Microsoft.Xna.Framework.Graphics.RenderTarget2D).IsContentLost)
        return;
      this.Recreate();
    }

    public void Recreate()
    {
      if (!DuckGame.Graphics.inFocus)
        return;
      for (int index = 0; index < 4; ++index)
      {
        this._sprite.texture.Dispose();
        this._sprite.texture = DuckGame.Graphics.Recolor(Content.Load<Tex2D>("duck"), this._color);
        this._featherSprite.texture.Dispose();
        this._featherSprite.texture = DuckGame.Graphics.Recolor(Content.Load<Tex2D>("feather"), this._color);
        this._fingerPositionSprite.texture.Dispose();
        this._fingerPositionSprite.texture = DuckGame.Graphics.Recolor(Content.Load<Tex2D>("fingerPositions"), this._color);
        this._crowdSprite.texture.Dispose();
        this._crowdSprite.texture = DuckGame.Graphics.Recolor(Content.Load<Tex2D>("seatDuck"), this._color);
        this._quackSprite.texture.Dispose();
        this._quackSprite.texture = DuckGame.Graphics.Recolor(Content.Load<Tex2D>("quackduck"), this._color);
        this._armSprite.texture.Dispose();
        this._armSprite.texture = DuckGame.Graphics.Recolor(Content.Load<Tex2D>("duckArms"), this._color);
        this._controlledSprite.texture.Dispose();
        this._controlledSprite.texture = DuckGame.Graphics.Recolor(Content.Load<Tex2D>("controlledDuck"), this._color);
        this._skipSprite.texture.Dispose();
        this._skipSprite.texture = DuckGame.Graphics.Recolor(Content.Load<Tex2D>("skipSign"), this._color);
        this._arrowSprite.texture.Dispose();
        this._arrowSprite.texture = DuckGame.Graphics.Recolor(Content.Load<Tex2D>("startArrow"), this._color);
        this._defaultHead.texture.Dispose();
        this._defaultHead.texture = DuckGame.Graphics.Recolor(Content.Load<Tex2D>("hats/default"), this._color);
      }
    }
  }
}
