// Decompiled with JetBrains decompiler
// Type: DuckGame.DrumSet
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  [EditorGroup("stuff")]
  [BaggedProperty("canSpawn", false)]
  public class DrumSet : Holdable, IPlatform
  {
    private BassDrum _bass;
    private Snare _snare;
    private HiHat _hat;
    private LowTom _lowTom;
    private CrashCymbal _crash;
    private MediumTom _medTom;
    private HighTom _highTom;
    public StateBinding _netBassDrumBinding = (StateBinding) new NetSoundBinding(nameof (_netBassDrum));
    public StateBinding _netSnareBinding = (StateBinding) new NetSoundBinding(nameof (_netSnare));
    public StateBinding _netHatBinding = (StateBinding) new NetSoundBinding(nameof (_netHat));
    public StateBinding _netHatAlternateBinding = (StateBinding) new NetSoundBinding(nameof (_netHatAlternate));
    public StateBinding _netLowTomBinding = (StateBinding) new NetSoundBinding(nameof (_netLowTom));
    public StateBinding _netMediumTomBinding = (StateBinding) new NetSoundBinding(nameof (_netMediumTom));
    public StateBinding _netHighTomBinding = (StateBinding) new NetSoundBinding(nameof (_netHighTom));
    public StateBinding _netCrashBinding = (StateBinding) new NetSoundBinding(nameof (_netCrash));
    public StateBinding _netThrowStickBinding = (StateBinding) new NetSoundBinding(nameof (_netThrowStick));
    public NetSoundEffect _netBassDrum = new NetSoundEffect();
    public NetSoundEffect _netSnare = new NetSoundEffect();
    public NetSoundEffect _netHat = new NetSoundEffect();
    public NetSoundEffect _netHatAlternate = new NetSoundEffect();
    public NetSoundEffect _netLowTom = new NetSoundEffect();
    public NetSoundEffect _netMediumTom = new NetSoundEffect();
    public NetSoundEffect _netHighTom = new NetSoundEffect();
    public NetSoundEffect _netCrash = new NetSoundEffect();
    public NetSoundEffect _netThrowStick = new NetSoundEffect();
    private int hits;
    private int tick = 15;
    private int hitsSinceThrow;

    public DrumSet(float xpos, float ypos)
      : base(xpos, ypos)
    {
      this.center = new Vec2(8f, 8f);
      this.collisionOffset = new Vec2(-8f, -11f);
      this.collisionSize = new Vec2(16f, 14f);
      this.depth = (Depth) 0.5f;
      this.thickness = 4f;
      this.weight = 7f;
      this._holdOffset = new Vec2(1f, 7f);
      this.flammable = 0.3f;
      this.collideSounds.Add("rockHitGround2");
      this.handOffset = new Vec2(0.0f, -9999f);
      this.hugWalls = WallHug.Floor;
    }

    public void ThrowStick()
    {
      if ((double) Rando.Float(1f) >= 0.5)
        Level.Add((Thing) new DrumStick(this.x - 5f, this.y - 8f));
      else
        Level.Add((Thing) new DrumStick(this.x + 5f, this.y - 8f));
    }

    public override void Initialize()
    {
      this._bass = new BassDrum(this.x, this.y);
      Level.Add((Thing) this._bass);
      this._snare = new Snare(this.x, this.y);
      Level.Add((Thing) this._snare);
      this._hat = new HiHat(this.x, this.y);
      Level.Add((Thing) this._hat);
      this._lowTom = new LowTom(this.x, this.y);
      Level.Add((Thing) this._lowTom);
      this._crash = new CrashCymbal(this.x, this.y);
      Level.Add((Thing) this._crash);
      this._medTom = new MediumTom(this.x, this.y);
      Level.Add((Thing) this._medTom);
      this._highTom = new HighTom(this.x, this.y);
      Level.Add((Thing) this._highTom);
      this._bass.position = this.position;
      this._bass.depth = this.depth + 1;
      this._snare.position = this.position + new Vec2(10f, -7f);
      this._snare.depth = this.depth;
      this._hat.depth = this.depth - 1;
      this._hat.position = this.position + new Vec2(13f, -11f);
      this._lowTom.depth = this.depth - 1;
      this._lowTom.position = this.position + new Vec2(-9f, -5f);
      this._crash.depth = this.depth;
      this._crash.position = this.position + new Vec2(-15f, -15f);
      this._medTom.depth = this.depth + 3;
      this._medTom.position = this.position + new Vec2(-8f, -12f);
      this._highTom.depth = this.depth + 3;
      this._highTom.position = this.position + new Vec2(7f, -12f);
      this._netBassDrum.function = new NetSoundEffect.Function(((Drum) this._bass).Hit);
      this._netSnare.function = new NetSoundEffect.Function(((Drum) this._snare).Hit);
      this._netHat.function = new NetSoundEffect.Function(((Drum) this._hat).Hit);
      this._netHatAlternate.function = new NetSoundEffect.Function(((Drum) this._hat).AlternateHit);
      this._netLowTom.function = new NetSoundEffect.Function(((Drum) this._lowTom).Hit);
      this._netMediumTom.function = new NetSoundEffect.Function(((Drum) this._medTom).Hit);
      this._netHighTom.function = new NetSoundEffect.Function(((Drum) this._highTom).Hit);
      this._netCrash.function = new NetSoundEffect.Function(((Drum) this._crash).Hit);
      this._netThrowStick.function = new NetSoundEffect.Function(this.ThrowStick);
    }

    public override void Terminate()
    {
      Level.Remove((Thing) this._bass);
      Level.Remove((Thing) this._snare);
      Level.Remove((Thing) this._hat);
      Level.Remove((Thing) this._lowTom);
      Level.Remove((Thing) this._medTom);
      Level.Remove((Thing) this._highTom);
      Level.Remove((Thing) this._crash);
    }

    public override void Update()
    {
      --this.tick;
      if (this.tick <= 0)
      {
        this.tick = 15;
        --this.hits;
      }
      if (this.hits < 0)
        this.hits = 0;
      if (this.owner != null)
      {
        this.owner.vSpeed = 0.0f;
        this.owner.hSpeed = 0.0f;
        if (this.isServerForObject)
        {
          int hits = this.hits;
          if (this.duck.inputProfile.Pressed("UP"))
          {
            if (Network.isActive)
              this._netCrash.Play();
            else
              this._crash.Hit();
            ++this.hits;
          }
          if (this.duck.inputProfile.Pressed("SHOOT"))
          {
            if (Network.isActive)
              this._netSnare.Play();
            else
              this._snare.Hit();
            ++this.hits;
          }
          if (this.duck.inputProfile.Pressed("RIGHT"))
          {
            if (Network.isActive)
              this._netHighTom.Play();
            else
              this._highTom.Hit();
            ++this.hits;
          }
          if (this.duck.inputProfile.Pressed("DOWN"))
          {
            if (Network.isActive)
              this._netMediumTom.Play();
            else
              this._medTom.Hit();
            ++this.hits;
          }
          if (this.duck.inputProfile.Pressed("LEFT"))
          {
            if (Network.isActive)
              this._netLowTom.Play();
            else
              this._lowTom.Hit();
            ++this.hits;
          }
          if (this.duck.inputProfile.Pressed("JUMP"))
          {
            if (Network.isActive)
              this._netHat.Play();
            else
              this._hat.Hit();
            ++this.hits;
          }
          if (this.duck.inputProfile.Pressed("LTRIGGER"))
          {
            if (Network.isActive)
              this._netHat.Play();
            else
              this._hat.AlternateHit();
            ++this.hits;
          }
          if (this.duck.inputProfile.Pressed("RAGDOLL") || this.duck.inputProfile.Pressed("STRAFE"))
          {
            if (Network.isActive)
              this._netBassDrum.Play();
            else
              this._bass.Hit();
            ++this.hits;
          }
          if (hits != this.hits)
          {
            ++this.hitsSinceThrow;
            if ((double) ((float) this.hits * 0.02f) > (double) Rando.Float(1f) && (double) Rando.Float(1f) > 0.949999988079071 && this.hitsSinceThrow > 10)
            {
              if (Network.isActive)
                this._netThrowStick.Play();
              else
                this.ThrowStick();
              this.hitsSinceThrow = 0;
            }
          }
        }
      }
      this._bass.position = this.position;
      this._bass.depth = this.depth + 1;
      this._snare.position = this.position + new Vec2(10f, -7f);
      this._snare.depth = this.depth;
      this._hat.depth = this.depth - 1;
      this._hat.position = this.position + new Vec2(13f, -11f);
      this._lowTom.depth = this.depth - 1;
      this._lowTom.position = this.position + new Vec2(-9f, -5f);
      this._crash.depth = this.depth;
      this._crash.position = this.position + new Vec2(-15f, -15f);
      this._medTom.depth = this.depth + 3;
      this._medTom.position = this.position + new Vec2(-8f, -12f);
      this._highTom.depth = this.depth + 3;
      this._highTom.position = this.position + new Vec2(7f, -12f);
    }
  }
}
