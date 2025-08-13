using Microsoft.Xna.Framework;
using System;

namespace irda
{
    public abstract class State
    {
        public bool Enable { get; protected set; }
        public String Name { get; }

        public State(String name, bool active)
        {
            Enable = active;
            Name = name;
        }

        public State(String name)
        {
            Enable = false;
            Name = name;
        }

        public void SetActive(bool active) => Enable = active;

        public abstract void OnAttach();
        public abstract void OnDetach();
        public abstract void Update(GameTime gameTime);
        public abstract void Draw(GameTime gameTime);
    }

    public sealed class StateMachine
    {
        private static List<State> m_states;

        private StateMachine()
        {
            m_states = new List<State>();
        }

        private static Lazy<StateMachine> m_instance = new Lazy<StateMachine>(InitializeInstance, LazyThreadSafetyMode.ExecutionAndPublication);
        private static StateMachine InitializeInstance()
        {
            return new StateMachine();
        }

        public static StateMachine GetInstance()
        {
            return m_instance.Value;
        }

        public void AddState(State state) 
        {
            m_states.Add(state);
            if (Renderer.GetInstance().Device != null) 
            {
                state.OnAttach();
            }
        }

        public void RemoveState(String name)
        {
            var state = m_states.Find(s => s.Name == name);

            if (state != null)
            {              
                state.OnDetach();
                m_states.Remove(state);
            }
        }

        public void RemoveAll()
        {
            m_states.ForEach(s => s.OnDetach());
            m_states.Clear();
        }

        public void AttachAll()
        {
            m_states.ForEach(s => s.OnAttach());
        }

        public void Update(GameTime gameTime)
        {
            m_states.ForEach(s => s.Update(gameTime));
        }

        public void Draw(GameTime gameTime)
        {
            m_states.ForEach(s => s.Draw(gameTime));
        }

        public void Clear()
        {
            m_states.ForEach(s => s.OnDetach());
            m_states.Clear();
        }
    }
}
