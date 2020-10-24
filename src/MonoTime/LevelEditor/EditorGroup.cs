// Decompiled with JetBrains decompiler
// Type: DuckGame.EditorGroup
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace DuckGame
{
  public class EditorGroup
  {
    private List<Thing> _things = new List<Thing>();
    private List<Thing> _filteredThings = new List<Thing>();
    private List<Thing> _onlineFilteredThings = new List<Thing>();
    private List<System.Type> _types = new List<System.Type>();
    public List<EditorGroup> SubGroups = new List<EditorGroup>();
    public string Name = "";

    public List<Thing> Things
    {
      get
      {
        if (Main.isDemo)
          return this._filteredThings;
        return Editor.onlineMode ? this._onlineFilteredThings : this._things;
      }
    }

    public List<Thing> AllThings => this._things;

    public List<System.Type> AllTypes => this._types;

    public EditorGroup()
    {
    }

    public EditorGroup(System.Type filter = null, HashSet<System.Type> types = null) => this.Initialize(filter, types);

    public bool Contains(System.Type t)
    {
      if (this._types.Contains(t))
        return true;
      foreach (EditorGroup subGroup in this.SubGroups)
      {
        if (subGroup.Contains(t))
          return true;
      }
      return false;
    }

    private void AddType(System.Type t, string group)
    {
      if (MonoMain.lockLoading)
      {
        MonoMain.loadingLocked = true;
        while (MonoMain.lockLoading)
          Thread.Sleep(10);
      }
      MonoMain.loadingLocked = false;
      if (group == "survival")
        return;
      if (group == "")
      {
        Main.SpecialCode = "creating " + t.AssemblyQualifiedName;
        Thing typeInstance = Editor.GetOrCreateTypeInstance(t);
        Main.SpecialCode = "accessing " + t.AssemblyQualifiedName;
        IReadOnlyPropertyBag bag = ContentProperties.GetBag(t);
        if (bag.GetOrDefault<bool>("isInDemo", false))
          this._filteredThings.Add(typeInstance);
        if (bag.GetOrDefault<bool>("isOnlineCapable", true))
          this._onlineFilteredThings.Add(typeInstance);
        this._things.Add(typeInstance);
        this._types.Add(t);
        Editor.MapThing(typeInstance);
        ++MonoMain.lazyLoadyBits;
        Main.SpecialCode = "finished " + t.AssemblyQualifiedName;
      }
      else
      {
        string[] groupName = group.Split('|');
        EditorGroup editorGroup = this.SubGroups.FirstOrDefault<EditorGroup>((Func<EditorGroup, bool>) (x => x.Name == groupName[0]));
        if (editorGroup == null)
        {
          editorGroup = new EditorGroup();
          editorGroup.Name = groupName[0];
          this.SubGroups.Add(editorGroup);
        }
        string str = group;
        string group1 = ((IEnumerable<string>) groupName).Count<string>() <= 1 ? str.Remove(0, groupName[0].Length) : str.Remove(0, groupName[0].Length + 1);
        editorGroup.AddType(t, group1);
      }
    }

    private void Sort()
    {
      this.SubGroups.Sort((Comparison<EditorGroup>) ((x, y) => string.Compare(x.Name, y.Name)));
      this.Things.Sort((Comparison<Thing>) ((x, y) => string.Compare(x.editorName, y.editorName)));
      foreach (EditorGroup subGroup in this.SubGroups)
        subGroup.Sort();
    }

    private void Initialize(System.Type filter = null, HashSet<System.Type> types = null)
    {
      List<System.Type> typeList = new List<System.Type>();
      if (types == null)
        typeList.AddRange((IEnumerable<System.Type>) Editor.ThingTypes);
      else
        typeList.AddRange((IEnumerable<System.Type>) types);
      for (int index = 0; index < typeList.Count; ++index)
      {
        if (MonoMain.lockLoading)
        {
          MonoMain.loadingLocked = true;
          while (MonoMain.lockLoading)
            Thread.Sleep(10);
        }
        MonoMain.loadingLocked = false;
        System.Type type = typeList[index];
        if (!(filter != (System.Type) null) || !(type != filter) || Editor.AllBaseTypes[type].Contains(filter))
        {
          object[] customAttributes = type.GetCustomAttributes(typeof (EditorGroupAttribute), false);
          if (customAttributes.Length != 0)
          {
            string editorGroup = (customAttributes[0] as EditorGroupAttribute).editorGroup;
            this.AddType(type, editorGroup);
          }
        }
      }
      this.Sort();
    }
  }
}
