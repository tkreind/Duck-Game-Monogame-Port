﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.Script
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Reflection;

namespace DuckGame
{
  public class Script
  {
    private static Profile _activeProfile;
    private static int _currentPosition;
    private static List<List<Profile>> _positions;
    private static PropertyInfo _activeProfileProperty;
    private static DuckNews _activeNewsStory;
    private static Dictionary<string, MethodInfo> _availableFunctions = new Dictionary<string, MethodInfo>();
    private static List<string> _highlightRatings = new List<string>()
    {
      "Ughhhh...",
      "I fell asleep",
      "Really Boring",
      "Kinda Boring",
      "Not Terribly Exciting",
      "About Average",
      "Mildly Entertaining",
      "Pretty Exciting",
      "Awesome",
      "Super Awesome",
      "Heart Stopping Action",
      "Insane Non-stop Insanity"
    };

    public static Profile activeProfile
    {
      get => Script._activeProfile;
      set => Script._activeProfile = value;
    }

    public static int currentPosition
    {
      get => Script._currentPosition;
      set => Script._currentPosition = value;
    }

    public static List<List<Profile>> positions
    {
      get => Script._positions;
      set => Script._positions = value;
    }

    public static DuckNews activeNewsStory
    {
      get => Script._activeNewsStory;
      set => Script._activeNewsStory = value;
    }

    public static MethodInfo GetMethod(string name)
    {
      MethodInfo methodInfo = (MethodInfo) null;
      Script._availableFunctions.TryGetValue(name, out methodInfo);
      return methodInfo;
    }

    public static object CallMethod(string name, object value)
    {
      MethodInfo method = Script.GetMethod(name);
      if (!(method != (MethodInfo) null))
        return (object) null;
      MethodInfo methodInfo = method;
      object[] parameters;
      if (value == null)
        parameters = (object[]) null;
      else
        parameters = new object[1]{ value };
      return methodInfo.Invoke((object) null, parameters);
    }

    public static void Initialize()
    {
      Script._activeProfile = Profiles.DefaultPlayer1;
      Script._activeProfileProperty = typeof (Script).GetProperty("activeProfile", BindingFlags.Static | BindingFlags.Public);
      foreach (MethodInfo method in typeof (Script).GetMethods(BindingFlags.Static | BindingFlags.Public))
        Script._availableFunctions[method.Name] = method;
    }

    public static int profileScore() => Script.activeProfile.endOfRoundStats.GetProfileScore();

    public static int negProfileScore() => -Script.activeProfile.endOfRoundStats.GetProfileScore();

    public static ScriptObject stat(string statName)
    {
      PropertyInfo property = typeof (ProfileStats).GetProperty(statName);
      if (!(property != (PropertyInfo) null))
        return (ScriptObject) null;
      return new ScriptObject()
      {
        obj = (object) Script.activeProfile.endOfRoundStats,
        objectProperty = property
      };
    }

    public static ScriptObject statNegative(string statName)
    {
      PropertyInfo property = typeof (ProfileStats).GetProperty(statName);
      if (!(property != (PropertyInfo) null))
        return (ScriptObject) null;
      return new ScriptObject()
      {
        obj = (object) Script.activeProfile.endOfRoundStats,
        objectProperty = property,
        negative = true
      };
    }

    public static string coolnessString() => Script.activeProfile.endOfRoundStats.GetCoolnessString();

    public static ScriptObject prevStat(string statName)
    {
      PropertyInfo property = typeof (ProfileStats).GetProperty(statName);
      if (!(property != (PropertyInfo) null))
        return (ScriptObject) null;
      return new ScriptObject()
      {
        obj = (object) Script.activeProfile.prevStats,
        objectProperty = property
      };
    }

    public static string previousTitleOwner(string name)
    {
      DuckTitle title = DuckTitle.GetTitle(name);
      return title != null ? title.previousOwner : "";
    }

    public static float sin(float val) => (float) Math.Sin((double) val);

    public static float cos(float val) => (float) Math.Cos((double) val);

    public static float round(float val) => (float) Math.Round((double) val);

    public static float toFloat(int val) => (float) val;

    public static int place() => Script.currentPosition;

    public static float random() => Rando.Float(1f);

    public static string winner() => Results.winner.name;

    public static string RatingsString(int wow)
    {
      if (wow > Global.data.highestNewsCast)
        Global.data.highestNewsCast = wow;
      int num1 = 60;
      int num2 = 250 + (int) ((double) Global.data.highestNewsCast * (double) Rando.Float(0.1f, 0.25f));
      if (wow < num1)
        wow = num1;
      if (wow > num2)
        wow = num2;
      wow -= num1;
      float num3 = (float) wow / (float) (num2 - num1);
      return Script._highlightRatings[(int) Math.Round((double) num3 * (double) (Script._highlightRatings.Count - 1))];
    }

    public static string highlightRating()
    {
      float num = 0.0f;
      List<Recording> highlights = Highlights.GetHighlights();
      foreach (Recording recording in highlights)
        num += recording.highlightScore;
      return Script.RatingsString((int) (num / (float) highlights.Count * 1.5f));
    }

    public static float floatVALUE()
    {
      if (Script._activeNewsStory != null && Script._activeNewsStory.valueCalculation != null)
      {
        object result = Script._activeNewsStory.valueCalculation.result;
        if (result != null)
          return Change.ToSingle(result);
      }
      return 0.0f;
    }

    public static float floatVALUE2()
    {
      if (Script._activeNewsStory != null && Script._activeNewsStory.valueCalculation != null)
      {
        object result = Script._activeNewsStory.valueCalculation2.result;
        if (result != null)
          return Change.ToSingle(result);
      }
      return 0.0f;
    }

    public static int numInPlace(int p) => Script.positions == null || p < 0 || p >= Script.positions.Count ? 0 : Script.positions[Script.positions.Count - 1 - p].Count;

    public static bool skippedNewscast() => HighlightLevel.didSkip;

    public static bool hasPurchaseInfo() => Main.foundPurchaseInfo;

    public static bool doesNotHavePurchaseInfo() => !Main.foundPurchaseInfo;

    public static bool isDemo() => Main.isDemo;

    public static bool isNotDemo() => !Main.isDemo;

    public static float greatest(string val)
    {
      float num1 = -99999f;
      foreach (Profile profile in Profiles.active)
      {
        float num2 = -99999f;
        ScriptObject scriptObject = Script.stat(val);
        if (scriptObject != null)
          num2 = Change.ToSingle(scriptObject.objectProperty.GetValue(scriptObject.obj, (object[]) null)) * (scriptObject.negative ? -1f : 1f);
        if ((double) num2 > (double) num1)
          num1 = num2;
      }
      return num1;
    }

    public static bool hasGreatest(string val)
    {
      float num1 = -999999f;
      Profile profile1 = (Profile) null;
      foreach (Profile profile2 in Profiles.active)
      {
        float num2 = -999999f;
        Profile activeProfile = Script.activeProfile;
        Script.activeProfile = profile2;
        if (Script._activeNewsStory != null && val == "VALUE")
        {
          object result = Script._activeNewsStory.valueCalculation.result;
          if (result != null)
            num2 = Change.ToSingle(result);
        }
        else if (val != "VALUE")
        {
          ScriptObject scriptObject = Script.stat(val);
          if (scriptObject != null)
            num2 = Change.ToSingle(scriptObject.objectProperty.GetValue(scriptObject.obj, (object[]) null)) * (scriptObject.negative ? -1f : 1f);
        }
        Script.activeProfile = activeProfile;
        if ((double) num2 > (double) num1)
        {
          num1 = num2;
          profile1 = profile2;
        }
      }
      return profile1 == Script.activeProfile;
    }
  }
}
