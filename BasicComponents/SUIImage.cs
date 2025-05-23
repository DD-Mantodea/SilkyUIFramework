﻿using ReLogic.Content;

namespace SilkyUIFramework.BasicComponents;

public class SUIImage : UIView
{
    #region Texture2D

    public delegate void TextureChangeEventHandler(SUIImage sender, Asset<Texture2D> newTexture2D, Asset<Texture2D> oldTexture2D);

    private Asset<Texture2D> _texture2D;

    public Asset<Texture2D> Texture2D
    {
        get => _texture2D;
        set
        {
            ArgumentNullException.ThrowIfNull(value);
            OnTextureChanged(this, value, _texture2D);
            _texture2D = value;
        }
    }
    public Vector2 ImageOriginalSize
    {
        get
        {
            if (Texture2D.Value is { } texture2D)
                return new(texture2D.Width, texture2D.Height);
            return Vector2.Zero;
        }
    }

    public event TextureChangeEventHandler TextureChanged;

    protected virtual void OnTextureChanged(SUIImage sender, Asset<Texture2D> newTexture2D, Asset<Texture2D> oldTexture2D) =>
        TextureChanged?.Invoke(sender, newTexture2D, oldTexture2D);

    #endregion

    public Vector2 ImageOffset = Vector2.Zero;
    public Vector2 ImagePercent = Vector2.Zero;
    public Vector2 ImageAlign = Vector2.Zero;
    public Vector2 ImageOriginPercent = Vector2.Zero;

    /// <summary> 图片缩放 </summary>
    public Vector2 ImageScale { get; set; } = Vector2.One;

    /// <summary> 图片染色 </summary>
    public Color ImageColor { get; set; } = Color.White;

    public Rectangle? SourceRectangle { get; set; }

    public SUIImage(Asset<Texture2D> texture)
    {
        ArgumentNullException.ThrowIfNull(texture);

        Texture2D = texture;
        FitWidth = true;
        FitHeight = true;
    }

    public override void Prepare(float? width, float? height)
    {
        base.Prepare(width, height);

        if (Texture2D == null || Texture2D.Value == null) return;

        if (FitWidth)
        {
            DefineInnerBoundsWidth(Texture2D.Value.Width);
        }

        if (FitHeight)
        {
            DefineInnerBoundsWidth(Texture2D.Value.Height);
        }
    }

    protected override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
    {
        base.Draw(gameTime, spriteBatch);

        if (Texture2D.Value is null) return;

        var position = InnerBounds.Position;
        var size = (Vector2)InnerBounds.Size;

        var imageOriginalSize = ImageOriginalSize;
        var completeOffset = ImageOffset + size * ImagePercent + (size - imageOriginalSize * ImageScale) * ImageAlign;

        spriteBatch.Draw(Texture2D.Value, position + completeOffset, SourceRectangle,
            ImageColor, 0f, imageOriginalSize * ImageOriginPercent, ImageScale, 0f, 0f);
    }
}