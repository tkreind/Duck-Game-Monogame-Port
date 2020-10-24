// Decompiled with JetBrains decompiler
// Type: DuckGame.Options
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace DuckGame
{
  internal static class Options
  {
    private static UIMenu _optionsMenu;
    private static OptionsData _data = new OptionsData();
    private static bool _removedOptionsMenu = false;
    private static bool _openedOptionsMenu = false;
    public static UIMenu openOnClose = (UIMenu) null;

    public static UIMenu optionsMenu => Options._optionsMenu;

    public static OptionsData Data
    {
      get => Options._data;
      set => Options._data = value;
    }

    public static bool menuOpen => Options._optionsMenu.open;

    public static void Initialize()
    {
      Options._optionsMenu = new UIMenu("@WRENCH@OPTIONS@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@DPAD@ADJUST @QUACK@BACK");
      Options._optionsMenu.Add((UIComponent) new UIMenuItemSlider("SFX VOLUME", field: new FieldBinding((object) Options.Data, "sfxVolume"), step: 0.125f), true);
      Options._optionsMenu.Add((UIComponent) new UIMenuItemSlider("MUSIC VOLUME", field: new FieldBinding((object) Options.Data, "musicVolume"), step: 0.125f), true);
      Options._optionsMenu.Add((UIComponent) new UIMenuItemToggle("SHENANIGANS", field: new FieldBinding((object) Options.Data, "shennanigans")), true);
      Options._optionsMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      Options._optionsMenu.Add((UIComponent) new UIMenuItemToggle("FULLSCREEN", field: new FieldBinding((object) Options.Data, "fullscreen")), true);
      Options._optionsMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      Options._optionsMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionCloseMenuCallFunction((UIComponent) Options._optionsMenu, new UIMenuActionCloseMenuCallFunction.Function(Options.OptionsMenuClosed)), backButton: true), true);
      Options._optionsMenu.Close();
    }

    public static void OpenOptionsMenu()
    {
      Options._removedOptionsMenu = false;
      Options._openedOptionsMenu = true;
      Level.Add((Thing) Options._optionsMenu);
      Options._optionsMenu.Open();
    }

    public static void OptionsMenuClosed()
    {
      Options.Save();
      if (Options.openOnClose == null)
        return;
      Options.openOnClose.Open();
    }

    public static void Save()
    {
      XDocument doc = new XDocument();
      XElement xelement = new XElement((XName) "Data");
      xelement.Add((object) Options._data.Serialize());
      doc.Add((object) xelement);
      string path = DuckFile.optionsDirectory + "options.dat";
      DuckFile.SaveXDocument(doc, path);
    }

    public static void Load()
    {
      XDocument xdocument = DuckFile.LoadXDocument(DuckFile.optionsDirectory + "options.dat");
      if (xdocument != null)
      {
        Profile profile = new Profile("");
        IEnumerable<XElement> source = xdocument.Elements((XName) "Data");
        if (source != null)
        {
          foreach (XElement element in source.Elements<XElement>())
          {
            if (element.Name.LocalName == nameof (Options))
            {
              Options._data.Deserialize(element);
              break;
            }
          }
        }
      }
      if (DuckFile.cloudOverload)
        Options.Data.cloud = true;
      if ((double) Options.Data.musicVolume > 1.0)
        Options.Data.musicVolume /= 100f;
      if ((double) Options.Data.sfxVolume <= 1.0)
        return;
      Options.Data.sfxVolume /= 100f;
    }

    public static void Update()
    {
      Music.masterVolume = Math.Min(1f, Math.Max(0.0f, Options.Data.musicVolume));
      SFX.volume = Math.Min(1f, Math.Max(0.0f, Options.Data.sfxVolume));
      if (!Options._openedOptionsMenu || Options._removedOptionsMenu || (Options._optionsMenu.open || Options._optionsMenu.animating))
        return;
      Options._openedOptionsMenu = false;
      Options._removedOptionsMenu = true;
      Level.Remove((Thing) Options._optionsMenu);
    }
  }
}
