// Decompiled with JetBrains decompiler
// Type: DuckGame.EditorGroupMenu
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections;
using System.Collections.Generic;

namespace DuckGame
{
  public class EditorGroupMenu : ContextMenu
  {
    protected bool willOnlineGrayout = true;
    private static int deep;

    public EditorGroupMenu(IContextListener owner, bool root = false, SpriteMap image = null)
      : base(owner, image)
    {
      this.itemSize.x = 140f;
      this.itemSize.y = 16f;
      this._root = root;
      if (this._root)
        return;
      this.greyOut = Main.isDemo || Editor.onlineMode;
    }

    public void UpdateGrayout()
    {
      this.greyOut = false;
      if (Editor.onlineMode && this.willOnlineGrayout)
        this.greyOut = true;
      foreach (ContextMenu contextMenu in this._items)
      {
        if (contextMenu is EditorGroupMenu)
          (contextMenu as EditorGroupMenu).UpdateGrayout();
        else if (contextMenu.contextThing != null)
        {
          contextMenu.greyOut = false;
          IReadOnlyPropertyBag bag = ContentProperties.GetBag(contextMenu.contextThing.GetType());
          if (Editor.onlineMode && !bag.GetOrDefault<bool>("isOnlineCapable", true))
            contextMenu.greyOut = true;
        }
      }
    }

    public void InitializeGroups(
      EditorGroup group,
      FieldBinding radioBinding = null,
      EditorGroup scriptingGroup = null)
    {
      ++EditorGroupMenu.deep;
      this._text = group.Name;
      this.itemSize.x = (float) (this._text.Length * 8 + 16);
      foreach (EditorGroup subGroup in group.SubGroups)
      {
        EditorGroupMenu editorGroupMenu = new EditorGroupMenu((IContextListener) this);
        editorGroupMenu.fancy = this.fancy;
        editorGroupMenu.InitializeGroups(subGroup, radioBinding);
        if (!editorGroupMenu.greyOut)
          this.greyOut = false;
        if (!editorGroupMenu.willOnlineGrayout)
          this.willOnlineGrayout = false;
        this.AddItem((ContextMenu) editorGroupMenu);
      }
      if (scriptingGroup != null)
      {
        EditorGroupMenu editorGroupMenu = new EditorGroupMenu((IContextListener) this);
        editorGroupMenu.InitializeGroups(scriptingGroup, radioBinding);
        if (!editorGroupMenu.greyOut)
          this.greyOut = false;
        if (!editorGroupMenu.willOnlineGrayout)
          this.willOnlineGrayout = false;
        this.AddItem((ContextMenu) editorGroupMenu);
      }
      foreach (Thing allThing in group.AllThings)
      {
        IReadOnlyPropertyBag bag = ContentProperties.GetBag(allThing.GetType());
        if (Main.isDemo && bag.GetOrDefault<bool>("isInDemo", false))
          this.greyOut = false;
        if (bag.GetOrDefault<bool>("isOnlineCapable", true))
        {
          this.greyOut = false;
          this.willOnlineGrayout = false;
        }
        switch (allThing)
        {
          case BackgroundTile _:
          case ForegroundTile _:
          case SubBackgroundTile _:
            ContextBackgroundTile contextBackgroundTile = new ContextBackgroundTile(allThing, (IContextListener) this);
            contextBackgroundTile.contextThing = allThing;
            this.AddItem((ContextMenu) contextBackgroundTile);
            continue;
          default:
            if (radioBinding != null)
            {
              if (radioBinding.value is IList)
              {
                if (radioBinding.value is List<TypeProbPair>)
                {
                  ContextSlider contextSlider = new ContextSlider(allThing.editorName, (IContextListener) this, radioBinding, 0.05f, myType: allThing.GetType());
                  contextSlider.greyOut = Main.isDemo && !bag.GetOrDefault<bool>("isInDemo", false);
                  contextSlider.contextThing = allThing;
                  if (!bag.Contains("isOnlineCapable") || bag.Get<bool>("isOnlineCapable"))
                    this.willOnlineGrayout = false;
                  this.AddItem((ContextMenu) contextSlider);
                  continue;
                }
                ContextCheckBox contextCheckBox = new ContextCheckBox(allThing.editorName, (IContextListener) this, radioBinding, allThing.GetType());
                contextCheckBox.greyOut = Main.isDemo && !bag.GetOrDefault<bool>("isInDemo", false);
                contextCheckBox.contextThing = allThing;
                if (!bag.Contains("isOnlineCapable") || bag.Get<bool>("isOnlineCapable"))
                  this.willOnlineGrayout = false;
                this.AddItem((ContextMenu) contextCheckBox);
                continue;
              }
              ContextRadio contextRadio = new ContextRadio(allThing.editorName, false, (object) allThing.GetType(), (IContextListener) this, radioBinding);
              contextRadio.greyOut = Main.isDemo && !bag.GetOrDefault<bool>("isInDemo", false);
              contextRadio.contextThing = allThing;
              if (!bag.Contains("isOnlineCapable") || bag.Get<bool>("isOnlineCapable"))
                this.willOnlineGrayout = false;
              this.AddItem((ContextMenu) contextRadio);
              continue;
            }
            ContextObject contextObject = new ContextObject(allThing, (IContextListener) this);
            contextObject.contextThing = allThing;
            this.AddItem((ContextMenu) contextObject);
            continue;
        }
      }
      --EditorGroupMenu.deep;
      if (EditorGroupMenu.deep != 0)
        return;
      this.UpdateGrayout();
    }

    public void InitializeTeams(FieldBinding radioBinding)
    {
      this.AddItem((ContextMenu) new ContextRadio("None", false, (object) 0, (IContextListener) this, radioBinding));
      EditorGroupMenu editorGroupMenu = (EditorGroupMenu) null;
      int num1 = 0;
      if (Teams.all.Count > 10)
      {
        editorGroupMenu = new EditorGroupMenu((IContextListener) this);
        editorGroupMenu.text = "Hats " + (object) num1;
      }
      int num2 = 0;
      for (int index = 0; index < Teams.all.Count; ++index)
      {
        if (index >= 4)
        {
          ContextRadio contextRadio = new ContextRadio(Teams.all[index].name, false, (object) index, (IContextListener) this, radioBinding);
          if (editorGroupMenu != null)
            editorGroupMenu.AddItem((ContextMenu) contextRadio);
          else
            this.AddItem((ContextMenu) contextRadio);
          ++num2;
          if (num2 == 10)
          {
            ++num1;
            num2 = 0;
            this.AddItem((ContextMenu) editorGroupMenu);
            editorGroupMenu = new EditorGroupMenu((IContextListener) this);
            editorGroupMenu.text = "Hats " + (object) num1;
          }
        }
      }
      if (editorGroupMenu == null || num2 <= 0)
        return;
      this.AddItem((ContextMenu) editorGroupMenu);
    }
  }
}
