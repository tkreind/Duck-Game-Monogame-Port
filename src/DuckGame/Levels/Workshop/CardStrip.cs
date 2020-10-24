﻿// Decompiled with JetBrains decompiler
// Type: DuckGame.CardStrip
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using System;
using System.Collections.Generic;
using System.Linq;

namespace DuckGame
{
  public class CardStrip : Thing
  {
    private int _levelIndex;
    private int _selectedCardIndex;
    private float _indexSlide;
    private List<Card> _cards = new List<Card>();
    private Sprite _arrow;
    private Card _cardSelected;
    private IPageListener _listener;
    private int _numCardsPerScreen = 3;
    private int _maxCardsPerStrip = 5;
    private bool _large;
    private string _heading;
    private bool _selected;
    private static BitmapFont _font = new BitmapFont("biosFont", 8);

    public Card cardSelected => this._cardSelected;

    public bool selected
    {
      get => this._selected;
      set
      {
        if (this._selected == value)
          return;
        this._selected = value;
        this._selectedCardIndex = this._levelIndex;
      }
    }

    public CardStrip(
      float xpos,
      float ypos,
      List<Card> cards,
      IPageListener listener,
      bool largeCard,
      int cardsPerScreen = 3,
      string heading = null)
      : base(xpos, ypos)
    {
      if (cards.Count > this._maxCardsPerStrip)
      {
        this._cards = cards.GetRange(0, 5);
        if (cards[0] is LevelInfo)
        {
          LevelInfo card = this._cards[0] as LevelInfo;
          List<Card> cards1 = this._cards;
          LevelInfo levelInfo1 = new LevelInfo();
          levelInfo1.specialText = "VIEW ALL";
          levelInfo1.large = card.large;
          LevelInfo levelInfo2 = levelInfo1;
          cards1.Add((Card) levelInfo2);
        }
      }
      else
        this._cards = cards;
      this._listener = listener;
      this._large = largeCard;
      this._numCardsPerScreen = cardsPerScreen;
      this._heading = heading;
      if (cards == null || cards.Count <= 0)
        return;
      float height = cards[0].height;
      if (heading != null && heading != "")
        height += 10f;
      this.collisionSize = new Vec2((float) this._numCardsPerScreen * (cards[0].width + 4f), height);
    }

    public override void Initialize()
    {
      this.layer = Layer.HUD;
      this._arrow = new Sprite("levelBrowserArrow");
      this._arrow.CenterOrigin();
    }

    public override void Update()
    {
      if (this._selected)
      {
        if (InputProfile.active.Pressed("LEFT"))
          --this._selectedCardIndex;
        else if (InputProfile.active.Pressed("RIGHT"))
          ++this._selectedCardIndex;
        else if (InputProfile.active.Pressed("SELECT"))
          this._listener.CardSelected(this._cards[this._selectedCardIndex]);
      }
      if (this._selectedCardIndex >= this._cards.Count<Card>())
        this._selectedCardIndex = this._cards.Count<Card>() - 1;
      else if (this._selectedCardIndex < 0)
        this._selectedCardIndex = 0;
      if (this._levelIndex + (this._numCardsPerScreen - 1) < this._selectedCardIndex)
      {
        if ((double) this._indexSlide > -1.0)
          this._indexSlide = Lerp.FloatSmooth(this._indexSlide, -1.2f, 0.2f);
        if ((double) this._indexSlide <= -1.0)
        {
          ++this._levelIndex;
          this._indexSlide = 0.0f;
        }
      }
      if (this._levelIndex <= this._selectedCardIndex)
        return;
      if ((double) this._indexSlide < 1.0)
        this._indexSlide = Lerp.FloatSmooth(this._indexSlide, 1.2f, 0.2f);
      if ((double) this._indexSlide < 1.0)
        return;
      --this._levelIndex;
      this._indexSlide = 0.0f;
    }

    public override void Draw()
    {
      float y = this.y;
      if (this._heading != null && this._heading != "")
      {
        CardStrip._font.scale = new Vec2(0.75f, 0.75f);
        CardStrip._font.Draw(this._heading, this.x + 4f, this.y, Color.White, new Depth(0.95f));
        y += 10f;
      }
      Vec2 position = Vec2.Zero;
      Vec2 vec2 = Vec2.Zero;
      if (this._cards.Count > 0)
      {
        vec2 = new Vec2(this._cards[0].width, this._cards[0].height);
        position = new Vec2((float) ((double) this.x - ((double) vec2.x + 4.0) + (double) this._indexSlide * ((double) vec2.x + 4.0)), y);
      }
      int num = 0;
      for (int index = this._levelIndex - 1; index < this._levelIndex + (this._numCardsPerScreen + 1); ++index)
      {
        if (index >= 0 && index < this._cards.Count)
        {
          Card card = this._cards[index];
          float alpha = 1f;
          if (num == this._numCardsPerScreen + 1)
            alpha = Math.Abs(this._indexSlide);
          else if (num == this._numCardsPerScreen && (double) this._indexSlide > 0.0)
          {
            alpha = 1f - Math.Abs(this._indexSlide);
          }
          else
          {
            switch (num)
            {
              case 0:
                alpha = Math.Abs(this._indexSlide);
                break;
              case 1:
                if ((double) this._indexSlide < 0.0)
                {
                  alpha = 1f - Math.Abs(this._indexSlide);
                  break;
                }
                break;
            }
          }
          card.Draw(position, this._selected && index == this._selectedCardIndex, alpha);
        }
        position.x += vec2.x + 4f;
        ++num;
      }
      this._arrow.xscale = this._arrow.yscale = 0.25f;
      this._arrow.depth = new Depth(0.98f);
      if (this._levelIndex + this._numCardsPerScreen < this._cards.Count)
      {
        this._arrow.flipH = false;
        Graphics.Draw(this._arrow, 312f, y + vec2.y / 2f);
      }
      if (this._levelIndex <= 0)
        return;
      this._arrow.flipH = true;
      Graphics.Draw(this._arrow, 8f, y + vec2.y / 2f);
    }
  }
}
