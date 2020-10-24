// Decompiled with JetBrains decompiler
// Type: DuckGame.CustomPlatform
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("blocks|custom")]
  [BaggedProperty("isInDemo", false)]
  public class CustomPlatform : AutoPlatform
  {
    private static CustomType _customType = CustomType.Platform;
    public int customIndex;
    private string _currentTileset = "";

    public static string customPlatform01
    {
      get => Custom.data[CustomPlatform._customType][0];
      set => Custom.data[CustomPlatform._customType][0] = value;
    }

    public CustomPlatform(float x, float y, string t = "CUSTOMPLAT01")
      : base(x, y, "")
    {
      this._tileset = t;
      this.customIndex = 0;
      this._editorName = "Custom Platform 01";
      this.physicsMaterial = PhysicsMaterial.Metal;
      this.verticalWidth = 14f;
      this.verticalWidthThick = 15f;
      this.horizontalHeight = 8f;
      this.UpdateCurrentTileset();
    }

    public void UpdateCurrentTileset()
    {
      CustomTileData data = Custom.GetData(this.customIndex, CustomPlatform._customType);
      int num = 0;
      if (this._sprite != null)
        num = this._sprite.frame;
      if (data != null && data.texture != null)
      {
        this._sprite = new SpriteMap((Tex2D) data.texture, 16, 16);
        this.horizontalHeight = (float) data.horizontalHeight;
        this.verticalWidth = (float) data.verticalWidth;
        this.verticalWidthThick = (float) data.verticalWidthThick;
        this._hasLeftNub = data.leftNubber;
        this._hasRightNub = data.rightNubber;
      }
      else
      {
        this._sprite = new SpriteMap("scaffolding", 16, 16);
        this.verticalWidth = 14f;
        this.verticalWidthThick = 15f;
        this.horizontalHeight = 8f;
      }
      if ((double) this.horizontalHeight == 0.0)
        this.horizontalHeight = 8f;
      if ((double) this.verticalWidth == 0.0)
        this.verticalWidth = 14f;
      if ((double) this.verticalWidthThick == 0.0)
        this.verticalWidthThick = 15f;
      this._sprite.frame = num;
      this._tileset = "CUSTOMPLAT0" + (object) (this.customIndex + 1);
      this._currentTileset = Custom.data[CustomPlatform._customType][this.customIndex];
      this.graphic = (Sprite) this._sprite;
      this.UpdateNubbers();
    }

    public override void Update() => base.Update();

    public override void EditorUpdate()
    {
      if (!(Level.current is Editor) || !(this._currentTileset != Custom.data[CustomPlatform._customType][this.customIndex]))
        return;
      this.UpdateCurrentTileset();
    }

    public override ContextMenu GetContextMenu()
    {
      EditorGroupMenu editorGroupMenu = new EditorGroupMenu((IContextListener) null, true);
      editorGroupMenu.AddItem((ContextMenu) new ContextFile("style", (IContextListener) null, new FieldBinding((object) this, "customPlatform0" + (object) (this.customIndex + 1)), ContextFileType.Platform));
      return (ContextMenu) editorGroupMenu;
    }
  }
}
