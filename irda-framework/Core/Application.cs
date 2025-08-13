using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using ImGuiNet.Monogame;

namespace irda
{
    public class Application : Game
    {
        private GraphicsDeviceManager m_graphics;
        private Color m_backgroundColor;
        private ImGuiRenderer m_imGuiRenderer;

        public Application()
        {            
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            m_backgroundColor = new Color(0.2f, 0.2f, 0.2f);
        }

        public Application(String title, Point size)
        {
            m_graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            IsMouseVisible = true;
            m_backgroundColor = new Color(0.2f, 0.2f, 0.2f);
            Window.Title = title;
            m_graphics.PreferredBackBufferWidth = size.X;
            m_graphics.PreferredBackBufferHeight = size.Y;
            m_graphics.ApplyChanges();
        }
           
        public void Borderless(bool active) => Window.IsBorderless = active;
        public void SetBackgroundColor(Color color) => m_backgroundColor = color;        
        public void SetSize(Point size)
        {
            m_graphics.PreferredBackBufferWidth = size.X;
            m_graphics.PreferredBackBufferHeight = size.Y;
            m_graphics.ApplyChanges();
        }        
    
        protected override void Initialize()
        {
            m_imGuiRenderer = new ImGuiRenderer(this);
            m_imGuiRenderer.RebuildFontAtlas();

            var renderer = Renderer.GetInstance();
            renderer.SetGraphicsDevice(GraphicsDevice);
            renderer.SetSpriteBatch(new SpriteBatch(GraphicsDevice));

            var resourceManager = ResourceManager.GetIntance();
            resourceManager.SetContentManager(Content);

            StateMachine.GetInstance().AttachAll();

            base.Initialize();
        }

        protected override void LoadContent()
        {

        }

        protected override void Update(GameTime gameTime)
        {
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed || Keyboard.GetState().IsKeyDown(Keys.Escape))
                Exit();

            StateMachine.GetInstance().Update(gameTime);
            ECS.GetInstance().Update(gameTime);

            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            Renderer.GetInstance().Device.Clear(m_backgroundColor);           

            m_imGuiRenderer.BeginLayout(gameTime);
            Renderer.GetInstance().Batch.Begin(SpriteSortMode.FrontToBack, BlendState.AlphaBlend, SamplerState.PointClamp);

            StateMachine.GetInstance().Draw(gameTime);
            ECS.GetInstance().Draw(gameTime);

            Renderer.GetInstance().Batch.End();
            m_imGuiRenderer.EndLayout();

            base.Draw(gameTime);          
        }
    }
}
