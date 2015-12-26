using System;

namespace DesignPatternMono.Proxy
{
	public static class Test
	{
		public static void Run()
		{
			var proxy = new MathProxy();

			Console.WriteLine("4 + 2 = " + proxy.Add(4, 2));
			Console.WriteLine("4 - 2 = " + proxy.Sub(4, 2));
			Console.WriteLine("4 * 2 = " + proxy.Mul(4, 2));
			Console.WriteLine("4 / 2 = " + proxy.Div(4, 2));
		}
	}

	/// <summary>
	/// The 'Subject' interface
	/// </summary>
	public interface IMath
	{
		double Add(double x, double y);
		double Sub(double x, double y);
		double Mul(double x, double y);
		double Div(double x, double y);
	}

	/// <summary>
	/// The 'RealSubject' class
	/// </summary>
	class Math : MarshalByRefObject, IMath
	{
		public double Add(double x, double y) { return x + y; }	
		public double Sub(double x, double y) { return x - y; }	
		public double Mul(double x, double y) { return x * y; }	
		public double Div(double x, double y) { return x / y; }	
	}

	/// <summary>
	/// The remote 'Proxy Object' class
	/// </summary>
	class MathProxy : IMath
	{
		private Math _math;	

		public MathProxy()
		{
			var ad = AppDomain.CreateDomain("MathDomain", null, null);

			var o = ad.CreateInstance(
				"DesignPattern-Mono",
				"DesignPatternMono.Proxy.Math");
			_math = (Math)o.Unwrap();
		}

		public double Add(double x, double y)
		{
			return _math.Add(x, y);
		}

		public double Sub(double x, double y)
		{
			return _math.Sub(x, y);
		}

		public double Mul(double x, double y)
		{
			return _math.Mul(x, y);
		}

		public double Div(double x, double y)
		{
			return _math.Div(x, y);
		}
	}
}

