// Decompiled with JetBrains decompiler
// Type: DuckGame.DG
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;

namespace DuckGame
{
  internal static class DG
  {
    private static int _versionHigh = 6451;
    private static int _versionLow = 16789;
    private static ulong _localID = 0;
    private static bool _drmFailure = false;
    private static bool _devBuild = false;
    private static bool _betaBuild = true;
    private static bool _pressBuild = true;

    public static string version => "1.0." + (object) DG._versionHigh + "." + (object) DG._versionLow;

    public static int versionHigh => DG._versionHigh;

    public static int versionLow => DG._versionLow;

    public static bool isHalloween
    {
      get
      {
        if (DateTime.Now.Month != 10)
          return false;
        return DateTime.Now.Day == 28 || DateTime.Now.Day == 29 || DateTime.Now.Day == 30 || DateTime.Now.Day == 31;
      }
    }

    public static string platform
    {
      get
      {
        string str1 = Environment.OSVersion.ToString();
        string str2 = "Windows Mystery Edition";
        if (str1.Contains("5.0"))
          str2 = "Windows 2000";
        else if (str1.Contains("5.1"))
          str2 = "Windows XP";
        else if (str1.Contains("5.2"))
          str2 = "Windows XP 64-Bit Edition";
        else if (str1.Contains("6.0"))
          str2 = "Windows Vista";
        else if (str1.Contains("6.1"))
          str2 = "Windows 7";
        else if (str1.Contains("6.2"))
          str2 = "Windows 8";
        else if (str1.Contains("6.3"))
          str2 = "Windows 8.1";
        else if (str1.Contains("10.0"))
          str2 = "Windows 10";
        return str2;
      }
    }

    public static ulong localID => Steam.user != null ? Steam.user.id : DG._localID;

    public static void SetVersion(string v)
    {
      string[] strArray = v.Split('.');
      if (strArray.Length == 4)
      {
        try
        {
          DG._versionLow = Convert.ToInt32(strArray[3]);
          DG._versionHigh = Convert.ToInt32(strArray[2]);
        }
        catch (Exception ex)
        {
        }
      }
      DG._localID = (ulong) Rando.Long();
    }

    public static bool InitializeDRM() => true;

    public static bool drmFailure => DG._drmFailure;

    public static bool devBuild => DG._devBuild;

    public static bool betaBuild => DG._betaBuild;

    public static bool pressBuild => DG._pressBuild;

    public static bool buildExpired => false;
  }
}
