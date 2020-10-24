// Decompiled with JetBrains decompiler
// Type: DuckGame.SteamUploadDialog
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DuckGame
{
  public class SteamUploadDialog : ContextMenu
  {
    private FancyBitmapFont _font;
    private Textbox _descriptionBox;
    private Textbox _nameBox;
    private MessageDialogue _confirm;
    private NotifyDialogue _notify;
    private UploadDialogue _upload;
    private DeathmatchTestDialogue _deathmatchTest;
    private TestSuccessDialogue _testSuccess;
    private ArcadeTestDialogue _arcadeTest;
    private Sprite _previewTarget;
    private string _filePath;
    private ulong _workshopID;
    private bool _addDeathmatchTag;
    private bool _addMachineTag;
    private SpriteMap _workshopTag;
    private Sprite _workshopTagMiddle;
    private Sprite _tagPlus;
    private LevelSize _levelSize;
    private LevelType _levelType;
    private string[] _extraTags;
    private int _arcadeTestIndex;
    private List<string> _possibleTags = new List<string>()
    {
      "Dumb",
      "Fast",
      "Luck",
      "Weird",
      "Fire",
      "Pro"
    };
    private Vec2 _acceptPos;
    private Vec2 _acceptSize;
    private bool _acceptHover;
    private Vec2 _cancelPos;
    private Vec2 _cancelSize;
    private bool _cancelHover;
    private WorkshopItem _currentItem;
    private bool _uploading;
    private bool _doingPublish;
    private bool _testing;
    private Vec2 _plusPosition;
    private ContextMenu _tagMenu;
    private Dictionary<string, Vec2> tagPositions = new Dictionary<string, Vec2>();
    private float hOffset;
    private float _fdHeight = 262f;
    public bool drag;

    public SteamUploadDialog()
      : base((IContextListener) null)
    {
    }

    public override void Initialize()
    {
      this.layer = Layer.HUD;
      this.depth = (Depth) 0.9f;
      this._showBackground = false;
      this.itemSize = new Vec2(386f, 16f);
      this._root = true;
      this.drawControls = false;
      this._descriptionBox = new Textbox(this.x + 5f, this.y + 225f, 316f, 40f, 0.5f, 9, "<ENTER DESCRIPTION>");
      this._nameBox = new Textbox(this.x + 5f, this.y + (float) byte.MaxValue, 316f, 12f, maxLines: 1, emptyText: "<ENTER NAME>");
      this._font = new FancyBitmapFont("smallFont");
      this._confirm = new MessageDialogue();
      Level.Add((Thing) this._confirm);
      this._upload = new UploadDialogue();
      Level.Add((Thing) this._upload);
      this._notify = new NotifyDialogue();
      Level.Add((Thing) this._notify);
      this._deathmatchTest = new DeathmatchTestDialogue();
      Level.Add((Thing) this._deathmatchTest);
      this._testSuccess = new TestSuccessDialogue();
      Level.Add((Thing) this._testSuccess);
      this._arcadeTest = new ArcadeTestDialogue();
      Level.Add((Thing) this._arcadeTest);
    }

    public void Open(
      RenderTarget2D preview,
      string filePath,
      ulong workshopID = 0,
      LevelType levType = LevelType.Deathmatch,
      LevelSize levSize = LevelSize.Ginormous,
      string[] extraTags = null)
    {
      this._currentItem = (WorkshopItem) null;
      Texture2D texture2D = new Texture2D(DuckGame.Graphics.device, preview.width, preview.height);
      Color[] data = new Color[preview.width * preview.height];
      preview.GetData<Color>(data);
      texture2D.SetData<Color>(data);
      this._levelType = levType;
      this._levelSize = levSize;
      this._addDeathmatchTag = false;
      this._addMachineTag = false;
      Editor.lockInput = (ContextMenu) this;
      SFX.Play("openClick", 0.4f);
      this.opened = true;
      this._previewTarget = new Sprite((Tex2D) texture2D);
      this._filePath = filePath;
      this._workshopID = workshopID;
      if (this._workshopID != 0UL)
        this._currentItem = new WorkshopItem(workshopID);
      if (Editor.workshopName == null || Editor.workshopName == "")
        Editor.workshopName = Path.GetFileNameWithoutExtension(filePath);
      this._nameBox.text = Editor.workshopName;
      this._descriptionBox.text = Editor.workshopDescription;
      this._workshopTag = new SpriteMap("workshopTag", 4, 8);
      this._workshopTagMiddle = new Sprite("workshopTagMiddle");
      this._tagPlus = new Sprite("tagPlus");
      this._extraTags = extraTags;
      this._arcadeTestIndex = 0;
    }

    public void Close()
    {
      Editor.lockInput = (ContextMenu) null;
      this.opened = false;
      this._descriptionBox.LoseFocus();
      this._nameBox.LoseFocus();
      this._currentItem = (WorkshopItem) null;
      this.ClearItems();
    }

    public override void Selected(ContextMenu item)
    {
      SFX.Play("highClick", 0.3f);
      if (item != null && item.text != "")
        Editor.workshopTags.Add(item.text);
      if (this._tagMenu == null)
        return;
      this._tagMenu.opened = false;
      Level.Remove((Thing) this._tagMenu);
      this._tagMenu = (ContextMenu) null;
      if (Editor.PeekFocus() != this._tagMenu)
        return;
      Editor.PopFocus();
    }

    public override void Toggle(ContextMenu item)
    {
    }

    public override void Update()
    {
      if (this._doingPublish && this._currentItem != null)
      {
        if (!this._currentItem.finishedProcessing)
          return;
        if (this._uploading)
        {
          this._uploading = false;
          if (this._currentItem.needsLegal)
            Steam.ShowWorkshopLegalAgreement("312530");
          this._upload.Close();
          if (this._currentItem.result == SteamResult.OK)
            this._notify.Open("Item published!");
          else
            this._notify.Open("Failed (" + this._currentItem.result.ToString() + ")");
          this._doingPublish = false;
        }
        else if (this._currentItem.result == SteamResult.OK)
        {
          this._uploading = true;
          WorkshopItemData dat = new WorkshopItemData();
          dat.name = this._nameBox.text;
          dat.description = this._descriptionBox.text;
          dat.visibility = RemoteStoragePublishedFileVisibility.Public;
          dat.tags = new List<string>();
          if (this._levelType == LevelType.ArcadeMachine)
            dat.tags.Add("Machine");
          else
            dat.tags.Add("Map");
          dat.tags.Add(this._levelSize.ToString());
          if (this._levelType != LevelType.Deathmatch)
            dat.tags.Add(this._levelType.ToString());
          Editor.workshopName = dat.name;
          Editor.workshopDescription = dat.description;
          Editor.workshopAuthor = Steam.user.name;
          Editor.workshopID = this._currentItem.id;
          if (this._addDeathmatchTag)
          {
            dat.tags.Add("Deathmatch");
            Editor.workshopLevelDeathmatchReady = true;
          }
          else if (this._addMachineTag)
          {
            dat.tags.Add("Arcade Machine");
            Editor.workshopLevelDeathmatchReady = true;
          }
          else if (this._levelType == LevelType.Deathmatch)
            dat.tags.Add("Strange");
          dat.tags.AddRange((IEnumerable<string>) Editor.workshopTags);
          if (this._extraTags != null && ((IEnumerable<string>) this._extraTags).Count<string>() > 0)
            dat.tags.AddRange((IEnumerable<string>) this._extraTags);
          (Level.current as Editor).Save();
          this._workshopID = this._currentItem.id;
          string pathString1 = DuckFile.workshopDirectory + (object) this._workshopID + "/";
          string pathString2 = DuckFile.workshopDirectory + (object) this._workshopID + "-preview/";
          DuckFile.CreatePath(pathString1);
          DuckFile.CreatePath(pathString2);
          string withoutExtension = Path.GetFileNameWithoutExtension(this._filePath);
          string str = pathString1 + Path.GetFileName(this._filePath);
          if (File.Exists(str))
            File.Delete(str);
          File.Copy(this._filePath, str);
          File.SetAttributes(this._filePath, FileAttributes.Normal);
          dat.contentFolder = pathString1;
          string path = pathString2 + withoutExtension + ".png";
          if (File.Exists(path))
            File.Delete(path);
          Stream stream = (Stream) DuckFile.Create(path);
          ((Texture2D) this._previewTarget.texture.nativeObject).SaveAsPng(stream, this._previewTarget.width, this._previewTarget.height);
          stream.Dispose();
          dat.previewPath = path;
          this._currentItem.ApplyWorkshopData(dat);
          if (this._currentItem.needsLegal)
            Steam.ShowWorkshopLegalAgreement("312530");
          this._upload.Open("Uploading...", this._currentItem);
        }
        else
        {
          this._notify.Open("Failed (" + this._currentItem.result.ToString() + ")");
          this._doingPublish = false;
        }
        this._currentItem.ResetProcessing();
      }
      else if (!this.opened || this._opening || (this._confirm.opened || this._upload.opened) || (this._deathmatchTest.opened || this._arcadeTest.opened || this._testSuccess.opened))
      {
        if (this.opened)
          Keyboard.keyString = "";
        this._opening = false;
        foreach (ContextMenu contextMenu in this._items)
          contextMenu.disabled = true;
      }
      else if (this._confirm.result)
      {
        if (this._levelType == LevelType.ArcadeMachine)
          this._arcadeTest.Open("This machine can automatically show up in generated arcades, if you pass this validity test. You need to get the Developer trophy on all 3 challenges (oh boy)!");
        else
          this._deathmatchTest.Open("In order to upload this map as a deathmatch level, all ducks need to be able to be eliminated. Do you want to launch the map and show that the map is functional? You don't have to do this, but the map won't show up with the DEATHMATCH tag without completing this test. If this is a challenge map, then don't worry about it!");
        this._confirm.result = false;
      }
      else if (this._testing)
      {
        Keyboard.keyString = "";
        if (DeathmatchTestDialogue.success)
        {
          this._testSuccess.Open("Test success! The level can now be published as a deathmatch level!");
          this._addDeathmatchTag = true;
        }
        else if (ArcadeTestDialogue.success)
        {
          if (this._arcadeTestIndex != 2)
          {
            ++this._arcadeTestIndex;
            ArcadeTestDialogue.success = false;
            ArcadeTestDialogue.currentEditor = Level.current as Editor;
            Level.current = this._arcadeTestIndex != 0 ? (this._arcadeTestIndex != 1 ? (Level) new ChallengeLevel(((Level.current as Editor).levelThings[0] as ArcadeMachine).challenge03Data, true) : (Level) new ChallengeLevel(((Level.current as Editor).levelThings[0] as ArcadeMachine).challenge02Data, true)) : (Level) new ChallengeLevel(((Level.current as Editor).levelThings[0] as ArcadeMachine).challenge01Data, true);
            this._testing = true;
            return;
          }
          this._testSuccess.Open("Test success! The arcade machine can now be published to the workshop!");
          this._addMachineTag = true;
        }
        else if (DeathmatchTestDialogue.tooSlow)
        {
          this._notify.Open("Framerate too low!");
        }
        else
        {
          this._notify.Open("Testing failed.");
          this._arcadeTestIndex = 0;
        }
        DeathmatchTestDialogue.success = false;
        ArcadeTestDialogue.success = false;
        this._testing = false;
      }
      else if (this._testSuccess.result)
      {
        this._doingPublish = true;
        if (this._currentItem == null)
          this._currentItem = Steam.CreateItem();
        else
          this._currentItem.SkipProcessing();
        this._testSuccess.result = false;
      }
      else if (this._deathmatchTest.result != -1)
      {
        if (this._deathmatchTest.result == 1)
        {
          this._doingPublish = true;
          if (this._currentItem == null)
            this._currentItem = Steam.CreateItem();
          else
            this._currentItem.SkipProcessing();
        }
        else if (this._deathmatchTest.result == 0)
        {
          DeathmatchTestDialogue.success = false;
          DeathmatchTestDialogue.currentEditor = Level.current as Editor;
          Level.current = (Level) new GameLevel(this._filePath, validityTest: true);
          this._testing = true;
        }
        this._deathmatchTest.result = -1;
      }
      else if (this._arcadeTest.result != -1)
      {
        if (this._arcadeTest.result == 1)
        {
          this._doingPublish = true;
          if (this._currentItem == null)
            this._currentItem = Steam.CreateItem();
          else
            this._currentItem.SkipProcessing();
        }
        else if (this._arcadeTest.result == 0)
        {
          ArcadeTestDialogue.success = false;
          ArcadeTestDialogue.currentEditor = Level.current as Editor;
          Level.current = (Level) new ChallengeLevel(((Level.current as Editor).levelThings[0] as ArcadeMachine).challenge01Data, true);
          this._testing = true;
        }
        this._arcadeTest.result = -1;
      }
      else
      {
        if (this._tagMenu != null)
          return;
        Vec2 vec2 = new Vec2((float) ((double) this.layer.width / 2.0 - (double) this.width / 2.0) + this.hOffset, (float) ((double) this.layer.height / 2.0 - (double) this.height / 2.0 - 15.0)) + new Vec2(7f, 276f);
        foreach (KeyValuePair<string, Vec2> tagPosition in this.tagPositions)
        {
          if ((double) Mouse.x > (double) tagPosition.Value.x && (double) Mouse.x < (double) tagPosition.Value.x + 8.0 && ((double) Mouse.y > (double) tagPosition.Value.y && (double) Mouse.y < (double) tagPosition.Value.y + 8.0) && Mouse.left == InputState.Pressed)
          {
            Editor.workshopTags.Remove(tagPosition.Key);
            return;
          }
        }
        if (this.tagPositions.Count != this._possibleTags.Count)
        {
          bool flag = false;
          if ((double) Mouse.x > (double) this._plusPosition.x && (double) Mouse.x < (double) this._plusPosition.x + 8.0 && ((double) Mouse.y > (double) this._plusPosition.y && (double) Mouse.y < (double) this._plusPosition.y + 8.0))
            flag = true;
          if (flag && Mouse.left == InputState.Pressed)
          {
            ContextMenu contextMenu = new ContextMenu((IContextListener) this);
            contextMenu.x = this._plusPosition.x;
            contextMenu.y = this._plusPosition.y;
            contextMenu.root = true;
            contextMenu.depth = this.depth + 20;
            int num = 0;
            foreach (string possibleTag in this._possibleTags)
            {
              if (!Editor.workshopTags.Contains(possibleTag))
              {
                contextMenu.AddItem(new ContextMenu((IContextListener) this)
                {
                  itemSize = {
                    x = 40f
                  },
                  text = possibleTag
                });
                ++num;
              }
            }
            contextMenu.y -= (float) (num * 16 + 10);
            Level.Add((Thing) contextMenu);
            contextMenu.opened = true;
            contextMenu.closeOnRight = true;
            this._tagMenu = contextMenu;
            return;
          }
        }
        Editor.lockInput = (ContextMenu) this;
        this._descriptionBox.Update();
        this._nameBox.Update();
        this._acceptHover = false;
        this._cancelHover = false;
        if ((double) Mouse.x > (double) this._acceptPos.x && (double) Mouse.x < (double) this._acceptPos.x + (double) this._acceptSize.x && ((double) Mouse.y > (double) this._acceptPos.y && (double) Mouse.y < (double) this._acceptPos.y + (double) this._acceptSize.y))
          this._acceptHover = true;
        if ((double) Mouse.x > (double) this._cancelPos.x && (double) Mouse.x < (double) this._cancelPos.x + (double) this._cancelSize.x && ((double) Mouse.y > (double) this._cancelPos.y && (double) Mouse.y < (double) this._cancelPos.y + (double) this._cancelSize.y))
          this._cancelHover = true;
        if (this._acceptHover && Mouse.left == InputState.Pressed)
        {
          if (this._nameBox.text == "")
            this._notify.Open("Please enter a name :(");
          else
            this._confirm.Open("Upload to workshop?");
        }
        if (this._cancelHover && Mouse.left == InputState.Pressed)
          this.Close();
        base.Update();
      }
    }

    public override void Draw()
    {
      this.menuSize.y = this._fdHeight;
      if (!this.opened)
        return;
      base.Draw();
      float num1 = 328f;
      float num2 = this._fdHeight + 22f;
      Vec2 p1 = new Vec2((float) ((double) this.layer.width / 2.0 - (double) num1 / 2.0) + this.hOffset, (float) ((double) this.layer.height / 2.0 - (double) num2 / 2.0 - 15.0));
      Vec2 p2 = new Vec2((float) ((double) this.layer.width / 2.0 + (double) num1 / 2.0) + this.hOffset, (float) ((double) this.layer.height / 2.0 + (double) num2 / 2.0 - 12.0));
      DuckGame.Graphics.DrawRect(p1, p2, new Color(70, 70, 70), this.depth, false);
      DuckGame.Graphics.DrawRect(p1, p2, new Color(30, 30, 30), this.depth - 8);
      DuckGame.Graphics.DrawRect(p1 + new Vec2(4f, 23f), p2 + new Vec2(-4f, -160f), new Color(10, 10, 10), this.depth - 4);
      DuckGame.Graphics.DrawRect(p1 + new Vec2(4f, 206f), p2 + new Vec2(-4f, -66f), new Color(10, 10, 10), this.depth - 4);
      DuckGame.Graphics.DrawRect(p1 + new Vec2(4f, 224f), p2 + new Vec2(-4f, -14f), new Color(10, 10, 10), this.depth - 4);
      DuckGame.Graphics.DrawRect(p1 + new Vec2(3f, 3f), new Vec2(p2.x - 3f, p1.y + 19f), new Color(70, 70, 70), this.depth - 4);
      if (Editor.arcadeMachineMode)
        DuckGame.Graphics.DrawString("Upload " + this._levelType.ToString() + " to Workshop", p1 + new Vec2(5f, 7f), Color.White, this.depth + 8);
      else
        DuckGame.Graphics.DrawString("Upload " + this._levelSize.ToString() + " " + this._levelType.ToString() + " to Workshop", p1 + new Vec2(5f, 7f), Color.White, this.depth + 8);
      this._descriptionBox.position = p1 + new Vec2(6f, 226f);
      this._descriptionBox.depth = this.depth + 2;
      this._descriptionBox.Draw();
      this._nameBox.position = p1 + new Vec2(6f, 208f);
      this._nameBox.depth = this.depth + 2;
      this._nameBox.Draw();
      List<string> stringList = new List<string>()
      {
        "Map"
      };
      if (this._levelType == LevelType.ArcadeMachine)
        stringList = new List<string>() { "Machine" };
      stringList.AddRange((IEnumerable<string>) Editor.workshopTags);
      int num3 = 0;
      Vec2 vec2 = p1 + new Vec2(7f, 276f);
      int num4 = 0;
      this.tagPositions.Clear();
      foreach (string str in stringList)
      {
        bool flag = Editor.workshopTags.Contains(str);
        this._workshopTag.depth = this.depth + 8;
        this._workshopTag.frame = 0;
        DuckGame.Graphics.Draw((Sprite) this._workshopTag, vec2.x, vec2.y);
        float stringWidth = DuckGame.Graphics.GetStringWidth(str, scale: 0.5f);
        float num5 = 4f;
        if (!flag)
          num5 = 0.0f;
        else
          ++num4;
        DuckGame.Graphics.DrawTexturedLine(this._workshopTagMiddle.texture, vec2 + new Vec2(4f, 4f), vec2 + new Vec2(4f + stringWidth + num5, 4f), Color.White, depth: (this.depth + 10));
        DuckGame.Graphics.DrawString(str, vec2 + new Vec2(4f, 2f), Color.Black, this.depth + 14, scale: 0.5f);
        if (flag)
        {
          Vec2 position = vec2 + new Vec2(stringWidth + 6f, 2f);
          this.tagPositions[str] = position;
          DuckGame.Graphics.DrawString("x", position, Color.Red, this.depth + 14, scale: 0.5f);
        }
        this._workshopTag.frame = 1;
        DuckGame.Graphics.Draw((Sprite) this._workshopTag, (float) ((double) vec2.x + (double) num5 + 4.0) + stringWidth, vec2.y);
        vec2.x += stringWidth + 11f + num5;
        ++num3;
      }
      if (num4 < this._possibleTags.Count)
      {
        this._tagPlus.depth = this.depth + 8;
        vec2.x += 2f;
        DuckGame.Graphics.Draw(this._tagPlus, vec2.x, vec2.y);
        this._plusPosition = vec2;
      }
      this._acceptPos = p2 + new Vec2(-78f, -12f);
      this._acceptSize = new Vec2(34f, 8f);
      DuckGame.Graphics.DrawRect(this._acceptPos, this._acceptPos + this._acceptSize, this._acceptHover ? new Color(180, 180, 180) : new Color(110, 110, 110), this.depth - 4);
      DuckGame.Graphics.DrawString("PUBLISH!", this._acceptPos + new Vec2(2f, 2f), Color.White, this.depth + 8, scale: 0.5f);
      this._cancelPos = p2 + new Vec2(-36f, -12f);
      this._cancelSize = new Vec2(32f, 8f);
      DuckGame.Graphics.DrawRect(this._cancelPos, this._cancelPos + this._cancelSize, this._cancelHover ? new Color(180, 180, 180) : new Color(110, 110, 110), this.depth - 4);
      DuckGame.Graphics.DrawString("CANCEL!", this._cancelPos + new Vec2(2f, 2f), Color.White, this.depth + 8, scale: 0.5f);
      if (this._previewTarget.width < 300)
      {
        this._previewTarget.depth = this.depth + 10;
        this._previewTarget.scale = new Vec2(0.5f, 0.5f);
        DuckGame.Graphics.Draw(this._previewTarget, (float) ((double) p1.x + ((double) p2.x - (double) p1.x) / 2.0 - (double) this._previewTarget.width * (double) this._previewTarget.scale.x / 2.0), (float) ((double) p1.y + ((double) p2.y - (double) p1.y) / 2.0 - (double) this._previewTarget.height * (double) this._previewTarget.scale.y / 2.0 - 20.0));
      }
      else
      {
        this._previewTarget.depth = this.depth + 10;
        this._previewTarget.scale = new Vec2(0.25f, 0.25f);
        DuckGame.Graphics.Draw(this._previewTarget, p1.x + 4f, p1.y + 23f);
      }
    }
  }
}
