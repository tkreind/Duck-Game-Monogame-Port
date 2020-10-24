// Decompiled with JetBrains decompiler
// Type: DuckGame.ScoreRock
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Linq;

namespace DuckGame
{
  [BaggedProperty("canSpawn", false)]
  public class ScoreRock : Holdable, IPlatform
  {
    public StateBinding _planeBinding = new StateBinding("planeOfExistence");
    public StateBinding _depthBinding = new StateBinding(nameof (netDepth));
    public StateBinding _zBinding = new StateBinding("z");
    public StateBinding _profileBinding = new StateBinding(nameof (netProfileIndex));
    private byte _netProfileIndex;
    private SpriteMap _sprite;
    private Sprite _dropShadow = new Sprite("dropShadow");
    private Vec2 _dropShadowPoint = new Vec2();
    private Vec2 _pos = Vec2.Zero;
    private Profile _profile;

    public float netDepth
    {
      get => this.depth.value;
      set => this.depth = (Depth) value;
    }

    public byte netProfileIndex
    {
      get => this._netProfileIndex;
      set
      {
        this._netProfileIndex = value;
        this._profile = Profiles.all.ElementAt<Profile>((int) this._netProfileIndex);
        if (this._profile == null)
          return;
        if (this._profile.team == Teams.Player1)
          this._sprite.frame = 1;
        else if (this._profile.team == Teams.Player2)
          this._sprite.frame = 2;
        else if (this._profile.team == Teams.Player3)
        {
          this._sprite.frame = 3;
        }
        else
        {
          if (this._profile.team != Teams.Player4)
            return;
          this._sprite.frame = 4;
        }
      }
    }

    public ScoreRock(float xpos, float ypos, Profile profile)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("scoreRock", 16, 16);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-8f, -6f);
      this.collisionSize = new Vec2(16f, 13f);
      this.depth = (Depth) -0.5f;
      this.thickness = 4f;
      this.weight = 7f;
      if (profile != null)
      {
        if (profile.team == Teams.Player1)
          this._sprite.frame = 1;
        else if (profile.team == Teams.Player2)
          this._sprite.frame = 2;
        else if (profile.team == Teams.Player3)
          this._sprite.frame = 3;
        else if (profile.team == Teams.Player4)
          this._sprite.frame = 4;
      }
      this.flammable = 0.3f;
      this.collideSounds.Add("rockHitGround2");
      this._dropShadow.CenterOrigin();
      this._profile = profile;
      this.impactThreshold = 1f;
    }

    public override void Update()
    {
      foreach (Block block in Level.CheckLineAll<Block>(this.position, this.position + new Vec2(0.0f, 100f)))
      {
        if (block.solid)
        {
          this._dropShadowPoint.x = this.x;
          this._dropShadowPoint.y = block.top;
        }
      }
      if (RockScoreboard.wallMode && (double) this.x > 610.0)
      {
        this.x = 610f;
        this.hSpeed = -1f;
        SFX.Play("rockHitGround2", pitch: -0.4f);
      }
      if (RockScoreboard.wallMode && (double) this.x > 610.0)
      {
        this.x = 610f;
        this.hSpeed = -1f;
        SFX.Play("rockHitGround2", pitch: -0.4f);
      }
      this._pos = this.position;
      base.Update();
    }

    public override void Draw()
    {
      base.Draw();
      if (this._profile == null || !this._profile.team.hasHat)
        return;
      this._profile.team.hat.depth = this.depth + 1;
      this._profile.team.hat.center = new Vec2(16f, 16f);
      Vec2 vec2 = this.position - this._profile.team.hatOffset;
      Graphics.Draw((Sprite) this._profile.team.hat, vec2.x, vec2.y - 5f);
    }
  }
}
