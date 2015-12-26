using System;

namespace DesignPatternMono.Chain
{
	public static class Test
	{
		public static void Run()
		{
			Approver larry = new Director();
			Approver sam = new VicePresident();
			Approver tammy = new President();

			larry.Successor = sam;
			sam.Successor = tammy;

			var purchase = new Purchase { Number = 2034, Amount = 350.0, Purpose = "Supplies" };
			larry.ProcessRequest(purchase);

			purchase = new Purchase { Number = 2035, Amount = 32590.1, Purpose = "Project X" };
			larry.ProcessRequest(purchase);

			purchase = new Purchase { Number = 2036, Amount = 122100.0, Purpose = "Project Y" };
			larry.ProcessRequest(purchase);
		}
	}

	public class PurchaseEventArgs : EventArgs
	{
		internal Purchase Purchase { get; set; }
	}

	/// <summary>
	/// The 'Handler' abstract class
	/// </summary>
	abstract class Approver
	{
		public event EventHandler<PurchaseEventArgs> Purchase;

		public abstract void PurchaseHandler(object sender, PurchaseEventArgs e);

		public Approver()
		{
			Purchase += PurchaseHandler;
		}

		public void ProcessRequest(Purchase purchase)
		{
			OnPurchase(new PurchaseEventArgs { Purchase = purchase });
		}

		public virtual void OnPurchase(PurchaseEventArgs e)
		{
			if (Purchase != null)	
			{
				Purchase(this, e);
			}
		}

		public Approver Successor { get; set; }
	}

	/// <summary>
	/// The 'ConcreteHandler' class
	/// </summary>
	class Director : Approver
	{
		public override void PurchaseHandler (object sender, PurchaseEventArgs e)
		{
			if (e.Purchase.Amount < 10000.0)			
			{
				Console.WriteLine("{0} approved request# {1}", this.GetType().Name, e.Purchase.Number);	
			}
			else if (Successor != null)
			{
				Successor.PurchaseHandler(this, e);
			}
		}
	}

	/// <summary>
	/// The 'ConcreteHandler' class
	/// </summary>
	class VicePresident : Approver
	{
		public override void PurchaseHandler (object sender, PurchaseEventArgs e)
		{
			if (e.Purchase.Amount < 25000.0)
			{
				Console.WriteLine("{0} approved request# {1}", this.GetType().Name, e.Purchase.Number);
			}
			else if (Successor != null)
			{
				Successor.PurchaseHandler(this, e);
			}
		}
	}

	/// <summary>
	/// The 'ConcreteHandler' class
	/// </summary>
	class President : Approver
	{
		public override void PurchaseHandler (object sender, PurchaseEventArgs e)
		{
			if (e.Purchase.Amount < 100000.0)
			{
				Console.WriteLine("{0} approved request# {1}", this.GetType().Name, e.Purchase.Number);
			}
			else if (Successor != null)
			{
				Successor.PurchaseHandler(this, e);
			}
			else
			{
				Console.WriteLine("Request# {0} requires an executive meeting!", e.Purchase.Number);	
			}
		}
	}

	/// <summary>
	/// Class that holds request details
	/// </summary>
	class Purchase
	{
		public double Amount { get; set; }
		public string Purpose { get; set; }
		public int Number { get; set; }
	}
}

