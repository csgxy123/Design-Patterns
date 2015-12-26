using System;

namespace DesignPatternMono.State
{
	public static class Test
	{
		public static void Run()
		{
			var account = new Account("Jim Johnson");

			account.Deposit(500.0);
			account.Deposit(300.0);
			account.Deposit(550.0);
			account.PayInterest();
			account.Withdraw(2000.00);
			account.Withdraw(1100.00);
		}
	}

	/// <summary>
	/// The 'State' abstract class
	/// </summary>
	abstract class State
	{
		protected double interest;
		protected double lowerLimit;
		protected double upperLimit;

		public Account Account { get; set; }

		public double Balance { get; set; }

		public abstract void Deposit(double amount);
		public abstract void Withdraw(double amount);
		public abstract void PayInterest();
	}


	/// <summary>
	/// A 'ConcreateState' class
	/// <remarks>
	/// Red state indicates account is overdrawn
	/// </remarks>
	/// </summary>
	class RedState : State
	{
		private double _serviceFee;

		public RedState(State state)
		{
			Balance = state.Balance;
			Account = state.Account;
			Initialize();
		}

		private void Initialize()
		{
			interest = 0.0;
			lowerLimit = -100.0;
			upperLimit = 0.0;
			_serviceFee = 15.00;
		}

		public override void Deposit(double amount)
		{
			Balance += amount;
			StateChangeCheck();
		}

		public override void Withdraw(double amount)
		{
			amount = amount - _serviceFee;
			Console.WriteLine("No funds available for withdrawl!");
		}

		public override void PayInterest ()
		{
			
		}

		private void StateChangeCheck()
		{
			if (Balance > upperLimit)
			{
				Account.State = new SilverState(this);
			}
		}
	}

	/// <summary>
	/// A 'ConcreteState' class
	/// <remarks>
	/// Silver state is non-interest bearing state
	/// </remarks>
	/// </summary>
	class SilverState : State
	{
		public SilverState(State state) : this(state.Balance, state.Account)
		{}

		public SilverState(double balance, Account account)
		{
			Balance = balance;
			Account = account;
			Initialize();
		}

		private void Initialize()
		{
			interest = 0.0;
			lowerLimit = 0.0;
			upperLimit = 1000.0;
		}

		public override void Deposit(double amount)
		{
			Balance += amount;
			StateChangeCheck();
		}

		public override void Withdraw(double amount)
		{
			Balance -= amount;
			StateChangeCheck();
		}

		public override void PayInterest ()
		{
			Balance += interest * Balance;
			StateChangeCheck();
		}

		private void StateChangeCheck()
		{
			if (Balance < lowerLimit)
			{
				Account.State = new RedState(this);
			}
			else if (Balance > upperLimit)
			{
				Account.State = new GoldState(this);
			}
		}
	}

	/// <summary>
	/// A 'ConcreteState' class
	/// <remarks>
	/// Gold incidates interest bearing state
	/// </remarks>
	/// </summary>
	class GoldState : State
	{
		public GoldState(State state) : this(state.Balance, state.Account)
		{}

		public GoldState(double balance, Account account)
		{
			Balance = balance;
			Account = account;
			Initialize();
		}

		private void Initialize()
		{
			interest = 0.05;
			lowerLimit = 1000.0;
			upperLimit = 10000000.0;
		}

		public override void Deposit(double amount)
		{
			Balance += amount;
			StateChangeCheck();
		}

		public override void Withdraw (double amount)
		{
			Balance -= amount;
			StateChangeCheck();
		}

		public override void PayInterest ()
		{
			Balance += interest * Balance;
			StateChangeCheck();
		}

		private void StateChangeCheck()
		{
			if (Balance < 0.0)
			{
				Account.State = new RedState(this);
			}
			else if (Balance < lowerLimit)
			{
				Account.State = new SilverState(this);
			}
		}
	}

	/// <summary>
	/// The 'Context' class
	/// </summary>
	class Account
	{
		private string _owner;

		public Account(string owner)
		{
			this._owner = owner;
			this.State = new SilverState(0.0, this);
		}

		public double Balance
		{
			get { return State.Balance; }
		}

		public State State { get; set; }

		public void Deposit(double amount)
		{
			State.Deposit(amount);
			Console.WriteLine("Deposited {0:C} --- ", amount);
			Console.WriteLine(" Balance\t= {0:C}", this.Balance);
			Console.WriteLine(" Status\t= {0}", this.State.GetType().Name);
			Console.WriteLine("");
		}

		public void Withdraw(double amount)
		{
			State.Withdraw(amount);
			Console.WriteLine("Withdrew {0:C} --- ", amount);
			Console.WriteLine(" Balance\t= {0:C}", this.Balance);
			Console.WriteLine(" Status\t= {0}\n", this.State.GetType().Name);
		}

		public void PayInterest()
		{
			State.PayInterest();
			Console.WriteLine("Interest Paid --- ");
			Console.WriteLine(" Balance\t= {0:C}", this.Balance);
			Console.WriteLine(" Status\t= {0}\n", this.State.GetType().Name);
		}
	}
}

