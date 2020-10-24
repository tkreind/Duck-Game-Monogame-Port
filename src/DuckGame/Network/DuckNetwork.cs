// Decompiled with JetBrains decompiler
// Type: DuckGame.DuckNetwork
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuckGame
{
  public class DuckNetwork
  {
    private static List<OnlineLevel> _levels = new List<OnlineLevel>()
    {
      new OnlineLevel() { num = 1, xpRequired = 0 },
      new OnlineLevel() { num = 2, xpRequired = 175 },
      new OnlineLevel() { num = 3, xpRequired = 400 },
      new OnlineLevel() { num = 4, xpRequired = 1200 },
      new OnlineLevel() { num = 5, xpRequired = 3500 },
      new OnlineLevel() { num = 6, xpRequired = 6500 },
      new OnlineLevel() { num = 7, xpRequired = 10000 },
      new OnlineLevel() { num = 8, xpRequired = 13000 },
      new OnlineLevel() { num = 9, xpRequired = 16000 },
      new OnlineLevel() { num = 10, xpRequired = 19000 },
      new OnlineLevel() { num = 11, xpRequired = 23000 },
      new OnlineLevel() { num = 12, xpRequired = 28000 },
      new OnlineLevel() { num = 13, xpRequired = 34000 },
      new OnlineLevel() { num = 14, xpRequired = 40000 },
      new OnlineLevel() { num = 15, xpRequired = 45000 },
      new OnlineLevel() { num = 16, xpRequired = 50000 },
      new OnlineLevel() { num = 17, xpRequired = 56000 },
      new OnlineLevel() { num = 18, xpRequired = 62000 },
      new OnlineLevel() { num = 19, xpRequired = 75000 },
      new OnlineLevel() { num = 20, xpRequired = 100000 }
    };
    public static int kills;
    public static int deaths;
    private static DuckNetworkCore _core = new DuckNetworkCore();
    private static FancyBitmapFont _chatFont;
    private static UIMenu _ducknetMenu;
    private static UIMenu _optionsMenu;
    private static UIMenu _confirmMenu;
    private static UIMenu _confirmKick;
    private static MenuBoolean _quit = new MenuBoolean();
    private static MenuBoolean _menuClosed = new MenuBoolean();
    public static int numSlots = 4;
    public static Profile kickContext;
    public static List<ulong> _invitedFriends = new List<ulong>();
    private static MenuBoolean _inviteFriends = new MenuBoolean();
    private static UIMenu _inviteMenu;
    private static UIMenu _slotEditor;
    private static UIMenu _matchSettingMenu;
    private static UIMenu _matchModifierMenu;
    private static UIComponent _noModsUIGroup;
    private static UIMenu _noModsMenu;
    private static UIComponent _restartModsUIGroup;
    private static UIMenu _restartModsMenu;
    private static bool _pauseOpen = false;
    private static string _settingsBeforeOpen = "";
    private static bool _willOpenSettingsInfo = false;
    private static UIMenu _levelSelectMenu;
    private static Profile _menuOpenProfile;
    private static UIComponent _uhOhGroup;
    private static UIMenu _uhOhMenu;

    public static OnlineLevel GetLevel(int lev)
    {
      foreach (OnlineLevel level in DuckNetwork._levels)
      {
        if (level.num == lev)
          return level;
      }
      return DuckNetwork._levels.Last<OnlineLevel>();
    }

    public static Dictionary<string, XPPair> _xpEarned
    {
      get => DuckNetwork._core._xpEarned;
      set => DuckNetwork._core._xpEarned = value;
    }

    public static void GiveXP(
      string category,
      int num,
      int xp,
      int type = 4,
      int firstCap = 9999999,
      int secondCap = 9999999,
      int finalCap = 9999999)
    {
      if (Profiles.experienceProfile == null)
        return;
      if (!DuckNetwork._xpEarned.ContainsKey(category))
        DuckNetwork._xpEarned[category] = new XPPair();
      DuckNetwork._xpEarned[category].num += num;
      if (DuckNetwork._xpEarned[category].xp > secondCap)
        DuckNetwork._xpEarned[category].xp += xp / 4;
      else if (DuckNetwork._xpEarned[category].xp > firstCap)
        DuckNetwork._xpEarned[category].xp += xp / 2;
      else
        DuckNetwork._xpEarned[category].xp += xp;
      if (DuckNetwork._xpEarned[category].xp > finalCap)
        DuckNetwork._xpEarned[category].xp = finalCap;
      DuckNetwork._xpEarned[category].type = type;
    }

    private static UIMenu _xpMenu
    {
      get => DuckNetwork._core.xpMenu;
      set => DuckNetwork._core.xpMenu = value;
    }

    public static bool ShowUserXPGain()
    {
      if (DuckNetwork._xpEarned.Count <= 0)
        return false;
      DuckNetwork._xpMenu = (UIMenu) new UILevelBox("@LWING@PAUSE@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f, conString: "@DPAD@MOVE  @SELECT@SELECT");
      MonoMain.pauseMenu = (UIComponent) DuckNetwork._xpMenu;
      return true;
    }

    public static KeyValuePair<string, XPPair> TakeXPStat()
    {
      if (DuckNetwork._xpEarned.Count == 0)
        return new KeyValuePair<string, XPPair>();
      KeyValuePair<string, XPPair> keyValuePair = DuckNetwork._xpEarned.ElementAt<KeyValuePair<string, XPPair>>(0);
      DuckNetwork._xpEarned.Remove(keyValuePair.Key);
      return keyValuePair;
    }

    public static int GetTotalXPEarned()
    {
      int num = 0;
      foreach (KeyValuePair<string, XPPair> keyValuePair in DuckNetwork._xpEarned)
        num += keyValuePair.Value.xp;
      return num;
    }

    public static DuckNetworkCore core
    {
      get => DuckNetwork._core;
      set => DuckNetwork._core = value;
    }

    public static NetworkConnection localConnection
    {
      get => DuckNetwork._core.localConnection;
      set => DuckNetwork._core.localConnection = value;
    }

    public static bool active => DuckNetwork._core.status != DuckNetStatus.Disconnected;

    public static byte levelIndex
    {
      get => DuckNetwork._core.levelIndex;
      set => DuckNetwork._core.levelIndex = value;
    }

    public static MemoryStream compressedLevelData
    {
      get => DuckNetwork._core.compressedLevelData;
      set => DuckNetwork._core.compressedLevelData = value;
    }

    public static List<Profile> profiles => DuckNetwork._core.profiles;

    public static int localDuckIndex => DuckNetwork._core.localDuckIndex;

    public static int hostDuckIndex => DuckNetwork._core.hostDuckIndex;

    public static DuckNetStatus status => DuckNetwork._core.status;

    public static int randomID => DuckNetwork._core.randomID;

    public static DuckNetErrorInfo error => DuckNetwork._core.error;

    public static bool inGame
    {
      get => DuckNetwork._core.inGame;
      set => DuckNetwork._core.inGame = value;
    }

    public static bool enteringText => DuckNetwork._core.enteringText;

    public static void RaiseError(DuckNetErrorInfo e)
    {
      if (e == null)
        return;
      DevConsole.Log(DCSection.DuckNet, e.message);
      if (DuckNetwork._core.error != null)
        return;
      DuckNetwork._core.error = e;
    }

    private static UIComponent _ducknetUIGroup
    {
      get => DuckNetwork._core.ducknetUIGroup;
      set => DuckNetwork._core.ducknetUIGroup = value;
    }

    public static UIComponent duckNetUIGroup => DuckNetwork._ducknetUIGroup;

    public static void Initialize() => DuckNetwork._chatFont = new FancyBitmapFont("smallFontChat");

    public static void Kick(Profile p)
    {
      if (p.slotType == SlotType.Local)
      {
        DuckNetwork.SendToEveryone((NetMessage) new NMClientDisconnect(DG.localID.ToString(), p.networkIndex));
        DuckNetwork.ResetProfile(p);
        p.team = (Team) null;
      }
      else
      {
        if (!Network.isServer || p == null || (p.connection == null || p.connection == DuckNetwork.localConnection))
          return;
        SFX.Play("little_punch");
        Send.Message((NetMessage) new NMKick(), p.connection);
        p.networkStatus = DuckNetStatus.Kicking;
      }
    }

    public static void ChangeSlotSettings()
    {
      bool flag1 = true;
      bool flag2 = true;
      DuckNetwork.numSlots = 0;
      foreach (Profile profile in DuckNetwork.profiles)
      {
        if (profile.connection != DuckNetwork.localConnection)
        {
          if (profile.slotType != SlotType.Friend)
            flag1 = false;
          if (profile.slotType != SlotType.Invite)
            flag2 = false;
          if (profile.slotType != SlotType.Closed)
            ++DuckNetwork.numSlots;
        }
        else if (profile.slotType != SlotType.Closed)
          ++DuckNetwork.numSlots;
      }
      if (!Network.isServer)
        return;
      if (Steam.lobby != null)
      {
        Steam.lobby.type = !flag1 ? (!flag2 ? SteamLobbyType.Public : SteamLobbyType.Private) : SteamLobbyType.FriendsOnly;
        Steam.lobby.maxMembers = 32;
        Steam.lobby.SetLobbyData("numSlots", DuckNetwork.numSlots.ToString());
      }
      Send.Message((NetMessage) new NMChangeSlots((byte) DuckNetwork.profiles[0].slotType, (byte) DuckNetwork.profiles[1].slotType, (byte) DuckNetwork.profiles[2].slotType, (byte) DuckNetwork.profiles[3].slotType));
    }

    public static void KickedPlayer()
    {
      if (DuckNetwork.kickContext == null)
        return;
      DuckNetwork.Kick(DuckNetwork.kickContext);
      DuckNetwork.kickContext = (Profile) null;
    }

    public static void ClosePauseMenu()
    {
      if (!Network.isActive || MonoMain.pauseMenu == null)
        return;
      MonoMain.pauseMenu.Close();
      MonoMain.pauseMenu = (UIComponent) null;
      if (DuckNetwork._ducknetUIGroup == null)
        return;
      Level.Remove((Thing) DuckNetwork._ducknetUIGroup);
      DuckNetwork._ducknetUIGroup = (UIComponent) null;
    }

    public static void OpenMatchSettingsInfo() => DuckNetwork._willOpenSettingsInfo = true;

    public static void OpenNoModsWindow(
      UIMenuActionCloseMenuCallFunction.Function acceptFunction)
    {
      DuckNetwork._noModsUIGroup = new UIComponent(320f / 2f, 180f / 2f, 0.0f, 0.0f);
      DuckNetwork._noModsMenu = DuckNetwork.CreateNoModsOnlineWindow(acceptFunction);
      DuckNetwork._noModsUIGroup.Add((UIComponent) DuckNetwork._noModsMenu, false);
      DuckNetwork._noModsUIGroup.Close();
      DuckNetwork._noModsUIGroup.Close();
      Level.Add((Thing) DuckNetwork._noModsUIGroup);
      DuckNetwork._noModsUIGroup.Update();
      DuckNetwork._noModsUIGroup.Update();
      DuckNetwork._noModsUIGroup.Update();
      DuckNetwork._noModsUIGroup.Open();
      DuckNetwork._noModsMenu.Open();
      MonoMain.pauseMenu = DuckNetwork._noModsUIGroup;
      DuckNetwork._pauseOpen = true;
      SFX.Play("pause", 0.6f);
    }

    private static UIMenu CreateNoModsOnlineWindow(
      UIMenuActionCloseMenuCallFunction.Function acceptFunction)
    {
      UIMenu uiMenu = new UIMenu("@LWING@YOU HAVE MODS ENABLED@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 230f, conString: "@QUACK@BACK");
      BitmapFont f = new BitmapFont("smallBiosFontUI", 7, 5);
      UIText uiText1 = new UIText("YOU WILL |DGRED|NOT|WHITE| BE ABLE TO PLAY", Color.White);
      uiText1.SetFont(f);
      uiMenu.Add((UIComponent) uiText1, true);
      UIText uiText2 = new UIText("ONLINE WITH ANYONE WHO DOES ", Color.White);
      uiText2.SetFont(f);
      uiMenu.Add((UIComponent) uiText2, true);
      UIText uiText3 = new UIText("NOT HAVE THE |DGRED|SAME MODS|WHITE|.     ", Color.White);
      uiText3.SetFont(f);
      uiMenu.Add((UIComponent) uiText3, true);
      UIText uiText4 = new UIText("", Color.White);
      uiText4.SetFont(f);
      uiMenu.Add((UIComponent) uiText4, true);
      UIText uiText5 = new UIText("WOULD YOU LIKE TO |DGGREEN|DISABLE|WHITE|   ", Color.White);
      uiText5.SetFont(f);
      uiMenu.Add((UIComponent) uiText5, true);
      UIText uiText6 = new UIText("MODS AND RESTART THE GAME?  ", Color.White);
      uiText6.SetFont(f);
      uiMenu.Add((UIComponent) uiText6, true);
      UIText uiText7 = new UIText("", Color.White);
      uiText7.SetFont(f);
      uiMenu.Add((UIComponent) uiText7, true);
      UIText uiText8 = new UIText("", Color.White);
      uiText8.SetFont(f);
      uiMenu.Add((UIComponent) uiText8, true);
      uiMenu.Add((UIComponent) new UIMenuItem("|DGGREEN|DISABLE MODS AND RESTART", (UIMenuAction) new UIMenuActionCloseMenuCallFunction(DuckNetwork._noModsUIGroup, new UIMenuActionCloseMenuCallFunction.Function(ModLoader.DisableModsAndRestart)), c: Color.White), true);
      uiMenu.Add((UIComponent) new UIMenuItem("|DGYELLOW|I KNOW WHAT I'M DOING", (UIMenuAction) new UIMenuActionCloseMenuCallFunction(DuckNetwork._noModsUIGroup, acceptFunction), c: Color.White), true);
      uiMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionCloseMenu(DuckNetwork._noModsUIGroup), backButton: true), true);
      uiMenu.Close();
      return uiMenu;
    }

    public static UIComponent OpenModsRestartWindow(UIMenu openOnClose)
    {
      DuckNetwork._restartModsUIGroup = new UIComponent(320f / 2f, 180f / 2f, 0.0f, 0.0f);
      DuckNetwork._restartModsMenu = DuckNetwork.CreateModsRestartWindow(openOnClose);
      DuckNetwork._restartModsUIGroup.Add((UIComponent) DuckNetwork._restartModsMenu, false);
      DuckNetwork._restartModsUIGroup.Close();
      DuckNetwork._restartModsUIGroup.Close();
      Level.Add((Thing) DuckNetwork._restartModsUIGroup);
      DuckNetwork._restartModsUIGroup.Update();
      DuckNetwork._restartModsUIGroup.Update();
      DuckNetwork._restartModsUIGroup.Update();
      DuckNetwork._restartModsUIGroup.Open();
      DuckNetwork._restartModsMenu.Open();
      MonoMain.pauseMenu = DuckNetwork._restartModsUIGroup;
      DuckNetwork._pauseOpen = true;
      SFX.Play("pause", 0.6f);
      return DuckNetwork._restartModsUIGroup;
    }

    private static UIMenu CreateModsRestartWindow(UIMenu openOnClose)
    {
      UIMenu uiMenu = new UIMenu("@LWING@MODS CHANGED@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 230f, conString: "@QUACK@BACK");
      BitmapFont f = new BitmapFont("smallBiosFontUI", 7, 5);
      UIText uiText1 = new UIText("YOU NEED TO RESTART THE GAME", Color.White);
      uiText1.SetFont(f);
      uiMenu.Add((UIComponent) uiText1, true);
      UIText uiText2 = new UIText("FOR CHANGES TO TAKE EFFECT. ", Color.White);
      uiText2.SetFont(f);
      uiMenu.Add((UIComponent) uiText2, true);
      UIText uiText3 = new UIText("", Color.White);
      uiText3.SetFont(f);
      uiMenu.Add((UIComponent) uiText3, true);
      UIText uiText4 = new UIText("DO YOU WANT TO |DGGREEN|RESTART|WHITE| NOW? ", Color.White);
      uiText4.SetFont(f);
      uiMenu.Add((UIComponent) uiText4, true);
      uiMenu.Add((UIComponent) new UIMenuItem("|DGGREEN|RESTART", (UIMenuAction) new UIMenuActionCloseMenuCallFunction(DuckNetwork._restartModsUIGroup, new UIMenuActionCloseMenuCallFunction.Function(ModLoader.RestartGame)), c: Color.White), true);
      uiMenu.Add((UIComponent) new UIMenuItem("|DGYELLOW|CONTINUE", (UIMenuAction) new UIMenuActionOpenMenu(DuckNetwork._restartModsUIGroup, (UIComponent) openOnClose), c: Color.White), true);
      uiMenu.Close();
      return uiMenu;
    }

    private static UIMenu CreateMatchSettingsInfoWindow(UIMenu openOnClose = null)
    {
      UIMenu uiMenu = openOnClose == null ? new UIMenu("@LWING@NEW SETTINGS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@QUACK@BACK") : new UIMenu("@LWING@MATCH SETTINGS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@QUACK@BACK");
      BitmapFont f = new BitmapFont("smallBiosFontUI", 7, 5);
      UIText uiText1 = new UIText("THE HOST HAS CHANGED", Color.White);
      uiText1.SetFont(f);
      if (openOnClose == null)
      {
        uiMenu.Add((UIComponent) uiText1, true);
        UIText uiText2 = new UIText("THE MATCH SETTINGS.", Color.White);
        uiText2.SetFont(f);
        uiMenu.Add((UIComponent) uiText2, true);
        UIText uiText3 = new UIText(" ", Color.White);
        uiText3.SetFont(f);
        uiMenu.Add((UIComponent) uiText3, true);
      }
      MatchSetting matchSetting1 = TeamSelect2.GetMatchSetting("requiredwins");
      int num1 = 16;
      int num2 = 5;
      string name1 = matchSetting1.name;
      string str1 = matchSetting1.value.ToString();
      while (name1.Length < num1)
        name1 += " ";
      while (str1.Length < num2)
        str1 = " " + str1;
      string textVal1 = name1 + " " + str1;
      UIText uiText4 = matchSetting1.value == matchSetting1.prevValue ? new UIText(textVal1, Colors.Silver) : new UIText(textVal1, Colors.DGGreen);
      matchSetting1.prevValue = matchSetting1.value;
      uiText4.SetFont(f);
      uiMenu.Add((UIComponent) uiText4, true);
      MatchSetting matchSetting2 = TeamSelect2.GetMatchSetting("restsevery");
      string name2 = matchSetting2.name;
      string str2 = matchSetting2.value.ToString();
      while (name2.Length < num1)
        name2 += " ";
      while (str2.Length < num2)
        str2 = " " + str2;
      string textVal2 = name2 + " " + str2;
      UIText uiText5 = matchSetting2.value == matchSetting2.prevValue ? new UIText(textVal2, Colors.Silver) : new UIText(textVal2, Colors.DGGreen);
      matchSetting2.prevValue = matchSetting2.value;
      uiText5.SetFont(f);
      uiMenu.Add((UIComponent) uiText5, true);
      MatchSetting matchSetting3 = TeamSelect2.GetMatchSetting("normalmaps");
      string name3 = matchSetting3.name;
      string str3 = matchSetting3.value.ToString() + "%";
      if (matchSetting3.minString != null && matchSetting3.value is int && (int) matchSetting3.value == matchSetting3.min)
        str3 = matchSetting3.minString;
      int startIndex1 = matchSetting3.name.LastIndexOf('|');
      for (string str4 = matchSetting3.name.Substring(startIndex1, matchSetting3.name.Count<char>() - startIndex1); str4.Length < num1; str4 += " ")
        name3 += " ";
      while (str3.Length < num2)
        str3 = " " + str3;
      string textVal3 = name3 + " " + str3;
      UIText uiText6 = matchSetting3.value == matchSetting3.prevValue ? new UIText(textVal3, Colors.Silver) : new UIText(textVal3, Colors.DGGreen);
      matchSetting3.prevValue = matchSetting3.value;
      uiText6.SetFont(f);
      uiMenu.Add((UIComponent) uiText6, true);
      MatchSetting matchSetting4 = TeamSelect2.GetMatchSetting("custommaps");
      string name4 = matchSetting4.name;
      string str5 = matchSetting4.value.ToString() + "%";
      if (matchSetting4.minString != null && matchSetting4.value is int && (int) matchSetting4.value == matchSetting4.min)
        str5 = matchSetting4.minString;
      int startIndex2 = matchSetting4.name.LastIndexOf('|');
      for (string str4 = matchSetting4.name.Substring(startIndex2, matchSetting4.name.Count<char>() - startIndex2); str4.Length < num1; str4 += " ")
        name4 += " ";
      while (str5.Length < num2)
        str5 = " " + str5;
      string textVal4 = name4 + " " + str5;
      UIText uiText7 = matchSetting4.value == matchSetting4.prevValue ? new UIText(textVal4, Colors.Silver) : new UIText(textVal4, Colors.DGGreen);
      matchSetting4.prevValue = matchSetting4.value;
      uiText7.SetFont(f);
      uiMenu.Add((UIComponent) uiText7, true);
      string str6 = "Num Custom: ";
      string str7 = TeamSelect2.customLevels.ToString();
      while (str6.Length < num1)
        str6 += " ";
      while (str7.Length < num2)
        str7 = " " + str7;
      string textVal5 = str6 + " " + str7;
      UIText uiText8 = TeamSelect2.customLevels == TeamSelect2.prevCustomLevels ? new UIText(textVal5, Colors.Silver) : new UIText(textVal5, TeamSelect2.customLevels > 0 ? Colors.DGGreen : Colors.DGRed);
      TeamSelect2.prevCustomLevels = TeamSelect2.customLevels;
      uiText8.SetFont(f);
      uiMenu.Add((UIComponent) uiText8, true);
      MatchSetting matchSetting5 = TeamSelect2.GetMatchSetting("randommaps");
      string name5 = matchSetting5.name;
      string str8 = matchSetting5.value.ToString() + "%";
      int startIndex3 = matchSetting5.name.LastIndexOf('|');
      for (string str4 = matchSetting5.name.Substring(startIndex3, matchSetting5.name.Count<char>() - startIndex3); str4.Length < num1; str4 += " ")
        name5 += " ";
      while (str8.Length < num2)
        str8 = " " + str8;
      string textVal6 = name5 + " " + str8;
      UIText uiText9 = matchSetting5.value == matchSetting5.prevValue ? new UIText(textVal6, Colors.Silver) : new UIText(textVal6, Colors.DGGreen);
      matchSetting5.prevValue = matchSetting5.value;
      uiText9.SetFont(f);
      uiMenu.Add((UIComponent) uiText9, true);
      MatchSetting matchSetting6 = TeamSelect2.GetMatchSetting("workshopmaps");
      string name6 = matchSetting6.name;
      string str9 = matchSetting6.value.ToString() + "%";
      int startIndex4 = matchSetting6.name.LastIndexOf('|');
      for (string str4 = matchSetting6.name.Substring(startIndex4, matchSetting6.name.Count<char>() - startIndex4); str4.Length < num1; str4 += " ")
        name6 += " ";
      while (str9.Length < num2)
        str9 = " " + str9;
      string textVal7 = name6 + " " + str9;
      UIText uiText10 = matchSetting6.value == matchSetting6.prevValue ? new UIText(textVal7, Colors.Silver) : new UIText(textVal7, Colors.DGGreen);
      matchSetting6.prevValue = matchSetting6.value;
      uiText10.SetFont(f);
      uiMenu.Add((UIComponent) uiText10, true);
      MatchSetting onlineSetting = TeamSelect2.GetOnlineSetting("teams");
      string name7 = onlineSetting.name;
      string str10 = onlineSetting.value.ToString();
      while (name7.Length < num1)
        name7 += " ";
      while (str10.Length < num2)
        str10 = " " + str10;
      string textVal8 = name7 + " " + str10;
      UIText uiText11 = onlineSetting.value == onlineSetting.prevValue ? new UIText(textVal8, Colors.Silver) : new UIText(textVal8, (bool) onlineSetting.value ? Colors.DGGreen : Colors.DGRed);
      onlineSetting.prevValue = onlineSetting.value;
      uiText11.SetFont(f);
      uiMenu.Add((UIComponent) uiText11, true);
      UIText uiText12 = new UIText(" ", Color.White);
      uiText12.SetFont(f);
      uiMenu.Add((UIComponent) uiText12, true);
      MatchSetting matchSetting7 = TeamSelect2.GetMatchSetting("wallmode");
      string name8 = matchSetting7.name;
      string str11 = matchSetting7.value.ToString();
      while (name8.Length < num1)
        name8 += " ";
      while (str11.Length < num2)
        str11 = " " + str11;
      string textVal9 = name8 + " " + str11;
      UIText uiText13 = matchSetting7.value == matchSetting7.prevValue ? new UIText(textVal9, Colors.Silver) : new UIText(textVal9, (bool) matchSetting7.value ? Colors.DGGreen : Colors.DGRed);
      matchSetting7.prevValue = matchSetting7.value;
      uiText13.SetFont(f);
      uiMenu.Add((UIComponent) uiText13, true);
      UIText uiText14 = new UIText(" ", Color.White);
      uiText14.SetFont(f);
      uiMenu.Add((UIComponent) uiText14, true);
      foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
      {
        if (unlock.onlineEnabled)
        {
          string shortName = unlock.shortName;
          while (shortName.Length < 20)
            shortName += " ";
          if (unlock.enabled != unlock.prevEnabled || unlock.enabled)
          {
            string textVal10 = !unlock.enabled ? "@USEROFFLINE@" + shortName : "@USERONLINE@" + shortName;
            UIText uiText2 = unlock.enabled == unlock.prevEnabled ? new UIText(textVal10, unlock.enabled ? Color.White : Colors.Silver) : new UIText(textVal10, unlock.enabled ? Colors.DGGreen : Colors.DGRed);
            uiText2.SetFont(f);
            uiMenu.Add((UIComponent) uiText2, true);
          }
          unlock.prevEnabled = unlock.enabled;
        }
      }
      UIText uiText15 = new UIText(" ", Color.White);
      uiText15.SetFont(f);
      uiMenu.Add((UIComponent) uiText15, true);
      if (openOnClose != null)
        uiMenu.Add((UIComponent) new UIMenuItem("OK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) uiMenu, (UIComponent) openOnClose), c: Color.White, backButton: true), true);
      else
        uiMenu.Add((UIComponent) new UIMenuItem("OK", (UIMenuAction) new UIMenuActionCloseMenu(DuckNetwork._ducknetUIGroup), c: Color.White, backButton: true), true);
      uiMenu.Close();
      return uiMenu;
    }

    private static void DoMatchSettingsInfoOpen()
    {
      DuckNetwork._ducknetUIGroup = new UIComponent(320f / 2f, 180f / 2f, 0.0f, 0.0f);
      DuckNetwork._matchSettingMenu = DuckNetwork.CreateMatchSettingsInfoWindow();
      DuckNetwork._ducknetUIGroup.Add((UIComponent) DuckNetwork._matchSettingMenu, false);
      DuckNetwork._ducknetUIGroup.Close();
      DuckNetwork._ducknetUIGroup.Close();
      Level.Add((Thing) DuckNetwork._ducknetUIGroup);
      DuckNetwork._ducknetUIGroup.Update();
      DuckNetwork._ducknetUIGroup.Update();
      DuckNetwork._ducknetUIGroup.Update();
      DuckNetwork._ducknetUIGroup.Open();
      DuckNetwork._matchSettingMenu.Open();
      MonoMain.pauseMenu = DuckNetwork._ducknetUIGroup;
      DuckNetwork._pauseOpen = true;
      SFX.Play("pause", 0.6f);
    }

    private static void OpenMenu(Profile whoOpen)
    {
      if (DuckNetwork._ducknetUIGroup != null)
        Level.Remove((Thing) DuckNetwork._ducknetUIGroup);
      bool flag = Level.current is TeamSelect2;
      DuckNetwork._menuOpenProfile = whoOpen;
      float num1 = 320f;
      float num2 = 180f;
      DuckNetwork._ducknetUIGroup = new UIComponent(num1 / 2f, num2 / 2f, 0.0f, 0.0f);
      DuckNetwork._ducknetMenu = new UIMenu("@LWING@MULTIPLAYER@RWING@", num1 / 2f, num2 / 2f, 210f, conString: "@DPAD@MOVE @SELECT@SELECT @QUACK@BACK");
      DuckNetwork._confirmMenu = whoOpen.slotType != SlotType.Local ? new UIMenu("REALLY QUIT?", num1 / 2f, num2 / 2f, 160f, conString: "@SELECT@SELECT") : new UIMenu("REALLY BACK OUT?", num1 / 2f, num2 / 2f, 160f, conString: "@SELECT@SELECT");
      DuckNetwork._confirmKick = new UIMenu("REALLY KICK?", num1 / 2f, num2 / 2f, 160f, conString: "@SELECT@SELECT");
      DuckNetwork._optionsMenu = new UIMenu("@WRENCH@OPTIONS@SCREWDRIVER@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@DPAD@ADJUST @QUACK@BACK");
      DuckNetwork._settingsBeforeOpen = TeamSelect2.GetMatchSettingString();
      foreach (Profile profile in DuckNetwork.profiles)
      {
        if (profile.connection != null)
          DuckNetwork._ducknetMenu.Add((UIComponent) new UIConnectionInfo(profile, DuckNetwork._ducknetMenu, DuckNetwork._confirmKick), true);
      }
      if (whoOpen.slotType != SlotType.Local)
        DuckNetwork._ducknetMenu.Add((UIComponent) new UIMenuItem("OPTIONS", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._ducknetMenu, (UIComponent) DuckNetwork._optionsMenu), UIAlign.Left), true);
      if (whoOpen.slotType != SlotType.Local && flag && Network.isServer)
      {
        DuckNetwork._slotEditor = (UIMenu) new UISlotEditor(DuckNetwork._ducknetMenu, Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f);
        DuckNetwork._slotEditor.Close();
        DuckNetwork._ducknetUIGroup.Add((UIComponent) DuckNetwork._slotEditor, false);
        DuckNetwork._ducknetMenu.Add((UIComponent) new UIMenuItem("|DGBLUE|EDIT SLOTS", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._ducknetMenu, (UIComponent) DuckNetwork._slotEditor), UIAlign.Left), true);
        DuckNetwork._matchSettingMenu = new UIMenu("@LWING@MATCH SETTINGS@RWING@", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 190f, conString: "@DPAD@ADJUST  @QUACK@BACK");
        DuckNetwork._matchModifierMenu = new UIMenu("MODIFIERS", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 240f, conString: "@DPAD@ADJUST  @QUACK@BACK");
        DuckNetwork._levelSelectMenu = (UIMenu) new LevelSelectCompanionMenu(num1 / 2f, num2 / 2f, DuckNetwork._matchSettingMenu);
        foreach (UnlockData unlock in Unlocks.GetUnlocks(UnlockType.Modifier))
        {
          if (unlock.onlineEnabled)
          {
            if (unlock.unlocked)
              DuckNetwork._matchModifierMenu.Add((UIComponent) new UIMenuItemToggle(unlock.shortName, field: new FieldBinding((object) unlock, "enabled")), true);
            else
              DuckNetwork._matchModifierMenu.Add((UIComponent) new UIMenuItem("@TINYLOCK@LOCKED", c: Color.Red), true);
          }
        }
        DuckNetwork._matchModifierMenu.Add((UIComponent) new UIText(" ", Color.White), true);
        DuckNetwork._matchModifierMenu.Add((UIComponent) new UIMenuItem("OK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._matchModifierMenu, (UIComponent) DuckNetwork._matchSettingMenu), backButton: true), true);
        DuckNetwork._matchModifierMenu.Close();
        foreach (MatchSetting matchSetting in TeamSelect2.matchSettings)
        {
          if ((!(matchSetting.id == "workshopmaps") || Network.available) && matchSetting.id != "partymode")
            DuckNetwork._matchSettingMenu.AddMatchSetting(matchSetting, false);
        }
        DuckNetwork._matchSettingMenu.AddMatchSetting(TeamSelect2.GetOnlineSetting("teams"), false);
        DuckNetwork._matchSettingMenu.Add((UIComponent) new UIText(" ", Color.White), true);
        DuckNetwork._matchSettingMenu.Add((UIComponent) new UIModifierMenuItem((UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._matchSettingMenu, (UIComponent) DuckNetwork._matchModifierMenu)), true);
        DuckNetwork._matchSettingMenu.Add((UIComponent) new UICustomLevelMenu((UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._matchSettingMenu, (UIComponent) DuckNetwork._levelSelectMenu)), true);
        DuckNetwork._matchSettingMenu.Add((UIComponent) new UIText(" ", Color.White), true);
        DuckNetwork._matchSettingMenu.Add((UIComponent) new UIMenuItem("OK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._matchSettingMenu, (UIComponent) DuckNetwork._ducknetMenu), c: Color.White, backButton: true), true);
        DuckNetwork._matchSettingMenu.Close();
        DuckNetwork._ducknetUIGroup.Add((UIComponent) DuckNetwork._matchSettingMenu, false);
        DuckNetwork._ducknetUIGroup.Add((UIComponent) DuckNetwork._matchModifierMenu, false);
        DuckNetwork._ducknetUIGroup.Add((UIComponent) DuckNetwork._levelSelectMenu, false);
        DuckNetwork._ducknetUIGroup.Close();
        DuckNetwork._ducknetMenu.Add((UIComponent) new UIMenuItem("|DGBLUE|MATCH SETTINGS", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._ducknetMenu, (UIComponent) DuckNetwork._matchSettingMenu), UIAlign.Left), true);
      }
      if (Network.isClient && whoOpen.slotType != SlotType.Local)
      {
        UIMenu settingsInfoWindow = DuckNetwork.CreateMatchSettingsInfoWindow(DuckNetwork._ducknetMenu);
        DuckNetwork._ducknetUIGroup.Add((UIComponent) settingsInfoWindow, false);
        DuckNetwork._ducknetMenu.Add((UIComponent) new UIMenuItem("|DGBLUE|VIEW MATCH SETTINGS", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._ducknetMenu, (UIComponent) settingsInfoWindow), UIAlign.Left), true);
      }
      DuckNetwork._ducknetMenu.Add((UIComponent) new UIText("", Color.White), true);
      if (flag && whoOpen.slotType != SlotType.Local)
      {
        DuckNetwork._inviteMenu = (UIMenu) new UIInviteMenu("INVITE FRIENDS", (UIMenuAction) null, Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 160f);
        ((UIInviteMenu) DuckNetwork._inviteMenu).SetAction((UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._inviteMenu, (UIComponent) DuckNetwork._ducknetMenu));
        DuckNetwork._inviteMenu.Close();
        DuckNetwork._ducknetUIGroup.Add((UIComponent) DuckNetwork._inviteMenu, false);
        DuckNetwork._ducknetMenu.Add((UIComponent) new UIMenuItem("|DGGREEN|INVITE FRIENDS", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._ducknetMenu, (UIComponent) DuckNetwork._inviteMenu), UIAlign.Left), true);
      }
      if (whoOpen.slotType != SlotType.Local || Level.current is TeamSelect2)
      {
        if (whoOpen.slotType == SlotType.Local)
          DuckNetwork._ducknetMenu.Add((UIComponent) new UIMenuItem("|DGRED|BACK OUT", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._ducknetMenu, (UIComponent) DuckNetwork._confirmMenu), UIAlign.Left), true);
        else
          DuckNetwork._ducknetMenu.Add((UIComponent) new UIMenuItem("|DGRED|DISCONNECT", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._ducknetMenu, (UIComponent) DuckNetwork._confirmMenu), UIAlign.Left), true);
      }
      DuckNetwork._ducknetMenu.Add((UIComponent) new UIMenuItem("RESUME", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(DuckNetwork._ducknetUIGroup, DuckNetwork._menuClosed), UIAlign.Left, backButton: true), true);
      if (Level.current is GameLevel && Level.current.isCustomLevel)
      {
        DuckNetwork._ducknetMenu.Add((UIComponent) new UIText(" ", Color.White), true);
        if ((Level.current as GameLevel).data.metaData.workshopID != 0UL)
        {
          if ((WorkshopItem.GetItem((Level.current as GameLevel).data.metaData.workshopID).stateFlags & WorkshopItemState.Subscribed) != WorkshopItemState.None)
            DuckNetwork._ducknetMenu.Add((UIComponent) new UIMenuItem("@STEAMICON@|DGRED|UNSUBSCRIBE", (UIMenuAction) new UIMenuActionCloseMenuCallFunction(DuckNetwork._ducknetUIGroup, new UIMenuActionCloseMenuCallFunction.Function(GameMode.Subscribe)), UIAlign.Left), true);
          else
            DuckNetwork._ducknetMenu.Add((UIComponent) new UIMenuItem("@STEAMICON@|DGGREEN|SUBSCRIBE", (UIMenuAction) new UIMenuActionCloseMenuCallFunction(DuckNetwork._ducknetUIGroup, new UIMenuActionCloseMenuCallFunction.Function(GameMode.Subscribe)), UIAlign.Left), true);
        }
        if (!(Level.current as GameLevel).matchOver && Network.isServer)
          DuckNetwork._ducknetMenu.Add((UIComponent) new UIMenuItem("@SKIPSPIN@|DGRED|SKIP", (UIMenuAction) new UIMenuActionCloseMenuCallFunction(DuckNetwork._ducknetUIGroup, new UIMenuActionCloseMenuCallFunction.Function(GameMode.Skip)), UIAlign.Left), true);
      }
      DuckNetwork._ducknetMenu.Close();
      DuckNetwork._ducknetMenu.SelectLastMenuItem();
      DuckNetwork._ducknetUIGroup.Add((UIComponent) DuckNetwork._ducknetMenu, false);
      DuckNetwork._ducknetUIGroup.Add((UIComponent) Options.optionsMenu, false);
      Options.openOnClose = DuckNetwork._ducknetMenu;
      DuckNetwork._confirmMenu.Add((UIComponent) new UIMenuItem("NO!", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._confirmMenu, (UIComponent) DuckNetwork._ducknetMenu)), true);
      DuckNetwork._confirmMenu.Add((UIComponent) new UIMenuItem("YES!", (UIMenuAction) new UIMenuActionCloseMenuSetBoolean(DuckNetwork._ducknetUIGroup, DuckNetwork._quit)), true);
      DuckNetwork._confirmMenu.Close();
      DuckNetwork._ducknetUIGroup.Add((UIComponent) DuckNetwork._confirmMenu, false);
      DuckNetwork._confirmKick.Add((UIComponent) new UIMenuItem("NO!", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._confirmKick, (UIComponent) DuckNetwork._ducknetMenu)), true);
      DuckNetwork._confirmKick.Add((UIComponent) new UIMenuItem("YES!", (UIMenuAction) new UIMenuActionCloseMenuCallFunction(DuckNetwork._ducknetUIGroup, new UIMenuActionCloseMenuCallFunction.Function(DuckNetwork.KickedPlayer))), true);
      DuckNetwork._confirmKick.Close();
      DuckNetwork._ducknetUIGroup.Add((UIComponent) DuckNetwork._confirmKick, false);
      DuckNetwork._optionsMenu.Add((UIComponent) new UIMenuItemSlider("SFX VOLUME", field: new FieldBinding((object) Options.Data, "sfxVolume"), step: 0.125f), true);
      DuckNetwork._optionsMenu.Add((UIComponent) new UIMenuItemSlider("MUSIC VOLUME", field: new FieldBinding((object) Options.Data, "musicVolume"), step: 0.125f), true);
      DuckNetwork._optionsMenu.Add((UIComponent) new UIMenuItemToggle("SHENANIGANS", field: new FieldBinding((object) Options.Data, "shennanigans")), true);
      DuckNetwork._optionsMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      DuckNetwork._optionsMenu.Add((UIComponent) new UIMenuItemToggle("FULLSCREEN", field: new FieldBinding((object) Options.Data, "fullscreen")), true);
      DuckNetwork._optionsMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      DuckNetwork._optionsMenu.Add((UIComponent) new UIMenuItem("BACK", (UIMenuAction) new UIMenuActionOpenMenu((UIComponent) DuckNetwork._optionsMenu, (UIComponent) DuckNetwork._ducknetMenu), backButton: true), true);
      DuckNetwork._optionsMenu.Close();
      DuckNetwork._ducknetUIGroup.Add((UIComponent) DuckNetwork._optionsMenu, false);
      DuckNetwork._ducknetUIGroup.Close();
      Level.Add((Thing) DuckNetwork._ducknetUIGroup);
      DuckNetwork._ducknetUIGroup.Update();
      DuckNetwork._ducknetUIGroup.Update();
      DuckNetwork._ducknetUIGroup.Update();
      DuckNetwork._ducknetUIGroup.Open();
      DuckNetwork._ducknetMenu.Open();
      MonoMain.pauseMenu = DuckNetwork._ducknetUIGroup;
      HUD.AddCornerControl(HUDCorner.TopLeft, "CHAT@CHAT@");
      DuckNetwork._pauseOpen = true;
      SFX.Play("pause", 0.6f);
    }

    public static void SendCustomLevelData()
    {
      int num = 512;
      MemoryStream compressedActiveLevelData = Editor.GetCompressedActiveLevelData();
      long length1 = compressedActiveLevelData.Length;
      int offset = 0;
      Math.Ceiling((double) length1 / (double) num);
      compressedActiveLevelData.Position = 0L;
      Send.Message((NetMessage) new NMLevelDataHeader(DuckNetwork._core.levelTransferSession, (int) length1));
      while ((long) offset != length1)
      {
        BitBuffer dat = new BitBuffer();
        int length2 = (int) Math.Min(length1 - (long) offset, (long) num);
        dat.Write(compressedActiveLevelData.GetBuffer(), offset, length2);
        offset += length2;
        Send.Message((NetMessage) new NMLevelDataChunk(DuckNetwork._core.levelTransferSession, dat));
      }
      ++DuckNetwork._core.levelTransferSession;
    }

    public static void SendCurrentLevelData(ushort session, NetworkConnection c)
    {
      int num = 512;
      MemoryStream compressedLevelData = DuckNetwork.compressedLevelData;
      long length1 = compressedLevelData.Length;
      int offset = 0;
      Math.Ceiling((double) length1 / (double) num);
      compressedLevelData.Position = 0L;
      Send.Message((NetMessage) new NMLevelDataHeader(session, (int) length1), c);
      while ((long) offset != length1)
      {
        BitBuffer dat = new BitBuffer();
        int length2 = (int) Math.Min(length1 - (long) offset, (long) num);
        dat.Write(compressedLevelData.GetBuffer(), offset, length2);
        offset += length2;
        Send.Message((NetMessage) new NMLevelDataChunk(session, dat), c);
      }
    }

    private static void OpenTeamSwitchDialogue(Profile p)
    {
      if (DuckNetwork._uhOhGroup != null && DuckNetwork._uhOhGroup.open)
        return;
      if (DuckNetwork._uhOhGroup != null)
        Level.Remove((Thing) DuckNetwork._uhOhGroup);
      DuckNetwork.ClearTeam(p);
      DuckNetwork._uhOhGroup = new UIComponent(Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 0.0f, 0.0f);
      DuckNetwork._uhOhMenu = new UIMenu("UH OH", Layer.HUD.camera.width / 2f, Layer.HUD.camera.height / 2f, 230f, conString: "@SELECT@OK");
      float num = 190f;
      string str1 = "The host isn't allowing teams, and someone else is already wearing your hat :(";
      string textVal = "";
      string str2 = "";
      while (true)
      {
        if (str1.Length > 0 && str1[0] != ' ')
        {
          str2 += (string) (object) str1[0];
        }
        else
        {
          if ((double) ((textVal.Length + str2.Length) * 8) > (double) num)
          {
            DuckNetwork._uhOhMenu.Add((UIComponent) new UIText(textVal, Color.White, UIAlign.Left), true);
            textVal = "";
          }
          if (textVal.Length > 0)
            textVal += " ";
          textVal += str2;
          str2 = "";
        }
        if (str1.Length != 0)
          str1 = str1.Remove(0, 1);
        else
          break;
      }
      if (str2.Length > 0)
      {
        if (textVal.Length > 0)
          textVal += " ";
        textVal += str2;
      }
      if (textVal.Length > 0)
        DuckNetwork._uhOhMenu.Add((UIComponent) new UIText(textVal, Color.White, UIAlign.Left), true);
      DuckNetwork._uhOhMenu.Add((UIComponent) new UIText(" ", Color.White), true);
      DuckNetwork._uhOhMenu.Add((UIComponent) new UIMenuItem("OH DEAR", (UIMenuAction) new UIMenuActionCloseMenu(DuckNetwork._uhOhGroup), c: Colors.MenuOption, backButton: true), true);
      DuckNetwork._uhOhMenu.Close();
      DuckNetwork._uhOhGroup.Add((UIComponent) DuckNetwork._uhOhMenu, false);
      DuckNetwork._uhOhGroup.Close();
      Level.Add((Thing) DuckNetwork._uhOhGroup);
      DuckNetwork._uhOhGroup.Open();
      DuckNetwork._uhOhMenu.Open();
      MonoMain.pauseMenu = DuckNetwork._uhOhGroup;
      SFX.Play("pause", 0.6f);
    }

    public static void ClearTeam(Profile p)
    {
      if (!(Level.current is TeamSelect2))
        return;
      (Level.current as TeamSelect2).ClearTeam((int) p.networkIndex);
    }

    public static bool OnTeamSwitch(Profile p)
    {
      if (TeamSelect2.GetSettingBool("teams"))
        return true;
      Team team = p.team;
      bool flag = true;
      if (team != null)
      {
        foreach (Profile profile in DuckNetwork.profiles)
        {
          if (p.connection != null && p != profile && (profile.team == p.team && Network.isServer))
          {
            if (p.connection == DuckNetwork.localConnection)
              DuckNetwork.OpenTeamSwitchDialogue(p);
            else
              Send.Message((NetMessage) new NMTeamSetDenied(p.networkIndex, (byte) Teams.all.IndexOf(p.team)), p.connection);
            flag = false;
            return flag;
          }
        }
      }
      return flag;
    }

    private static void SendJoinMessage(NetworkConnection c)
    {
      bool wasInvited = false;
      if (Steam.lobby != null && NCSteam.inviteLobbyID != 0UL && (long) NCSteam.inviteLobbyID == (long) Steam.lobby.id)
        wasInvited = true;
      NCSteam.inviteLobbyID = 0UL;
      Send.Message((NetMessage) new NMRequestJoin(DG.version, DuckNetwork.randomID, Profile.CalculateLocalFlippers(), Network.activeNetwork.core.GetLocalName(), varHasCustomHats: (Teams.core.extraTeams.Count > 0), wasInvited: wasInvited), NetMessagePriority.ReliableOrdered, c);
    }

    public static void Join(string id, string ip = "localhost")
    {
      if (DuckNetwork._core.status != DuckNetStatus.Disconnected)
        return;
      DevConsole.Log(DCSection.DuckNet, "|LIME|Attempting to join " + id);
      DuckNetwork.Reset();
      foreach (Profile universalProfile in Profiles.universalProfileList)
        universalProfile.team = (Team) null;
      for (int index = 0; index < 4; ++index)
        Teams.all[index].customData = (byte[]) null;
      foreach (Profile profile in DuckNetwork.profiles)
        profile.slotType = SlotType.Open;
      DuckNetwork._core.error = (DuckNetErrorInfo) null;
      DuckNetwork._core.localDuckIndex = -1;
      TeamSelect2.DefaultSettings();
      Network.JoinServer(id, ip: ip);
      DuckNetwork.localConnection.AttemptConnection();
      DuckNetwork._core.attemptTimeout = 15f;
      DuckNetwork._core.status = DuckNetStatus.EstablishingCommunication;
    }

    public static void Host(int maxPlayers, NetworkLobbyType lobbyType)
    {
      if (DuckNetwork._core.status != DuckNetStatus.Disconnected)
        return;
      DevConsole.Log(DCSection.DuckNet, "|LIME|Hosting new server. ");
      DuckNetwork.Reset();
      foreach (Profile universalProfile in Profiles.universalProfileList)
        universalProfile.team = (Team) null;
      DuckNetwork._core.error = (DuckNetErrorInfo) null;
      TeamSelect2.DefaultSettings();
      Network.HostServer(lobbyType, maxPlayers);
      DuckNetwork.localConnection.AttemptConnection();
      foreach (Profile profile in DuckNetwork.profiles)
      {
        switch (lobbyType)
        {
          case NetworkLobbyType.Private:
            profile.slotType = SlotType.Invite;
            break;
          case NetworkLobbyType.FriendsOnly:
            profile.slotType = SlotType.Friend;
            break;
          default:
            profile.slotType = SlotType.Open;
            break;
        }
        if ((int) profile.networkIndex >= maxPlayers)
          profile.slotType = SlotType.Closed;
      }
      int num = 1;
      DuckNetwork._core.localDuckIndex = -1;
      foreach (MatchmakingPlayer matchmakingProfile in UIMatchmakingBox.matchmakingProfiles)
      {
        string name = Network.activeNetwork.core.GetLocalName();
        if (num > 1)
          name = name + "(" + num.ToString() + ")";
        if (DuckNetwork._core.localDuckIndex == -1)
        {
          DuckNetwork._core.localDuckIndex = (int) matchmakingProfile.duckIndex;
          DuckNetwork._core.hostDuckIndex = (int) matchmakingProfile.duckIndex;
        }
        Profile profile = DuckNetwork.CreateProfile(DuckNetwork._core.localConnection, name, (int) matchmakingProfile.duckIndex, matchmakingProfile.inputProfile);
        if (num > 1)
          profile.slotType = SlotType.Local;
        DuckNetwork._core.localConnection.loadingStatus = (byte) 0;
        profile.networkStatus = DuckNetStatus.Connected;
        if (matchmakingProfile.team != null)
        {
          if (matchmakingProfile.team.customData != null)
          {
            profile.team = Teams.all[(int) matchmakingProfile.duckIndex];
            Team.MapFacade(profile.steamID, matchmakingProfile.team);
          }
          else
            profile.team = matchmakingProfile.team;
        }
        ++num;
      }
      DuckNetwork._core.localConnection.isHost = true;
      DuckNetwork._core.status = DuckNetStatus.Connecting;
    }

    public static Profile JoinLocalDuck(InputProfile input)
    {
      int num = 1;
      foreach (Profile profile in DuckNetwork.profiles)
      {
        if (profile.connection == DuckNetwork.localConnection)
          ++num;
      }
      string name = Network.activeNetwork.core.GetLocalName();
      if (num > 1)
        name = name + "(" + num.ToString() + ")";
      Profile profile1 = DuckNetwork.CreateProfile(DuckNetwork.localConnection, name, input: input, local: true);
      if (profile1 == null)
        return (Profile) null;
      profile1.networkStatus = !Network.isClient ? DuckNetStatus.Connected : DuckNetStatus.Connecting;
      Level.current.OnNetworkConnecting(profile1);
      DuckNetwork.SendNewProfile(profile1, DuckNetwork.localConnection);
      return profile1;
    }

    private static Profile CreateProfile(
      NetworkConnection c,
      string name = "",
      int index = -1,
      InputProfile input = null,
      bool hasCustomHats = false,
      bool invited = false,
      bool local = false)
    {
      bool friend = false;
      if (c.data is User && (c.data as User).id != 0UL)
      {
        if (DuckNetwork._invitedFriends.Contains((c.data as User).id))
          invited = true;
        if ((c.data as User).relationship == FriendRelationship.Friend)
          friend = true;
      }
      Profile profile = index != -1 ? DuckNetwork.profiles[index] : DuckNetwork.profiles.FirstOrDefault<Profile>((Func<Profile, bool>) (x => x.connection == null && x.reservedUser != null && c.data == x.reservedUser)) ?? DuckNetwork.profiles.FirstOrDefault<Profile>((Func<Profile, bool>) (x => x.connection == null && x.slotType == SlotType.Invite && invited)) ?? DuckNetwork.profiles.FirstOrDefault<Profile>((Func<Profile, bool>) (x => x.connection == null && x.slotType == SlotType.Friend && friend)) ?? (!local ? DuckNetwork.profiles.FirstOrDefault<Profile>((Func<Profile, bool>) (x => x.connection == null && x.slotType == SlotType.Open)) : DuckNetwork.profiles.FirstOrDefault<Profile>((Func<Profile, bool>) (x => x.connection == null && x.slotType == SlotType.Local)));
      index = DuckNetwork.profiles.IndexOf(profile);
      if (profile == null)
        return (Profile) null;
      if (c != null && profile.connection != null && profile.connection != c)
        profile.connection.Disconnect();
      DuckNetwork.profiles[index].linkedProfile = (Profile) null;
      if (Steam.user != null && c == DuckNetwork.localConnection)
      {
        foreach (Profile allCustomProfile in Profiles.allCustomProfiles)
        {
          if ((long) allCustomProfile.steamID == (long) Steam.user.id)
          {
            DuckNetwork.profiles[index].linkedProfile = allCustomProfile;
            break;
          }
        }
      }
      profile.hasCustomHats = hasCustomHats;
      profile.name = name;
      c.profile = profile;
      profile.connection = c;
      profile.team = profile.reservedUser == null || profile.reservedTeam == null ? Teams.all[(int) profile.networkIndex] : profile.reservedTeam;
      profile.reservedUser = (object) null;
      profile.reservedTeam = (Team) null;
      if (profile.slotType == SlotType.Reserved)
        profile.slotType = SlotType.Invite;
      profile.persona = Persona.all.ElementAt<DuckPersona>((int) profile.networkIndex);
      if (c == DuckNetwork._core.localConnection)
      {
        if (NetworkDebugger.enabled)
          profile.inputProfile = NetworkDebugger.inputProfiles[(int) profile.networkIndex];
        else if (input != null)
          profile.inputProfile = input;
      }
      else
        profile.inputProfile = InputProfile.GetVirtualInput((int) profile.networkIndex);
      return profile;
    }

    public static void Reset()
    {
      foreach (Profile profile in DuckNetwork.profiles)
      {
        profile.connection = (NetworkConnection) null;
        profile.team = (Team) null;
        profile.networkStatus = DuckNetStatus.Disconnected;
        profile.hasCustomHats = false;
        profile.reservedUser = (object) null;
        profile.reservedTeam = (Team) null;
        profile.linkedProfile = (Profile) null;
        profile.acceptedMigration = false;
        profile.flippers = (byte) 0;
        if (profile.inputProfile != null)
          profile.inputProfile.lastActiveOverride = (InputDevice) null;
      }
      Level.core.gameInProgress = false;
      DuckNetwork._invitedFriends.Clear();
      Team.ClearFacades();
      Main.ResetGameStuff();
      Main.ResetMatchStuff();
      DuckNetwork._core.RecreateProfiles();
      DuckNetwork._core.hostDuckIndex = -1;
      DuckNetwork._core.localDuckIndex = -1;
      DuckNetwork._core.localConnection = new NetworkConnection((object) null);
      DuckNetwork._core.inGame = false;
      DuckNetwork._core.status = DuckNetStatus.Disconnected;
      DuckNetwork._core.migrationIndex = byte.MaxValue;
      DuckNetwork._core.levelIndex = (byte) 0;
      DuckNetwork._core.levelTransferSession = (ushort) 0;
      DuckNetwork._core.levelTransferProgress = 0;
      DuckNetwork._core.levelTransferSize = 0;
    }

    public static DuckNetStatus GeneralStatus()
    {
      DuckNetStatus duckNetStatus = DuckNetStatus.Connected;
      if (DuckNetwork.GetProfiles(DuckNetwork.localConnection).Count == 0)
        return DuckNetStatus.Disconnected;
      foreach (Profile profile in DuckNetwork.GetProfiles(DuckNetwork.localConnection))
      {
        if (profile.networkStatus != DuckNetStatus.Connected)
        {
          duckNetStatus = profile.networkStatus;
          break;
        }
      }
      switch (Level.current)
      {
        case TeamSelect2 _:
        case JoinServer _:
        case DisconnectError _:
        case ConnectionError _:
label_11:
          return duckNetStatus;
        default:
          if (duckNetStatus != DuckNetStatus.Disconnected && duckNetStatus != DuckNetStatus.Disconnecting)
          {
            duckNetStatus = DuckNetStatus.Connected;
            goto label_11;
          }
          else
            goto label_11;
      }
    }

    public static void ResetProfile(Profile p)
    {
      p.networkStatus = DuckNetStatus.Disconnected;
      p.connection = (NetworkConnection) null;
      p.team = (Team) null;
      p.linkedProfile = (Profile) null;
      p.furniturePositionData.Clear();
    }

    public static void OnDisconnect(NetworkConnection connection, string reason, bool kicked = false)
    {
      if (DuckNetwork._core.localDuckIndex == -1)
        return;
      List<Profile> profiles = DuckNetwork.GetProfiles(connection);
      bool flag = false;
      foreach (Profile profile1 in profiles)
      {
        flag = true;
        if (Network.isServer && profile1.connection != DuckNetwork.localConnection)
        {
          profile1.networkStatus = DuckNetStatus.Disconnected;
          DuckNetwork.SendToEveryone((NetMessage) new NMClientDisconnect(profile1.connection.identifier, profile1.networkIndex));
          GhostManager.context.FreeOldGhosts();
          if (!(Level.current is TeamSelect2) && !kicked)
          {
            profile1.slotType = SlotType.Reserved;
            profile1.reservedUser = profile1.connection.data;
            profile1.reservedTeam = profile1.team;
          }
          Team.ClearFacade(profile1.steamID);
          profile1.connection = (NetworkConnection) null;
          profile1.team = (Team) null;
          profile1.flippers = (byte) 0;
          profile1.linkedProfile = (Profile) null;
          if (profile1.inputProfile != null)
            profile1.inputProfile.lastActiveOverride = (InputDevice) null;
          DevConsole.Log(DCSection.DuckNet, "|RED|" + profile1.name + " Has left the DuckNet.");
        }
        else if (profile1.connection != DuckNetwork.localConnection)
        {
          GhostManager.context.FreeOldGhosts();
          if (profile1.reservedUser == null)
          {
            if (profile1.networkStatus == DuckNetStatus.Disconnecting || reason == "CONTROLLED DISCONNECT" && profile1.connection.isHost)
            {
              DevConsole.Log(DCSection.DuckNet, "|RED|" + profile1.name + " disconnected.");
              profile1.networkStatus = DuckNetStatus.Disconnected;
            }
            else
            {
              DevConsole.Log(DCSection.DuckNet, "|RED|Trouble communicating with " + profile1.name + "...");
              profile1.networkStatus = DuckNetStatus.RequiresNewConnection;
              foreach (Profile profile2 in DuckNetwork.core.profiles)
              {
                if (profile2.connection != null && profile2.connection.isHost)
                  Send.Message((NetMessage) new NMRequiresNewConnection(profile1.connection.identifier), profile2.connection);
              }
            }
          }
        }
        else
          profile1.networkStatus = DuckNetStatus.Disconnected;
      }
      if (!flag || DuckNetwork.status == DuckNetStatus.Disconnecting || DuckNetwork.status == DuckNetStatus.Disconnected)
        return;
      if (connection.isHost)
        DuckNetwork.RaiseError(new DuckNetErrorInfo()
        {
          error = DuckNetError.HostDisconnected,
          message = "|RED|Host disconnected!"
        });
      else if (!(Level.current is TeamSelect2) && !(Level.current is TitleScreen) && Network.isServer)
      {
        Send.Message((NetMessage) new NMInGameDisconnect(connection.identifier));
        Level.current = (Level) new TeamSelect2();
      }
      if (connection != DuckNetwork.localConnection)
        return;
      DuckNetwork.RaiseError(new DuckNetErrorInfo()
      {
        error = DuckNetError.ConnectionTimeout,
        message = "|RED|Connection failure!"
      });
    }

    public static void OnSessionEnded() => DuckNetwork.Reset();

    public static void OnConnection(NetworkConnection c)
    {
      if (DuckNetwork.status == DuckNetStatus.EstablishingCommunication)
      {
        if (!c.isHost)
          return;
        DevConsole.Log(DCSection.DuckNet, "|LIME|Host contacted. Sending join request. ");
        DuckNetwork._core.status = DuckNetStatus.Connecting;
        DuckNetwork.SendJoinMessage(c);
      }
      else
        DevConsole.Log(DCSection.DuckNet, "|RED|Host contacted. Join request not sent (" + DuckNetwork.status.ToString() + ")");
    }

    public static void Update()
    {
      if (MonoMain.pauseMenu == null && DuckNetwork._pauseOpen)
      {
        HUD.CloseAllCorners();
        DuckNetwork._pauseOpen = false;
        if (Network.isServer)
          TeamSelect2.UpdateModifierStatus();
      }
      if (MonoMain.pauseMenu == null && DuckNetwork._willOpenSettingsInfo)
      {
        DuckNetwork.DoMatchSettingsInfoOpen();
        DuckNetwork._willOpenSettingsInfo = false;
      }
      if (DuckNetwork._core.status == DuckNetStatus.Disconnected || DuckNetwork._core.status == DuckNetStatus.Disconnecting)
      {
        DuckNetwork._quit.value = false;
      }
      else
      {
        if (DuckNetwork._quit.value)
        {
          if (DuckNetwork._menuOpenProfile != null && DuckNetwork._menuOpenProfile.slotType == SlotType.Local)
          {
            DuckNetwork.Kick(DuckNetwork._menuOpenProfile);
          }
          else
          {
            if (Steam.lobby != null)
              UIMatchmakingBox.nonPreferredServers.Add(Steam.lobby.id);
            Level.current = (Level) new DisconnectFromGame();
          }
          DuckNetwork._quit.value = false;
        }
        if (DuckNetwork._menuClosed.value && Network.isServer)
        {
          if (TeamSelect2.GetMatchSettingString() != DuckNetwork._settingsBeforeOpen)
          {
            TeamSelect2.SendMatchSettings();
            Send.Message((NetMessage) new NMMatchSettingsChanged());
          }
          (Network.activeNetwork.core as NCSteam).ApplyLobbyData();
          DuckNetwork._menuClosed.value = false;
        }
        if (Keyboard.Pressed(Keys.F1) && !Keyboard.Down(Keys.LeftShift) && !Keyboard.Down(Keys.RightShift))
          ConnectionStatusUI.Show();
        if (Keyboard.Released(Keys.F1))
          ConnectionStatusUI.Hide();
        if (Network.isClient)
        {
          if (DuckNetwork.status == DuckNetStatus.Connected)
            DuckNetwork._core.attemptTimeout = 10f;
          if ((double) DuckNetwork._core.attemptTimeout > 0.0)
            DuckNetwork._core.attemptTimeout -= Maths.IncFrameTimer();
          else if (DuckNetwork.status != DuckNetStatus.Connected)
            DuckNetwork.RaiseError(new DuckNetErrorInfo()
            {
              error = DuckNetError.ConnectionTimeout,
              message = "|RED|Connection timeout."
            });
        }
        if (MonoMain.pauseMenu != null)
        {
          DuckNetwork._core.enteringText = false;
          DuckNetwork._core.stopEnteringText = false;
        }
        bool flag1 = false;
        if (Network.isActive && MonoMain.pauseMenu == null)
        {
          List<ChatMessage> chatMessageList = new List<ChatMessage>();
          foreach (ChatMessage chatMessage in DuckNetwork._core.chatMessages)
          {
            chatMessage.timeout -= 0.016f;
            if ((double) chatMessage.timeout < 0.0)
              chatMessage.alpha -= 0.01f;
            if ((double) chatMessage.alpha < 0.0)
              chatMessageList.Add(chatMessage);
          }
          foreach (ChatMessage chatMessage in chatMessageList)
            DuckNetwork._core.chatMessages.Remove(chatMessage);
          if (DuckNetwork._core.stopEnteringText)
          {
            DuckNetwork._core.enteringText = false;
            DuckNetwork._core.stopEnteringText = false;
          }
          if (!DevConsole.core.open)
          {
            bool enteringText = DuckNetwork._core.enteringText;
            DuckNetwork._core.enteringText = false;
            bool flag2 = Input.Pressed("CHAT");
            DuckNetwork._core.enteringText = enteringText;
            if (flag2)
            {
              if (!DuckNetwork._core.enteringText)
              {
                DuckNetwork._core.enteringText = true;
                DuckNetwork._core.currentEnterText = "";
                Keyboard.keyString = "";
              }
              else
              {
                if (DuckNetwork._core.currentEnterText != "")
                {
                  NMChatMessage message = new NMChatMessage((byte) DuckNetwork._core.localDuckIndex, DuckNetwork._core.currentEnterText, DuckNetwork._core.chatIndex);
                  ++DuckNetwork._core.chatIndex;
                  DuckNetwork.SendToEveryone((NetMessage) message);
                  DuckNetwork.ChatMessageReceived(message);
                  DuckNetwork._core.currentEnterText = "";
                }
                DuckNetwork._core.stopEnteringText = true;
              }
            }
            else if (DuckNetwork._core.enteringText && Keyboard.Pressed(Keys.Escape))
            {
              DuckNetwork._core.stopEnteringText = true;
              flag1 = true;
            }
          }
          if (DuckNetwork._core.enteringText)
          {
            if (Keyboard.keyString.Length > 90)
              Keyboard.keyString = Keyboard.keyString.Substring(0, 90);
            DuckNetwork._core.currentEnterText = Keyboard.keyString;
          }
        }
        bool flag3 = false;
        int num = 0;
        foreach (Profile profile in DuckNetwork.profiles)
        {
          if (profile.connection != null)
          {
            if (MonoMain.pauseMenu == null && (DuckNetwork._ducknetUIGroup == null || !DuckNetwork._ducknetUIGroup.open) && (profile.connection == DuckNetwork.localConnection && profile.inputProfile.Pressed("START") && (!flag1 && !flag3)))
            {
              switch (Level.current)
              {
                case TeamSelect2 _:
                case GameLevel _:
                case RockScoreboard _:
                  DuckNetwork.OpenMenu(profile);
                  flag3 = true;
                  break;
              }
            }
            ++num;
            if (profile.connection.status == ConnectionStatus.Connected && profile.networkStatus != DuckNetStatus.Disconnecting && (profile.networkStatus != DuckNetStatus.Disconnected && profile.networkStatus != DuckNetStatus.Failure))
            {
              profile.currentStatusTimeout -= Maths.IncFrameTimer();
              if (profile.networkStatus == DuckNetStatus.NeedsNotificationWhenReadyForData)
              {
                if ((double) profile.currentStatusTimeout <= 0.0)
                {
                  profile.currentStatusTimeout = 2f;
                  ++profile.currentStatusTries;
                }
                if (profile.currentStatusTries > 10)
                {
                  if (profile.isHost)
                    DuckNetwork.RaiseError(new DuckNetErrorInfo()
                    {
                      error = DuckNetError.ConnectionTimeout,
                      message = "|RED|Took too long to receive level data."
                    });
                  profile.connection.Disconnect();
                }
                if (Network.isServer || (int) DuckNetwork.localConnection.loadingStatus == (int) DuckNetwork.levelIndex)
                {
                  if (profile.connection != DuckNetwork.localConnection)
                    Send.Message((NetMessage) new NMLevelDataReady(DuckNetwork.levelIndex), profile.connection);
                  profile.networkStatus = DuckNetStatus.WaitingForLoadingToBeFinished;
                }
              }
              if (profile.networkStatus == DuckNetStatus.WaitingForLoadingToBeFinished)
              {
                if ((int) profile.connection.loadingStatus == (int) DuckNetwork.levelIndex)
                {
                  profile.networkStatus = DuckNetStatus.Connected;
                }
                else
                {
                  if ((double) profile.currentStatusTimeout <= 0.0)
                  {
                    if (profile.connection != DuckNetwork.localConnection)
                    {
                      Send.Message((NetMessage) new NMAwaitingLevelReady(DuckNetwork.levelIndex), profile.connection);
                      DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Requesting level data from " + profile.name);
                    }
                    else
                      DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Still waiting for level data...");
                    profile.currentStatusTimeout = 7f;
                    ++profile.currentStatusTries;
                  }
                  if (profile.currentStatusTries > 4)
                  {
                    if (profile.isHost)
                      DuckNetwork.RaiseError(new DuckNetErrorInfo()
                      {
                        error = DuckNetError.ConnectionTimeout,
                        message = "|RED|Took too long to connect with " + profile.name + "."
                      });
                    profile.connection.Disconnect();
                  }
                }
              }
              if (profile.connection.wantsGhostData == (int) DuckNetwork.levelIndex && Level.current.initialized)
              {
                GhostManager.context.RefreshGhosts();
                Level.current.SendLevelData(profile.connection);
                GhostManager.context.SendAllGhostData(false, NetMessagePriority.ReliableOrdered, profile.connection);
                profile.connection.sendPacketsNow = true;
                Send.Message((NetMessage) new NMEndOfGhostData(DuckNetwork.levelIndex), profile.connection);
                Send.Message((NetMessage) new NMLevelDataReady(DuckNetwork.levelIndex), profile.connection);
                DevConsole.Log(DCSection.DuckNet, "|LIME|" + profile.connection.identifier + " LOADED LEVEL (" + (object) DuckNetwork.levelIndex + ")");
                profile.connection.wantsGhostData = -1;
              }
            }
          }
        }
        if (DuckNetwork.error == null || DuckNetwork._core.status == DuckNetStatus.Disconnecting)
          return;
        DuckNetwork._core.status = DuckNetStatus.Disconnecting;
        Network.Disconnect();
      }
    }

    public static void ChatMessageReceived(NMChatMessage message)
    {
      if ((int) message.profileIndex >= DuckNetwork._core.profiles.Count)
        return;
      DuckNetwork._core.chatMessages.Add(new ChatMessage(DuckNetwork._core.profiles[(int) message.profileIndex], message.text, message.index));
      DuckNetwork._core.chatMessages = DuckNetwork._core.chatMessages.OrderBy<ChatMessage, int>((Func<ChatMessage, int>) (x => (int) -x.index)).ToList<ChatMessage>();
      SFX.Play("chatmessage", 0.8f, Rando.Float(-0.15f, 0.15f));
    }

    public static List<Profile> GetProfiles(NetworkConnection connection)
    {
      List<Profile> profileList = new List<Profile>();
      foreach (Profile profile in DuckNetwork.profiles)
      {
        if (profile.connection == connection)
          profileList.Add(profile);
      }
      return profileList;
    }

    public static int IndexOf(Profile p) => DuckNetwork.profiles.IndexOf(p);

    public static void Disconnect(Profile who)
    {
      GhostManager.context.FreeOldGhosts();
      who.networkStatus = DuckNetStatus.Disconnected;
      Team.ClearFacade(who.steamID);
      who.connection = (NetworkConnection) null;
    }

    private static Profile GetOpenProfile() => DuckNetwork._core.profiles.FirstOrDefault<Profile>((Func<Profile, bool>) (x => x.connection == null));

    public static void SendNewProfile(
      Profile joinProfile,
      NetworkConnection notifyCreation,
      bool notifyConnectionIsExisting = false)
    {
      if (notifyCreation != null && notifyCreation != DuckNetwork.localConnection)
      {
        Send.Message((NetMessage) new NMJoinDuckNetwork(joinProfile.networkIndex), notifyCreation);
        if (joinProfile.team != null)
          Send.Message((NetMessage) new NMSetTeam(joinProfile.networkIndex, (byte) Teams.IndexOf(joinProfile.team)));
        if (joinProfile.furniturePositions.Count > 0)
          Send.Message((NetMessage) new NMRoomData(joinProfile.furniturePositionData, joinProfile.networkIndex));
      }
      byte varTeam1 = joinProfile.networkIndex;
      if (joinProfile.team != null)
      {
        if (Teams.IndexOf(joinProfile.team) >= Teams.allStock.Count)
          joinProfile.team = Teams.all[(int) joinProfile.networkIndex];
        varTeam1 = (byte) Teams.IndexOf(joinProfile.team);
      }
      int num = 0;
      foreach (Profile profile in DuckNetwork.profiles)
      {
        if (profile.networkStatus != DuckNetStatus.Disconnecting)
        {
          if (profile != joinProfile && profile.connection != null && profile.connection != notifyCreation)
          {
            byte varTeam2 = profile.networkIndex;
            if (profile.team != null)
            {
              if (Teams.IndexOf(profile.team) >= Teams.allStock.Count)
                profile.team = Teams.all[(int) profile.networkIndex];
              varTeam2 = (byte) Teams.IndexOf(profile.team);
            }
            if (!notifyConnectionIsExisting && notifyCreation != null && notifyCreation != DuckNetwork.localConnection)
            {
              Send.Message((NetMessage) new NMRemoteJoinDuckNetwork((byte) num, profile.connection == DuckNetwork.localConnection ? "SERVER" : profile.connection.identifier, profile.name, profile.hasCustomHats, varTeam2, profile.flippers), notifyCreation);
              if (profile.team != null)
              {
                if (profile.team.customData != null)
                  Send.Message((NetMessage) new NMSpecialHat(profile.team, profile.steamID));
                Send.Message((NetMessage) new NMSetTeam((byte) num, (byte) Teams.IndexOf(profile.team)));
              }
              if (profile.furniturePositions.Count > 0)
                Send.Message((NetMessage) new NMRoomData(profile.furniturePositionData, (byte) num));
            }
            if (profile.connection != DuckNetwork.localConnection)
              Send.Message((NetMessage) new NMRemoteJoinDuckNetwork(joinProfile.networkIndex, notifyCreation == DuckNetwork.localConnection ? "SERVER" : notifyCreation.identifier, joinProfile.name, profile.hasCustomHats, varTeam1, profile.flippers), profile.connection);
          }
          ++num;
        }
      }
      if (notifyConnectionIsExisting || notifyCreation == null || notifyCreation == DuckNetwork.localConnection)
        return;
      Send.Message((NetMessage) new NMEndOfDuckNetworkData(), notifyCreation);
    }

    public static NMVersionMismatch.Type CheckVersion(string id)
    {
      string[] strArray = id.Split('.');
      NMVersionMismatch.Type type = NMVersionMismatch.Type.Match;
      if (strArray.Length == 4)
      {
        try
        {
          int int32_1 = Convert.ToInt32(strArray[3]);
          int int32_2 = Convert.ToInt32(strArray[2]);
          if (int32_2 < DG.versionHigh)
            type = NMVersionMismatch.Type.Older;
          else if (int32_2 > DG.versionHigh)
            type = NMVersionMismatch.Type.Newer;
          else if (int32_1 < DG.versionLow)
            type = NMVersionMismatch.Type.Older;
          else if (int32_1 > DG.versionLow)
            type = NMVersionMismatch.Type.Newer;
        }
        catch
        {
          type = NMVersionMismatch.Type.Error;
        }
      }
      return type;
    }

    public static NetMessage OnMessageFromNewClient(NetMessage m)
    {
      if (Network.isServer)
      {
        switch (m)
        {
          case NMRequestJoin _:
            if (DuckNetwork.inGame)
              return (NetMessage) new NMGameInProgress();
            NMRequestJoin nmRequestJoin = m as NMRequestJoin;
            DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Join attempt from " + nmRequestJoin.name);
            NMVersionMismatch.Type code = DuckNetwork.CheckVersion(nmRequestJoin.id);
            if (code != NMVersionMismatch.Type.Match)
            {
              DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|" + nmRequestJoin.name + " had a version mismatch.");
              return (NetMessage) new NMVersionMismatch(code, DG.version);
            }
            Profile profile = DuckNetwork.CreateProfile(m.connection, nmRequestJoin.name, hasCustomHats: nmRequestJoin.hasCustomHats, invited: nmRequestJoin.wasInvited);
            if (profile == null)
            {
              DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|" + nmRequestJoin.name + " could not join, server is full.");
              return (NetMessage) new NMServerFull();
            }
            profile.flippers = nmRequestJoin.flippers;
            profile.networkStatus = DuckNetStatus.WaitingForLoadingToBeFinished;
            DuckNetwork._core.status = DuckNetStatus.Connected;
            Level.current.OnNetworkConnecting(profile);
            DuckNetwork.SendNewProfile(profile, m.connection);
            Send.Message((NetMessage) new NMChangeSlots((byte) DuckNetwork.profiles[0].slotType, (byte) DuckNetwork.profiles[1].slotType, (byte) DuckNetwork.profiles[2].slotType, (byte) DuckNetwork.profiles[3].slotType), m.connection);
            TeamSelect2.SendMatchSettings(m.connection, true);
            return (NetMessage) null;
          case NMMessageIgnored _:
            return (NetMessage) null;
        }
      }
      else
      {
        switch (m)
        {
          case NMRequestJoin _:
            DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Another computer has requested a matchmaking connection.");
            return (NetMessage) new NMGameInProgress();
          case NMMessageIgnored _:
            return (NetMessage) null;
        }
      }
      return (NetMessage) new NMMessageIgnored();
    }

    public static bool HandleCoreConnectionMessages(NetMessage m)
    {
      switch (m)
      {
        case NMClientNeedsLevelData _:
          NMClientNeedsLevelData clientNeedsLevelData = m as NMClientNeedsLevelData;
          if ((int) clientNeedsLevelData.levelIndex == (int) DuckNetwork.levelIndex)
          {
            m.connection.dataTransferProgress = 0;
            m.connection.dataTransferSize = 0;
            DuckNetwork.SendCurrentLevelData(clientNeedsLevelData.transferSession, m.connection);
          }
          return true;
        case NMLevelDataHeader _:
          if (Network.isClient)
          {
            NMLevelDataHeader nmLevelDataHeader = m as NMLevelDataHeader;
            if ((int) DuckNetwork._core.levelTransferSession == (int) nmLevelDataHeader.transferSession)
            {
              DuckNetwork._core.levelTransferProgress = 0;
              DuckNetwork._core.levelTransferSize = nmLevelDataHeader.length;
            }
          }
          return true;
        case NMLevelDataChunk _:
          if (Network.isClient)
          {
            NMLevelDataChunk nmLevelDataChunk = m as NMLevelDataChunk;
            if ((int) DuckNetwork._core.levelTransferSession == (int) nmLevelDataChunk.transferSession)
            {
              DuckNetwork._core.levelTransferProgress += nmLevelDataChunk.GetBuffer().lengthInBytes;
              if (DuckNetwork._core.compressedLevelData == null)
                DuckNetwork._core.compressedLevelData = new MemoryStream();
              DuckNetwork._core.compressedLevelData.Write(nmLevelDataChunk.GetBuffer().buffer, 0, nmLevelDataChunk.GetBuffer().lengthInBytes);
              if (DuckNetwork._core.levelTransferProgress == DuckNetwork._core.levelTransferSize)
                (Level.core.currentLevel as XMLLevel).ApplyLevelData(Editor.ReadCompressedLevelData(DuckNetwork._core.compressedLevelData));
            }
          }
          return true;
        case NMChatMessage _:
          NMChatMessage message = m as NMChatMessage;
          message.index = DuckNetwork._core.chatIndex;
          ++DuckNetwork._core.chatIndex;
          DuckNetwork.ChatMessageReceived(message);
          return true;
        case NMLevelDataReady _:
          if ((int) (m as NMLevelDataReady).levelIndex == (int) DuckNetwork.levelIndex)
          {
            DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Level ready message from " + m.connection.name + "(" + (object) (m as NMLevelDataReady).levelIndex + ")");
            foreach (Profile profile in DuckNetwork.GetProfiles(m.connection))
              profile.connection.loadingStatus = (m as NMLevelDataReady).levelIndex;
            if (Network.isServer)
            {
              bool flag = true;
              foreach (Profile profile in DuckNetwork._core.profiles)
              {
                if (profile.connection != DuckNetwork.localConnection && profile.connection != null && (int) profile.connection.loadingStatus != (int) DuckNetwork.levelIndex)
                {
                  flag = false;
                  break;
                }
              }
              if (flag)
                DuckNetwork.SendToEveryone((NetMessage) new NMAllClientsReady());
            }
          }
          else
            DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Level ready message from " + m.connection.name + "(BAD LEVEL)");
          return true;
        case NMAwaitingLevelReady _:
          if ((int) (m as NMAwaitingLevelReady).levelIndex == (int) DuckNetwork.levelIndex)
          {
            if ((int) DuckNetwork.levelIndex == (int) DuckNetwork.localConnection.loadingStatus)
            {
              Send.Message((NetMessage) new NMLevelDataReady(DuckNetwork.levelIndex), m.connection);
            }
            else
            {
              foreach (Profile profile in DuckNetwork.GetProfiles(m.connection))
              {
                if (profile.networkStatus == DuckNetStatus.Connected || profile.networkStatus == DuckNetStatus.WaitingForLoadingToBeFinished)
                  profile.networkStatus = DuckNetStatus.NeedsNotificationWhenReadyForData;
              }
            }
            DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|" + m.connection.name + " requested level loaded confirmation.");
          }
          else
          {
            DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|" + m.connection.name + " tried to request an invalid level (" + (object) (m as NMAwaitingLevelReady).levelIndex + " VS " + (object) DuckNetwork.levelIndex + ")");
            Send.Message((NetMessage) new NMInvalidLevel(), m.connection);
          }
          return true;
        default:
          if (Network.isServer)
          {
            if (m is NMRequiresNewConnection)
            {
              List<Profile> profiles = DuckNetwork.GetProfiles(m.connection);
              if (profiles.Count > 0)
              {
                NMRequiresNewConnection requiresNewConnection = m as NMRequiresNewConnection;
                bool flag = false;
                foreach (Profile profile1 in DuckNetwork.profiles)
                {
                  if (profile1.connection != null && profile1.connection.identifier == requiresNewConnection.toWhom && profile1.connection != DuckNetwork.localConnection)
                  {
                    byte varTeam1 = profile1.networkIndex;
                    if (profile1.team != null)
                    {
                      if (Teams.IndexOf(profile1.team) >= Teams.allStock.Count)
                        profile1.team = Teams.all[(int) profile1.networkIndex];
                      varTeam1 = (byte) Teams.IndexOf(profile1.team);
                    }
                    Send.Message((NetMessage) new NMRemoteJoinDuckNetwork(profile1.networkIndex, profile1.connection.identifier, profile1.name, profile1.hasCustomHats, varTeam1, profile1.flippers), m.connection);
                    if (profile1.team != null)
                    {
                      if (profile1.team.customData != null)
                        Send.Message((NetMessage) new NMSpecialHat(profile1.team, profile1.steamID), m.connection);
                      Send.Message((NetMessage) new NMSetTeam(profile1.networkIndex, (byte) Teams.IndexOf(profile1.team)), m.connection);
                    }
                    if (profile1.furniturePositions.Count > 0)
                      Send.Message((NetMessage) new NMRoomData(profile1.furniturePositionData, profile1.networkIndex), m.connection);
                    foreach (Profile profile2 in profiles)
                    {
                      byte varTeam2 = profile2.networkIndex;
                      if (profile2.team != null)
                      {
                        if (Teams.IndexOf(profile2.team) >= Teams.allStock.Count)
                          profile2.team = Teams.all[(int) profile2.networkIndex];
                        varTeam2 = (byte) Teams.IndexOf(profile2.team);
                      }
                      Send.Message((NetMessage) new NMRemoteJoinDuckNetwork(profile2.networkIndex, profile2.connection.identifier, profile2.name, profile2.hasCustomHats, varTeam2, profile1.flippers), profile1.connection);
                      if (profile2.team != null)
                      {
                        if (profile2.team.customData != null)
                          Send.Message((NetMessage) new NMSpecialHat(profile2.team, profile2.steamID), profile1.connection);
                        Send.Message((NetMessage) new NMSetTeam(profile2.networkIndex, (byte) Teams.IndexOf(profile2.team)), profile1.connection);
                      }
                      if (profile2.furniturePositions.Count > 0)
                        Send.Message((NetMessage) new NMRoomData(profile2.furniturePositionData, profile2.networkIndex), profile1.connection);
                    }
                    DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|" + m.connection.name + " needs a connection to " + profile1.connection.name + "...");
                    flag = true;
                  }
                }
                if (!flag)
                {
                  Send.Message((NetMessage) new NMNoConnectionExists(requiresNewConnection.toWhom), m.connection);
                  DevConsole.Log(DCSection.DuckNet, "|RED|" + m.connection.name + " requested a bad connection (" + requiresNewConnection.toWhom + ")");
                }
              }
              else
              {
                Send.Message((NetMessage) new NMInvalidUser(), m.connection);
                DevConsole.Log(DCSection.DuckNet, "|RED|Invalid user requested a connection (" + m.connection.name + ")");
              }
              return true;
            }
          }
          else
          {
            switch (m)
            {
              case NMKick _:
                DuckNetwork._core.status = DuckNetStatus.Failure;
                if (Steam.lobby != null)
                  UIMatchmakingBox.nonPreferredServers.Add(Steam.lobby.id);
                DuckNetwork.RaiseError(new DuckNetErrorInfo()
                {
                  error = DuckNetError.Kicked,
                  message = "|RED|Oh no! The host kicked you :("
                });
                return true;
              case NMClientDisconnect _:
                NMClientDisconnect clientDisconnect = m as NMClientDisconnect;
                if (DuckNetwork.profiles[(int) clientDisconnect.duckIndex].connection != null && DuckNetwork.profiles[(int) clientDisconnect.duckIndex].connection.identifier == clientDisconnect.whom)
                {
                  if (DuckNetwork.profiles[(int) clientDisconnect.duckIndex].connection == Network.host && (int) clientDisconnect.duckIndex != DuckNetwork.hostDuckIndex)
                  {
                    DuckNetwork.profiles[(int) clientDisconnect.duckIndex].networkStatus = DuckNetStatus.Disconnected;
                    DuckNetwork.profiles[(int) clientDisconnect.duckIndex].connection = (NetworkConnection) null;
                    DuckNetwork.profiles[(int) clientDisconnect.duckIndex].team = (Team) null;
                    DevConsole.Log(DCSection.DuckNet, "|RED|Host local disconnect (" + m.connection.name + ")");
                  }
                  else
                  {
                    if (DuckNetwork.profiles[(int) clientDisconnect.duckIndex].connection.status == ConnectionStatus.Disconnected)
                    {
                      DuckNetwork.profiles[(int) clientDisconnect.duckIndex].networkStatus = DuckNetStatus.Disconnected;
                    }
                    else
                    {
                      DuckNetwork.profiles[(int) clientDisconnect.duckIndex].networkStatus = DuckNetStatus.Disconnecting;
                      DuckNetwork.profiles[(int) clientDisconnect.duckIndex].connection.Disconnect();
                    }
                    Team.ClearFacade(DuckNetwork.profiles[(int) clientDisconnect.duckIndex].steamID);
                    DevConsole.Log(DCSection.DuckNet, "|RED|Client disconnect (" + m.connection.name + ")");
                  }
                }
                return true;
              case NMGameInProgress _:
                DuckNetwork._core.status = DuckNetStatus.Failure;
                DuckNetwork.RaiseError(new DuckNetErrorInfo()
                {
                  error = DuckNetError.GameInProgress,
                  message = "|RED|Game was already in progress."
                });
                return true;
              case NMServerFull _:
                DuckNetwork._core.status = DuckNetStatus.Failure;
                DuckNetwork.RaiseError(new DuckNetErrorInfo()
                {
                  error = DuckNetError.FullServer,
                  message = "|RED|The game was full!"
                });
                return true;
              case NMInvalidLevel _:
                DuckNetwork._core.status = DuckNetStatus.Failure;
                DuckNetwork.RaiseError(new DuckNetErrorInfo()
                {
                  error = DuckNetError.InvalidLevel,
                  message = "|RED|Level request was invalid!"
                });
                return true;
              case NMInvalidUser _:
                DuckNetwork._core.status = DuckNetStatus.Failure;
                DuckNetwork.RaiseError(new DuckNetErrorInfo()
                {
                  error = DuckNetError.InvalidUser,
                  message = "|RED|The host did not reconize you!"
                });
                return true;
              case NMInvalidCustomHat _:
                DuckNetwork._core.status = DuckNetStatus.Failure;
                DuckNetwork.RaiseError(new DuckNetErrorInfo()
                {
                  error = DuckNetError.InvalidCustomHat,
                  message = "|RED|Your custom hat was invalid!"
                });
                return true;
              case NMVersionMismatch _:
                DuckNetwork._core.status = DuckNetStatus.Failure;
                NMVersionMismatch nmVersionMismatch = m as NMVersionMismatch;
                DuckNetwork.FailWithVersionMismatch(nmVersionMismatch.serverVersion, nmVersionMismatch.GetCode());
                return true;
            }
          }
          return false;
      }
    }

    public static void FailWithVersionMismatch(string theirVersion, NMVersionMismatch.Type type)
    {
      DuckNetwork._core.status = DuckNetStatus.Failure;
      string str = "";
      bool flag = false;
      switch (type)
      {
        case NMVersionMismatch.Type.Older:
          str = "|RED|Your version is too new.\n\n|WHITE|HOST: |GREEN|" + theirVersion + "\n|WHITE|YOU:  |RED|" + DG.version;
          flag = true;
          break;
        case NMVersionMismatch.Type.Newer:
          str = "|RED|Your version is too old.\n\n|WHITE|HOST: |GREEN|" + theirVersion + "\n|WHITE|YOU:  |RED|" + DG.version;
          break;
        case NMVersionMismatch.Type.Error:
          str = "|RED|Your game version caused an error.\n\n|WHITE|HOST: |GREEN|" + theirVersion + "\n|WHITE|YOU:  |RED|" + DG.version;
          break;
      }
      DuckNetwork.RaiseError(new DuckNetErrorInfo()
      {
        error = DuckNetError.VersionMismatch,
        message = str,
        tooNew = flag
      });
    }

    public static DuckNetErrorInfo AssembleMismatchError(string theirVersion)
    {
      string str = "";
      NMVersionMismatch.Type type = DuckNetwork.CheckVersion(theirVersion);
      bool flag = false;
      switch (type)
      {
        case NMVersionMismatch.Type.Older:
          str = "|RED|Your version is too new.\n\n|WHITE|HOST: |GREEN|" + theirVersion + "\n|WHITE|YOU:  |RED|" + DG.version;
          flag = true;
          break;
        case NMVersionMismatch.Type.Newer:
          str = "|RED|Your version is too old.\n\n|WHITE|HOST: |GREEN|" + theirVersion + "\n|WHITE|YOU:  |RED|" + DG.version;
          break;
        case NMVersionMismatch.Type.Error:
          str = "|RED|Your game version caused an error.\n\n|WHITE|HOST: |GREEN|" + theirVersion + "\n|WHITE|YOU:  |RED|" + DG.version;
          break;
      }
      return new DuckNetErrorInfo()
      {
        error = DuckNetError.VersionMismatch,
        message = str,
        tooNew = flag
      };
    }

    public static void OnMessage(NetMessage m)
    {
      if (m is NMJoinDuckNetwork)
        DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Join message");
      if (DuckNetwork.status == DuckNetStatus.Disconnected)
        return;
      if (m is NMDuckNetworkEvent)
      {
        (m as NMDuckNetworkEvent).Activate();
      }
      else
      {
        if (m == null)
          Main.codeNumber = 13371;
        UIMatchmakingBox.pulseNetwork = true;
        if (DuckNetwork.GetProfiles(m.connection).Count == 0 && m.connection != Network.host)
        {
          Main.codeNumber = 13372;
          NetMessage msg = DuckNetwork.OnMessageFromNewClient(m);
          if (msg == null)
            return;
          Send.Message(msg, m.connection);
        }
        else
        {
          if (DuckNetwork.HandleCoreConnectionMessages(m) || DuckNetwork.status == DuckNetStatus.Disconnecting)
            return;
          Main.codeNumber = 13373;
          foreach (Profile profile in DuckNetwork.GetProfiles(m.connection))
          {
            if (profile.networkStatus == DuckNetStatus.Disconnecting || profile.networkStatus == DuckNetStatus.Disconnected || profile.networkStatus == DuckNetStatus.Failure)
              return;
          }
          Main.codeNumber = (int) m.typeIndex;
          if (Network.isServer)
          {
            switch (m)
            {
              case NMLateJoinDuckNetwork _:
                if (!(Level.current is TeamSelect2))
                {
                  Send.Message((NetMessage) new NMGameInProgress(), NetMessagePriority.ReliableOrdered, m.connection);
                  break;
                }
                NMLateJoinDuckNetwork lateJoinDuckNetwork = m as NMLateJoinDuckNetwork;
                DevConsole.Log(DCSection.DuckNet, "|DGYELLOW|Late join attempt from " + lateJoinDuckNetwork.name);
                Profile profile1 = DuckNetwork.CreateProfile(m.connection, lateJoinDuckNetwork.name, (int) lateJoinDuckNetwork.duckIndex);
                if (profile1 != null)
                {
                  profile1.networkStatus = DuckNetStatus.Connected;
                  Level.current.OnNetworkConnecting(profile1);
                  DuckNetwork.SendNewProfile(profile1, m.connection, true);
                  break;
                }
                Send.Message((NetMessage) new NMServerFull(), NetMessagePriority.ReliableOrdered, m.connection);
                break;
              case NMJoinedDuckNetwork _:
                foreach (Profile profile2 in DuckNetwork.GetProfiles(m.connection))
                  DevConsole.Log(DCSection.DuckNet, "|LIME|" + profile2.name + " Has joined the DuckNet");
                Send.Message((NetMessage) new NMSwitchLevel("@TEAMSELECT", DuckNetwork.levelIndex, (ushort) (int) GhostManager.context.currentGhostIndex), m.connection);
                break;
              case NMClientLoadedLevel _:
                if ((int) (m as NMClientLoadedLevel).levelIndex == (int) DuckNetwork.levelIndex)
                {
                  m.connection.wantsGhostData = (int) (m as NMClientLoadedLevel).levelIndex;
                  break;
                }
                DevConsole.Log(DCSection.DuckNet, "|DGRED|" + m.connection.identifier + " LOADED WRONG LEVEL! (" + (object) DuckNetwork.levelIndex + " VS " + (object) (m as NMClientLoadedLevel).levelIndex + ")");
                break;
              case NMSetTeam _:
                NMSetTeam nmSetTeam1 = m as NMSetTeam;
                if (nmSetTeam1.duck < (byte) 0 || nmSetTeam1.duck >= (byte) 4)
                  break;
                Profile profile3 = DuckNetwork.profiles[(int) nmSetTeam1.duck];
                if (profile3.connection == null || profile3.team == null)
                  break;
                profile3.team = Teams.all[(int) nmSetTeam1.team];
                if (!DuckNetwork.OnTeamSwitch(profile3))
                  break;
                Send.MessageToAllBut((NetMessage) new NMSetTeam(nmSetTeam1.duck, nmSetTeam1.team), NetMessagePriority.ReliableOrdered, m.connection);
                break;
              case NMRoomData _:
                NMRoomData nmRoomData1 = m as NMRoomData;
                if (nmRoomData1.duck < (byte) 0 || nmRoomData1.duck >= (byte) 4)
                  break;
                Profile profile4 = DuckNetwork.profiles[(int) nmRoomData1.duck];
                if (profile4.connection == null || profile4.connection == DuckNetwork.localConnection)
                  break;
                profile4.furniturePositionData = nmRoomData1.data;
                Send.MessageToAllBut((NetMessage) new NMRoomData(nmRoomData1.data, nmRoomData1.duck), NetMessagePriority.ReliableOrdered, m.connection);
                break;
              case NMSpecialHat _:
                NMSpecialHat nmSpecialHat1 = m as NMSpecialHat;
                Team t1 = Team.Deserialize(nmSpecialHat1.GetData());
                using (List<Profile>.Enumerator enumerator = DuckNetwork.profiles.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    Profile current = enumerator.Current;
                    if ((long) current.steamID == (long) nmSpecialHat1.link)
                    {
                      if (t1 != null)
                        Team.MapFacade(current.steamID, t1);
                      else
                        Team.ClearFacade(current.steamID);
                      Send.MessageToAllBut((NetMessage) new NMSpecialHat(t1, current.steamID), NetMessagePriority.ReliableOrdered, m.connection);
                    }
                  }
                  break;
                }
            }
          }
          else
          {
            switch (m)
            {
              case NMSpecialHat _:
                NMSpecialHat nmSpecialHat2 = m as NMSpecialHat;
                Team t2 = Team.Deserialize(nmSpecialHat2.GetData());
                using (List<Profile>.Enumerator enumerator = DuckNetwork.profiles.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    Profile current = enumerator.Current;
                    if ((long) current.steamID == (long) nmSpecialHat2.link)
                    {
                      if (t2 != null)
                        Team.MapFacade(current.steamID, t2);
                      else
                        Team.ClearFacade(current.steamID);
                    }
                  }
                  break;
                }
              case NMJoinDuckNetwork _:
                if (!(m is NMRemoteJoinDuckNetwork remoteJoinDuckNetwork))
                {
                  DevConsole.Log(DCSection.DuckNet, "|LIME|Connection with host was established!");
                  NMJoinDuckNetwork nmJoinDuckNetwork = m as NMJoinDuckNetwork;
                  DuckNetwork._core.status = DuckNetStatus.Connected;
                  if (DuckNetwork.profiles[(int) nmJoinDuckNetwork.duckIndex].connection == DuckNetwork.localConnection)
                  {
                    DuckNetwork.profiles[(int) nmJoinDuckNetwork.duckIndex].networkStatus = DuckNetStatus.Connected;
                    break;
                  }
                  Profile profile2 = DuckNetwork.CreateProfile(DuckNetwork.localConnection, Network.activeNetwork.core.GetLocalName(), (int) nmJoinDuckNetwork.duckIndex, UIMatchmakingBox.matchmakingProfiles.Count > 0 ? UIMatchmakingBox.matchmakingProfiles[0].inputProfile : InputProfile.DefaultPlayer1, Teams.core.extraTeams.Count > 0);
                  DuckNetwork._core.localDuckIndex = (int) nmJoinDuckNetwork.duckIndex;
                  profile2.flippers = Profile.CalculateLocalFlippers();
                  profile2.networkStatus = DuckNetStatus.WaitingForLoadingToBeFinished;
                  break;
                }
                NetworkConnection networkConnection = remoteJoinDuckNetwork.connection;
                Main.codeNumber = 133701;
                if (remoteJoinDuckNetwork.identifier == "SERVER")
                {
                  Main.codeNumber = 133702;
                  Profile profile2 = DuckNetwork.CreateProfile(networkConnection, remoteJoinDuckNetwork.name, (int) remoteJoinDuckNetwork.duckIndex, hasCustomHats: remoteJoinDuckNetwork.hasCustomHats);
                  profile2.flippers = remoteJoinDuckNetwork.flippers;
                  profile2.team = Teams.all[(int) remoteJoinDuckNetwork.team];
                  if (DuckNetwork._core.hostDuckIndex == -1)
                    DuckNetwork._core.hostDuckIndex = (int) remoteJoinDuckNetwork.duckIndex;
                  Main.codeNumber = 133703;
                  bool flag = false;
                  foreach (Profile profile5 in DuckNetwork.GetProfiles(networkConnection))
                  {
                    if (profile5 != profile2)
                    {
                      profile2.networkStatus = profile5.networkStatus;
                      flag = true;
                      break;
                    }
                  }
                  Main.codeNumber = 133704;
                  if (flag)
                    break;
                  profile2.networkStatus = DuckNetStatus.WaitingForLoadingToBeFinished;
                  break;
                }
                Main.codeNumber = 133705;
                bool flag1 = false;
                DuckNetStatus duckNetStatus = DuckNetStatus.NeedsNotificationWhenReadyForData;
                foreach (Profile profile2 in DuckNetwork.GetProfiles(networkConnection))
                {
                  if (profile2.connection.identifier == remoteJoinDuckNetwork.identifier)
                  {
                    networkConnection = profile2.connection;
                    flag1 = true;
                    duckNetStatus = profile2.networkStatus;
                    break;
                  }
                }
                Main.codeNumber = 133706;
                if (!flag1)
                {
                  networkConnection = Network.activeNetwork.core.AttemptConnection(remoteJoinDuckNetwork.identifier);
                  if (networkConnection == null)
                  {
                    DuckNetwork.RaiseError(new DuckNetErrorInfo()
                    {
                      error = DuckNetError.InvalidConnectionInformation,
                      message = "Invalid connection information (" + remoteJoinDuckNetwork.identifier + ")"
                    });
                    break;
                  }
                }
                Main.codeNumber = 133707;
                Profile profile6 = DuckNetwork.CreateProfile(networkConnection, remoteJoinDuckNetwork.name, (int) remoteJoinDuckNetwork.duckIndex, hasCustomHats: remoteJoinDuckNetwork.hasCustomHats);
                profile6.team = Teams.all[(int) remoteJoinDuckNetwork.team];
                profile6.networkStatus = duckNetStatus;
                profile6.flippers = remoteJoinDuckNetwork.flippers;
                break;
              case NMEndOfDuckNetworkData _:
                Send.Message((NetMessage) new NMJoinedDuckNetwork(), m.connection);
                using (List<Profile>.Enumerator enumerator = DuckNetwork.profiles.GetEnumerator())
                {
                  while (enumerator.MoveNext())
                  {
                    Profile current = enumerator.Current;
                    if (current.connection == DuckNetwork.localConnection)
                    {
                      Send.Message((NetMessage) new NMProfileInfo(current.networkIndex, current.stats.unloyalFans, current.stats.loyalFans));
                      if (current.linkedProfile == Profiles.experienceProfile && Profiles.experienceProfile != null && Profiles.experienceProfile.furniturePositions.Count > 0)
                        Send.Message((NetMessage) new NMRoomData(Profiles.experienceProfile.furniturePositionData, current.networkIndex), m.connection);
                    }
                  }
                  break;
                }
              case NMEndOfGhostData _:
                if ((int) (m as NMEndOfGhostData).levelIndex == (int) DuckNetwork.levelIndex)
                {
                  DevConsole.Log(DCSection.DuckNet, "|DGGREEN|Received Host Level Information (" + (object) (m as NMEndOfGhostData).levelIndex + ").");
                  Level.current.TransferComplete(m.connection);
                  DuckNetwork.SendToEveryone((NetMessage) new NMLevelDataReady(DuckNetwork.levelIndex));
                  using (List<Profile>.Enumerator enumerator = DuckNetwork.GetProfiles(DuckNetwork.localConnection).GetEnumerator())
                  {
                    while (enumerator.MoveNext())
                      enumerator.Current.connection.loadingStatus = (m as NMEndOfGhostData).levelIndex;
                    break;
                  }
                }
                else
                {
                  DevConsole.Log(DCSection.DuckNet, "|DGRED|Recieved data for wrong level.");
                  break;
                }
              case NMSetTeam _:
                NMSetTeam nmSetTeam2 = m as NMSetTeam;
                if (nmSetTeam2.duck < (byte) 0 || nmSetTeam2.duck >= (byte) 4)
                  break;
                Profile profile7 = DuckNetwork.profiles[(int) nmSetTeam2.duck];
                if (profile7.connection == null || profile7.team == null)
                  break;
                profile7.team = Teams.all[(int) nmSetTeam2.team];
                break;
              case NMRoomData _:
                NMRoomData nmRoomData2 = m as NMRoomData;
                if (nmRoomData2.duck < (byte) 0 || nmRoomData2.duck >= (byte) 4)
                  break;
                Profile profile8 = DuckNetwork.profiles[(int) nmRoomData2.duck];
                if (profile8.connection == null || profile8.connection == DuckNetwork.localConnection)
                  break;
                profile8.furniturePositionData = nmRoomData2.data;
                break;
              case NMTeamSetDenied _:
                NMTeamSetDenied nmTeamSetDenied = m as NMTeamSetDenied;
                if (nmTeamSetDenied.duck < (byte) 0 || nmTeamSetDenied.duck >= (byte) 4)
                  break;
                Profile profile9 = DuckNetwork.profiles[(int) nmTeamSetDenied.duck];
                if (profile9.connection != DuckNetwork.localConnection || profile9.team == null || Teams.all.IndexOf(profile9.team) != (int) nmTeamSetDenied.team)
                  break;
                DuckNetwork.OpenTeamSwitchDialogue(profile9);
                break;
            }
          }
        }
      }
    }

    public static void SendToEveryone(NetMessage m)
    {
      foreach (Profile profile in DuckNetwork.profiles)
      {
        if (profile.connection != null && profile.connection != DuckNetwork.localConnection && (!profile.isHost || (int) profile.networkIndex == DuckNetwork.hostDuckIndex))
        {
          NetMessage instance = Activator.CreateInstance(m.GetType(), (object[]) null) as NetMessage;
          Editor.CopyClass((object) m, (object) instance);
          instance.ClearSerializedData();
          Send.Message(instance, NetMessagePriority.ReliableOrdered, profile.connection);
        }
      }
    }

    public static void Draw()
    {
      if (DuckNetwork._core.localDuckIndex == -1)
        return;
      Vec2 vec2 = new Vec2(Layer.Console.width, Layer.Console.height);
      float num1 = 0.0f;
      int num2 = 8;
      DuckNetwork._chatFont.scale = new Vec2(2f);
      if (DuckNetwork._core.enteringText && !DuckNetwork._core.stopEnteringText)
      {
        ++DuckNetwork._core.cursorFlash;
        if (DuckNetwork._core.cursorFlash > 30)
          DuckNetwork._core.cursorFlash = 0;
        bool flag = DuckNetwork._core.cursorFlash >= 15;
        Profile profile = DuckNetwork.profiles[DuckNetwork._core.localDuckIndex];
        string text = profile.name + ": " + DuckNetwork._core.currentEnterText;
        if (flag)
          text += "_";
        Vec2 p1 = new Vec2(14f, num1 + (vec2.y - 32f));
        Graphics.DrawRect(p1 + new Vec2(-1f, -1f), p1 + new Vec2(DuckNetwork._chatFont.GetWidth(text) + 4f, 20f) + new Vec2(1f, 1f), Color.Black * 0.6f, new Depth(0.7f), false);
        Color color = Color.White;
        Color black = Color.Black;
        if (profile.persona != null)
          color = profile.persona.colorUsable;
        Graphics.DrawRect(p1, p1 + new Vec2(DuckNetwork._chatFont.GetWidth(text) + 4f, 20f), color * 0.6f, new Depth(0.8f));
        DuckNetwork._chatFont.Draw(text, p1 + new Vec2(2f, 2f), black, new Depth(1f));
        num1 -= 22f;
      }
      float num3 = 1f;
      foreach (ChatMessage chatMessage in DuckNetwork._core.chatMessages)
      {
        string text = "|WHITE|" + chatMessage.who.name + ": |BLACK|" + chatMessage.text;
        float num4 = 20f;
        DuckNetwork._chatFont.scale = new Vec2(2f * chatMessage.scale);
        float num5 = DuckNetwork._chatFont.GetWidth(text) + num4;
        Vec2 p1 = new Vec2((float) (-((15.0 + (double) num5) * (1.0 - (double) chatMessage.slide)) + 14.0), num1 + (vec2.y - 32f));
        Graphics.DrawRect(p1 + new Vec2(-1f, -1f), p1 + new Vec2(num5 + 4f, 20f) + new Vec2(1f, 1f), Color.Black * 0.6f * chatMessage.alpha, new Depth(0.7f), false);
        float num6 = (float) (0.300000011920929 + (double) chatMessage.text.Length * 0.00700000021606684);
        if ((double) num6 > 0.5)
          num6 = 0.5f;
        if ((double) chatMessage.slide > 0.800000011920929)
          chatMessage.scale = Lerp.FloatSmooth(chatMessage.scale, 1f, 0.1f, 1.1f);
        else if ((double) chatMessage.slide > 0.5)
          chatMessage.scale = Lerp.FloatSmooth(chatMessage.scale, 1f + num6, 0.1f, 1.1f);
        chatMessage.slide = Lerp.FloatSmooth(chatMessage.slide, 1f, 0.1f, 1.1f);
        Color color = Color.White;
        Color black = Color.Black;
        if (chatMessage.who.persona != null)
        {
          color = chatMessage.who.persona.colorUsable;
          SpriteMap spriteMap = chatMessage.who.persona.defaultHead;
          Vec2 zero = Vec2.Zero;
          if (chatMessage.who.team != null && chatMessage.who.team.hasHat && (chatMessage.who.connection != DuckNetwork.localConnection || !chatMessage.who.team.locked))
          {
            Vec2 hatOffset = chatMessage.who.team.hatOffset;
            spriteMap = chatMessage.who.team.hat;
          }
          spriteMap.CenterOrigin();
          spriteMap.depth = (Depth) num3;
          spriteMap.alpha = chatMessage.alpha;
          spriteMap.scale = new Vec2(2f, 2f);
          Graphics.Draw((Sprite) spriteMap, p1.x, p1.y);
          spriteMap.scale = new Vec2(1f, 1f);
          spriteMap.alpha = 1f;
          color *= 0.85f;
          color.a = byte.MaxValue;
        }
        Graphics.DrawRect(p1, p1 + new Vec2(num5 + 4f, 20f), color * 0.75f * chatMessage.alpha, new Depth(0.6f));
        DuckNetwork._chatFont.Draw(text, p1 + new Vec2(2f + num4, 2f), black * chatMessage.alpha, new Depth(0.9f));
        num1 -= 26f;
        num3 -= 0.01f;
        if (num2 == 0)
          break;
        --num2;
      }
    }
  }
}
