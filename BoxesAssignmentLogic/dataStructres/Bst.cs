using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace BoxesAssignmentLogic
{
    class Bst<T> where T : IComparable<T>
    {
        Node root;
        public void Add(T newItem) //O(logN)
        {
            Node n = new Node(newItem);

            if (root == null) //empty tree
            {
                root = n;
                return;
            }

            Node tmp = root;
            Node parent = null;

            while (tmp != null)
            {
                parent = tmp;
                if (newItem.CompareTo(tmp.data) < 0) tmp = tmp.left;
                else tmp = tmp.right;
            }

            if (newItem.CompareTo(parent.data) < 0) parent.left = n;
            else parent.right = n;
        }

        private bool FindValue(ref T value, ref Node tmp, ref Node parent)
        {
            while (tmp.data.CompareTo(value) != 0) // find the requested node and his parent
            {
                parent = tmp;
                if (value.CompareTo(tmp.data) < 0) tmp = tmp.left;
                else tmp = tmp.right;
                if (tmp == null) return false;   // the value wasnt found!!
            }
            return true;
        }

        internal bool IsExistAndUpdate(ref T value) //O(logN)
        {
            Node n = new Node(value);
            if (IsEmptyTree())
            {
                root = n;
                return false;
            } 
            Node tmp = root;
            Node parent = null;
            if(FindValue(ref value, ref tmp, ref parent))  //value was found
            {
                value = tmp.data;
                return true;
            }
            else
            { 
                InsertValue(ref value, ref n, ref parent);
                return false;
            }
        }

        internal bool IsExistAndShow(ref T value) //O(logN)
        {
            if (IsEmptyTree()) return false;
            Node tmp = root;
            Node parent = null;
            if (FindValue(ref value, ref tmp, ref parent))  //value was found
            {
                value = tmp.data;
                return true;
            }
            else return false; 
        }

        internal bool IsFindBestMatch( T value, ref T ReturnedValue) //O(logN)
        {
            if (IsEmptyTree()) return false;
            Node tmp = root;
            Node bestNode = null;
            KeepingBestOption( value,  tmp, ref bestNode);
            if(bestNode != null)
            {
                ReturnedValue = bestNode.data;
                return true;
            }
            return false;
        }

        private void KeepingBestOption( T value,  Node tmp, ref Node bestNode)
        {
            while (tmp!= null) // find the requested node 
            {
                if (value.CompareTo(tmp.data) < 0)
                {
                    bestNode = tmp;
                    tmp = tmp.left;
                }
                else if (value.CompareTo(tmp.data) == 0)
                {
                    bestNode = tmp;
                    break;
                }  
                else tmp = tmp.right;
            }
        }

        private void InsertValue(ref T value, ref Node n, ref Node parent)
        {
            if (IsBigger(parent, value)) parent.left = n; // add like regular tree
            else parent.right = n;
            value = n.data;
        }

        public bool IsEmptyTree()
        {
            return root == null;
        }

        private void DeleteLeaf(Node nodeToDelete, Node nodeToDeleteparent)
        {
            if (nodeToDeleteparent == null)
            {
                root = null;  //delete root if it a leaf
                return;
            }
            if (IsSmallerValue(nodeToDelete, nodeToDeleteparent)) nodeToDeleteparent.right = null;
            else nodeToDeleteparent.left = null;

        }

        private void DeletePArentWithOneChild(Node nodeToDelete, Node nodeToDeleteparent)
        {
            if (nodeToDeleteparent == null)  //delete root if has one kid
            {
                if (nodeToDelete.right == null) root = nodeToDelete.left;
                else root = nodeToDelete.right;
                return;
            }
            if (IsSmallerValue(nodeToDelete, nodeToDeleteparent))
            {
                if (nodeToDelete.left == null) nodeToDeleteparent.right = nodeToDelete.right;
                else nodeToDeleteparent.right = nodeToDelete.left;
            }
            else
            {
                if (nodeToDelete.right == null) nodeToDeleteparent.left = nodeToDelete.left;
                else nodeToDeleteparent.left = nodeToDelete.right;
            }
        }

        private bool IsSmallerValue(Node nodeToDelete, Node nodeToDeleteparent)
        {
            if (nodeToDeleteparent.data.CompareTo(nodeToDelete.data) < 0) return true;
            else return false;
        }

        private bool IsBigger(Node n, T value)
        {
            if (n.data.CompareTo(value) > 0) return true;
            else return false;
        }

        private void DeletePArentWithTwoChild(Node tmp)
        {

            Node OriginNodeToDelete = tmp;
            Node bottomParent = tmp;
            tmp = tmp.right; // reach to the biggerValues
            while (tmp.left != null)
            {
                bottomParent = tmp;
                tmp = tmp.left;
            }
            if (tmp.right == null) DeleteLeaf(tmp, bottomParent);
            else DeletePArentWithOneChild(tmp, bottomParent);
            OriginNodeToDelete.data = tmp.data;
        }

        private int NumKidsOfNode(Node tmp)
        {
            int ChildrenQuantity = 0;
            if (tmp.left != null) ChildrenQuantity++;
            if (tmp.right != null) ChildrenQuantity++;
            return ChildrenQuantity;
        }

        internal bool Delete(T valueToDelete)
        {
            if (root == null) return false; //empty tree     
            Node tmp = root;
            Node parent = null;
            if (!FindValue(ref valueToDelete, ref tmp, ref parent)) return false;

            switch (NumKidsOfNode(tmp))      // act according to number of children
            {
                case 0:
                    DeleteLeaf(tmp, parent);
                    break;
                case 1:
                    DeletePArentWithOneChild(tmp, parent);
                    break;
                case 2:
                    DeletePArentWithTwoChild(tmp);
                    break;
            }
            return true;
        }

        class Node
        {
            public Node left;
            public Node right;
            public T data;
            public Node(T data)
            {
                this.data = data;
            }

        }

    }
}
