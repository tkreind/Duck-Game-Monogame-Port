﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.CategoryPage
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System.Collections.Generic;

namespace DuckGame
{
  public class CategoryPage : Level, IPageListener
  {
    private CategoryState _state;
    private BitmapFont _font;
    private List<Card> _cards = new List<Card>();
    private Card _pageToOpen;
    private Thing _strip;
    private bool _grid;
    public static float camOffset;

    public CategoryPage(List<Card> cards, bool grid)
    {
      this._grid = grid;
      this._cards = cards;
    }

    public void CardSelected(Card card)
    {
      this._state = CategoryState.OpenPage;
      this._pageToOpen = card;
    }

    public override void Initialize()
    {
      Layer.HUD.camera.x = CategoryPage.camOffset;
      this.backgroundColor = new Color(8, 12, 13);
      this._font = new BitmapFont("biosFont", 8);
      HUD.AddCornerControl(HUDCorner.BottomLeft, "SELECT@SELECT@");
      HUD.AddCornerControl(HUDCorner.BottomRight, "@QUACK@BACK");
      if (this._grid)
      {
        this._strip = (Thing) new CategoryGrid(12f, 31f, this._cards, (IPageListener) this);
        Level.Add(this._strip);
      }
      else
      {
        this._strip = (Thing) new CardStrip(12f, 31f, this._cards, (IPageListener) this, false, 4);
        Level.Add(this._strip);
      }
      base.Initialize();
    }

    public override void Update()
    {
      Layer.HUD.camera.x = CategoryPage.camOffset;
      if (this._state == CategoryState.OpenPage)
      {
        this._strip.active = false;
        CategoryPage.camOffset = Lerp.FloatSmooth(CategoryPage.camOffset, 360f, 0.1f);
        if ((double) CategoryPage.camOffset <= 330.0 || !(this._pageToOpen.specialText == "VIEW ALL"))
          return;
        Level.current = (Level) new CategoryPage(this._cards, true);
      }
      else
      {
        if (this._state != CategoryState.Idle)
          return;
        CategoryPage.camOffset = Lerp.FloatSmooth(CategoryPage.camOffset, -40f, 0.1f);
        if ((double) CategoryPage.camOffset < 0.0)
          CategoryPage.camOffset = 0.0f;
        this._strip.active = (double) CategoryPage.camOffset == 0.0;
      }
    }

    public override void PostDrawLayer(Layer layer)
    {
      if (layer != Layer.HUD)
        return;
      this._font.xscale = this._font.yscale = 1f;
      this._font.Draw("CUSTOM MAPS", 8f, 8f, Color.White, new Depth(0.95f));
      this._font.xscale = this._font.yscale = 0.75f;
      this._font.Draw("BEST NEW MAPS", 14f, 22f, Color.White, new Depth(0.95f));
    }
  }
}
