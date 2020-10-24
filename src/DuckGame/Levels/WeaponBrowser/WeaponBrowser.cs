// Decompiled with JetBrains decompiler
// Type: DuckGame.WeaponBrowser
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class WeaponBrowser : Level
  {
    private BitmapFont _font;

    public override void Initialize()
    {
      Layer.Add((Layer) new GridBackground("GRID", 99999));
      this._font = new BitmapFont("duckFont", 8);
      this._font.scale = new Vec2(2f, 2f);
      Gun gun = (Gun) new Saxaphone(0.0f, 0.0f);
      gun.scale = new Vec2(2f, 2f);
      UIMenu uiMenu = new UIMenu(gun.editorName, Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 220f);
      UIBox uiBox1 = new UIBox(false, false);
      UIImage uiImage = new UIImage(gun.GetEditorImage(64, 64, true));
      uiImage.collisionSize = new Vec2(64f, 32f);
      uiBox1.Add((UIComponent) uiImage, true);
      UIBox uiBox2 = new UIBox(isVisible: false);
      uiBox2.Add((UIComponent) new UIText("AMMO: " + (gun.ammo > 900 ? "INFINITE" : gun.ammo.ToString()), Color.White, UIAlign.Left), true);
      string str1 = "SHORT";
      if ((double) gun.ammoType.range > 150.0)
        str1 = "MEDIUM";
      if ((double) gun.ammoType.range > 300.0)
        str1 = "LONG";
      if ((double) gun.ammoType.range > 600.0)
        str1 = "EXTREME";
      uiBox2.Add((UIComponent) new UIText("RANGE: " + str1, Color.White, UIAlign.Left), true);
      if ((double) gun.ammoType.penetration > 0.0)
        uiBox2.Add((UIComponent) new UIText("PENETRATION: " + (object) gun.ammoType.penetration, Color.White, UIAlign.Left), true);
      else
        uiBox2.Add((UIComponent) new UIText("SPECIAL AMMO", Color.White, UIAlign.Left), true);
      uiBox1.Add((UIComponent) uiBox2, true);
      uiMenu.Add((UIComponent) uiBox1, true);
      UIBox uiBox3 = new UIBox(isVisible: false);
      uiBox3.Add((UIComponent) new UIText("---------------------", Color.White), true);
      float num = 190f;
      string str2 = gun.bio;
      string textVal = "";
      string str3 = "";
      while (true)
      {
        if (str2.Length > 0 && str2[0] != ' ')
        {
          str3 += (string) (object) str2[0];
        }
        else
        {
          if ((double) ((textVal.Length + str3.Length) * 8) > (double) num)
          {
            uiBox3.Add((UIComponent) new UIText(textVal, Color.White), true);
            textVal = "";
          }
          if (textVal.Length > 0)
            textVal += " ";
          textVal += str3;
          str3 = "";
        }
        if (str2.Length != 0)
          str2 = str2.Remove(0, 1);
        else
          break;
      }
      if (str3.Length > 0)
      {
        if (textVal.Length > 0)
          textVal += " ";
        textVal += str3;
      }
      if (textVal.Length > 0)
        uiBox3.Add((UIComponent) new UIText(textVal, Color.White), true);
      uiMenu.Add((UIComponent) uiBox3, true);
      Level.Add((Thing) uiMenu);
    }

    public override void Update()
    {
    }

    public override void Draw()
    {
    }

    public override void PostDrawLayer(Layer layer)
    {
    }
  }
}
