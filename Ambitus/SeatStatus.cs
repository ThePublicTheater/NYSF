using System;

namespace Ambitus
{
	public class SeatStatus
	{
		public short? Id { get; private set; }
		public string Code { get; private set; }
		public string Description { get; private set; }
		public int? ForegroundColor { get; private set; }
		public int? BackgroundColor { get; private set; }
		public byte? Priority { get; private set; }
		public char? Legend { get; private set; }

		public SeatStatus(short? id, string code, string desc, int? foreColor,
				int? backColor, byte? priority, char? legend)
		{
			Id = id;
			Code = code;
			Description = desc;
			ForegroundColor = foreColor;
			BackgroundColor = backColor;
			Priority = priority;
			Legend = legend;
		}

		public override string ToString()
		{
			return Description ?? String.Empty;
		}
	}
}
