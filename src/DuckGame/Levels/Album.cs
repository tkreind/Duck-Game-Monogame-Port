// Decompiled with JetBrains decompiler
// Type: DuckGame.Album
// Assembly: DuckGame, Version=1.0.7567.18440, Culture=neutral, PublicKeyToken=null
// MVID: 141E8A2E-D79A-4662-B1CF-5A369FF52288
// Assembly location: C:\Users\Tristan Kreindler\Documents\Duck Game\Duck Game\DuckGame.exe

using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

namespace DuckGame
{
  public class Album : Level
  {
    private List<AlbumPic> _images = new List<AlbumPic>();
    private List<AlbumPage> _pages = new List<AlbumPage>();
    private List<Texture2D> _textures = new List<Texture2D>();
    private int _currentPage;
    private int _prevPage = -1;
    private Sprite _album;
    private Sprite _screen;
    private Material _pageMaterial;
    private BitmapFont _font;
    private bool _quit;

    public Album() => this._centeredView = true;

    public override void Initialize()
    {
      this._album = new Sprite("album");
      this._screen = new Sprite("albumpic");
      this._pageMaterial = (Material) new MaterialAlbum();
      this._font = new BitmapFont("biosFont", 8);
      foreach (string file in DuckFile.GetFiles(DuckFile.albumDirectory))
      {
        try
        {
          DateTime dateTime = DateTime.Parse(Path.GetFileNameWithoutExtension(file).Replace(';', ':'), (IFormatProvider) CultureInfo.InvariantCulture);
          this._images.Add(new AlbumPic()
          {
            file = file,
            date = dateTime
          });
        }
        catch
        {
        }
      }
      this._images = this._images.OrderBy<AlbumPic, DateTime>((Func<AlbumPic, DateTime>) (x => x.date)).ToList<AlbumPic>();
      AlbumPage albumPage = (AlbumPage) null;
      int num = 1;
      foreach (AlbumPic image in this._images)
      {
        string str = image.date.ToString("MMMM", (IFormatProvider) CultureInfo.InvariantCulture) + " " + image.date.Year.ToString();
        if (albumPage == null)
        {
          num = 1;
          albumPage = new AlbumPage();
          albumPage.caption = str;
          this._pages.Add(albumPage);
        }
        if (!albumPage.caption.Contains(str))
        {
          num = 1;
          albumPage = new AlbumPage();
          albumPage.caption = str;
          this._pages.Add(albumPage);
        }
        if (albumPage.pics.Count == 4)
        {
          ++num;
          albumPage = new AlbumPage();
          albumPage.caption = str + " (" + num.ToString() + ")";
          this._pages.Add(albumPage);
        }
        albumPage.pics.Add(image);
      }
      HUD.AddCornerControl(HUDCorner.BottomRight, "@DPAD@FLIP PAGE");
      HUD.AddCornerControl(HUDCorner.TopRight, "@QUACK@EXIT");
      base.Initialize();
    }

    public override void Update()
    {
      base.Update();
      if (this._pages.Count > 0)
      {
        if (Input.Pressed("RIGHT"))
          ++this._currentPage;
        if (Input.Pressed("LEFT"))
          --this._currentPage;
        if (this._currentPage < 0)
          this._currentPage = 0;
        if (this._currentPage == this._pages.Count)
          this._currentPage = this._pages.Count - 1;
        if (this._currentPage != this._prevPage)
        {
          this._prevPage = this._currentPage;
          foreach (GraphicsResource texture in this._textures)
            texture.Dispose();
          this._textures.Clear();
          SFX.Play("flipPage", 0.8f);
          foreach (AlbumPic pic in this._pages[this._currentPage].pics)
          {
            try
            {
              Texture2D texture2D;
              using (FileStream fileStream = new FileStream(pic.file, FileMode.Open))
                texture2D = Texture2D.FromStream(DuckGame.Graphics.device, (Stream) fileStream);
              this._textures.Add(texture2D);
            }
            catch (Exception ex)
            {
            }
          }
        }
      }
      if (Input.Pressed("QUACK"))
      {
        this._quit = true;
        HUD.CloseAllCorners();
      }
      DuckGame.Graphics.fade = Lerp.Float(DuckGame.Graphics.fade, this._quit ? 0.0f : 1f, 0.05f);
      if ((double) DuckGame.Graphics.fade >= 0.00999999977648258 || !this._quit)
        return;
      Level.current = (Level) new DoorRoom();
    }

    public override void Draw() => base.Draw();

    public override void PostDrawLayer(Layer layer)
    {
      if (layer == Layer.HUD)
      {
        this._album.depth = (Depth) -0.8f;
        DuckGame.Graphics.Draw(this._album, 0.0f, 0.0f);
        this._screen.depth = (Depth) -0.6f;
        if (this._pages.Count > 0)
        {
          int index1 = 0;
          for (int index2 = 0; index2 < 2; ++index2)
          {
            for (int index3 = 0; index3 < 2; ++index3)
            {
              if (index1 < this._textures.Count)
              {
                if (DuckGame.Graphics.width > 1280)
                {
                  Vec2 vec2_1 = new Vec2(52f, 35f);
                  float num = 0.3f;
                  Vec2 vec2_2 = new Vec2(vec2_1.x + (float) (index3 * 110), vec2_1.y + (float) (index2 * 65));
                  DuckGame.Graphics.Draw((Tex2D) this._textures[index1], vec2_2.x, vec2_2.y, num, num);
                  DuckGame.Graphics.DrawRect(vec2_2 + new Vec2(-3f, -3f), vec2_2 + new Vec2((float) ((double) this._textures[index1].Width * (double) num + 3.0), (float) ((double) this._textures[index1].Height * (double) num + 3.0)), Color.White, (Depth) -0.7f);
                }
                else
                {
                  Vec2 vec2_1 = new Vec2(65f, 40f);
                  float num = 0.25f;
                  Vec2 vec2_2 = new Vec2(vec2_1.x + (float) (index3 * 100), vec2_1.y + (float) (index2 * 65));
                  DuckGame.Graphics.Draw((Tex2D) this._textures[index1], vec2_2.x, vec2_2.y, num, num);
                  DuckGame.Graphics.DrawRect(vec2_2 + new Vec2(-3f, -3f), vec2_2 + new Vec2((float) ((double) this._textures[index1].Width * (double) num + 3.0), (float) ((double) this._textures[index1].Height * (double) num + 3.0)), Color.White, (Depth) -0.7f);
                }
              }
              ++index1;
            }
          }
          string caption = this._pages[this._currentPage].caption;
          this._font.Draw(caption, new Vec2((float) ((double) Layer.HUD.width / 2.0 - (double) this._font.GetWidth(caption) / 2.0 - 4.0), 18f), Color.DarkSlateGray, (Depth) -0.5f);
        }
        else
        {
          string text = "EMPTY ALBUM :(";
          this._font.Draw(text, new Vec2((float) ((double) Layer.HUD.width / 2.0 - (double) this._font.GetWidth(text) / 2.0 - 4.0), 18f), Color.DarkSlateGray, (Depth) -0.5f);
        }
      }
      base.PostDrawLayer(layer);
    }
  }
}
