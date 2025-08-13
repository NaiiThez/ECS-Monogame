using Microsoft.Xna.Framework;

namespace irda
{
    public abstract class Component
    {
        private Entity m_owner;
        public abstract string Name { get; }

        protected Component(Entity owner) 
        {
            m_owner = owner;
        }

        public void RemoveMe()
        {
            m_owner.RemoveComponent(this);
        }

        public TComponent GetComponent<TComponent>(string name) where TComponent : Component
        {
            return m_owner == null ? null : m_owner.GetComponent<TComponent>(name);
        }

        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }
}
