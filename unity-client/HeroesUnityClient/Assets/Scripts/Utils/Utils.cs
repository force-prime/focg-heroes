using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public class EntityAndCount<TEntityKey>
    {
        static public readonly EntityAndCount<TEntityKey> EMPTY = new EntityAndCount<TEntityKey>();

        private readonly Dictionary<TEntityKey, int> _dict = new Dictionary<TEntityKey, int>();

        public bool HasEnough(EntityAndCount<TEntityKey> entityAndCount)
        {
            foreach (var key in entityAndCount._dict.Keys)
            {
                var myValue = _dict.GetValueOrDefault(key);
                if (myValue < entityAndCount._dict[key])
                    return false;
            }
            return true;
        }

        public void Multiply(int count)
        {
            List<TEntityKey> keys = _dict.Keys.ToList();

            foreach (var key in keys)
                _dict[key] = _dict[key] * count;
        }

        public bool IsEmpty()
        {
            foreach (var keyValue in _dict)
                if (keyValue.Value != 0)
                    return false;

            return true;
        }

        public int Get(TEntityKey key)
        {
            return _dict.GetValueOrDefault(key);
        }

        public void Set(TEntityKey key, int value)
        {
            _dict[key] = value;
        }

        public void Add(TEntityKey key, int value)
        {
            _dict[key] = _dict.GetValueOrDefault(key, 0) + value;
        }

        public void Sub(TEntityKey key, int value)
        {
            _dict[key] = _dict.GetValueOrDefault(key, 0) - value;
        }

        public void Add(EntityAndCount<TEntityKey> entityAndCount)
        {
            foreach (var keyValue in entityAndCount._dict)
                Add(keyValue.Key, keyValue.Value);
        }

        public void Sub(EntityAndCount<TEntityKey> entityAndCount)
        {
            foreach (var keyValue in entityAndCount._dict)
                Sub(keyValue.Key, keyValue.Value);
        }

        public void Iterate(Action<TEntityKey, int> action)
        {
            foreach (var keyValue in _dict)
            {
                var value = keyValue.Value;
                if (value != 0)
                    action(keyValue.Key, keyValue.Value);
            }
        }

        public void GetKeys(List<TEntityKey> results)
        {
            results.AddRange(_dict.Keys);
        }
    }

    public class MessageCenter : MessageCenter.IMessageCenter
    {
        public delegate void Handler<in TMessage>(TMessage message);
        public interface IMessageCenter
        {
            void Subscribe<TMessage>(Handler<TMessage> handler);
            void Unsubscribe<TMessage>(Handler<TMessage> handler);
        }

        private readonly Dictionary<Type, object> _callbacks = new Dictionary<Type, object>();

        public void Send<TMessage>(TMessage messageData)
        {
            if (_callbacks.TryGetValue(typeof(TMessage), out var holder))
                ((EventHolder<TMessage>)holder).Handle(messageData);
        }

        public void Subscribe<TMessage>(Handler<TMessage> handler)
        {
            object holder;
            if (!_callbacks.TryGetValue(typeof(TMessage), out holder))
            {
                holder = new EventHolder<TMessage>();
                _callbacks.Add(typeof(TMessage), holder);
            }
            ((EventHolder<TMessage>)holder).Add(handler);
        }

        public void Unsubscribe<TMessage>(Handler<TMessage> handler)
        {
            if (_callbacks.TryGetValue(typeof(TMessage), out var holder))
                ((EventHolder<TMessage>)holder).Remove(handler);
        }

        private class EventHolder<TMessage>
        {
            private event Handler<TMessage> _event;

            public void Add(Handler<TMessage> handler)
            {
                _event += handler;
            }

            public void Remove(Handler<TMessage> handler)
            {
                _event -= handler;
            }

            public void Handle(TMessage data)
            {
                _event?.Invoke(data);
            }

        }
    }

    public enum Direction
    {
        None,
        Left,
        Right,
        Up,
        Down,
    }

}