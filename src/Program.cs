// Decompiled with JetBrains decompiler
// Type: DuckGame.Program
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows.Forms;

namespace DuckGame
{
  /// <summary>The main class.</summary>
  public static class Program
  {
    private const uint WM_CLOSE = 16;
    public static bool testServer = false;
    public static DuckGame.Main main;
    /// <summary>The main entry point for the application.</summary>
    public static string commandLine = "";
    public static int constructorsLoaded;
    public static int thingTypes;

    private static void Main(string[] args)
    {
      AppDomain.CurrentDomain.UnhandledException += new UnhandledExceptionEventHandler(Program.UnhandledExceptionTrapper);
      Application.ThreadException += new ThreadExceptionEventHandler(Program.UnhandledThreadExceptionTrapper);
      Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
      Program.DoMain(args);
    }

    private static void DoMain(string[] args)
    {
            // TODO: figure out steam
            MonoMain.disableSteam = true;




            Directory.SetCurrentDirectory(AppDomain.CurrentDomain.BaseDirectory);
      for (int index = 0; index < args.Length; ++index)
      {
        Program.commandLine += args[index];
        if (index != args.Length - 1)
          Program.commandLine += " ";
        if (args[index] == "+connect_lobby")
        {
          ++index;
          if (((IEnumerable<string>) args).Count<string>() > index)
            DuckGame.Main.connectID = Convert.ToUInt64(args[index], (IFormatProvider) CultureInfo.InvariantCulture);
        }
        else if (args[index] == "-alternateFullscreen")
          MonoMain.alternateFullscreen = true;
        else if (args[index] == "-testserver")
          Program.testServer = true;
        else if (args[index] == "-nothreading")
          MonoMain.enableThreadedLoading = false;
        else if (args[index] == "-defaultcontrols")
          MonoMain.defaultControls = true;
        else if (args[index] == "-nofullscreen")
          MonoMain.noFullscreen = true;
        else if (args[index] == "-disablecloud")
          MonoMain.disableCloud = true;
        else if (args[index] == "-nosteam")
          MonoMain.disableSteam = true;
        else if (args[index] == "-cloudnoload")
          MonoMain.cloudNoLoad = true;
        else if (args[index] == "-cloudnosave")
          MonoMain.cloudNoSave = true;
        else if (args[index] == "-enablecloud")
          MonoMain.disableCloud = false;
        else if (args[index] == "-nomods")
          MonoMain.moddingEnabled = false;
        else if (args[index] == "-nointro")
          MonoMain.noIntro = true;
        else if (args[index] == "-startineditor")
          MonoMain.startInEditor = true;
        else if (args[index] == "-moddebug")
          MonoMain.modDebugging = true;
        else if (args[index] == "-downloadmods")
          MonoMain.downloadWorkshopMods = true;
      }
      if (!MonoMain.disableSteam)
      {
        if (MonoMain.breakSteam || !Steam.InitializeCore())
          Program.LogLine("Steam INIT Failed!");
        Steam.Initialize();
      }
      DeviceChangeNotifier.Start();
      Program.main = new DuckGame.Main();
      Program.main.Run();
    }

    [DllImport("user32.dll", CharSet = CharSet.Auto)]
    private static extern IntPtr SendMessage(
      IntPtr hWnd,
      uint Msg,
      IntPtr wParam,
      IntPtr lParam);

    public static void UnhandledThreadExceptionTrapper(object sender, ThreadExceptionEventArgs e) => Program.HandleGameCrash(e.Exception);

    public static void UnhandledExceptionTrapper(object sender, UnhandledExceptionEventArgs e) => Program.HandleGameCrash(e.ExceptionObject as Exception);

    public static void HandleGameCrash(Exception e)
    {
      try
      {
        string s = !MonoMain.hadInfiniteLoop ? MonoMain.GetExceptionString((object) e) : MonoMain.infiniteLoopDetails;
        Exception exception = e;
        bool flag1 = false;
        bool flag2 = false;
        Assembly assembly = (Assembly) null;
        try
        {
          foreach (Mod allMod in (IEnumerable<Mod>) ModLoader.allMods)
          {
            switch (allMod)
            {
              case CoreMod _:
              case DisabledMod _:
                continue;
              default:
                bool flag3 = allMod.configuration.assembly == exception.TargetSite.DeclaringType.Assembly;
                if (!flag3)
                {
                  foreach (System.Type type in allMod.configuration.assembly.GetTypes())
                  {
                    if (e.StackTrace.Contains(type.ToString()))
                    {
                      flag3 = true;
                      break;
                    }
                    if (e.InnerException != null && e.InnerException.StackTrace.Contains(type.ToString()))
                    {
                      flag3 = true;
                      break;
                    }
                  }
                }
                if (flag3)
                {
                  assembly = allMod.configuration.assembly;
                  flag1 = true;
                  if (!MonoMain.modDebugging)
                  {
                    allMod.configuration.Disable();
                    flag2 = true;
                    continue;
                  }
                  continue;
                }
                continue;
            }
          }
          Exception innerException = e.InnerException;
        }
        catch (Exception ex)
        {
        }
        try
        {
          s = s.Replace("E:\\gamedev\\DuckGame_Steam\\duckgame_halloween_new\\duckgame\\", "");
        }
        catch (Exception ex)
        {
        }
        Program.WriteToLog(s);
        Program.SendMessage(Program.main.Window.Handle, 16U, IntPtr.Zero, IntPtr.Zero);
        Process.Start("CrashWindow.exe", "-modResponsible " + (flag1 ? "1" : "0") + " -modDisabled " + (flag2 ? "1" : "0") + " -modAssembly " + (assembly != (Assembly) null ? assembly.GetName().Name : "UNKNOWN") + " -exceptionString \"" + s.Replace("\n", "|NEWLINE|").Replace("\r", "|NEWLINE2|") + "\" -source " + exception.Source + " -commandLine \"" + Program.commandLine + "\" -executable \"" + Application.ExecutablePath + "\"");
        Environment.Exit(1);
      }
      catch (Exception ex)
      {
        StreamWriter streamWriter = new StreamWriter("ducklog.txt", true);
        streamWriter.WriteLine("Failed to write exception to log.\n");
        streamWriter.Close();
      }
    }

    public static void WriteToLog(string s)
    {
      try
      {
        StreamWriter streamWriter = new StreamWriter("ducklog.txt", true);
        streamWriter.WriteLine(s + "\n");
        streamWriter.Close();
        Program.MakeNetLog();
        MonoMain.UploadError(s);
      }
      catch (Exception ex)
      {
        StreamWriter streamWriter = new StreamWriter("ducklog.txt", true);
        streamWriter.WriteLine(ex.ToString() + "\n");
        streamWriter.Close();
      }
    }

    public static void LogLine(string line)
    {
      try
      {
        StreamWriter streamWriter = new StreamWriter("ducklog.txt", true);
        streamWriter.WriteLine(line + "\n");
        streamWriter.Close();
      }
      catch
      {
      }
    }

    public static void MakeNetLog()
    {
      StreamWriter streamWriter = new StreamWriter("netlog.txt", false);
      foreach (DCLine line in DevConsole.core.lines)
        streamWriter.WriteLine(line.timestamp.ToLongTimeString() + " " + line.SectionString() + " " + line.line + "\n");
      foreach (DCLine pendingLine in DevConsole.core.pendingLines)
        streamWriter.WriteLine(pendingLine.timestamp.ToLongTimeString() + " " + pendingLine.SectionString() + " " + pendingLine.line + "\n");
      streamWriter.WriteLine("\n");
      streamWriter.Close();
    }

    private static void UnhandledExceptionTrapperTestServer(
      object sender,
      UnhandledExceptionEventArgs e)
    {
      string exceptionString = MonoMain.GetExceptionString(e);
      StreamWriter streamWriter = new StreamWriter("ducklog.txt", true);
      streamWriter.WriteLine(exceptionString + "\n");
      streamWriter.Close();
      Environment.Exit(1);
    }
  }
}
