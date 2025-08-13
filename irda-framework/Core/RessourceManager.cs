using Microsoft.Xna.Framework.Content;

namespace irda
{
    public sealed class ResourceManager
    {
        public ContentManager Content { get; private set; }

        private ResourceManager()
        {

        }

        private static Lazy<ResourceManager> m_instance = new Lazy<ResourceManager>(InitializeInstance, LazyThreadSafetyMode.ExecutionAndPublication);
        private static ResourceManager InitializeInstance() => new ResourceManager();
       
        public static ResourceManager GetIntance() => m_instance.Value;
        public void SetContentManager(ContentManager content) => Content = content;
    }
}
