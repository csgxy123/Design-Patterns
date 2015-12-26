using System;
using System.Collections.Generic;

namespace DesignPatternMono.Observer
{
	public static class Test
	{
		public static void Run()
		{
			var ibm = new IBM(120.0);

			ibm.Attach(new Investor { Name = "Sorros" });
			ibm.Attach(new Investor { Name = "Berkshire" });

			ibm.Price = 120.10;
			ibm.Price = 121.00;
			ibm.Price = 120.50;
			ibm.Price = 120.75;
		}
	}

	public class ChangeEventArgs : EventArgs
	{
		public string Symbol { get; set; }

		public double Price { get; set; }
	}

	/// <summary>
	/// The 'Subject' abstract class
	/// </summary>
	abstract class Stock
	{
		protected string _symbol;
		protected double _price;

		public Stock(string symbol, double price)
		{
			_symbol = symbol;
			_price = price;
		}

		public event EventHandler<ChangeEventArgs> Change;

		public virtual void OnChange(ChangeEventArgs e)
		{
			if (Change != null)
			{
				Change(this, e);
			}
		}

		public void Attach(IInvestor investor)
		{
			Change += investor.Update;
		}

		public void Detach(IInvestor investor)
		{
			Change -= investor.Update;
		}

		public double Price
		{
			get { return _price; }
			set
			{
				if (_price != value)
				{
					_price = value;
					OnChange(new ChangeEventArgs { Symbol = _symbol, Price = _price });
					Console.WriteLine("");
				}
			}
		}
	}

	/// <summary>
	/// The 'ConcreteSubject' class
	/// </summary>
	class IBM : Stock
	{
		public IBM(double price) : base("IBM", price)
		{}
	}

	/// <summary>
	/// The 'Observer' interface
	/// </summary>
	interface IInvestor
	{
		void Update(object sender, ChangeEventArgs e);
	}

	/// <summary>
	/// The 'ConcreteObserver' class
	/// </summary>
	class Investor : IInvestor
	{
		public string Name { get; set; }

		public Stock Stock { get; set; }

		public void Update(object sender, ChangeEventArgs e)
		{
			Console.WriteLine("Notified {0} of {1}'s " + 
				"change to {2:C}", Name, e.Symbol, e.Price);
		}
	}
}

