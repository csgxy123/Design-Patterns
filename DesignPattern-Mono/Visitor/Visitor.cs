using System;
using System.Collections.Generic;

namespace DesignPatternMono.Visitor
{
	public static class Test
	{
		public static void Run()
		{
			var e = new Employees();
			e.Attach(new Clerk());
			e.Attach(new Director());
			e.Attach(new President());

			e.Accept(new IncomeVisitor());
			e.Accept(new VacationVisitor());
		}
	}

	/// <summary>
	/// The 'Visitor' abstract class
	/// </summary>
	public abstract class Visitor
	{
		public void ReflectiveVisit(IElement element)
		{
			var types = new Type[] { element.GetType() };
			var mi = this.GetType().GetMethod("Visit", types);

			if (mi != null)
			{
				mi.Invoke(this, new object[] { element });
			}
		}
	}

	/// <summary>
	/// A 'ConcreteVisitor' class
	/// </summary>
	class IncomeVisitor : Visitor
	{
		public void Visit(Clerk clerk)
		{
			DoVisit(clerk);
		}

		public void Visit(Director director)
		{
			DoVisit(director);
		}

		private void DoVisit(IElement element)
		{
			var employee = element as Employee;
			employee.Income *= 1.10;
			Console.WriteLine("{0} {1}'s new income: {2:C}",
			employee.GetType().Name, employee.Name, employee.Income);
		}
	}

	/// <summary>
	/// A 'ConcreteVisitor' class
	/// </summary>
	class VacationVisitor : Visitor
	{
		public void Visit(Director director)
		{
			DoVisit(director);
		}

		private void DoVisit(IElement element)
		{
			var employee = element as Employee;

			employee.VacationDays += 3;
			Console.WriteLine("{0} {1}'s new vacation days: {2}",
			employee.GetType().Name, employee.Name, employee.VacationDays);
		}
	}

	/// <summary>
	/// The 'Element' interface
	/// </summary>
	public interface IElement
	{
		void Accept(Visitor visitor);
	}

	/// <summary>
	/// The 'ConcreteElement' class
	/// </summary>
	class Employee : IElement
	{
		public Employee(string name, double income, int vacationDays)
		{
			this.Name = name;
			this.Income = income;
			this.VacationDays = vacationDays;
		}

		public string Name { get; set; }

		public double Income { get; set; }

		public int VacationDays { get; set; }

		public virtual void Accept(Visitor visitor)
		{
			visitor.ReflectiveVisit(this);
		}
	}

	/// <summary>
	/// The 'ObjectStructure' class
	/// </summary>
	class Employees : List<Employee>
	{
		public void Attach(Employee employee)
		{
			Add(employee);
		}

		public void Detach(Employee employee)
		{
			Remove(employee);
		}

		public void Accept(Visitor visitor)
		{
			this.ForEach(employee => employee.Accept(visitor));
			Console.WriteLine();
		}
	}

	class Clerk : Employee
	{
		public Clerk() : base("Hank", 25000.0, 14) {}
	}

	class Director : Employee
	{
		public Director() : base("Elly", 25000.0, 16) {}
	}

	class President : Employee
	{
		public President() : base("Dick", 45000.0, 21) {}
	}
}

