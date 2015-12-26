using System;
using System.Collections.Generic;

namespace DesignPatternMono.Mediator
{
	public static class Test
	{
		public static void Run()
		{
			Participant George 	= new Beatle { Name = "George" };
			Participant Paul	= new Beatle { Name = "Paul" };
			Participant Ringo	= new Beatle { Name = "Ringo" };
			Participant John	= new Beatle { Name = "John" };
			Participant Yoko	= new NonBeatle { Name = "Yoko" };

			var chatroom = new Chatroom();
			chatroom.Register(George);
			chatroom.Register(Paul);
			chatroom.Register(Ringo);
			chatroom.Register(John);
			chatroom.Register(Yoko);

			Yoko.Send("John", "Hi John!");
			Paul.Send("Ringo", "All you need is love");
			Ringo.Send("George", "My sweet Lord");
			Paul.Send("John", "Can't buy me love");
			John.Send("Yoko", "My sweet love");
		}
	}

	/// <summary>
	/// The 'Mediator' interface
	/// </summary>
	interface IChatrootm
	{
		void Register(Participant participant);	
		void Send(string from, string to, string message);
	}

	/// <summary>
	/// The 'ConcreteMediator' class
	/// </summary>
	class Chatroom : IChatrootm
	{
		private Dictionary<string, Participant> _participants 
		  = new Dictionary<string, Participant>();

		public void Register(Participant participant)
		{
			if (!_participants.ContainsKey(participant.Name))	
			{
				_participants.Add(participant.Name, participant);
			}
			participant.Chatroom = this;
		}

		public void Send(string from, string to, string message)
		{
			var participant = _participants[to];	
			if (participant != null)
			{
				participant.Receive(from, message);
			}
		}
	}

	/// <summary>
	/// The 'AbstractColleague' class
	/// </summary>
	class Participant
	{
		public string Name { get; set; }

		public Chatroom Chatroom { get; set; }

		public void Send(string to, string message)
		{
			Chatroom.Send(Name, to, message);	
		}

		public virtual void Receive(string from, string message)
		{
			Console.WriteLine("{0} to {1}: '{2}'", from, Name, message);	
		}
	}

	/// <summary>
	/// A 'ConcreteColleague' class
	/// </summary>
	class Beatle : Participant
	{
		public override void Receive (string from, string message)
		{
			Console.WriteLine("To a Beatle: ");
			base.Receive(from, message);
		}	
	}

	/// <summary>
	/// A 'ConcreteColleague' class
	/// </summary>
	class NonBeatle : Participant
	{
		public override void Receive (string from, string message)
		{
			Console.WriteLine("To a non-Beatle: ");
			base.Receive(from, message);
		}	
	}
}

