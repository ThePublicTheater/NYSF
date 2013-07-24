using System;

namespace Ambitus
{
	public class Attribute
	{
		public int? Id { get; private set; }
		public string Description { get; private set; }
		public AttributeDataType? DataType { get; private set; }
		public string EditMask { get; private set; }
		public bool? AllowMultipleSelections { get; private set; }
		public AttributeOptionCollection Options { get; private set; }

		public Attribute(int? id, string desc, AttributeDataType? dataType, string editMask,
				bool? allowMultiple, AttributeOptionCollection options)
		{
			Id = id;
			Description = desc;
			DataType = dataType;
			EditMask = editMask;
			AllowMultipleSelections = allowMultiple;
			Options = options;
		}

		public override string ToString()
		{
			return Description ?? String.Empty;
		}
	}
}
