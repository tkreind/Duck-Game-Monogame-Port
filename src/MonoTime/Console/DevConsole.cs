// Decompiled with JetBrains decompiler
// Type: DuckGame.DevConsole
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace DuckGame
{
  public class DevConsole
  {
    public static bool fancyMode = false;
    private static DevConsoleCore _core = new DevConsoleCore();
    private static bool _enableNetworkDebugging = false;
    private static bool _oldConsole;
    public static bool fuckUpPacketOrder = false;
    public static List<DCLine> debuggerLines = new List<DCLine>();
    public static Sprite _tray;
    public static Sprite _scan;

    public static DevConsoleCore core
    {
      get => DevConsole._core;
      set => DevConsole._core = value;
    }

    public static bool open => DevConsole._core.open;

    public static void SuppressDevConsole()
    {
      DevConsole._oldConsole = DevConsole._enableNetworkDebugging;
      DevConsole._enableNetworkDebugging = false;
    }

    public static void RestoreDevConsole() => DevConsole._enableNetworkDebugging = DevConsole._oldConsole;

    public static bool enableNetworkDebugging
    {
      get => DevConsole._enableNetworkDebugging;
      set => DevConsole._enableNetworkDebugging = value;
    }

    public static bool splitScreen
    {
      get => DevConsole._core.splitScreen;
      set => DevConsole._core.splitScreen = value;
    }

    public static bool rhythmMode
    {
      get => DevConsole._core.rhythmMode;
      set => DevConsole._core.rhythmMode = value;
    }

    public static bool qwopMode
    {
      get => DevConsole._core.qwopMode;
      set => DevConsole._core.qwopMode = value;
    }

    public static bool showIslands
    {
      get => DevConsole._core.showIslands;
      set => DevConsole._core.showIslands = value;
    }

    public static bool showCollision
    {
      get => DevConsole._core.showCollision;
      set => DevConsole._core.showCollision = value;
    }

    public static bool shieldMode
    {
      get => DevConsole._core.shieldMode;
      set => DevConsole._core.shieldMode = value;
    }

    public static void Draw()
    {
      if (DevConsole._core.font == null)
      {
        DevConsole._core.font = new BitmapFont("biosFont", 8);
        DevConsole._core.font.scale = new Vec2(2f, 2f);
        DevConsole._core.fancyFont = new FancyBitmapFont("smallFont");
        DevConsole._core.fancyFont.scale = new Vec2(2f, 2f);
      }
      if ((double) DevConsole._core.alpha <= 0.00999999977648258)
        return;
      float num1 = 256f;
      int width = Graphics.width;
      DevConsole._core.font.alpha = DevConsole._core.alpha;
      DevConsole._core.font.Draw(DevConsole._core.typing, 16f, num1 + 20f, Color.White, new Depth(0.9f));
      int index1 = DevConsole._core.lines.Count - 1;
      float num2 = 0.0f;
      for (int index2 = 0; (double) index2 < (double) num1 / 18.0 && index1 >= 0; ++index2)
      {
        DCLine dcLine = DevConsole._core.lines.ElementAt<DCLine>(index1);
        if (!NetworkDebugger.enabled || dcLine.threadIndex == NetworkDebugger.networkDrawingIndex)
        {
          DevConsole._core.font.scale = new Vec2(dcLine.scale);
          DevConsole._core.font.Draw(dcLine.SectionString() + dcLine.line, 16f, num1 - 20f - num2, dcLine.color * 0.8f, new Depth(0.9f));
          num2 += (float) (18.0 * ((double) dcLine.scale * 0.5));
          DevConsole._core.font.scale = new Vec2(2f);
        }
        --index1;
      }
      if (DevConsole._tray == null)
        return;
      DevConsole._tray.alpha = DevConsole._core.alpha;
      DevConsole._tray.scale = new Vec2(4f, 4f);
      DevConsole._tray.depth = new Depth(0.75f);
      Graphics.Draw(DevConsole._tray, 0.0f, 0.0f);
      DevConsole._scan.alpha = DevConsole._core.alpha;
      DevConsole._scan.scale = new Vec2(2f, 2f);
      DevConsole._scan.depth = new Depth(0.95f);
      Graphics.Draw(DevConsole._scan, 0.0f, 0.0f);
      DevConsole._core.fancyFont.depth = new Depth(0.98f);
      DevConsole._core.fancyFont.alpha = DevConsole._core.alpha;
      string version = DG.version;
      DevConsole._core.fancyFont.Draw(version, new Vec2(1116f, 284f), Colors.SuperDarkBlueGray, new Depth(0.98f));
    }

    public static Profile ProfileByName(string findName)
    {
      foreach (Profile profile in Profiles.all)
      {
        if (profile.team != null)
        {
          string str = profile.name.ToLower();
          if (findName == "player1" && (profile.persona == Persona.Duck1 || profile.duck != null && profile.duck.persona == Persona.Duck1))
            str = findName;
          else if (findName == "player2" && (profile.persona == Persona.Duck2 || profile.duck != null && profile.duck.persona == Persona.Duck2))
            str = findName;
          else if (findName == "player3" && (profile.persona == Persona.Duck3 || profile.duck != null && profile.duck.persona == Persona.Duck3))
            str = findName;
          else if (findName == "player4" && (profile.persona == Persona.Duck4 || profile.duck != null && profile.duck.persona == Persona.Duck4))
            str = findName;
          if (str == findName)
            return profile;
        }
      }
      return (Profile) null;
    }

    public static void RunCommand(string command)
    {
      if (DG.buildExpired)
        return;
      DevConsole._core.logScores = -1;
      if (!(command != ""))
        return;
      CultureInfo currentCulture = CultureInfo.CurrentCulture;
      command = command.ToLower(currentCulture);
      bool flag1 = false;
      ConsoleCommand consoleCommand = new ConsoleCommand(command);
      string str1 = consoleCommand.NextWord();
      if (str1 == "spawn")
      {
        if (Network.isActive || Level.current is ChallengeLevel)
        {
          DevConsole._core.lines.Enqueue(new DCLine()
          {
            line = "You can't do that here!",
            color = Color.Red
          });
          return;
        }
        flag1 = true;
        string str2 = consoleCommand.NextWord();
        float single1;
        float single2;
        try
        {
          single1 = Change.ToSingle((object) consoleCommand.NextWord());
          single2 = Change.ToSingle((object) consoleCommand.NextWord());
        }
        catch
        {
          DevConsole._core.lines.Enqueue(new DCLine()
          {
            line = "Parameters in wrong format.",
            color = Color.Red
          });
          return;
        }
        if (consoleCommand.NextWord() != "")
        {
          DevConsole._core.lines.Enqueue(new DCLine()
          {
            line = "Too many parameters!",
            color = Color.Red
          });
          return;
        }
        System.Type t = (System.Type) null;
        foreach (System.Type thingType in Editor.ThingTypes)
        {
          if (thingType.Name.ToLower(currentCulture) == str2)
          {
            t = thingType;
            break;
          }
        }
        if (t == (System.Type) null)
        {
          DevConsole._core.lines.Enqueue(new DCLine()
          {
            line = "The type " + str2 + " does not exist!",
            color = Color.Red
          });
          return;
        }
        if (!Editor.HasConstructorParameter(t))
        {
          DevConsole._core.lines.Enqueue(new DCLine()
          {
            line = str2 + " can not be spawned this way.",
            color = Color.Red
          });
          return;
        }
        Thing thing = (Thing) (Editor.CreateThing(t) as PhysicsObject);
        if (thing != null)
        {
          thing.x = single1;
          thing.y = single2;
          Level.Add(thing);
          SFX.Play("hitBox");
        }
      }
      if (str1 == "modhash")
      {
        DevConsole._core.lines.Enqueue(new DCLine()
        {
          line = ModLoader._modString,
          color = Color.Red
        });
        DevConsole._core.lines.Enqueue(new DCLine()
        {
          line = ModLoader.modHash,
          color = Color.Red
        });
      }
      else if (str1 == "netdebug")
      {
        DevConsole._enableNetworkDebugging = !DevConsole._enableNetworkDebugging;
        DevConsole._core.lines.Enqueue(new DCLine()
        {
          line = "Network Debugging Enabled",
          color = Color.Green
        });
      }
      else if (str1 == "record")
      {
        DevConsole._core.lines.Enqueue(new DCLine()
        {
          line = "Recording Started",
          color = Color.Green
        });
        Recorder.globalRecording = new FileRecording();
        MonoMain.StartRecording(consoleCommand.NextWord().ToLower(currentCulture));
      }
      else
      {
        if (str1 == "close")
          DevConsole._core.open = !DevConsole._core.open;
        if (str1 == "stop")
        {
          if (Recorder.globalRecording == null)
            return;
          DevConsole._core.lines.Enqueue(new DCLine()
          {
            line = "Recording Stopped",
            color = Color.Green
          });
          MonoMain.StopRecording();
        }
        else
        {
          if (str1 == "playback")
          {
            flag1 = true;
            string lower = consoleCommand.NextWord().ToLower(currentCulture);
            if (lower == "")
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "Parameters in wrong format.",
                color = Color.Red
              });
              return;
            }
            if (consoleCommand.NextWord() != "")
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "Too many parameters!",
                color = Color.Red
              });
              return;
            }
            if (!File.Exists(lower + ".vid"))
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "Could not find recording called " + lower + "!",
                color = Color.Red
              });
              return;
            }
            Recorder.globalRecording = new FileRecording();
            Recorder.globalRecording.fileName = lower;
            DevConsole._core.lines.Enqueue(new DCLine()
            {
              line = lower + " set for playback.",
              color = Color.Green
            });
            MonoMain.StartPlayback();
          }
          if (str1 == "level")
          {
            if (Network.isActive || Level.current is ChallengeLevel)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You can't do that here!",
                color = Color.Red
              });
              return;
            }
            flag1 = true;
            string lower = consoleCommand.NextWord().ToLower(currentCulture);
            if (lower == "")
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "Parameters in wrong format.",
                color = Color.Red
              });
              return;
            }
            if (consoleCommand.NextWord() != "")
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "Too many parameters!",
                color = Color.Red
              });
              return;
            }
            bool flag2 = false;
            LevelData levelData = DuckFile.LoadLevel(Content.path + "/levels/" + ("deathmatch/" + lower) + ".lev");
            if (levelData != null)
            {
              Level.current = (Level) new GameLevel(levelData.metaData.guid);
              flag2 = true;
            }
            if (!flag2)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "The level \"" + lower + "\" does not exist!",
                color = Color.Red
              });
              return;
            }
          }
          if (str1 == "team")
          {
            if (Network.isActive || Level.current is ChallengeLevel)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You can't do that here!",
                color = Color.Red
              });
              return;
            }
            flag1 = true;
            string findName = consoleCommand.NextWord();
            Profile profile = DevConsole.ProfileByName(findName);
            if (profile != null)
            {
              string str2 = consoleCommand.NextWord();
              if (str2 == "")
              {
                DevConsole._core.lines.Enqueue(new DCLine()
                {
                  line = "Parameters in wrong format.",
                  color = Color.Red
                });
                return;
              }
              if (consoleCommand.NextWord() != "")
              {
                DevConsole._core.lines.Enqueue(new DCLine()
                {
                  line = "Too many parameters!",
                  color = Color.Red
                });
                return;
              }
              string lower = str2.ToLower();
              bool flag2 = false;
              foreach (Team team in Teams.all)
              {
                if (team.name.ToLower() == lower)
                {
                  flag2 = true;
                  profile.team = team;
                  break;
                }
              }
              if (!flag2)
              {
                DevConsole._core.lines.Enqueue(new DCLine()
                {
                  line = "No team named " + lower + ".",
                  color = Color.Red
                });
                return;
              }
            }
            else
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "No profile named " + findName + ".",
                color = Color.Red
              });
              return;
            }
          }
          if (str1 == "give")
          {
            if (!Network.isActive)
            {
              switch (Level.current)
              {
                case ChallengeLevel _:
                case ArcadeLevel _:
                  break;
                default:
                  flag1 = true;
                  string findName = consoleCommand.NextWord();
                  Profile profile = DevConsole.ProfileByName(findName);
                  if (profile != null)
                  {
                    if (profile.duck != null)
                    {
                      string str2 = consoleCommand.NextWord();
                      if (str2 == "")
                      {
                        DevConsole._core.lines.Enqueue(new DCLine()
                        {
                          line = "Parameters in wrong format.",
                          color = Color.Red
                        });
                        return;
                      }
                      if (consoleCommand.NextWord() != "")
                      {
                        DevConsole._core.lines.Enqueue(new DCLine()
                        {
                          line = "Too many parameters!",
                          color = Color.Red
                        });
                        return;
                      }
                      System.Type t = (System.Type) null;
                      foreach (System.Type thingType in Editor.ThingTypes)
                      {
                        if (thingType.Name.ToLower(currentCulture) == str2)
                        {
                          t = thingType;
                          break;
                        }
                      }
                      if (t == (System.Type) null)
                      {
                        DevConsole._core.lines.Enqueue(new DCLine()
                        {
                          line = "The type " + str2 + " does not exist!",
                          color = Color.Red
                        });
                        return;
                      }
                      if (!Editor.HasConstructorParameter(t))
                      {
                        DevConsole._core.lines.Enqueue(new DCLine()
                        {
                          line = str2 + " can not be spawned this way.",
                          color = Color.Red
                        });
                        return;
                      }
                      if (Editor.CreateThing(t) is Holdable thing)
                      {
                        Level.Add((Thing) thing);
                        profile.duck.GiveHoldable(thing);
                        SFX.Play("hitBox");
                        goto label_96;
                      }
                      else
                      {
                        DevConsole._core.lines.Enqueue(new DCLine()
                        {
                          line = str2 + " can not be held.",
                          color = Color.Red
                        });
                        return;
                      }
                    }
                    else
                    {
                      DevConsole._core.lines.Enqueue(new DCLine()
                      {
                        line = findName + " is not in the game!",
                        color = Color.Red
                      });
                      return;
                    }
                  }
                  else
                  {
                    DevConsole._core.lines.Enqueue(new DCLine()
                    {
                      line = "No profile named " + findName + ".",
                      color = Color.Red
                    });
                    return;
                  }
              }
            }
            DevConsole._core.lines.Enqueue(new DCLine()
            {
              line = "You can't do that here!",
              color = Color.Red
            });
            return;
          }
label_96:
          if (str1 == "call")
          {
            if (Network.isActive || Level.current is ChallengeLevel)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You can't do that here!",
                color = Color.Red
              });
              return;
            }
            flag1 = true;
            string str2 = consoleCommand.NextWord();
            bool flag2 = false;
            foreach (Profile profile in Profiles.all)
            {
              if (profile.name.ToLower(currentCulture) == str2)
              {
                if (profile.duck != null)
                {
                  flag2 = true;
                  string str3 = consoleCommand.NextWord();
                  if (str3 == "")
                  {
                    DevConsole._core.lines.Enqueue(new DCLine()
                    {
                      line = "Parameters in wrong format.",
                      color = Color.Red
                    });
                    return;
                  }
                  if (consoleCommand.NextWord() != "")
                  {
                    DevConsole._core.lines.Enqueue(new DCLine()
                    {
                      line = "Too many parameters!",
                      color = Color.Red
                    });
                    return;
                  }
                  MethodInfo[] methods = typeof (Duck).GetMethods();
                  bool flag3 = false;
                  foreach (MethodInfo methodInfo in methods)
                  {
                    if (methodInfo.Name.ToLower(currentCulture) == str3)
                    {
                      flag3 = true;
                      if (((IEnumerable<ParameterInfo>) methodInfo.GetParameters()).Count<ParameterInfo>() > 0)
                      {
                        DevConsole._core.lines.Enqueue(new DCLine()
                        {
                          line = "You can only call functions with no parameters.",
                          color = Color.Red
                        });
                        return;
                      }
                      try
                      {
                        methodInfo.Invoke((object) profile.duck, (object[]) null);
                      }
                      catch
                      {
                        DevConsole._core.lines.Enqueue(new DCLine()
                        {
                          line = "The function threw an exception.",
                          color = Color.Red
                        });
                        return;
                      }
                    }
                  }
                  if (!flag3)
                  {
                    DevConsole._core.lines.Enqueue(new DCLine()
                    {
                      line = "Duck has no function called " + str3 + ".",
                      color = Color.Red
                    });
                    return;
                  }
                }
                else
                {
                  DevConsole._core.lines.Enqueue(new DCLine()
                  {
                    line = str2 + " is not in the game!",
                    color = Color.Red
                  });
                  return;
                }
              }
            }
            if (!flag2)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "No profile named " + str2 + ".",
                color = Color.Red
              });
              return;
            }
          }
          if (str1 == "set")
          {
            if (Network.isActive || Level.current is ChallengeLevel)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You can't do that here!",
                color = Color.Red
              });
              return;
            }
            flag1 = true;
            string str2 = consoleCommand.NextWord();
            bool flag2 = false;
            foreach (Profile profile in Profiles.all)
            {
              if (profile.name.ToLower(currentCulture) == str2)
              {
                if (profile.duck != null)
                {
                  flag2 = true;
                  string str3 = consoleCommand.NextWord();
                  if (str3 == "")
                  {
                    DevConsole._core.lines.Enqueue(new DCLine()
                    {
                      line = "Parameters in wrong format.",
                      color = Color.Red
                    });
                    return;
                  }
                  System.Type type = typeof (Duck);
                  PropertyInfo[] properties = type.GetProperties();
                  bool flag3 = false;
                  foreach (PropertyInfo propertyInfo in properties)
                  {
                    if (propertyInfo.Name.ToLower(currentCulture) == str3)
                    {
                      flag3 = true;
                      if (propertyInfo.PropertyType == typeof (float))
                      {
                        float single;
                        try
                        {
                          single = Change.ToSingle((object) consoleCommand.NextWord());
                        }
                        catch
                        {
                          DevConsole._core.lines.Enqueue(new DCLine()
                          {
                            line = "Parameters in wrong format.",
                            color = Color.Red
                          });
                          return;
                        }
                        if (consoleCommand.NextWord() != "")
                        {
                          DevConsole._core.lines.Enqueue(new DCLine()
                          {
                            line = "Too many parameters!",
                            color = Color.Red
                          });
                          return;
                        }
                        propertyInfo.SetValue((object) profile.duck, (object) single, (object[]) null);
                      }
                      if (propertyInfo.PropertyType == typeof (bool))
                      {
                        bool boolean;
                        try
                        {
                          boolean = Convert.ToBoolean(consoleCommand.NextWord());
                        }
                        catch
                        {
                          DevConsole._core.lines.Enqueue(new DCLine()
                          {
                            line = "Parameters in wrong format.",
                            color = Color.Red
                          });
                          return;
                        }
                        if (consoleCommand.NextWord() != "")
                        {
                          DevConsole._core.lines.Enqueue(new DCLine()
                          {
                            line = "Too many parameters!",
                            color = Color.Red
                          });
                          return;
                        }
                        propertyInfo.SetValue((object) profile.duck, (object) boolean, (object[]) null);
                      }
                      if (propertyInfo.PropertyType == typeof (int))
                      {
                        int int32;
                        try
                        {
                          int32 = Convert.ToInt32(consoleCommand.NextWord());
                        }
                        catch
                        {
                          DevConsole._core.lines.Enqueue(new DCLine()
                          {
                            line = "Parameters in wrong format.",
                            color = Color.Red
                          });
                          return;
                        }
                        if (consoleCommand.NextWord() != "")
                        {
                          DevConsole._core.lines.Enqueue(new DCLine()
                          {
                            line = "Too many parameters!",
                            color = Color.Red
                          });
                          return;
                        }
                        propertyInfo.SetValue((object) profile.duck, (object) int32, (object[]) null);
                      }
                      if (propertyInfo.PropertyType == typeof (Vec2))
                      {
                        float single1;
                        float single2;
                        try
                        {
                          single1 = Change.ToSingle((object) consoleCommand.NextWord());
                          single2 = Change.ToSingle((object) consoleCommand.NextWord());
                        }
                        catch
                        {
                          DevConsole._core.lines.Enqueue(new DCLine()
                          {
                            line = "Parameters in wrong format.",
                            color = Color.Red
                          });
                          return;
                        }
                        if (consoleCommand.NextWord() != "")
                        {
                          DevConsole._core.lines.Enqueue(new DCLine()
                          {
                            line = "Too many parameters!",
                            color = Color.Red
                          });
                          return;
                        }
                        propertyInfo.SetValue((object) profile.duck, (object) new Vec2(single1, single2), (object[]) null);
                      }
                    }
                  }
                  if (!flag3)
                  {
                    foreach (FieldInfo field in type.GetFields())
                    {
                      if (field.Name.ToLower(currentCulture) == str3)
                      {
                        flag3 = true;
                        if (field.FieldType == typeof (float))
                        {
                          float single;
                          try
                          {
                            single = Change.ToSingle((object) consoleCommand.NextWord());
                          }
                          catch
                          {
                            DevConsole._core.lines.Enqueue(new DCLine()
                            {
                              line = "Parameters in wrong format.",
                              color = Color.Red
                            });
                            return;
                          }
                          if (consoleCommand.NextWord() != "")
                          {
                            DevConsole._core.lines.Enqueue(new DCLine()
                            {
                              line = "Too many parameters!",
                              color = Color.Red
                            });
                            return;
                          }
                          field.SetValue((object) profile.duck, (object) single);
                        }
                        if (field.FieldType == typeof (bool))
                        {
                          bool boolean;
                          try
                          {
                            boolean = Convert.ToBoolean(consoleCommand.NextWord());
                          }
                          catch
                          {
                            DevConsole._core.lines.Enqueue(new DCLine()
                            {
                              line = "Parameters in wrong format.",
                              color = Color.Red
                            });
                            return;
                          }
                          if (consoleCommand.NextWord() != "")
                          {
                            DevConsole._core.lines.Enqueue(new DCLine()
                            {
                              line = "Too many parameters!",
                              color = Color.Red
                            });
                            return;
                          }
                          field.SetValue((object) profile.duck, (object) boolean);
                        }
                        if (field.FieldType == typeof (int))
                        {
                          int int32;
                          try
                          {
                            int32 = Convert.ToInt32(consoleCommand.NextWord());
                          }
                          catch
                          {
                            DevConsole._core.lines.Enqueue(new DCLine()
                            {
                              line = "Parameters in wrong format.",
                              color = Color.Red
                            });
                            return;
                          }
                          if (consoleCommand.NextWord() != "")
                          {
                            DevConsole._core.lines.Enqueue(new DCLine()
                            {
                              line = "Too many parameters!",
                              color = Color.Red
                            });
                            return;
                          }
                          field.SetValue((object) profile.duck, (object) int32);
                        }
                        if (field.FieldType == typeof (Vec2))
                        {
                          float single1;
                          float single2;
                          try
                          {
                            single1 = Change.ToSingle((object) consoleCommand.NextWord());
                            single2 = Change.ToSingle((object) consoleCommand.NextWord());
                          }
                          catch
                          {
                            DevConsole._core.lines.Enqueue(new DCLine()
                            {
                              line = "Parameters in wrong format.",
                              color = Color.Red
                            });
                            return;
                          }
                          if (consoleCommand.NextWord() != "")
                          {
                            DevConsole._core.lines.Enqueue(new DCLine()
                            {
                              line = "Too many parameters!",
                              color = Color.Red
                            });
                            return;
                          }
                          field.SetValue((object) profile.duck, (object) new Vec2(single1, single2));
                        }
                      }
                    }
                    if (!flag3)
                    {
                      DevConsole._core.lines.Enqueue(new DCLine()
                      {
                        line = "Duck has no variable called " + str3 + ".",
                        color = Color.Red
                      });
                      return;
                    }
                  }
                }
                else
                {
                  DevConsole._core.lines.Enqueue(new DCLine()
                  {
                    line = str2 + " is not in the game!",
                    color = Color.Red
                  });
                  return;
                }
              }
            }
            if (!flag2)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "No profile named " + str2 + ".",
                color = Color.Red
              });
              return;
            }
          }
          if (str1 == "kill")
          {
            if (Network.isActive || Level.current is ChallengeLevel)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You can't do that here!",
                color = Color.Red
              });
              return;
            }
            flag1 = true;
            string str2 = consoleCommand.NextWord();
            if (consoleCommand.NextWord() != "")
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "Too many parameters!",
                color = Color.Red
              });
              return;
            }
            bool flag2 = false;
            foreach (Profile profile in Profiles.all)
            {
              if (profile.name.ToLower(currentCulture) == str2)
              {
                if (profile.duck != null)
                {
                  profile.duck.Kill((DestroyType) new DTIncinerate((Thing) null));
                  flag2 = true;
                }
                else
                {
                  DevConsole._core.lines.Enqueue(new DCLine()
                  {
                    line = str2 + " is not in the game!",
                    color = Color.Red
                  });
                  return;
                }
              }
            }
            if (!flag2)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "No profile named " + str2 + ".",
                color = Color.Red
              });
              return;
            }
          }
          if (str1 == "globalscores")
          {
            flag1 = true;
            using (List<Profile>.Enumerator enumerator = Profiles.active.GetEnumerator())
            {
              if (enumerator.MoveNext())
              {
                Profile current = enumerator.Current;
                DevConsole._core.lines.Enqueue(new DCLine()
                {
                  line = current.name + ": " + current.stats.CalculateProfileScore().ToString("0.000"),
                  color = Color.Red
                });
              }
            }
          }
          if (str1 == "scorelog")
          {
            flag1 = true;
            string str2 = consoleCommand.NextWord();
            if (consoleCommand.NextWord() != "")
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "Too many parameters!",
                color = Color.Red
              });
              return;
            }
            if (str2 == "")
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You need to provide a player number.",
                color = Color.Red
              });
              return;
            }
            int int32;
            try
            {
              int32 = Convert.ToInt32(str2);
            }
            catch
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "Parameters in wrong format.",
                color = Color.Red
              });
              return;
            }
            DevConsole._core.logScores = int32;
          }
          if (str1 == "toggle")
          {
            if (Network.isActive || Level.current is ChallengeLevel)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You can't do that here!",
                color = Color.Red
              });
              return;
            }
            flag1 = true;
            string str2 = consoleCommand.NextWord();
            if (consoleCommand.NextWord() != "")
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "Too many parameters!",
                color = Color.Red
              });
              return;
            }
            if (str2 == "")
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You need to provide a layer to toggle.",
                color = Color.Red
              });
              return;
            }
            if (str2 == "background")
              Layer.Background.visible = !Layer.Background.visible;
            else if (str2 == "parallax")
              Layer.Parallax.visible = !Layer.Parallax.visible;
            else if (str2 == "foreground")
              Layer.Foreground.visible = !Layer.Foreground.visible;
            else if (str2 == "game")
              Layer.Game.visible = !Layer.Game.visible;
            else if (str2 == "HUD")
              Layer.HUD.visible = !Layer.HUD.visible;
          }
          if (str1 == "splitscreen")
          {
            if (Network.isActive || Level.current is ChallengeLevel)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You can't do that here!",
                color = Color.Red
              });
              return;
            }
            flag1 = true;
            DevConsole._core.splitScreen = !DevConsole._core.splitScreen;
          }
          if (str1 == "clearmainprofile")
          {
            if (Network.isActive || Level.current is ChallengeLevel)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You can't do that here!",
                color = Color.Red
              });
              return;
            }
            DevConsole._core.lines.Enqueue(new DCLine()
            {
              line = "Your main account has been R U I N E D !",
              color = Color.Red
            });
            ulong steamId = Profiles.experienceProfile.steamID;
            string varName = steamId.ToString();
            steamId = Profiles.experienceProfile.steamID;
            string varID = steamId.ToString();
            Profile p = new Profile(varName, varID: varID);
            p.steamID = Profiles.experienceProfile.steamID;
            Profiles.Remove(Profiles.experienceProfile);
            Profiles.Add(p);
            flag1 = true;
          }
          if (str1 == "rhythmmode")
          {
            if (Network.isActive || Level.current is ChallengeLevel)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You can't do that here!",
                color = Color.Red
              });
              return;
            }
            flag1 = true;
            if (!DevConsole._core.rhythmMode)
              Music.Stop();
            DevConsole._core.rhythmMode = !DevConsole._core.rhythmMode;
            if (DevConsole._core.rhythmMode)
              Music.Play(Music.RandomTrack("InGame"));
          }
          if (str1 == "fancymode")
          {
            if (Network.isActive || Level.current is ChallengeLevel)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You can't do that here!",
                color = Color.Red
              });
              return;
            }
            DevConsole.fancyMode = !DevConsole.fancyMode;
            flag1 = true;
          }
          if (str1 == "shieldmode")
          {
            if (Network.isActive || Level.current is ChallengeLevel)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You can't do that here!",
                color = Color.Red
              });
              return;
            }
            DevConsole.shieldMode = !DevConsole.shieldMode;
            flag1 = true;
          }
          if (str1 == "qwopmode")
          {
            if (Network.isActive || Level.current is ChallengeLevel)
            {
              DevConsole._core.lines.Enqueue(new DCLine()
              {
                line = "You can't do that here!",
                color = Color.Red
              });
              return;
            }
            flag1 = true;
            DevConsole._core.qwopMode = !DevConsole._core.qwopMode;
          }
          if (str1 == "showislands")
          {
            flag1 = true;
            DevConsole._core.showIslands = !DevConsole._core.showIslands;
          }
          if (str1 == "showcollision")
          {
            flag1 = true;
            DevConsole._core.showCollision = !DevConsole._core.showCollision;
          }
          if (flag1)
            DevConsole._core.lines.Enqueue(new DCLine()
            {
              line = command,
              color = Color.White
            });
          else
            DevConsole._core.lines.Enqueue(new DCLine()
            {
              line = str1 + " is not a valid command!",
              color = Color.Red
            });
        }
      }
    }

    public static void Log(string text, Color c, float scale = 2f, int index = -1)
    {
      DCLine dcLine = new DCLine()
      {
        line = text,
        color = c,
        scale = scale,
        threadIndex = index < 0 ? NetworkDebugger.networkDrawingIndex : index,
        timestamp = DateTime.Now
      };
      if (NetworkDebugger.enabled)
      {
        lock (DevConsole.debuggerLines)
          DevConsole.debuggerLines.Add(dcLine);
      }
      else
      {
        lock (DevConsole._core.pendingLines)
          DevConsole._core.pendingLines.Add(dcLine);
      }
    }

    public static void Log(DCSection section, string text, int netIndex = -1) => DevConsole.Log(section, Verbosity.Normal, text, netIndex);

    public static void Log(DCSection section, Verbosity verbose, string text, int netIndex = -1)
    {
      DCLine dcLine = new DCLine()
      {
        line = text,
        section = section,
        verbosity = verbose,
        color = Color.White,
        scale = 2f,
        threadIndex = netIndex < 0 ? NetworkDebugger.networkDrawingIndex : netIndex,
        timestamp = DateTime.Now
      };
      if (NetworkDebugger.enabled)
      {
        lock (DevConsole.debuggerLines)
          DevConsole.debuggerLines.Add(dcLine);
      }
      else
      {
        lock (DevConsole._core.pendingLines)
          DevConsole._core.pendingLines.Add(dcLine);
      }
    }

    public static void Chart(string chart, string section, double x, double y, Color c)
    {
      lock (DevConsole._core.pendingChartValues)
        DevConsole._core.pendingChartValues.Add(new DCChartValue()
        {
          chart = chart,
          section = section,
          x = x,
          y = y,
          color = c,
          threadIndex = NetworkDebugger.networkDrawingIndex
        });
    }

    public static void UpdateGraph(int index, NetGraph target)
    {
    }

    public static void Update()
    {
      if (DevConsole._core.logScores >= 0)
      {
        DevConsole._core.lines.Clear();
        int num = 0;
        foreach (Profile profile in Profiles.active)
        {
          if (num == DevConsole._core.logScores)
          {
            float profileScore1 = profile.endOfRoundStats.CalculateProfileScore();
            float profileScore2 = (float) profile.endOfRoundStats.GetProfileScore();
            DevConsole._core.pendingLines.Clear();
            DevConsole.Log(profile.name + ": " + profileScore1.ToString("0.000") + "   Cool: " + profileScore2.ToString(), Color.Green);
            double profileScore3 = (double) profile.endOfRoundStats.CalculateProfileScore(true);
          }
          ++num;
        }
      }
      lock (DevConsole._core.pendingLines)
      {
        List<DCLine> pendingLines = DevConsole._core.pendingLines;
        DevConsole._core.pendingLines = new List<DCLine>();
        foreach (DCLine dcLine in pendingLines)
          DevConsole._core.lines.Enqueue(dcLine);
      }
      if (Keyboard.Pressed(Keys.OemTilde) && !DuckNetwork.core.enteringText)
      {
        if (DevConsole._tray == null)
        {
          DevConsole._tray = new Sprite("devTray");
          DevConsole._scan = new Sprite("devScan");
        }
        DevConsole._core.open = !DevConsole._core.open;
        if (DevConsole._core.open)
          Keyboard.keyString = DevConsole._core.typing;
      }
      DevConsole._core.alpha = Maths.LerpTowards(DevConsole._core.alpha, DevConsole._core.open ? 1f : 0.0f, 0.05f);
      if (!DevConsole._core.open)
        return;
      DevConsole._core.typing = Keyboard.keyString;
      if (Keyboard.Pressed(Keys.Enter))
      {
        DevConsole.RunCommand(DevConsole._core.typing);
        DevConsole._core.lastLine = DevConsole._core.typing;
        Keyboard.keyString = "";
      }
      if (!Keyboard.Pressed(Keys.Up))
        return;
      Keyboard.keyString = DevConsole._core.lastLine;
    }
  }
}
