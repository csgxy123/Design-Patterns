using System;
using System.Collections.Generic;

namespace DesignPatternMono.Composite
{
	public static class Test
	{
		public static void Run()
		{
			var root = new TreeNode<Shape> { Node = new Shape("Picture") };	

			root.Add(new Shape("Red Line"));
			root.Add(new Shape("Blue Circle"));
			root.Add(new Shape("Green Box"));

			var branch = root.Add(new Shape("Two Circles"));
			branch.Add(new Shape("Black Circle"));
			branch.Add(new Shape("White Circle"));

			var shape = new Shape("Yellow Line");
			root.Add(shape);
			root.Remove(shape);
			root.Add(shape);

			TreeNode<Shape>.Display(root, 1);

		}
	}

	/// <summary>
	/// Generic tree node class
	/// </summary>
	class TreeNode<T> where T : IComparable<T>
	{
		private List<TreeNode<T>> _children = new List<TreeNode<T>>();

		public TreeNode<T> Add(T child)
		{
			var newNode = new TreeNode<T> { Node = child };
			_children.Add(newNode);
			return newNode;
		}

		public void Remove(T child)
		{
			foreach(var treeNode in _children)
			{
				if (treeNode.Node.CompareTo(child) == 0)	
				{
					_children.Remove(treeNode);
					return;
				}
			}
		}

		public T Node { get; set; }

		public List<TreeNode<T>> Children
		{
			get { return _children; }
		}

		public static void Display(TreeNode<T> node, int indentation)
		{
			string line = new string('-', indentation);	
			Console.WriteLine(line + " " + node.Node);
			node.Children.ForEach(n => Display(n, indentation + 1));
		}
	}

	/// <summary>
	/// Shape class
	/// <remarks>
	/// Implements generic IComparable interface
	/// </remarks>
	/// </summary>
	class Shape : IComparable<Shape>
	{
		private string _name;

		public Shape(string name)
		{
			this._name = name;
		}

		public override string ToString ()
		{
			return _name;
		}

		public int CompareTo(Shape other)
		{
			return (this == other) ? 0 : -1;
		}
	}
}

