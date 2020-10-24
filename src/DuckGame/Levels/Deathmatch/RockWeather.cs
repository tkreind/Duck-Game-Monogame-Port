﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.RockWeather
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;

namespace DuckGame
{
  public class RockWeather : Thing
  {
    private static Weather _weather = Weather.Sunny;
    private Color skyColor = new Color(0.5450981f, 0.8f, 0.972549f);
    private Vec3 winterColor = new Vec3(-0.1f, -0.1f, 0.2f);
    private Vec3 summerColor = new Vec3(0.0f, 0.0f, 0.0f);
    private List<WeatherParticle> _particles = new List<WeatherParticle>();
    private float _particleWait;
    private RockScoreboard _board;
    private Color _skyColor;
    private Vec3 _enviroColor;
    private static float _timeOfDay = 0.25f;
    private static float _weatherTime = 1f;
    private List<RockWeatherState> timeOfDayColorMultMap = new List<RockWeatherState>()
    {
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.06f),
        multiply = new Vec3(0.98f, 0.98f, 1f),
        sky = new Vec3(0.15f, 0.15f, 0.25f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 0.0f,
        rainbowLight = 0.0f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.1f, 0.1f, 0.06f),
        multiply = new Vec3(0.9f, 0.9f, 1f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.0f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.1f, 0.1f, 0.06f),
        multiply = new Vec3(0.9f, 0.9f, 1f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.0f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.1f, 0.1f, 0.06f),
        multiply = new Vec3(0.9f, 0.9f, 1f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.0f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.1f, 0.1f, 0.06f),
        multiply = new Vec3(0.9f, 0.9f, 1f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.0f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.08f, 0.05f, 0.0f),
        multiply = new Vec3(1f, 0.8f, 0.7f),
        sky = new Vec3(0.9803922f, 0.7450981f, 0.5490196f),
        sunPos = new Vec2(0.0f, 1f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.25f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f),
        multiply = new Vec3(1f, 1f, 1f),
        sky = new Vec3(0.5450981f, 0.8f, 0.972549f),
        sunPos = new Vec2(0.3f, 1.8f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.25f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f),
        multiply = new Vec3(1f, 1f, 1f),
        sky = new Vec3(0.5450981f, 0.8f, 0.972549f),
        sunPos = new Vec2(0.5f, 0.9f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.2f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f),
        multiply = new Vec3(1f, 1f, 1f),
        sky = new Vec3(0.5450981f, 0.8f, 0.972549f),
        sunPos = new Vec2(0.5f, 0.9f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.2f,
        rainbowLight2 = 0.3f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f),
        multiply = new Vec3(1f, 1f, 1f),
        sky = new Vec3(0.5450981f, 0.8f, 0.972549f),
        sunPos = new Vec2(0.5f, 0.9f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.25f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.08f, 0.05f, 0.0f),
        multiply = new Vec3(1f, 0.8f, 0.7f),
        sky = new Vec3(0.7843137f, 0.4705882f, 0.7450981f),
        sunPos = new Vec2(0.6f, 0.0f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.35f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.08f, 0.05f, 0.0f),
        multiply = new Vec3(1f, 0.8f, 0.7f),
        sky = new Vec3(0.9803922f, 0.6666667f, 0.4313726f),
        sunPos = new Vec2(0.7f, -0.5f),
        lightOpacity = 0.0f,
        sunGlow = 0.3f,
        sunOpacity = 1f,
        rainbowLight = 0.15f,
        rainbowLight2 = 0.15f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f),
        multiply = new Vec3(1f, 1f, 1f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.8f, -1f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.0f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.06f),
        multiply = new Vec3(0.98f, 0.98f, 1f),
        sky = new Vec3(0.15f, 0.15f, 0.25f),
        sunPos = new Vec2(0.9f, -1.2f),
        lightOpacity = 1f,
        sunGlow = -0.2f,
        sunOpacity = 0.0f,
        rainbowLight = 0.0f
      }
    };
    private List<RockWeatherState> timeOfDayColorMultMapWinter = new List<RockWeatherState>()
    {
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.06f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(0.98f, 0.98f, 1f),
        sky = new Vec3(0.15f, 0.15f, 0.25f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 0.0f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.1f, 0.1f, 0.06f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(0.9f, 0.9f, 1f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.1f, 0.1f, 0.06f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(0.9f, 0.9f, 1f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.1f, 0.1f, 0.06f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(0.9f, 0.9f, 1f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.1f, 0.1f, 0.06f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(0.9f, 0.9f, 1f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.08f, 0.05f, 0.0f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(1f, 0.8f, 0.7f),
        sky = new Vec3(0.9803922f, 0.7450981f, 0.5490196f),
        sunPos = new Vec2(0.0f, 1f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.25f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(1f, 1f, 1f),
        sky = new Vec3(0.5450981f, 0.8f, 0.972549f),
        sunPos = new Vec2(0.3f, 1.8f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.25f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(1f, 1f, 1f),
        sky = new Vec3(0.5450981f, 0.8f, 0.972549f),
        sunPos = new Vec2(0.5f, 0.9f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.2f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(1f, 1f, 1f),
        sky = new Vec3(0.5450981f, 0.8f, 0.972549f),
        sunPos = new Vec2(0.5f, 0.9f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.2f,
        rainbowLight2 = 0.3f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(1f, 1f, 1f),
        sky = new Vec3(0.5450981f, 0.8f, 0.972549f),
        sunPos = new Vec2(0.5f, 0.9f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.25f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.08f, 0.05f, 0.0f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(1f, 0.8f, 0.7f),
        sky = new Vec3(0.7843137f, 0.4705882f, 0.7450981f),
        sunPos = new Vec2(0.6f, 0.0f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 1f,
        rainbowLight = 0.35f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.08f, 0.05f, 0.0f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(1f, 0.8f, 0.7f),
        sky = new Vec3(0.9803922f, 0.6666667f, 0.4313726f),
        sunPos = new Vec2(0.7f, -0.5f),
        lightOpacity = 0.0f,
        sunGlow = 0.3f,
        sunOpacity = 1f,
        rainbowLight = 0.15f,
        rainbowLight2 = 0.15f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(1f, 1f, 1f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.8f, -1f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.06f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(0.98f, 0.98f, 1f),
        sky = new Vec3(0.15f, 0.15f, 0.25f),
        sunPos = new Vec2(0.9f, -1.2f),
        lightOpacity = 1f,
        sunGlow = -0.2f,
        sunOpacity = 0.0f
      }
    };
    private List<RockWeatherState> timeOfDayColorMultMapRaining = new List<RockWeatherState>()
    {
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.06f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(0.91f, 0.99f, 0.94f),
        sky = new Vec3(0.15f, 0.15f, 0.25f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 0.0f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.1f, 0.1f, 0.06f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(0.91f, 0.99f, 0.94f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.1f, 0.1f, 0.06f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(0.91f, 0.99f, 0.94f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.1f, 0.1f, 0.06f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(0.91f, 0.99f, 0.94f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.1f, 0.1f, 0.06f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(0.9f, 0.9f, 1f),
        sky = new Vec3(0.1f, 0.1f, 0.2f),
        sunPos = new Vec2(0.0f, 0.6f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 1f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.08f, 0.05f, 0.0f) + new Vec3(-0.15f, -0.1f, 0.1f),
        multiply = new Vec3(1f, 0.85f, 0.7f),
        sky = new Vec3(0.8627451f, 0.6666667f, 0.5490196f),
        sunPos = new Vec2(0.0f, 1f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 0.5f,
        rainbowLight = 0.25f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f) + new Vec3(-0.15f, -0.1f, 0.1f),
        multiply = new Vec3(0.89f, 1.05f, 1f),
        sky = new Vec3(0.3647059f, 0.5647059f, 0.6078432f),
        sunPos = new Vec2(0.3f, 1.8f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 0.4f,
        rainbowLight = 0.25f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f) + new Vec3(-0.19f, -0.09f, 0.1f),
        multiply = new Vec3(0.89f, 1.05f, 1f),
        sky = new Vec3(0.3647059f, 0.5647059f, 0.6078432f),
        sunPos = new Vec2(0.5f, 0.9f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 0.4f,
        rainbowLight = 0.2f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f) + new Vec3(-0.2f, -0.1f, 0.07f),
        multiply = new Vec3(0.89f, 1.05f, 1f),
        sky = new Vec3(0.3647059f, 0.5647059f, 0.6078432f),
        sunPos = new Vec2(0.5f, 0.9f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 0.4f,
        rainbowLight = 0.2f,
        rainbowLight2 = 0.3f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f) + new Vec3(-0.22f, -0.12f, 0.0f),
        multiply = new Vec3(0.86f, 1.05f, 1f),
        sky = new Vec3(0.3254902f, 0.4470588f, 0.5137255f),
        sunPos = new Vec2(0.5f, 0.9f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 0.4f,
        rainbowLight = 0.25f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.08f, 0.05f, 0.0f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(1f, 0.8f, 0.8f),
        sky = new Vec3(0.5882353f, 0.3921569f, 0.627451f),
        sunPos = new Vec2(0.6f, 0.0f),
        lightOpacity = 0.0f,
        sunGlow = 0.0f,
        sunOpacity = 0.4f,
        rainbowLight = 0.35f,
        rainbowLight2 = 0.35f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.08f, 0.05f, 0.0f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(1f, 0.8f, 0.7f),
        sky = new Vec3(0.7058824f, 0.5490196f, 0.7058824f),
        sunPos = new Vec2(0.7f, -0.5f),
        lightOpacity = 0.0f,
        sunGlow = 0.3f,
        sunOpacity = 0.4f,
        rainbowLight = 0.15f,
        rainbowLight2 = 0.15f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.0f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(1f, 1f, 1f),
        sky = new Vec3(0.1f, 0.15f, 0.2f),
        sunPos = new Vec2(0.8f, -1f),
        lightOpacity = 1f,
        sunGlow = 0.0f,
        sunOpacity = 0.3f
      },
      new RockWeatherState()
      {
        add = new Vec3(0.0f, 0.0f, 0.06f) + new Vec3(-0.1f, -0.1f, 0.2f),
        multiply = new Vec3(0.91f, 0.99f, 0.94f),
        sky = new Vec3(0.15f, 0.15f, 0.25f),
        sunPos = new Vec2(0.9f, -1.2f),
        lightOpacity = 1f,
        sunGlow = -0.2f,
        sunOpacity = 0.0f
      }
    };
    public static Weather _prevWeather;
    public static float _prevWeatherLerp = 0.0f;
    public static float lightOpacity;
    public static float sunGlow;
    public static float sunOpacity = 1f;
    public static Vec2 sunPos;
    private RockWeatherState _lastAppliedState;
    private static float snowChance = 0.0f;
    private static float rainChance = 0.0f;
    private static float sunshowers = 0.0f;
    private float wait;
    public float rainbowLight;
    public float rainbowLight2;
    public static float rainbowFade = 0.0f;
    public static float rainbowTime = 0.0f;
    public static float _timeRaining = 0.0f;
    public static bool alwaysRainbow = false;
    public static bool neverRainbow = false;

    public static Weather weather => RockWeather._weather;

    private RockWeatherState GetWeatherState(float time, bool lerp = true)
    {
      RockWeatherState rockWeatherState1 = (RockWeatherState) null;
      RockWeatherState rockWeatherState2 = (RockWeatherState) null;
      float num1 = 0.0f;
      int index = 0;
      switch (RockWeather._weather)
      {
        case Weather.Sunny:
          num1 = 1f / (float) this.timeOfDayColorMultMap.Count;
          index = (int) ((double) RockWeather._timeOfDay * (double) this.timeOfDayColorMultMap.Count);
          if (index >= this.timeOfDayColorMultMap.Count)
            index = this.timeOfDayColorMultMap.Count - 1;
          rockWeatherState1 = this.timeOfDayColorMultMap[index];
          rockWeatherState2 = index + 1 <= this.timeOfDayColorMultMap.Count - 1 ? this.timeOfDayColorMultMap[index + 1] : this.timeOfDayColorMultMap[0];
          break;
        case Weather.Snowing:
          num1 = 1f / (float) this.timeOfDayColorMultMapWinter.Count;
          index = (int) ((double) RockWeather._timeOfDay * (double) this.timeOfDayColorMultMapWinter.Count);
          if (index >= this.timeOfDayColorMultMapWinter.Count)
            index = this.timeOfDayColorMultMapWinter.Count - 1;
          rockWeatherState1 = this.timeOfDayColorMultMapWinter[index];
          rockWeatherState2 = index + 1 <= this.timeOfDayColorMultMapWinter.Count - 1 ? this.timeOfDayColorMultMapWinter[index + 1] : this.timeOfDayColorMultMapWinter[0];
          break;
        case Weather.Raining:
          num1 = 1f / (float) this.timeOfDayColorMultMapRaining.Count;
          index = (int) ((double) RockWeather._timeOfDay * (double) this.timeOfDayColorMultMapRaining.Count);
          if (index >= this.timeOfDayColorMultMapRaining.Count)
            index = this.timeOfDayColorMultMapRaining.Count - 1;
          rockWeatherState1 = this.timeOfDayColorMultMapRaining[index];
          rockWeatherState2 = index + 1 <= this.timeOfDayColorMultMapRaining.Count - 1 ? this.timeOfDayColorMultMapRaining[index + 1] : this.timeOfDayColorMultMapRaining[0];
          break;
      }
      float num2 = Maths.NormalizeSection(RockWeather._timeOfDay, num1 * (float) index, num1 * (float) (index + 1));
      RockWeatherState rockWeatherState3 = new RockWeatherState();
      if (this._lastAppliedState == null)
        this._lastAppliedState = rockWeatherState1.Copy();
      if (lerp)
      {
        float amount = 1f / 1000f;
        rockWeatherState3.add = Lerp.Vec3(this._lastAppliedState.add, rockWeatherState1.add + (rockWeatherState2.add - rockWeatherState1.add) * num2, amount);
        rockWeatherState3.multiply = Lerp.Vec3(this._lastAppliedState.multiply, rockWeatherState1.multiply + (rockWeatherState2.multiply - rockWeatherState1.multiply) * num2, amount);
        rockWeatherState3.sky = Lerp.Vec3(this._lastAppliedState.sky, rockWeatherState1.sky + (rockWeatherState2.sky - rockWeatherState1.sky) * num2, amount);
        rockWeatherState3.lightOpacity = Lerp.Float(this._lastAppliedState.lightOpacity, rockWeatherState1.lightOpacity + (rockWeatherState2.lightOpacity - rockWeatherState1.lightOpacity) * num2, amount);
        rockWeatherState3.sunPos = Lerp.Vec2(this._lastAppliedState.sunPos, rockWeatherState1.sunPos + (rockWeatherState2.sunPos - rockWeatherState1.sunPos) * num2, amount);
        rockWeatherState3.sunGlow = Lerp.Float(this._lastAppliedState.sunGlow, rockWeatherState1.sunGlow + (rockWeatherState2.sunGlow - rockWeatherState1.sunGlow) * num2, amount);
        rockWeatherState3.sunOpacity = Lerp.Float(this._lastAppliedState.sunOpacity, rockWeatherState1.sunOpacity + (rockWeatherState2.sunOpacity - rockWeatherState1.sunOpacity) * num2, amount);
        rockWeatherState3.rainbowLight = Lerp.Float(this._lastAppliedState.rainbowLight, rockWeatherState1.rainbowLight + (rockWeatherState2.rainbowLight - rockWeatherState1.rainbowLight) * num2, amount);
        rockWeatherState3.rainbowLight2 = Lerp.Float(this._lastAppliedState.rainbowLight2, rockWeatherState1.rainbowLight2 + (rockWeatherState2.rainbowLight2 - rockWeatherState1.rainbowLight2) * num2, amount);
      }
      else
      {
        rockWeatherState3.add = rockWeatherState1.add + (rockWeatherState2.add - rockWeatherState1.add) * num2;
        rockWeatherState3.multiply = rockWeatherState1.multiply + (rockWeatherState2.multiply - rockWeatherState1.multiply) * num2;
        rockWeatherState3.sky = rockWeatherState1.sky + (rockWeatherState2.sky - rockWeatherState1.sky) * num2;
        rockWeatherState3.lightOpacity = rockWeatherState1.lightOpacity + (rockWeatherState2.lightOpacity - rockWeatherState1.lightOpacity) * num2;
        rockWeatherState3.sunPos = rockWeatherState1.sunPos + (rockWeatherState2.sunPos - rockWeatherState1.sunPos) * num2;
        rockWeatherState3.sunGlow = rockWeatherState1.sunGlow + (rockWeatherState2.sunGlow - rockWeatherState1.sunGlow) * num2;
        rockWeatherState3.sunOpacity = rockWeatherState1.sunOpacity + (rockWeatherState2.sunOpacity - rockWeatherState1.sunOpacity) * num2;
        rockWeatherState3.rainbowLight = rockWeatherState1.rainbowLight + (rockWeatherState2.rainbowLight - rockWeatherState1.rainbowLight) * num2;
        rockWeatherState3.rainbowLight2 = rockWeatherState1.rainbowLight2 + (rockWeatherState2.rainbowLight2 - rockWeatherState1.rainbowLight2) * num2;
      }
      this._lastAppliedState = rockWeatherState3;
      return rockWeatherState3;
    }

    private void ApplyWeatherState(RockWeatherState state)
    {
      Layer.Game.colorMul = state.multiply * Layer.Game.fade;
      Layer.Background.colorMul = state.multiply * Layer.Background.fade;
      this._board.fieldMulColor = state.multiply * Layer.Game.fade;
      Layer.Game.colorAdd = state.add * Layer.Game.fade;
      Layer.Background.colorAdd = state.add * Layer.Background.fade;
      this._board.fieldAddColor = state.add * Layer.Game.fade;
      Level.current.backgroundColor = new Color(state.sky.x, state.sky.y, state.sky.z) * Layer.Game.fade;
      RockWeather.lightOpacity = state.lightOpacity;
      RockWeather.sunPos = state.sunPos;
      RockWeather.sunGlow = state.sunGlow;
      RockWeather.sunOpacity = state.sunOpacity;
      this._lastAppliedState = state;
    }

    public RockWeather(RockScoreboard board)
      : base()
    {
      this.layer = Layer.Foreground;
      this._board = board;
      if (RockWeather._weather == Weather.Snowing)
      {
        this._skyColor = this.skyColor;
        this._enviroColor = this.winterColor;
      }
      else
      {
        this._skyColor = this.skyColor;
        this._enviroColor = this.summerColor;
      }
      RainParticle.splash = new SpriteMap("rainSplash", 8, 8);
    }

    public void Start() => this.ApplyWeatherState(this.GetWeatherState(RockWeather._timeOfDay, false));

    public BitBuffer NetSerialize()
    {
      BitBuffer bitBuffer = new BitBuffer();
      bitBuffer.Write(RockWeather._timeOfDay);
      bitBuffer.Write((byte) RockWeather._weather);
      return bitBuffer;
    }

    public void NetDeserialize(BitBuffer data)
    {
      RockWeather._timeOfDay = data.ReadFloat();
      RockWeather._weather = (Weather) data.ReadByte();
    }

    public static void TickWeather()
    {
      RockWeather._timeOfDay += 6.17284E-06f;
      RockWeather._weatherTime += 6.17284E-06f;
      if (RockWeather._weather == Weather.Raining)
        RockWeather._timeRaining += Maths.IncFrameTimer();
      if ((double) RockWeather._timeOfDay <= 1.0)
        return;
      RockWeather._timeOfDay = 0.0f;
    }

    public static void Reset()
    {
      RockWeather._timeOfDay = Rando.Float(0.35f, 0.42f);
      RockWeather._weatherTime = 1f;
      RockWeather._weather = Weather.Sunny;
      RockWeather.alwaysRainbow = false;
      RockWeather.neverRainbow = false;
      if (DateTime.Now.Month == 12 && (double) Rando.Float(1f) > 0.850000023841858)
        RockWeather._weather = Weather.Snowing;
      if (DateTime.Now.Month == 4 && (double) Rando.Float(1f) > 0.920000016689301)
        RockWeather._weather = Weather.Raining;
      if (DateTime.Now.Month == 12 && DateTime.Now.Day == 25)
        RockWeather._weather = Weather.Snowing;
      if (DateTime.Now.Month == 4 && DateTime.Now.Day == 20)
      {
        RockWeather._weather = Weather.Raining;
        RockWeather.neverRainbow = true;
      }
      if (DateTime.Now.Month < 3)
      {
        RockWeather.snowChance = 0.02f;
        if (DateTime.Now.Month < 2)
          RockWeather.snowChance = 0.04f;
        RockWeather.rainChance = 3f / 500f;
        if (DateTime.Now.Month < 2)
          RockWeather.rainChance = 3f / 1000f;
      }
      else if (DateTime.Now.Month > 6)
      {
        RockWeather.snowChance = 0.0001f;
        if (DateTime.Now.Month > 7)
        {
          RockWeather.snowChance = 1f / 1000f;
          if (DateTime.Now.Month > 8)
          {
            RockWeather.snowChance = 0.01f;
            if (DateTime.Now.Month > 9)
            {
              RockWeather.snowChance = 0.02f;
              if (DateTime.Now.Month > 10)
              {
                RockWeather.snowChance = 0.04f;
                if (DateTime.Now.Month == 12)
                  RockWeather.snowChance = 0.08f;
              }
            }
          }
        }
      }
      if (DateTime.Now.Month > 3)
      {
        RockWeather.rainChance = 0.02f;
        if (DateTime.Now.Month > 5)
          RockWeather.rainChance = 0.01f;
        if (DateTime.Now.Month > 7)
          RockWeather.rainChance = 0.005f;
        if (DateTime.Now.Month > 8)
          RockWeather.rainChance = 1f / 1000f;
        if (DateTime.Now.Month > 10)
          RockWeather.rainChance = 0.0f;
      }
      if (DateTime.Now.Month != 3 || DateTime.Now.Day != 9)
        return;
      RockWeather._weather = Weather.Sunny;
      RockWeather.alwaysRainbow = true;
      RockWeather.rainChance = 0.0f;
      RockWeather.snowChance = 0.0f;
    }

    public void SetWeather(Weather w)
    {
      RockWeather._weather = w;
      RockWeather._weatherTime = 0.0f;
    }

    public override void Update()
    {
      if (RockWeather.alwaysRainbow)
      {
        RockWeather.rainbowFade = 1f;
        RockWeather.rainbowTime = 1f;
      }
      RockWeather.rainbowFade = Lerp.Float(RockWeather.rainbowFade, (double) RockWeather.rainbowTime > 0.0 ? 1f : 0.0f, 1f / 1000f);
      RockWeather.rainbowTime -= Maths.IncFrameTimer();
      if (RockWeather._weather != Weather.Sunny)
        RockWeather.rainbowTime -= Maths.IncFrameTimer() * 8f;
      if ((double) RockWeather.rainbowTime < 0.0)
        RockWeather.rainbowTime = 0.0f;
      if (RockWeather.neverRainbow)
        RockWeather.rainbowFade = 0.0f;
      RockWeatherState weatherState = this.GetWeatherState(RockWeather._timeOfDay);
      this.rainbowLight = weatherState.rainbowLight * RockWeather.rainbowFade;
      this.rainbowLight2 = weatherState.rainbowLight2 * RockWeather.rainbowFade;
      this.ApplyWeatherState(weatherState);
      RockWeather._prevWeatherLerp = Lerp.Float(RockWeather._prevWeatherLerp, 0.0f, 0.05f);
      if (Network.isServer)
      {
        this.wait += 3f / 1000f;
        if ((double) this.wait > 1.0)
        {
          this.wait = 0.0f;
          if ((double) RockWeather._weatherTime > 0.100000001490116)
          {
            if ((double) RockWeather.snowChance > 0.0 && RockWeather._weather != Weather.Snowing && (double) Rando.Float(1f) > 1.0 - (double) RockWeather.snowChance)
            {
              RockWeather._prevWeatherLerp = 1f;
              RockWeather.sunshowers = 0.0f;
              RockWeather._prevWeather = RockWeather._weather;
              RockWeather._weather = Weather.Snowing;
              if (Network.isActive)
                Send.Message((NetMessage) new NMChangeWeather((byte) RockWeather._weather));
              RockWeather._weatherTime = 0.0f;
            }
            if ((double) RockWeather.rainChance > 0.0 && RockWeather._weather != Weather.Raining && (double) Rando.Float(1f) > 1.0 - (double) RockWeather.rainChance)
            {
              RockWeather._prevWeatherLerp = 1f;
              RockWeather.sunshowers = 0.0f;
              RockWeather._prevWeather = RockWeather._weather;
              RockWeather._weather = Weather.Raining;
              if (Network.isActive)
                Send.Message((NetMessage) new NMChangeWeather((byte) RockWeather._weather));
              RockWeather._weatherTime = 0.0f;
            }
            if (RockWeather._weather != Weather.Sunny && (double) Rando.Float(1f) > 0.980000019073486)
            {
              RockWeather._prevWeatherLerp = 1f;
              if (RockWeather._weather == Weather.Raining)
              {
                if ((double) RockWeather._timeRaining > 900.0 && (double) Rando.Float(1f) > 0.850000023841858 || (double) Rando.Float(1f) > 0.949999988079071)
                  RockWeather.rainbowTime = Rando.Float(30f, 240f);
                if ((double) Rando.Float(1f) > 0.300000011920929)
                  RockWeather.sunshowers = Rando.Float(0.1f, 60f);
              }
              RockWeather._timeRaining = 0.0f;
              RockWeather._prevWeather = RockWeather._weather;
              RockWeather._weather = Weather.Sunny;
              if (Network.isActive)
                Send.Message((NetMessage) new NMChangeWeather((byte) RockWeather._weather));
              RockWeather._weatherTime = 0.0f;
            }
          }
        }
      }
      RockWeather.sunshowers -= Maths.IncFrameTimer();
      if ((double) RockWeather.sunshowers <= 0.0)
        RockWeather.sunshowers = 0.0f;
      switch (RockWeather._weather)
      {
        case Weather.Snowing:
          while ((double) this._particleWait <= 0.0)
          {
            ++this._particleWait;
            SnowParticle snowParticle = new SnowParticle(new Vec2(Rando.Float(-100f, 400f), Rando.Float(-500f, -550f)));
            snowParticle.z = Rando.Float(0.0f, 200f);
            this._particles.Add((WeatherParticle) snowParticle);
          }
          this._particleWait -= 0.5f;
          break;
        case Weather.Raining:
          while ((double) this._particleWait <= 0.0)
          {
            ++this._particleWait;
            RainParticle rainParticle = new RainParticle(new Vec2(Rando.Float(-100f, 900f), Rando.Float(-500f, -550f)));
            rainParticle.z = Rando.Float(0.0f, 200f);
            this._particles.Add((WeatherParticle) rainParticle);
          }
          --this._particleWait;
          break;
        default:
          if ((double) RockWeather.sunshowers <= 0.0)
            break;
          goto case Weather.Raining;
      }
      List<WeatherParticle> weatherParticleList = new List<WeatherParticle>();
      foreach (WeatherParticle particle in this._particles)
      {
        particle.Update();
        if ((double) particle.position.y > 0.0)
          particle.die = true;
        switch (particle)
        {
          case RainParticle _ when (double) particle.z < 70.0 && (double) particle.position.y > -62.0:
            particle.die = true;
            particle.position.y = -58f;
            break;
          case RainParticle _ when (double) particle.z < 40.0 && (double) particle.position.y > -98.0:
            particle.die = true;
            particle.position.y = -98f;
            break;
          case RainParticle _ when (double) particle.z < 25.0 && ((double) particle.position.x > 175.0 && (double) particle.position.x < 430.0) && ((double) particle.position.y > -362.0 && (double) particle.position.y < -352.0):
            particle.die = true;
            particle.position.y = -362f;
            break;
        }
        if ((double) particle.alpha < 0.00999999977648258)
          weatherParticleList.Add(particle);
      }
      foreach (WeatherParticle weatherParticle in weatherParticleList)
        this._particles.Remove(weatherParticle);
    }

    public override void Draw()
    {
      if (RockScoreboard.drawingLighting)
        return;
      foreach (WeatherParticle particle in this._particles)
        particle.Draw();
    }
  }
}
