// Decompiled with JetBrains decompiler
// Type: DuckGame.DrinkRoom
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class DrinkRoom : Level, IHaveAVirtualTransition
  {
    private Level _next;
    private bool _fade;

    public DrinkRoom(Level next)
    {
      this.transitionSpeedMultiplier = 4f;
      this._centeredView = true;
      this._next = next;
    }

    public override void Initialize()
    {
      HUD.AddCornerMessage(HUDCorner.BottomRight, "@GRAB@CONTINUE");
      base.Initialize();
    }

    public override void Update()
    {
      if (Input.Pressed("GRAB"))
        this._fade = true;
      Graphics.fade = Lerp.Float(Graphics.fade, this._fade ? 0.0f : 1f, 0.1f);
      if (this._fade && (double) Graphics.fade < 0.00999999977648258)
        Level.current = this._next;
      base.Update();
    }

    public override void Draw()
    {
      float y = 12f;
      foreach (Profile p in Profiles.active)
      {
        bool flag = false;
        int drinks = Party.GetDrinks(p);
        if (drinks > 0)
        {
          string text = p.name + " |WHITE|drinks |RED|" + (object) drinks;
          Graphics.DrawString(text, new Vec2((float) ((double) Layer.HUD.camera.width / 2.0 - (double) Graphics.GetStringWidth(text) / 2.0), y), p.persona.colorUsable);
          y += 9f;
          flag = true;
        }
        foreach (PartyPerks perk in Party.GetPerks(p))
        {
          string text = p.name + " |WHITE|gets |GREEN|" + perk.ToString();
          Graphics.DrawString(text, new Vec2((float) ((double) Layer.HUD.camera.width / 2.0 - (double) Graphics.GetStringWidth(text) / 2.0), y), p.persona.colorUsable);
          y += 9f;
          flag = true;
        }
        if (flag)
          y += 9f;
      }
    }
  }
}
