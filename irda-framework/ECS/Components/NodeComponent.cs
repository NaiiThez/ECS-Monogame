using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace irda
{
    public class NodeComponent : Component
    {
        public Vector2 Position { get; private set; }
        public bool DebugDrawer { get; private set; }
        private Texture2D m_debugTexture;

        public override string Name => GetType().Name;
        public NodeComponent(Entity owner)
            : base(owner)
        {
            m_debugTexture = new Texture2D(Renderer.GetInstance().Device, 1, 1);
            m_debugTexture.SetData<Color>(new Color[] { Color.White });
            Position = Vector2.Zero;
            DebugDrawer = false;
        }

        public void EnableDebugDrawer(bool active) => DebugDrawer = active;

        public void SetPosition(Vector2 position)
        {
            Position = position;
        }

        public override void Update(GameTime gameTime)
        {
        }

        public override void Draw(GameTime gameTime)
        {
            if (DebugDrawer)
            {
                var debugDrawer = new CrossObj(10, Color.Purple);
                debugDrawer.SetGlobalPosition(Position);          
                debugDrawer.SetLayer(1.0f);

                Renderer.GetInstance().Draw(debugDrawer);            
            }
        }
    }
}
