using Farmerchess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Farmerchess
{
    class Node<T>
    {
        private Node<T> _parent;
        private List<Node<T>> _children;
        private int _id;
        private T _data;
        private bool _isMax;

        public Node(Node<T> parent, bool isMax, T data)
        {
            _parent = parent;
            _children = new List<Node<T>>();
            _data = data;
            _isMax = isMax;
        }

        public Node<T> Parent
        {
            get { return _parent; }
            set { _parent = value; }
        }

        public int Count
        {
            get
            {
                return _children.Count;
            }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public bool IsMax
        {
            get { return _isMax; }
        }

        public T Data
        {
            get { return _data; }
            set { _data = value; }
        }

        public Node<T> Get(int id)
        {
            return _children.First(x => x.Id == id);
        }

        public void Add(Node<T> item)
        {
            if (item.Parent != this)
            {
                item.Parent = this;
            }
            item.Id = _children.Count + 1;
            _children.Add(item);
        }

        public void Remove(Node<T> item)
        {
            var node = Get(item.Id);
            if (node != null)
            {
                _children.Remove(node);
            }
        }
    }
}
