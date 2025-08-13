
using ImGuiNET;
using Microsoft.Xna.Framework;
using System.ComponentModel.DataAnnotations;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;

namespace irda
{
    public sealed class ECS
    {
        private static List<Entity> m_entities = new List<Entity>();
        private bool m_debugDrawer;

        private ECS()
        {
            m_entities = new List<Entity>();
            m_debugDrawer = false;
        }

        private static Lazy<ECS> m_instance = new Lazy<ECS>(InitializeInstance, LazyThreadSafetyMode.ExecutionAndPublication);
        private static ECS InitializeInstance()
        {
            return new ECS();
        }

        public static ECS GetInstance()
        {
            return m_instance.Value;
        }

        public Entity CreateEntity(string name)
        {
            var e = new Entity(name);
            m_entities.Add(e);
            return e;
        }

        public void AddEntity(Entity entity)
        {
            m_entities.Add(entity);
        }

        public void Clear()
        {
            m_entities.Clear();
        }

        public void EnableDebugDrawer(bool active)
        {
            m_debugDrawer = active;
        }

        public void Update(GameTime gameTime)
        {
            m_entities.ForEach(e => e.Update(gameTime));
        }

        string selectedEntityName;
        public void Draw(GameTime gameTime)
        {
            foreach (var e in m_entities)
            {
                e.Draw(gameTime);

                if (m_debugDrawer)
                {
                    ImGui.Begin("Editor ECS");

                    ImGuiTreeNodeFlags flags = (e.Name == selectedEntityName) ? ImGuiTreeNodeFlags.Selected : ImGuiTreeNodeFlags.OpenOnArrow;
                    bool selected = ImGui.TreeNodeEx(e.Name, flags);

                    if (ImGui.IsItemClicked())
                    {
                        selectedEntityName = e.Name;
                    }

                    if (selected)
                    {
                        ImGui.TreePop();
                    }
                    ImGui.End();


                    ImGui.Begin("Properties");

                    if (selected)
                    {
                        ImGui.InputText("Tag", Encoding.ASCII.GetBytes(e.Name), 256);

                        if (e.HasComponenet<NodeComponent>("NodeComponent"))
                        {
                            var node = e.GetComponent<NodeComponent>("NodeComponent");
                            if (ImGui.TreeNodeEx("NodeComponent", ImGuiTreeNodeFlags.DefaultOpen))
                            {
                                bool debug = node.DebugDrawer;
                                if (ImGui.Checkbox("DebugDrawer", ref debug))
                                {
                                    if (node.DebugDrawer)
                                        node.EnableDebugDrawer(false);
                                    else
                                        node.EnableDebugDrawer(true);
                                }

                                System.Numerics.Vector2 vec = new (node.Position.X, node.Position.Y);
                                if (ImGui.DragFloat2("Position", ref vec))
                                {
                                    node.SetPosition(vec);

                                }
                                ImGui.TreePop();
                            }                                
                        }

                        if (e.HasComponenet<GraphicsComponent>("GraphicsComponent"))
                        {
                            var gfx = e.GetComponent<GraphicsComponent>("GraphicsComponent");
                            if (ImGui.TreeNodeEx("GraphicsComponent", ImGuiTreeNodeFlags.DefaultOpen))
                            {
                                foreach (var objName in gfx.Drawables.Keys)
                                {
                                    if (ImGui.TreeNodeEx(objName, ImGuiTreeNodeFlags.None))
                                    {
                                        var drawable = gfx.Drawables[objName];

                                        System.Numerics.Vector2 position = new(drawable.Position.X, drawable.Position.Y);
                                        if (ImGui.DragFloat2("Position", ref position))
                                        {
                                            drawable.SetPosition(position);
                                        }

                                        System.Numerics.Vector4 color = new(drawable.Color.R, drawable.Color.G, drawable.Color.B, drawable.Color.A);
                                        if (ImGui.ColorEdit4("Color", ref color, ImGuiColorEditFlags.PickerHueWheel))
                                        {
                                            drawable.SetColor(new Color(color));
                                        }

                                        ImGui.TreePop();
                                    }

                                }
                            }
                        }
                    }

                    ImGui.End();
                }
            }
        }
    }
}
