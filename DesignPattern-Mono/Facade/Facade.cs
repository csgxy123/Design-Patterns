﻿using System;

namespace DesignPatternMono.Facade
{
	public static class Test
	{
		public static void Run()
		{
			var mortgage = new Mortgage();	

			var customer = new Customer { Name = "Ann McKinsey" };
			bool eligible = mortgage.IsEligible(customer, 125000);

			Console.WriteLine("\n" + customer.Name + " has been " + (eligible ? "Approved" : "Rejected"));
		}
	}

	/// <summary>
	/// The 'Subsystem ClassA' class
	/// </summary>
	class Bank
	{
		public bool HasSufficientSavings(Customer c, int amount)
		{
			Console.WriteLine("Check bank for " + c.Name);	
			return true;
		}
	}

	/// <summary>
	/// The 'Subsystem ClassB' class
	/// </summary>
	class Credit
	{
		public bool HasGoodCredit(Customer c)
		{
			Console.WriteLine("Check credit for " + c.Name);
			return true;
		}
	}

	/// <summary>
	/// The 'Subsystem ClassC' class
	/// </summary>
	class Loan
	{
		public bool HasNoBadLoans(Customer c)
		{
			Console.WriteLine("Check loans for " + c.Name);
			return true;
		}
	}

	/// <summary>
	/// The 'Facade' class
	/// </summary>
	class Mortgage
	{
		private Bank _bank = new Bank();
		private Loan _loan = new Loan();
		private Credit _credit = new Credit();

		public bool IsEligible(Customer cust, int amount)
		{
			Console.WriteLine("{0} applies for {1:C} loan\n", cust.Name, amount);	

			bool eligible = true;

			if (!_bank.HasSufficientSavings(cust, amount))
			{
				eligible = false;
			}
			else if (!_loan.HasNoBadLoans(cust))
			{
				eligible = false;
			}
			else if (!_credit.HasGoodCredit(cust))
			{
				eligible = false;
			}

			return eligible;
		}
	}

	/// <summary>
	/// Customer class
	/// </summary>
	class Customer
	{
		public string Name { get; set; }
	}
}

