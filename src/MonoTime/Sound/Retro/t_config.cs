﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.t_config
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

namespace DuckGame
{
  public class t_config
  {
    public char[] version = new char[16];
    public byte hq_fm;
    public byte filter;
    public byte psgBoostNoise;
    public byte dac_bits;
    public byte ym2413;
    public short psg_preamp;
    public short fm_preamp;
    public short lp_range;
    public short low_freq;
    public short high_freq;
    public short lg;
    public short mg;
    public short hg;
    public float rolloff;
    public byte system;
    public byte region_detect;
    public byte master_clock;
    public byte vdp_mode;
    public byte force_dtack;
    public byte addr_error;
    public byte tmss;
    public byte bios;
    public byte lock_on;
    public byte hot_swap;
    public byte invert_mouse;
    public byte[] gun_cursor = new byte[2];
    public byte overscan;
    public byte ntsc;
    public byte vsync;
    public byte render;
    public byte tv_mode;
    public byte bilinear;
    public byte aspect;
    public short xshift;
    public short yshift;
    public short xscale;
    public short yscale;
    public byte autoload;
    public byte autocheat;
    public byte s_auto;
    public byte s_default;
    public byte s_device;
    public byte l_device;
    public byte bg_overlay;
    public short screen_w;
    public float bgm_volume;
    public float sfx_volume;
  }
}
