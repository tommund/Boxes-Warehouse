using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;

namespace BoxesAssignmentLogic.dataStructres
{
    class DoublyLinkList<T> 
    {
        private Node _start;
        private Node _end;
        internal Node Start { get { return _start; }}
        internal Node Last { get { return _end; }}

        internal class Node
        {
            public T _data;
            public Node _next;
            public Node _previous;

            internal  Node(T data)
            {
                _data = data;
                //next == null;
                // _previous = null;
            }
        }

        internal void DeleteNode(Node del)
        {
            // Base case
            if (_start == null || del == null) return;
            
            // If node to be deleted is start node
            if (_start == del) _start = del._next;
            
            // Change next only if node to be deleted
            // is NOT the last node
            if (del._next != null) del._next._previous = del._previous;

            // Change prev only if node to be deleted
            // is NOT the first node
            if (del._previous != null) del._previous._next = del._next;
            del._next = null;
            del._previous = null;

            return;
        }

        internal void DeleteNodeAndMakeFirst(Node del)
        {
            // Base case
            if (_start == null) return;


            // If node to be deleted is start node,leave it at start position
            if (_start == del) return;

            // Change next only if node to be deleted
            // is NOT the last node
            if (del._next != null) del._next._previous = del._previous;

            // Change prev only if node to be deleted
            // is NOT the first node
            if (del._previous != null) del._previous._next = del._next;
            del._next = null;
            del._previous = null;

            MakeExistingNodeTobeFirst(ref del);
            return;
        }

        internal void AddFirstNewNode(T val)
        {
            Node n = new Node(val);

            if (_start != null)
            {
                n._next = _start;
                _start._previous = n;
            }
            _start = n;
            if (_end == null)
                _end = n;
        }
        private void MakeExistingNodeTobeFirst(ref Node n)
        {
            if (_start != null)
            {
                n._next = _start;
                _start._previous = n;
            }
            _start = n;
            if (_end == null)
                _end = n;
        }
        internal bool RemoveFirst(out T saveFirstValue)
        {
            if (_start == null)
            {
                saveFirstValue = default;
                return false;
            }
            saveFirstValue = _start._data;
            if (_start._next != null)
            {
                _start = _start._next;
                _start._previous = null;
            }
            else
            {
                _start = null;
                _end = null;
            }
            return true;
        }
        internal bool RemoveLast(out T saveLastValue)
        {
            if (_end == null)
            {
                saveLastValue = default;
                return false;
            }
            saveLastValue = _end._data;
            if(_start.Equals(_end))
            {
                _start = null;
                _end = null;
                
            }
            else
            {
                _end._previous._next = null;
                _end = _end._previous;
            }
            return true;
        }
        
    }
}
