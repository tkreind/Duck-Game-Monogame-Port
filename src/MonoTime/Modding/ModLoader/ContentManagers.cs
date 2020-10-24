// Decompiled with JetBrains decompiler
// Type: DuckGame.ContentManagers
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  /// <summary>The class that stores content managers.</summary>
  public static class ContentManagers
  {
    private static Dictionary<System.Type, IManageContent> _contentManagers = new Dictionary<System.Type, IManageContent>();

    private static IManageContent AddContentManager(System.Type t)
    {
      IManageContent instance = (IManageContent) Activator.CreateInstance(t);
      ContentManagers._contentManagers.Add(t, instance);
      return instance;
    }

    internal static IManageContent GetContentManager(System.Type t)
    {
      if (t == (System.Type) null)
        t = typeof (DefaultContentManager);
      IManageContent manageContent;
      return ContentManagers._contentManagers.TryGetValue(t, out manageContent) ? manageContent : ContentManagers.AddContentManager(t);
    }
  }
}
