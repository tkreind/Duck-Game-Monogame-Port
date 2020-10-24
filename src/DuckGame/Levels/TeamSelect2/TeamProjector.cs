// Decompiled with JetBrains decompiler
// Type: DuckGame.TeamProjector
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class TeamProjector : Thing
  {
    private Sprite _selectPlatform;
    private Sprite _selectProjector;
    private SinWave _projectorSin = (SinWave) 0.5f;
    private Profile _profile;
    private List<Profile> _profiles = new List<Profile>();
    private bool _swap;
    private float _swapFade = 1f;

    public TeamProjector(float xpos, float ypos, Profile profile)
      : base(xpos, ypos)
    {
      this._selectPlatform = new Sprite("selectPlatform");
      this._selectPlatform.CenterOrigin();
      this._selectProjector = new Sprite("selectProjector");
      this._selectProjector.CenterOrigin();
      this._profile = profile;
      this._profiles.Add(profile);
    }

    public void SetProfile(Profile newProfile) => this._profile = newProfile;

    public override void Update()
    {
      List<Profile> profileList = new List<Profile>();
      profileList.Add(this._profile);
      Team team = this._profile.team;
      if (team != null)
        profileList = team.activeProfiles;
      bool flag = profileList.Count == this._profiles.Count;
      foreach (Profile profile in profileList)
      {
        if (!this._profiles.Contains(profile))
        {
          flag = false;
          break;
        }
      }
      if (!flag)
        this._swap = true;
      if (this._swap)
      {
        this._swapFade -= 0.1f;
      }
      else
      {
        this._swapFade += 0.1f;
        if ((double) this._swapFade > 1.0)
          this._swapFade = 1f;
      }
      if ((double) this._swapFade > 0.0 || !this._swap)
        return;
      this._swap = false;
      this._swapFade = 0.0f;
      this._profiles.Clear();
      this._profiles.AddRange((IEnumerable<Profile>) profileList);
    }

    public override void Draw()
    {
      this._selectProjector.depth = new Depth(-0.51f);
      this._selectProjector.alpha = (float) (0.300000011920929 + (double) this._projectorSin.normalized * 0.200000002980232);
      this._selectPlatform.depth = new Depth(-0.51f);
      int count = this._profiles.Count;
      int num1 = 0;
      foreach (Profile profile in this._profiles)
      {
        Color color = new Color(0.35f, 0.5f, 0.6f);
        profile.persona.sprite.alpha = Maths.Clamp(this._swapFade, 0.0f, 1f);
        profile.persona.sprite.color = color * (float) (0.699999988079071 + (double) this._projectorSin.normalized * 0.100000001490116);
        profile.persona.sprite.color = new Color(profile.persona.sprite.color.r, profile.persona.sprite.color.g, profile.persona.sprite.color.b);
        profile.persona.armSprite.alpha = Maths.Clamp(this._swapFade, 0.0f, 1f);
        profile.persona.armSprite.color = color * (float) (0.699999988079071 + (double) this._projectorSin.normalized * 0.100000001490116);
        profile.persona.armSprite.color = new Color(profile.persona.armSprite.color.r, profile.persona.armSprite.color.g, profile.persona.armSprite.color.b);
        profile.persona.sprite.scale = new Vec2(1f, 1f);
        profile.persona.armSprite.scale = new Vec2(1f, 1f);
        float num2 = 12f;
        float num3 = (float) ((double) this.x - (double) (count - 1) * (double) num2 / 2.0 + (double) num1 * (double) num2);
        profile.persona.sprite.depth = (Depth) (float) ((double) num1 * 0.00999999977648258 - 0.400000005960464);
        profile.persona.armSprite.depth = (Depth) (float) ((double) num1 * 0.00999999977648258 - 0.300000011920929);
        Graphics.Draw((Sprite) profile.persona.sprite, num3 + 1f, this.y - 17f, new Depth(-0.4f));
        Graphics.Draw((Sprite) profile.persona.armSprite, (float) ((double) num3 + 1.0 - 3.0), (float) ((double) this.y - 17.0 + 6.0));
        Team team = profile.team;
        if (team != null)
        {
          Vec2 hatPoint = DuckRig.GetHatPoint(profile.persona.sprite.imageIndex);
          team.hat.depth = profile.persona.sprite.depth + 1;
          team.hat.alpha = profile.persona.sprite.alpha;
          team.hat.color = profile.persona.sprite.color;
          team.hat.center = new Vec2(16f, 16f) + team.hatOffset;
          Graphics.Draw((Sprite) team.hat, (float) ((double) num3 + (double) hatPoint.x + 1.0), this.y - 17f + hatPoint.y);
          team.hat.color = Color.White;
        }
        this._profile.persona.sprite.color = Color.White;
        this._profile.persona.armSprite.color = Color.White;
        ++num1;
      }
      Graphics.Draw(this._selectPlatform, this.x, this.y);
      Graphics.Draw(this._selectProjector, this.x, this.y - 6f);
    }
  }
}
