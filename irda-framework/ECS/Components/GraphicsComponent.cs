using Microsoft.Xna.Framework;

namespace irda
{
    public class GraphicsComponent : Component
    {
        public Dictionary<string, DrawableObj> Drawables { get; private set; }

        public override string Name => GetType().Name;
        public GraphicsComponent(Entity owner)
            : base(owner)
        {
            Drawables = new Dictionary<string, DrawableObj>();            

            var nodeComponent = GetComponent<NodeComponent>("NodeComponent");
            if (nodeComponent == null)
            {
                throw new Exception("Could not add a GraphicsComponent without a NodeComponent !");
            }
        }

        public void AttachDrawable(string name, DrawableObj drawable)
        {
            Drawables.Add(name, drawable);
        }

        public override void Update(GameTime gameTime)
        {
            var nodeComponent = GetComponent<NodeComponent>("NodeComponent");

            if (nodeComponent != null)
            {
                foreach (var d in Drawables.Values)
                {
                    d.SetGlobalPosition(nodeComponent.Position + d.Position);
                }
            }
        }

        public override void Draw(GameTime gameTime)
        {
            if (Drawables.Count != 0) 
            {
                foreach (var d in Drawables.Values)
                {
                    d.Draw();
                }
            }
            else {
                throw new Exception("Drawable object not set !");
            }
        }
    }
}
