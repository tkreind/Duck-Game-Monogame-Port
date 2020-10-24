// Decompiled with JetBrains decompiler
// Type: DuckGame.VirtualTransition
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class VirtualTransition
  {
    private static VirtualTransitionCore _core = new VirtualTransitionCore();

    public static VirtualTransitionCore core
    {
      get => VirtualTransition._core;
      set => VirtualTransition._core = value;
    }

    public static void Initialize() => VirtualTransition._core.Initialize();

    public static void Update() => VirtualTransition._core.Update();

    public static void Draw() => VirtualTransition._core.Draw();

    public static bool doingVirtualTransition => VirtualTransition._core.doingVirtualTransition;

    public static bool isVirtual => VirtualTransition._core._virtualMode;

    public static void GoVirtual() => VirtualTransition._core.GoVirtual();

    public static void GoUnVirtual() => VirtualTransition._core.GoUnVirtual();

    public static bool active => VirtualTransition._core.active;
  }
}
