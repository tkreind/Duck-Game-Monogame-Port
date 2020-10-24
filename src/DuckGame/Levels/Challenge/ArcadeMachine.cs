// Decompiled with JetBrains decompiler
// Type: DuckGame.ArcadeMachine
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [BaggedProperty("isOnlineCapable", false)]
  [EditorGroup("special|arcade")]
  public class ArcadeMachine : Thing
  {
    public EditorProperty<string> name = new EditorProperty<string>("NAMELESS");
    public EditorProperty<bool> lit = new EditorProperty<bool>(true);
    public EditorProperty<int> style = new EditorProperty<int>(0, max: 16f, increment: 1f);
    public EditorProperty<int> requirement = new EditorProperty<int>(0, max: 100f, increment: 1f);
    public EditorProperty<float> respect = new EditorProperty<float>(0.0f, increment: 0.05f);
    public EditorProperty<string> challenge01 = new EditorProperty<string>("", increment: 0.0f, isLevel: true);
    public EditorProperty<string> challenge02 = new EditorProperty<string>("", increment: 0.0f, isLevel: true);
    public EditorProperty<string> challenge03 = new EditorProperty<string>("", increment: 0.0f, isLevel: true);
    private SpriteMap _sprite;
    private Sprite _customMachineOverlay;
    private Sprite _outline;
    private BitmapFont _font;
    private float _hoverFade;
    private ChallengeGroup _data;
    private SpriteMap _light;
    private Sprite _fixture;
    private DustSparkleEffect _dust;
    private bool _unlocked = true;
    private int _lightColor = 1;
    public bool flip;
    public bool hover;
    private SpriteMap _flash;
    private SpriteMap _flashLarge;
    private Sprite _covered;
    private Sprite _boom;
    private Thing _lighting;
    public string machineStyle = "";
    private string _previousMachineStyle = "";
    private Sprite _machineStyleSprite;
    public LevelData challenge01Data;
    public LevelData challenge02Data;
    public LevelData challenge03Data;

    public ChallengeGroup data => this._data;

    public override bool visible
    {
      get => base.visible;
      set
      {
        base.visible = value;
        this._dust.visible = base.visible;
      }
    }

    public bool unlocked
    {
      get => this._unlocked;
      set => this._unlocked = value;
    }

    public int lightColor
    {
      get => this._lightColor;
      set => this._lightColor = value;
    }

    public ArcadeMachine(float xpos, float ypos, ChallengeGroup c, int index)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("arcade/arcadeMachines", 29, 36);
      this._sprite.frame = index;
      this.graphic = (Sprite) this._sprite;
      this.depth = new Depth(-0.5f);
      this._canHaveChance = false;
      this._customMachineOverlay = new Sprite("arcade/customMachine");
      this._outline = new Sprite("arcade/arcadeMachineOutline");
      this._outline.depth = this.depth + 1;
      this._outline.CenterOrigin();
      this._boom = new Sprite("arcade/boommachine");
      this._font = new BitmapFont("biosFont", 8);
      this.center = new Vec2((float) (this._sprite.width / 2), (float) (this._sprite.h / 2));
      this._data = c;
      this._light = new SpriteMap("arcade/lights2", 56, 57);
      this._fixture = new Sprite("arcade/fixture");
      this._flash = new SpriteMap("arcade/monitorFlash", 11, 9);
      this._flash.AddAnimation("idle", 0.1f, true, 0, 1, 2);
      this._flash.SetAnimation("idle");
      this._flashLarge = new SpriteMap("arcade/monitorFlashLarge", 13, 10);
      this._flashLarge.AddAnimation("idle", 0.1f, true, 0, 1, 2);
      this._flashLarge.SetAnimation("idle");
      this._covered = new Sprite("arcade/coveredMachine");
      this._collisionSize = new Vec2(28f, 34f);
      this._collisionOffset = new Vec2(-14f, -17f);
      this.hugWalls = WallHug.Floor;
      this.respect._tooltip = "How much Chancy needs to like you before this machine unlocks.";
      this.requirement._tooltip = "How many challenges must be completed before this machine unlocks.";
      this.name._tooltip = "What's this collection of challenges called?";
    }

    public override void Initialize()
    {
      if (Level.current is Editor)
        return;
      this._data = new ChallengeGroup();
      this._data.name = this.name.value;
      this._data.trophiesRequired = this.requirement.value;
      this._data.challenges.Add(this.challenge01.value);
      this._data.challenges.Add(this.challenge02.value);
      this._data.challenges.Add(this.challenge03.value);
      if (this.level == null || this.level.bareInitialize)
        return;
      this._dust = new DustSparkleEffect(this.x - 28f, this.y - 40f, false, (bool) this.lit);
      Level.Add((Thing) this._dust);
      this._dust.depth = this.depth - 2;
      this._lighting = !(bool) this.lit ? (Thing) new ArcadeScreen(this.x, this.y) : (Thing) new ArcadeLight(this.x - 1f, this.y - 41f);
      Level.Add(this._lighting);
    }

    public bool CheckUnlocked(bool ignoreAlreadyUnlocked = true)
    {
      if (this._data == null || ignoreAlreadyUnlocked && this._unlocked)
        return false;
      if (this._data.required.Count > 0)
      {
        foreach (string name in this._data.required)
        {
          ChallengeData challenge = Challenges.GetChallenge(name);
          if (challenge != null)
          {
            ChallengeSaveData saveData = Challenges.GetSaveData(challenge.levelID, Profiles.active[0], true);
            if (saveData == null || saveData.trophy == TrophyType.Baseline)
              return false;
          }
        }
      }
      if ((double) (float) this.respect != 0.0 && (double) Challenges.GetChallengeSkillIndex() < (double) (float) this.respect)
        return false;
      return (int) this.requirement <= 0 || Challenges.GetNumTrophies(Profiles.active[0]) >= (int) this.requirement;
    }

    public void UpdateStyle()
    {
      if (!(this._previousMachineStyle != this.machineStyle))
        return;
      this._machineStyleSprite = this.machineStyle == null || this.machineStyle == "" ? (Sprite) null : new Sprite((Tex2D) Editor.StringToTexture(this.machineStyle));
      this._previousMachineStyle = this.machineStyle;
    }

    public override BinaryClassChunk Serialize()
    {
      BinaryClassChunk binaryClassChunk = base.Serialize();
      binaryClassChunk.AddProperty("machineStyle", (object) this.machineStyle);
      binaryClassChunk.AddProperty("arcadeMachineMode", (object) Editor.arcadeMachineMode);
      if (Editor.arcadeMachineMode)
      {
        LevelData level1 = Content.GetLevel(this.challenge01.value);
        binaryClassChunk.AddProperty("c1", (object) level1);
        LevelData level2 = Content.GetLevel(this.challenge02.value);
        binaryClassChunk.AddProperty("c2", (object) level2);
        LevelData level3 = Content.GetLevel(this.challenge03.value);
        binaryClassChunk.AddProperty("c3", (object) level3);
      }
      return binaryClassChunk;
    }

    public override bool Deserialize(BinaryClassChunk node)
    {
      base.Deserialize(node);
      this.machineStyle = node.GetProperty<string>("machineStyle");
      if (node.GetProperty<bool>("arcadeMachineMode"))
      {
        this.challenge01Data = node.GetProperty<LevelData>("c1");
        this.challenge02Data = node.GetProperty<LevelData>("c2");
        this.challenge03Data = node.GetProperty<LevelData>("c3");
      }
      return true;
    }

    public override void EditorUpdate()
    {
      this.UpdateStyle();
      base.EditorUpdate();
    }

    public override void Update()
    {
      this.UpdateStyle();
      if (this._unlocked)
      {
        Duck duck = Level.Nearest<Duck>(this.x, this.y);
        if (duck != null)
        {
          if (duck.grounded && (double) (duck.position - this.position).length < 20.0)
          {
            this._hoverFade = Lerp.Float(this._hoverFade, 1f, 0.1f);
            this.hover = true;
          }
          else
          {
            this._hoverFade = Lerp.Float(this._hoverFade, 0.0f, 0.1f);
            this.hover = false;
          }
        }
      }
      this._dust.fade = 0.7f;
      this._dust.visible = this._lighting.visible = this._unlocked && this.visible;
    }

    public override ContextMenu GetContextMenu()
    {
      ContextMenu contextMenu = base.GetContextMenu();
      contextMenu.AddItem((ContextMenu) new ContextFile("style", (IContextListener) null, new FieldBinding((object) this, "machineStyle"), ContextFileType.ArcadeStyle, "Custom art. Placing a single machine in a level with no other objects creates a new workshop machine."));
      return contextMenu;
    }

    public override void Draw()
    {
      this._sprite.frame = this.style.value;
      this._light.depth = this.depth - 6;
      this._flash.depth = this.depth + 1;
      if (this._unlocked)
      {
        this._light.frame = this._lightColor;
        this.graphic.color = Color.White;
        if (this.style.value == 15)
        {
          if (this.flipHorizontal)
            Graphics.Draw((Sprite) this._flashLarge, this.x - 3f, this.y - 8f);
          else
            Graphics.Draw((Sprite) this._flashLarge, this.x - 7f, this.y - 8f);
        }
        else if (this.flipHorizontal)
          Graphics.Draw((Sprite) this._flash, this.x - 3f, this.y - 7f);
        else
          Graphics.Draw((Sprite) this._flash, this.x - 7f, this.y - 7f);
      }
      else
      {
        this._light.frame = 0;
        this.graphic.color = Color.Black;
      }
      if ((bool) this.lit)
      {
        Graphics.Draw((Sprite) this._light, this.x - 28f, this.y - 40f);
        this._fixture.depth = this.depth - 1;
        Graphics.Draw(this._fixture, this.x - 10f, this.y - 65f);
      }
      this._sprite.flipH = false;
      if (this.style.value == 15)
      {
        this._boom.flipH = false;
        this._boom.depth = this.depth;
        Graphics.Draw(this._boom, this.x - 17f, this.y - 36f);
      }
      else if (this._machineStyleSprite != null)
      {
        if (this._machineStyleSprite.width > 27)
        {
          this._machineStyleSprite.flipH = this.flipHorizontal;
          this._machineStyleSprite.scale = new Vec2(0.5f, 0.5f);
          this._machineStyleSprite.center = this.center * 2f;
          this._machineStyleSprite.depth = this.depth;
          Graphics.Draw(this._machineStyleSprite, (float) ((double) this.x + 1.0 + (this.flipHorizontal ? -1.0 : 0.0)), this.y + 1f);
        }
        else
        {
          this._machineStyleSprite.flipH = this.flipHorizontal;
          this._machineStyleSprite.scale = new Vec2(1f, 1f);
          this._machineStyleSprite.center = this.center;
          this._machineStyleSprite.depth = this.depth;
          Graphics.Draw(this._machineStyleSprite, (float) ((double) this.x + 1.0 + (this.flipHorizontal ? -1.0 : 0.0)), this.y + 1f);
        }
        this._customMachineOverlay.flipH = this.flipHorizontal;
        this._customMachineOverlay.depth = this.depth + 10;
        this._customMachineOverlay.center = this.center;
        Graphics.Draw(this._customMachineOverlay, this.x + (this.flipHorizontal ? 1f : 0.0f), this.y);
      }
      else
        base.Draw();
      if (!this._unlocked)
      {
        this._covered.depth = this.depth + 2;
        if (this.flipHorizontal)
        {
          this._covered.flipH = true;
          Graphics.Draw(this._covered, this.x + 19f, this.y - 19f);
        }
        else
          Graphics.Draw(this._covered, this.x - 18f, this.y - 19f);
      }
      if ((double) this._hoverFade <= 0.0)
        return;
      this._outline.alpha = this._hoverFade;
      this._outline.flipH = this.flipHorizontal;
      if (this.flipHorizontal)
        Graphics.Draw(this._outline, this.x, this.y);
      else
        Graphics.Draw(this._outline, this.x + 1f, this.y);
      string name = this._data.name;
      this._font.alpha = this._hoverFade;
    }
  }
}
