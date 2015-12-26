using System;
using System.Collections.Generic;

namespace DesignPatternMono.Flyweight
{
	public static class Test
	{
		public static void Run()
		{
			string document = "AAZZBBZB";
			char[] chars = document.ToCharArray();

			var factory = new CharacterFactory();

			int pointSize = 10;

			foreach(char c in chars)
			{
				var character = factory[c];
				character.Display(++pointSize);
			}
		}
	}

	/// <summary>
	/// The 'FlyweightFactory' class
	/// </summary>
	class CharacterFactory
	{
		private Dictionary<char, Character> _characters = new Dictionary<char, Character>();

		public Character this[char key]
		{
			get
			{
				Character character = null;	
				if (_characters.ContainsKey(key))
				{
					character = _characters[key];
				}
				else
				{
					string name = this.GetType().Namespace + "." + "Character" + key.ToString();
					character = (Character)Activator.CreateInstance(Type.GetType(name));
					_characters[key] = character;
				}
				return character;
			}
		}
	}

	/// <summary>
	/// The 'Flyweight' class
	/// </summary>
	class Character
	{
		protected char symbol;	
		protected int width;
		protected int height;
		protected int ascent;
		protected int descent;

		public void Display(int pointSize)
		{
			Console.WriteLine(this.symbol + " (pointsize " + pointSize + ")");
		}
	}

	/// <summary>
	/// A 'ConcreteFlyweight' class
	/// </summary>
	class CharacterA : Character
	{
		public CharacterA()
		{
			this.symbol = 'A';
			this.height = 100;
			this.width = 120;
			this.ascent = 70;
			this.descent = 0;
		}
	}

	/// <summary>
	/// A 'ConcreteFlyweight' class
	/// </summary>
	class CharacterB : Character
	{
		public CharacterB()
		{
			this.symbol = 'B';
			this.height = 100;
			this.width = 140;
			this.ascent = 72;
			this.descent = 0;
		}
	}

	/// <summary>
	/// A 'ConcreteFlyweight' class
	/// </summary>
	class CharacterZ : Character
	{
		public CharacterZ()
		{
			this.symbol = 'Z';
			this.height = 100;
			this.width = 100;
			this.ascent = 68;
			this.descent = 0;
		}
	}
}

