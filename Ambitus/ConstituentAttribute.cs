using System;

namespace Ambitus
{
	public class ConstituentAttribute
	{
		public int? Id { get; private set; }
		public string Name { get; private set; }
		public string Value { get; private set; }
		public AttributeDataType? DataType { get; private set; }
		public byte? N1N2 { get; private set; }

		public ConstituentAttribute(int? id, string name, string value, AttributeDataType? dataType,
				byte? n1n2)
		{
			Id = id;
			Name = name;
			Value = value;
			DataType = dataType;
			N1N2 = n1n2;
		}

		public override string ToString()
		{
			return (Name ?? String.Empty) + ": " + (Value ?? String.Empty);
		}
	}
}
