using System;

namespace Ambitus.Internals
{
	public abstract class ApiResultInterpreter
	{
		protected bool? ToBool(string value)
		{
			if (String.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			switch (value.Trim())
			{
				case "Y": return true;
				case "N": return false;
				default: throw new ApiResultConversionException(value, typeof(bool?));
			}
		}

		protected bool? Invert(bool? value)
		{
			if (value.HasValue)
			{
				return !value.Value;
			}
			return null;
		}

		protected char? ToChar(string value)
		{
			if (String.IsNullOrEmpty(value))
			{
				return null;
			}
			string trimmed = value.Trim();
			if (trimmed.Length == 0)
			{
				return value[0];
			}
			return trimmed[0];
		}

		protected DateTime? ToDateTime(string value)
		{
			DateTime date;
			if (DateTime.TryParse(value, out date))
			{
				return date;
			}
			else
			{
				return null;
			}
		}

		protected HoldUntilMethod? ToHoldUntilMethod(string value)
		{
			if (value == null)
			{
				return null;
			}
			switch (value.Trim())
			{
				case "F": return HoldUntilMethod.Fixed;
				case "R": return HoldUntilMethod.Relative;
				default: throw new ApiResultConversionException(value, typeof(HoldUntilMethod?));
			}
		}

		protected ShippingMethodRestriction? ToShippingMethodRestriction(string value)
		{
			if (value == null)
			{
				return null;
			}
			switch (value.Trim())
			{
				case "N": return ShippingMethodRestriction.None;
				case "F": return ShippingMethodRestriction.ForeignCountry;
				case "T": return ShippingMethodRestriction.Time;
				default: throw new ApiResultConversionException(value,
						typeof(ShippingMethodRestriction));
			}
		}

		protected ContributionAction? ToContributionAction(string value)
		{
			if (value == null)
			{
				return null;
			}
			switch (value.Trim())
			{
				case "R": return ContributionAction.Renew;
				case "U": return ContributionAction.Upgrade;
				default: throw new ApiResultConversionException(value, typeof(ContributionAction));
			}
		}

		protected AttributeDataType? ToAttributeDataType(string value)
		{
			if (value == null)
			{
				return null;
			}
			int parsed;
			bool canParse = Int32.TryParse(value, out parsed);
			if (canParse)
			{
				return ToAttributeDataType(parsed);
			}
			throw new ApiResultConversionException(value, typeof(AttributeDataType));
		}

		protected AttributeDataType? ToAttributeDataType(int? value)
		{
			if (!value.HasValue)
			{
				return null;
			}
			switch (value.Value)
			{
				case 1: return AttributeDataType.String;
				case 2: return AttributeDataType.Integer;
				case 3: return AttributeDataType.DateTime;
				default: throw new ApiResultConversionException(value.ToString(),
						typeof(AttributeDataType));
			}
		}

		protected ContactRestrictionType? ToContactRestrictionType(string value)
		{
			if (String.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			switch (value.Trim())
			{
				case "E": return ContactRestrictionType.Email;
				case "P": return ContactRestrictionType.Phone;
				case "M": return ContactRestrictionType.Mail;
				default: throw new ApiResultConversionException(value,
						typeof(ContactRestrictionType));
			}
		}

		protected StateProvUsePolicy? ToStateProvUsePolicy(string value)
		{
			if (String.IsNullOrWhiteSpace(value))
			{
				return null;
			}
			switch (value.Trim())
			{
				case "Y": return StateProvUsePolicy.MustUse;
				case "N": return StateProvUsePolicy.MayNotUse;
				case "O": return StateProvUsePolicy.Optional;
				default: throw new ApiResultConversionException(value,
						typeof(StateProvUsePolicy));
			}
		}

		public class ApiResultConversionException : ApplicationException
		{
			public ApiResultConversionException(string value, Type expectedType)
					: base("Cannot convert value, \"" + value
							+ "\", to " + expectedType.ToString() + ".") { }
		}
	}
}
