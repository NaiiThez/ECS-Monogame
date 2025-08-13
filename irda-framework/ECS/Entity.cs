using Microsoft.Xna.Framework;

namespace irda
{
    public class Entity
    {
        public string Name { get; private set; }
        private readonly List<Component> m_components;

        public Entity(string name)
        {
            Name = name;    
            m_components = new List<Component>();   
        }

        public TComponent AddComponent<TComponent>(TComponent component) where TComponent : Component 
        {
            m_components.Add(component);
            return component;        
        }

        public void RemoveComponent(Component component)
        {
            if (m_components.Exists(c => component != null))
                m_components.Remove(component);
        }        
        
        public TComponent GetComponent<TComponent>(string componentName) where TComponent : Component
        {
            return m_components.Find(c => c.Name == componentName) as TComponent;
        }

        public bool HasComponenet<TComponent>(string componentName) where TComponent : Component
        {
            return m_components.Exists(c => c.Name == componentName);
        }

        public virtual void Update(GameTime gameTime)
        {
            foreach (var component in m_components)
                component.Update(gameTime);
        }

        public virtual void Draw(GameTime gameTime)
        {
            m_components.ForEach(c => c.Draw(gameTime));  
        }
    }
}