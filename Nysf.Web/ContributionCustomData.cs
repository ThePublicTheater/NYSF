using System;

namespace Nysf.Web
{
	public class ContributionCustomData
	{
		public const char Separator = ']';
		
		public string ProgName { get; private set; }
		public string LevelName { get; private set; }
		public char? Option { get; private set; }
		public bool MakeAnon { get; private set; }
		public string AcknowlName { get; private set; }
		public int TributeTypeId { get; private set; }
		public bool DeclineBenefits { get; private set; }
		public bool PlansCorpMatch { get; private set; }
		public string CorpName { get; private set; }

		public ContributionCustomData(string progName, string levelName, char? option,
				bool makeAnon, string acknowlName, int tributeTypeId, bool declineBenefits,
				bool plansCorpMatch, string corpName)
		{
			ProgName = progName;
			LevelName = levelName;
			Option = option;
			MakeAnon = makeAnon;
			AcknowlName = acknowlName;
			TributeTypeId = tributeTypeId;
			DeclineBenefits = declineBenefits;
			PlansCorpMatch = plansCorpMatch;
			CorpName = corpName;
		}

		public ContributionCustomData(string customDataString)
		{
			if (!String.IsNullOrWhiteSpace(customDataString))
			{
				if (customDataString.StartsWith("v1" + Separator))
				{
					string[] pairs = customDataString.Split(Separator);
					ProgName = ExtractValue(pairs[1]);
					LevelName = ExtractValue(pairs[2]);
					string option = ExtractValue(pairs[3]);
					Option = option.Length > 0 ? option[0] : ' ';
					MakeAnon = Boolean.Parse(ExtractValue(pairs[4]));
					AcknowlName = ExtractValue(pairs[5]);
					TributeTypeId = Int32.Parse(ExtractValue(pairs[6]));
					DeclineBenefits = Boolean.Parse(ExtractValue(pairs[7]));
					PlansCorpMatch = Boolean.Parse(ExtractValue(pairs[8]));
					CorpName = ExtractValue(pairs[9]);
				}
			}
		}

		private string ExtractValue(string pair)
		{
			if (pair.Length > 2)
			{
				int index = pair.IndexOf(':');
				if (index > -1 && index < pair.Length - 1)
				{
					return pair.Substring(index + 1);
				}
			}
			return String.Empty;
		}

		public override string ToString()
		{
			return "v1" + Separator + "ProgName:" + (ProgName ?? String.Empty) + Separator
					+ "LevelName:" + (LevelName ?? String.Empty) + Separator
					+ "Option:" + (Option ?? ' ') + Separator
					+ "MakeAnon:" + MakeAnon.ToString() + Separator
					+ "AcknowlName:" + (AcknowlName ?? String.Empty) + Separator
					+ "TributeTypeId:" + TributeTypeId.ToString() + Separator
					+ "DeclineBenefits:" + DeclineBenefits.ToString() + Separator
					+ "PlansCorpMatch:" + PlansCorpMatch.ToString() + Separator
					+ "CorpName:" + (CorpName ?? String.Empty);
		}
	}
}