using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace irda
{
    public enum DrawMode
    {
        Fill,
        Line
    };

    public abstract class DrawableObj
    {
        // TODO put in order like in the Draw function of monogame
        public Texture2D Texture { get; }
        public Vector2 GlobalPosition { get; private set; }
        public Vector2 Position { get; private set; }
        public Point Size { get; private set; }
        public Color Color { get; private set; }
        public float Rotation { get; private set; }
        public Vector2 Origin { get; private set; }
        public int Scale { get; private set; }
        public float DepthLayer { get; private set; }

        protected DrawableObj(Texture2D texture, Point size, Color color)
        {
            Texture = texture;
            GlobalPosition = Vector2.Zero;
            Position = Vector2.Zero;
            Size = size;
            Color = color;
            Rotation = 0.0f;
            Origin = Vector2.Zero;
            Scale = 1;
            DepthLayer = 0.0f;
        }

        public void SetGlobalPosition(Vector2 position) => GlobalPosition = position;
        public void SetPosition(Vector2 position) => Position = position;
        public void SetSize(Point size) => Size = size;
        public void SetColor(Color color) => Color = color;
        public void SetRotation(float rotation) => Rotation = rotation;
        public void SetOrigin(Vector2 origin) => Origin = origin;
        public void SetScale(int scale)
        {
            Size = new Point(Size.X * scale, Size.Y * scale);
            Scale = scale;
        }
        public void SetLayer(float layer) => DepthLayer = layer;

        public abstract void Draw();
    }

    public class RectangleObj : DrawableObj
    {
        public DrawMode Mode { get; private set; }

        public RectangleObj(Point size, Color color) 
            : base(Renderer.GetInstance().GenerateDefaultTexture(), size, color)
        {
            Mode = DrawMode.Fill;
        }

        public void SetDrawMode(DrawMode mode) => Mode = mode;

        public override void Draw()
        {
            Renderer.GetInstance().Draw(this);
        }
    }
    public class CrossObj : DrawableObj
    {
        public CrossObj(int size, Color color)
            : base(Renderer.GetInstance().GenerateDefaultTexture(), new Point(size), color)
        {
        }

        public override void Draw()
        {
            Renderer.GetInstance().Draw(this);
        }
    }
    public class CircleObj : DrawableObj
    {

        public CircleObj(int size, Color color) 
            : base(Renderer.GetInstance().GenerateDefaultTexture(), new Point(size), color)
        {
        }

        public override void Draw()
        {
            Renderer.GetInstance().Draw(this);
        }
    }

    public class CircleObjEx : DrawableObj
    {
        public CircleObjEx(int radius, Color color)
            : base(Renderer.GetInstance().GenerateDefaultCircleTexture(radius), Point.Zero, color)
        {
        }

        public override void Draw()
        {
            Renderer.GetInstance().Draw(this);
        }
    }

    public class TextObj : DrawableObj
    {
        public SpriteFont Font { get; private set; }
        public String Text { get; private set; }        

        public TextObj(String text, /*Size size,*/ Color color) 
            : base(null, Point.Zero, color)
        {
            Font = Renderer.GetInstance().GenerateDefaultFont();
            Text = text;
        }

        public void SetFont(SpriteFont font) => Font = font;

        public override void Draw()
        {
            Renderer.GetInstance().Draw(this);
        }
    }

    public class SpriteObj : DrawableObj
    {
        public SpriteObj(Texture2D texture)
            : base(texture, new Point(texture.Width, texture.Height), Color.White)
        {
        }

        public override void Draw()
        { 
            Renderer.GetInstance().Draw(this);
        }
    }

    public class AnimatedSpriteObj : DrawableObj
    {
        public AnimatedSpriteObj(Texture2D texture, Point size, Color color) 
            : base(texture, size, color)
        {
        }

        public override void Draw()
        {
            throw new NotImplementedException();
        }
    }

    public class TileMapObj : DrawableObj
    {
        public int[,] Tiles { get; private set; }
        public Point TileMapSize { get; private set; }

        public TileMapObj(Texture2D texture, Point mapSize) 
            : base(texture, Point.Zero, Color.White)
        {
            TileMapSize = mapSize;
            Tiles = new int[mapSize.X, mapSize.Y];
        }

        public void SetTile(Point pos, int tile) => Tiles[pos.X, pos.Y] = tile;
        public void SetTileMapSize(Point size)
        {
            TileMapSize = size;
            Tiles = new int[size.X, size.Y];
        }

        public void SetTiles(int[,] tiles, Point size)
        {
            TileMapSize = size;
            Tiles = tiles;
        }

        public void AddLine(int line, int index)
        {
            for (int x = 0; x < TileMapSize.X; ++x)
            {
                Tiles[x, line] = index;
            }
        }
        
        public void AddRow(int row, int index)
        {
            for (int y = 0; y < TileMapSize.Y; ++y) 
            {
                Tiles[row, y] = index;
            }
        }

        public override void Draw()
        {
            Renderer.GetInstance().Draw(this);
        }
    }
}