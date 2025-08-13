using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace irda
{
    public sealed class Renderer
    {
        public GraphicsDevice Device { get; private set; }
        public SpriteBatch Batch { get; private set; }

        private Renderer()
        {
        }

        private static Lazy<Renderer> m_renderer = new(InitializeInstance, LazyThreadSafetyMode.ExecutionAndPublication);
        private static Renderer InitializeInstance()
        {
            return new Renderer();
        }

        public static Renderer GetInstance() => m_renderer.Value;

        public void SetGraphicsDevice(GraphicsDevice device) => Device = device;
        public void SetSpriteBatch(SpriteBatch spriteBatch) => Batch = spriteBatch;

        public Texture2D GenerateDefaultTexture()
        {
            var texture = new Texture2D(Device, 1, 1);
            texture.SetData<Color>(new Color[] { Color.White });
            return texture;
        }

        public Texture2D GenerateDefaultCircleTexture(int outerRadius)
        {
            int radius = (outerRadius - 1) / 2;
            int innerRadius = (3 * radius) / 4;
            int crossWidth = 5;
            Texture2D texture = new Texture2D(Device, outerRadius, outerRadius, mipmap: false, SurfaceFormat.Color);

            Color[] data = new Color[outerRadius * outerRadius];

            for (int x = -radius; x <= radius; x++)
            {
                for (int y = -radius; y <= radius; y++)
                {
                    int i = (x + radius) * outerRadius + (y + radius);
                    data[i] = Color.Transparent; //default
                    int r2 = x * x + y * y;
                    if (r2 <= radius * radius)
                    {
                        if (r2 > innerRadius * innerRadius)
                        {   //ring
                            data[i] = Color.White;
                        }
                        if ((x - y) * (x - y) < crossWidth * crossWidth)
                        {   //part of cross lower-left to upper-right
                            data[i] = Color.White;
                        }
                        if ((x + y) * (x + y) < crossWidth * crossWidth)
                        {   //part of cross lower-right to upper-left
                            data[i] = Color.White;
                        }
                    }

                }
            }

            texture.SetData(data);
            return texture;
        }

        public SpriteFont GenerateDefaultFont() => ResourceManager.GetIntance().Content.Load<SpriteFont>("defaultFont");

        public void Draw(RectangleObj drawable)
        {
            if (drawable.Mode == DrawMode.Fill)
            {
                Batch.Draw(
                    drawable.Texture,
                    new Rectangle((int)drawable.GlobalPosition.X, (int)drawable.GlobalPosition.Y, drawable.Size.X, drawable.Size.Y),
                    new Rectangle(0, 0, drawable.Texture.Width, drawable.Texture.Height)/*destinationRect*/,
                    drawable.Color,
                    drawable.Rotation,
                    drawable.Origin,
                    SpriteEffects.None/*SpriteEffect*/,
                    drawable.DepthLayer);
            }
            else
            {
                Batch.Draw(
                    drawable.Texture,
                    new Rectangle((int)drawable.GlobalPosition.X, (int)drawable.GlobalPosition.Y, drawable.Size.X, 2),
                    new Rectangle(0, 0, drawable.Texture.Width, drawable.Texture.Height)/*destinationRect*/,
                    drawable.Color,
                    0.0f/*rotation*/,
                    Vector2.Zero/*origin*/,
                    SpriteEffects.None/*SpriteEffect*/,
                    drawable.DepthLayer);

                Batch.Draw(
                    drawable.Texture,
                    new Rectangle((int)drawable.GlobalPosition.X, (int)drawable.GlobalPosition.Y, 2, drawable.Size.Y),
                    new Rectangle(0, 0, drawable.Texture.Width, drawable.Texture.Height)/*destinationRect*/,
                    drawable.Color,
                    0.0f/*rotation*/,
                    Vector2.Zero/*origin*/,
                    SpriteEffects.None/*SpriteEffect*/,
                    drawable.DepthLayer);


                Batch.Draw(
                    drawable.Texture,
                    new Rectangle((int)drawable.GlobalPosition.X, (int)drawable.GlobalPosition.Y + drawable.Size.Y, drawable.Size.X, 2),
                    new Rectangle(0, 0, drawable.Texture.Width, drawable.Texture.Height)/*destinationRect*/,
                    drawable.Color,
                    0.0f/*rotation*/,
                    Vector2.Zero/*origin*/,
                    SpriteEffects.None/*SpriteEffect*/,
                    drawable.DepthLayer);

                Batch.Draw(
                    drawable.Texture,
                    new Rectangle((int)drawable.GlobalPosition.X + drawable.Size.X, (int)drawable.GlobalPosition.Y, 2, drawable.Size.Y + 2),
                    new Rectangle(0, 0, drawable.Texture.Width, drawable.Texture.Height)/*destinationRect*/,
                    drawable.Color,
                    0.0f/*rotation*/,
                    Vector2.Zero/*origin*/,
                    SpriteEffects.None/*SpriteEffect*/,
                    drawable.DepthLayer);

            }
        }

        public void Draw(TextObj drawable)
        {
            Batch.DrawString(drawable.Font,
                             drawable.Text,
                             drawable.GlobalPosition,
                             drawable.Color,
                             0.0f/*rotation*/,
                             Vector2.Zero/*origin*/,
                             1.0f/*scale*/,
                             SpriteEffects.None,
                             drawable.DepthLayer);
        }

        public void Draw(CrossObj drawable)
        {
            Batch.Draw(
                 drawable.Texture,
                 new Rectangle((int)drawable.GlobalPosition.X, (int)drawable.GlobalPosition.Y, drawable.Size.X, 1),
                 new Rectangle(0, 0, drawable.Texture.Width, drawable.Texture.Height)/*destinationRect*/,
                 drawable.Color,
                 0.0f/*rotation*/,
                 Vector2.Zero/*origin*/,
                 SpriteEffects.None/*SpriteEffect*/,
                 drawable.DepthLayer);

            Batch.Draw(
                 drawable.Texture,
                 new Rectangle((int)drawable.GlobalPosition.X, (int)drawable.GlobalPosition.Y, 1, drawable.Size.Y),
                 new Rectangle(0, 0, drawable.Texture.Width, drawable.Texture.Height)/*destinationRect*/,
                 drawable.Color,
                 0.0f/*rotation*/,
                 Vector2.Zero/*origin*/,
                 SpriteEffects.None/*SpriteEffect*/,
                 drawable.DepthLayer);

            Batch.Draw(
                 drawable.Texture,
                 new Rectangle((int)drawable.GlobalPosition.X, (int)drawable.GlobalPosition.Y + 1, drawable.Size.X, 1),
                 new Rectangle(0, 0, drawable.Texture.Width, drawable.Texture.Height)/*destinationRect*/,
                 drawable.Color,
                 MathHelper.ToRadians(-180.0f)/*rotation*/,
                 Vector2.Zero/*origin*/,
                 SpriteEffects.None/*SpriteEffect*/,
                 drawable.DepthLayer);

            Batch.Draw(
                 drawable.Texture,
                 new Rectangle((int)drawable.GlobalPosition.X + 1, (int)drawable.GlobalPosition.Y, 1, drawable.Size.Y),
                 new Rectangle(0, 0, drawable.Texture.Width, drawable.Texture.Height)/*destinationRect*/,
                 drawable.Color,
                 MathHelper.ToRadians(-180.0f)/*rotation*/,
                 Vector2.Zero/*origin*/,
                 SpriteEffects.None/*SpriteEffect*/,
                 drawable.DepthLayer);
        }

        public void Draw(CircleObj drawable)
        {
            var pos = Vector2.Zero;
            for (int i = 0; i < 360; ++i)
            {
                pos = new Vector2((float)Math.Cos(MathHelper.ToRadians(i)) * 50.0f, (float)Math.Sin(MathHelper.ToRadians(i)) * 50.0f);

                Batch.Draw(
                    drawable.Texture,
                    new Rectangle((int)(drawable.GlobalPosition.X + pos.X), (int)(drawable.GlobalPosition.Y + pos.Y), 1, 1),
                    new Rectangle(0, 0, drawable.Texture.Width, drawable.Texture.Height),
                    drawable.Color,
                    0.0f/*rotation*/,
                    Vector2.Zero/*origin*/,
                    SpriteEffects.None,
                    drawable.DepthLayer);
            }
        }

        public void Draw(CircleObjEx drawable)
        {
            Batch.Draw(drawable.Texture, new Rectangle((int)drawable.GlobalPosition.X, (int)drawable.GlobalPosition.Y, drawable.Texture.Width, drawable.Texture.Height), drawable.Color);
        }

        public void Draw(SpriteObj drawable)
        {
            Batch.Draw(
                drawable.Texture,
                new Rectangle((int)drawable.GlobalPosition.X, (int)drawable.GlobalPosition.Y, drawable.Size.X, drawable.Size.Y),
                new Rectangle(0, 0, drawable.Texture.Width, drawable.Texture.Height),
                drawable.Color,
                0.0f/*rotation*/,
                Vector2.Zero/*origin*/,
                SpriteEffects.None,
                drawable.DepthLayer);
        }

        public void Draw(TileMapObj drawable)
        {
            for (int x = 0; x < drawable.Size.X; ++x)
            {
                for (int y = 0; y < drawable.Size.Y; ++y)
                {
                    var index = drawable.Tiles[x, y];
                    var x_tile = index % drawable.TileMapSize.X;
                    var y_tile = index / drawable.TileMapSize.X;

                    Batch.Draw(
                        drawable.Texture,
                        new Rectangle((int)drawable.GlobalPosition.X + x * (int)(drawable.Size.X * drawable.Scale), (int)drawable.GlobalPosition.Y + y * (int)(drawable.Size.Y * drawable.Scale), (int)(drawable.Size.X * drawable.Scale), (int)(drawable.Size.Y * drawable.Scale)),
                        new Rectangle(x_tile * drawable.Size.X, y_tile * drawable.Size.Y, drawable.Size.X, drawable.Size.Y),
                        drawable.Color,
                        0.0f/*rotation*/,
                        Vector2.Zero/*origin*/,
                        SpriteEffects.None,
                        drawable.DepthLayer);
                }
            }
        }
    }
}
