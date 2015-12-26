using System;
using System.Collections.Generic;

namespace DesignPatternMono.Command
{
	public static class Test
	{
		public static void Run()
		{
			var user = new User();

			user.Compute('+', 100);
			user.Compute('-', 50);
			user.Compute('*', 10);
			user.Compute('/', 2);

			user.Undo(4);

			user.Redo(3);
		}
	}

	/// <summary>
	/// The 'Command' interface
	/// </summary>
	interface ICommand
	{
		void Execute();
		void UnExecute();
	}

	/// <summary>
	/// The 'Command' interface
	/// </summary>
	class CalculatorCommand: ICommand
	{
		private char		_operator;
		private int			_operand;
		private Calculator	_calculator;

		public CalculatorCommand(Calculator calculator, char @operator, int operand)
		{
			this._calculator	= calculator;
			this._operator		= @operator;
			this._operand		= operand;
		}

		public char Operator
		{
			set { _operator = value; }
		}

		public int Operand
		{
			set { _operand = value; }
		}

		public void Execute()
		{
			_calculator.Operation(_operator, _operand);
		}

		public void UnExecute()
		{
			_calculator.Operation(Undo(_operator), _operand);
		}

		private char Undo(char @operator)
		{
			switch(@operator)
			{
				case '+': return '-';
				case '-': return '+';
				case '*': return '/';
				case '/': return '*';
				default: throw new ArgumentException("@operator");
			}
		}
	}

	/// <summary>
	/// The 'Receiver' class
	/// </summary>
	class Calculator
	{
		private int _current = 0;

		public void Operation(char @operator, int operand)
		{
			switch(@operator)	
			{
				case '+': _current += operand;	break;
				case '-': _current -= operand;	break;
				case '*': _current *= operand;	break;
				case '/': _current /= operand;	break;
			}
			Console.WriteLine("Current value = {0, 3} (following {1} {2})", _current, @operator, operand);
		}
	}

	/// <summary>
	/// The 'Invoker' class
	/// </summary>
	class User
	{
		private Calculator		_calculator = new Calculator();
		private List<ICommand>	_commands	= new List<ICommand>();
		private int 			_current	= 0;

		public void Redo(int levels)
		{
			Console.WriteLine("\n---- Redo {0} levels ", levels);

			for (int i = 0; i < levels; ++i)
			{
				if (_current < _commands.Count - 1)
				{
					_commands[_current++].Execute();
				}
			}
		}

		public void Undo(int levels)
		{
			Console.WriteLine("\n---- Undo {0} levels ", levels);

			for (int i = 0; i < levels; ++i)
			{
				if (_current > 0)
				{
					_commands[--_current].UnExecute();
				}
			}
		}

		public void Compute(char @operator, int operand)
		{
			ICommand command = new CalculatorCommand(_calculator, @operator, operand);

			command.Execute();

			_commands.Add(command);
			_current++;
		}
	}
}

