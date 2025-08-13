using irda;
using ImGuiNET;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;
using System;

internal class Program
{
    class MapComponent : Component
    {
        public class Room
        {
            public Room(int x, int y)
            {
                position = new Point(x, y);
            }

            public Point position { get; private set; }
            public bool isEnable { get; private set; }
            public bool isLeftDoorOpen { get; private set; }
            public bool isRightDoorOpen { get; private set; }
            public bool isUpperDoorOpen { get; private set; }
            public bool isLowerDoorOpen { get; private set; }

            public void Enable()
            {
                isEnable = true;
            }

            public void OpenUpperDoor() { isUpperDoorOpen = true; }
            public void OpenLowerDoor() { isLowerDoorOpen = true; }
            public void OpenRightDoor() { isRightDoorOpen = true; }
            public void OpenLeftDoor() { isLeftDoorOpen = true; }
        }

        private Vector2 m_size;
        public Room[,] Map { get; private set; }
        public Room m_startingRoom { get; private set; }

        public override string Name => GetType().Name;
        public MapComponent(Entity owner) 
            : base(owner)
        {
        }

        public void GenerateDungeon(int width, int height, int nbRooms)
        {
            m_size = new Vector2(width, height);
            Map = new Room[width, height];

            if (nbRooms >= Map.Length)
                nbRooms = Map.Length;

            for (int i = 0; i < width; ++i)
            {
                for (int j = 0; j < height; ++j)
                {
                    Map[i, j] = new Room(i, j);
                }
            }

            var random = new Random();
            m_startingRoom = Map[random.Next(0, width), random.Next(0, height)];
            m_startingRoom.Enable();

            List<Room> rooms = new List<Room>(nbRooms);
            rooms.Add(m_startingRoom);

            while (rooms.Count < nbRooms)
            {
                int i = random.Next(0, rooms.Count);
                int direction = random.Next(0, 4);
                int x = (int)rooms[i].position.X;
                int y = (int)rooms[i].position.Y;
                Room currentRoom = rooms[i];
                Room newRoom;

                if (direction == 0 && y > 0)
                {
                    newRoom = Map[x, y - 1];
                    if (!newRoom.isEnable)
                    {
                        currentRoom.OpenUpperDoor();
                        newRoom.Enable();
                        newRoom.OpenLowerDoor();
                        rooms.Add(newRoom);
                    }
                }
                if (direction == 1 && x < width - 1)
                {
                    newRoom = Map[x + 1, y];
                    if (!newRoom.isEnable)
                    {
                        currentRoom.OpenRightDoor();
                        newRoom.Enable();
                        newRoom.OpenLeftDoor();
                        rooms.Add(newRoom);
                    }
                }
                if (direction == 2 && y < height - 1)
                {
                    newRoom = Map[x, y + 1];
                    if (!newRoom.isEnable)
                    {
                        currentRoom.OpenLowerDoor();
                        newRoom.Enable();
                        newRoom.OpenUpperDoor();
                        rooms.Add(newRoom);
                    }
                }
                if (direction == 3 && x > 0)
                {
                    newRoom = Map[x - 1, y];
                    if (!newRoom.isEnable)
                    {
                        currentRoom.OpenLeftDoor();
                        newRoom.Enable();
                        newRoom.OpenRightDoor();
                        rooms.Add(newRoom);
                    }
                }
            }

        }

        public override void Update(GameTime gameTime)
        {
        }      
        public override void Draw(GameTime gameTime)
        {
            int width = 33;
            int height = 15;
            int offset = 4;
            int x = 5;
            int y = 5;

            var rectangleObj = new RectangleObj(new Point(width, height), Color.White);
            var lineObj = new RectangleObj(Point.Zero, Color.Red);
            lineObj.SetLayer(0.1f);
            for (int line = 0; line < m_size.Y; ++line)
            {
                x = 5;
                for (int column = 0; column < m_size.X; ++column)
                {
                    Room room = Map[column, line];
                    if (room.isEnable)
                    {
                        if (room == m_startingRoom)
                            rectangleObj.SetColor(new Color(25, 255, 25));
                        else
                            rectangleObj.SetColor(Color.White);
                    }
                    else
                        rectangleObj.SetColor(new Color(60, 60, 60));
                        
                    rectangleObj.SetGlobalPosition(new Vector2(x, y));
                    Renderer.GetInstance().Draw(rectangleObj);

                    if (room.isUpperDoorOpen)
                    {
                        lineObj.SetGlobalPosition(new Vector2(x + (width - 2) / 2, y - 2));
                        lineObj.SetSize(new Point(4, 6));
                        Renderer.GetInstance().Draw(lineObj);                     
                    }
                    if (room.isLowerDoorOpen)
                    {
                        lineObj.SetGlobalPosition(new Vector2(x + (width - 2) / 2, y + height - 4));
                        lineObj.SetSize(new Point(4, 6));
                        Renderer.GetInstance().Draw(lineObj);
                    }                       
                    if (room.isRightDoorOpen)
                    {
                        lineObj.SetGlobalPosition(new Vector2(x + width - 4, y + (height - 2) / 2));
                        lineObj.SetSize(new Point(6, 4));
                        Renderer.GetInstance().Draw(lineObj);
                    }
                    if (room.isLeftDoorOpen) 
                    {
                        lineObj.SetGlobalPosition(new Vector2(x - 2, y + (height - 2) / 2));
                        lineObj.SetSize(new Point(6, 4));
                        Renderer.GetInstance().Draw(lineObj);   
                    }

                    x += width + offset;
                }

                y += height + offset;
            }
        }
    }

    class PlayerComponent : Component
    {
        public PlayerComponent(Entity owner)
            : base(owner)
        {

        }

        public override string Name => GetType().Name;

        public override void Draw(GameTime gameTime)
        {
            throw new NotImplementedException();
        }

        public override void Update(GameTime gameTime)
        {
            throw new NotImplementedException();
        }
    }

    class Player : Entity
    {       
        public Point RoomPosition { get; set; }
        private Vector2 m_position;

        public Player()
            : base("player")
        {
            ECS.GetInstance().AddEntity(this);

            m_position = new Vector2((1280 - 32) / 2, (720 - 32) / 2);

            AddComponent<NodeComponent>(new(this));
            var gfxComponent = AddComponent<GraphicsComponent>(new(this));

            var spriteObj = new CircleObjEx(32, Color.Red);
            spriteObj.SetPosition(m_position);
            spriteObj.SetLayer(0.8f);
            gfxComponent.AttachDrawable("sprite", spriteObj);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
        }
    }

    class SandboxState : State
    {
        private Entity m_dungeon;
        private Entity m_inventory;

        private Player m_player;

        public SandboxState()
            : base("SandboxState", true)
        {
        }

        public override void OnAttach()
        {
            m_inventory = ECS.GetInstance().CreateEntity("inventory");
            {
                m_inventory.AddComponent<NodeComponent>(new(m_inventory));
            }

            m_dungeon = ECS.GetInstance().CreateEntity("dungeon");
            {                
                m_dungeon.AddComponent<NodeComponent>(new(m_dungeon));
                m_dungeon.AddComponent<MapComponent>(new(m_dungeon));

                var gfxComponent = m_dungeon.AddComponent<GraphicsComponent>(new(m_dungeon));

                var spriteObj = new SpriteObj(ResourceManager.GetIntance().Content.Load<Texture2D>("map"));
                spriteObj.SetScale(2);
                spriteObj.SetPosition(new Vector2((1280 - spriteObj.Size.X)/2, (720 - spriteObj.Size.Y)/2));
                gfxComponent.AttachDrawable("sprite", spriteObj);

                var leftDoor = new RectangleObj(new(32, 32), Color.Black);
                leftDoor.SetPosition(new(spriteObj.Position.X, spriteObj.Position.Y + spriteObj.Size.Y/2));
                leftDoor.SetDrawMode(DrawMode.Line);
                leftDoor.SetLayer(0.5f);
                gfxComponent.AttachDrawable("leftDoor", leftDoor);

                var rightDoor = new RectangleObj(new(32, 32), Color.Black);
                rightDoor.SetPosition(new(spriteObj.Position.X + spriteObj.Size.X - 32, spriteObj.Position.Y + spriteObj.Size.Y / 2));
                rightDoor.SetDrawMode(DrawMode.Line);
                rightDoor.SetLayer(0.5f);
                gfxComponent.AttachDrawable("rightDoor", rightDoor);

                var upperDoor = new RectangleObj(new(32, 32), Color.Black);
                upperDoor.SetPosition(new(spriteObj.Position.X + spriteObj.Size.X/2, spriteObj.Position.Y));
                upperDoor.SetDrawMode(DrawMode.Line);
                upperDoor.SetLayer(0.5f);
                gfxComponent.AttachDrawable("upperDoor", upperDoor);

                var lowerDoor = new RectangleObj(new(32, 32), Color.Black);
                lowerDoor.SetPosition(new(spriteObj.Position.X + spriteObj.Size.X/2, spriteObj.Position.Y + spriteObj.Size.Y - 32));
                lowerDoor.SetDrawMode(DrawMode.Line);
                lowerDoor.SetLayer(0.5f);
                gfxComponent.AttachDrawable("lowerDoor", lowerDoor);
            }


            m_player = new Player();


            ECS.GetInstance().EnableDebugDrawer(true);
        }

        public override void OnDetach()
        {
            ECS.GetInstance().Clear();
        }     

        public override void Update(GameTime gameTime)
        {
            var dungeonComponent = m_dungeon.GetComponent<MapComponent>("MapComponent");
            var gfxComponent = m_dungeon.GetComponent<GraphicsComponent>("GraphicsComponent");
            if (dungeonComponent != null && dungeonComponent.Map != null)
            {
                var leftDoor = gfxComponent.Drawables["leftDoor"];
                var rightDoor = gfxComponent.Drawables["rightDoor"];
                var upperDoor = gfxComponent.Drawables["upperDoor"];
                var lowerDoor = gfxComponent.Drawables["lowerDoor"];

                if (dungeonComponent.Map[dungeonComponent.m_startingRoom.position.X, dungeonComponent.m_startingRoom.position.Y].isLeftDoorOpen == true)
                    leftDoor.SetColor(Color.Green);
                else
                    leftDoor.SetColor(Color.Black);

                if (dungeonComponent.Map[(int)dungeonComponent.m_startingRoom.position.X, (int)dungeonComponent.m_startingRoom.position.Y].isRightDoorOpen == true)
                    rightDoor.SetColor(Color.Green);
                else
                    rightDoor.SetColor(Color.Black);

                if (dungeonComponent.Map[(int)dungeonComponent.m_startingRoom.position.X, (int)dungeonComponent.m_startingRoom.position.Y].isUpperDoorOpen == true)
                    upperDoor.SetColor(Color.Green);
                else
                    upperDoor.SetColor(Color.Black);

                if (dungeonComponent.Map[(int)dungeonComponent.m_startingRoom.position.X, (int)dungeonComponent.m_startingRoom.position.Y].isLowerDoorOpen == true)
                    lowerDoor.SetColor(Color.Green);
                else
                    lowerDoor.SetColor(Color.Black);
            }
        }

        int width = 10;
        int height = 5;
        int nbRooms = 10;
        public override void Draw(GameTime gameTime)
        {
            var dungeonComponent = m_dungeon.GetComponent<MapComponent>("MapComponent");           

            ImGui.Begin("Editor");

            if (dungeonComponent != null) 
            {
                ImGui.DragInt("Width: ", ref width);
                ImGui.DragInt("Height: ", ref height);
                ImGui.DragInt("nbRooms: ", ref nbRooms);

                if (ImGui.Button("Generate"))
                {
                    dungeonComponent.GenerateDungeon(width, height, nbRooms);
                    m_player.RoomPosition = dungeonComponent.m_startingRoom.position;
                }
            }
            ImGui.End();          
        }
    }

    private static void Main(string[] args)
    {
        var app = new Application("ECS Testing", new(1280, 720));

        var stateMachine = StateMachine.GetInstance();
        stateMachine.AddState(new SandboxState());

        app.Run();
    }
}