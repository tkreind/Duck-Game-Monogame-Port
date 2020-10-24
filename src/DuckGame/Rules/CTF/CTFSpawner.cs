// Decompiled with JetBrains decompiler
// Type: DuckGame.CTFSpawner
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("spawns")]
  public class CTFSpawner : ItemSpawner
  {
    public EditorProperty<bool> team = new EditorProperty<bool>(false);
    private CTFPresent _present;

    public CTFSpawner(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this._sprite = new SpriteMap("ctf/spawner", 16, 4);
      this.graphic = (Sprite) this._sprite;
      this.center = new Vec2(8f, 2f);
      this.collisionOffset = new Vec2(-8f, -2f);
      this.collisionSize = new Vec2(16f, 4f);
      this.randomSpawn = true;
    }

    public override void SetHoverItem(Holdable hover)
    {
      if (hover != this._present || this._hoverItem == hover)
        return;
      base.SetHoverItem(hover);
    }

    public override void Update()
    {
      this.spawnTime = 4f;
      if (this._present != null && this._present.removeFromLevel)
        this._present = (CTFPresent) null;
      CTFPresent ctfPresent1 = (CTFPresent) null;
      CTFPresent ctfPresent2 = (CTFPresent) null;
      foreach (CTFPresent ctfPresent3 in Level.CheckCircleAll<CTFPresent>(this.position, 16f))
      {
        if (ctfPresent3 != this._present)
          ctfPresent2 = ctfPresent3;
        else
          ctfPresent1 = ctfPresent3;
      }
      if (ctfPresent2 != null & ctfPresent1 != null)
      {
        if (ctfPresent2.duck != null)
          ctfPresent2.duck.ThrowItem();
        Level.Remove((Thing) ctfPresent2);
        Level.Add((Thing) SmallSmoke.New(ctfPresent2.x, ctfPresent2.y));
        CTF.CaptureFlag((bool) this.team);
        SFX.Play("equip");
      }
      base.Update();
    }

    public override void Draw()
    {
      this._sprite.frame = (bool) this.team ? 0 : 1;
      base.Draw();
    }

    public override void SpawnItem()
    {
      if (this._present != null)
        return;
      this._spawnWait = 0.0f;
      this._present = new CTFPresent(this.x, this.y, (bool) this.team);
      this._present.x = this.x;
      this._present.y = (float) ((double) this.top + ((double) this._present.y - (double) this._present.bottom) - 6.0);
      this._present.vSpeed = -2f;
      Level.Add((Thing) this._present);
      if (!this._seated)
        return;
      this.SetHoverItem((Holdable) this._present);
    }
  }
}
