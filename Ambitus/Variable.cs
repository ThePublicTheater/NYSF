using System;

namespace Ambitus
{
	public class Variable
	{
		public string Name { get; private set; }
		public string Value { get; private set; }

		public Variable(string name, string value)
		{
			Name = name;
			Value = value;
		}

		public override string ToString()
		{
			return (Name ?? String.Empty) + ": " + (Value ?? String.Empty);
		}
	}
}
