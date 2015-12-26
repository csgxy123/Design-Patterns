using System;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Soap;

namespace DesignPatternMono.Memento
{
	public static class Test
	{
		public static void Run()
		{
			var s = new SalesProspect
			{
				Name = "Joel van Halen",
				Phone = "(412) 256-0990",
				Budget = 25000.0
			};

			var m = new ProspectMemory();
			m.Memento = s.SaveMemento();

			s.Name = "Leo Welch";
			s.Phone = "(310) 209-8111";
			s.Budget = 1000000.0;

			s.RestoreMemento(m.Memento);
		}
	}

	/// <summary>
	/// The 'Originator' class
	/// </summary>
	[Serializable]
	class SalesProspect
	{
		private string _name;
		private string _phone;
		private double _budget;

		public string Name
		{
			get { return _name; }
			set 
			{ 
				_name = value;
				Console.WriteLine("Name:\t" + _name);
			}
		}

		public string Phone
		{
			get { return _phone; }
			set
			{
				_phone = value;
				Console.WriteLine("Phone:\t" + _phone);
			}
		}

		public double Budget
		{
			get { return _budget; }
			set
			{
				_budget = value;
				Console.WriteLine("Budget:\t" + _budget);
			}
		}

		public Memento SaveMemento()
		{
			Console.WriteLine("\nSaving state --\n");
			var memento = new Memento();
			return memento.Serialize(this);
		}

		public void RestoreMemento(Memento memento)
		{
			Console.WriteLine("\nRestoring state --\n");
			SalesProspect s = (SalesProspect)memento.Deserialize();
			this.Name = s.Name;
			this.Phone = s.Phone;
			this.Budget = s.Budget;
		}
	}

	/// <summary>
	/// The 'Memento' class
	/// </summary>
	class Memento
	{
		private MemoryStream _stream	= new MemoryStream();
		private SoapFormatter _formatter= new SoapFormatter();

		public Memento Serialize(object o)
		{
			_formatter.Serialize(_stream, o);
			return this;
		}

		public object Deserialize()
		{
			_stream.Seek(0, SeekOrigin.Begin);
			object o = _formatter.Deserialize(_stream);
			_stream.Close();

			return o;
		}
	}

	/// <summary>
	/// The 'Caretaker' class
	/// </summary>
	class ProspectMemory
	{
		public Memento Memento { get; set; }
	}
}

