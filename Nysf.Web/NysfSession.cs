using Ambitus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Nysf.Web
{
	public class NysfSession : TessSession
	{
		#region Instance members

		public bool IsAnonymous
		{
			get
			{
				return Username == Settings.AnonymousUsername;
			}
		}

		//public Donorship Donorship { get; private set; }

		public NysfSession(string ip)
				: base(ip, Settings.AnonymousUsername, Settings.AnonymousPassword) { }

		public NysfSession(string ip, int sourceId)
				: base(ip, Settings.AnonymousUsername, Settings.AnonymousPassword, sourceId) { }

		public RegisterResult Register(string firstName, string lastName, string email,
				string password, bool emailOptIn, Dictionary<string,string> attributeValuePairs,
				bool sendConfirmationEmail, int emailTemplateId)
		{
			RegisterResult result = Register(firstName, lastName, email, email, password);
			if (result != RegisterResult.Success)
			{
				return result;
			}
			if (emailOptIn)
			{
				if (attributeValuePairs != null)
				{
					AddAttributes(attributeValuePairs);
				}
			}
			else
			{
				AccountInfoUpdate update = new AccountInfoUpdate();
				update.EmailRestrictionId = 2;
				UpdateAccountInfo(update);
			}
			if (sendConfirmationEmail)
			{
				SendGeneralAccountEmail(email, emailTemplateId);
			}
			return result;
		}

		public new UpdateAccountInfoResult UpdateAccountInfo(AccountInfoUpdate update)
		{
			if (update.Email.Delete || update.Email.Value != null)
			{
				// TODO: build in email update capacity based on UpdateLogin
				throw new ApplicationException(
						"NysfSession is not able to update an account's email address via UpdateAccountInfo.");
			}
			return base.UpdateAccountInfo(update);
		}

		public void AddContribution(decimal amount, int fundId, string progName, string levelName,
				char? option, bool makeAnon, string acknowlName, int tributeTypeId,
				bool declineBenefits, bool plansCorpMatch, string corpName)
		{
			base.AddContribution(amount, fundId);
			Cart cart = GetCart();
			Contribution lastCont = (from cont in cart.Contributions
									 orderby cont.Id descending
									 select cont).First<Contribution>();
			ContributionCustomData data = new ContributionCustomData(progName, levelName, option,
					makeAnon, acknowlName, tributeTypeId, declineBenefits, plansCorpMatch,
					corpName);
			UpdateContributionCustomData(lastCont.Id.Value, 10, data.ToString());
		}

		public void RecordFinalizedContributions(ContributionCollection conts)
		{
			foreach (Contribution cont in conts)
			{
				Dictionary<string,string> sqlParams = new Dictionary<string,string>();
				sqlParams.Add("ref_no", cont.Id.Value.ToString());
				ContributionCustomData data = new ContributionCustomData(cont.Custom0);
				sqlParams.Add("ack_name", data.AcknowlName);
				sqlParams.Add("anonymous", data.MakeAnon ? "Y" : "N");
				sqlParams.Add("tribute_type_no", data.TributeTypeId.ToString());
				sqlParams.Add("decline_benefits", data.DeclineBenefits ? "Y" : "N");
				sqlParams.Add("company_match", data.PlansCorpMatch ? "Y" : "N");
				sqlParams.Add("company_match_name", data.CorpName);
				if (data.Option.HasValue)
				{
					sqlParams.Add("opt", data.Option.Value.ToString());
				}
				Tess.ExecuteLocalProcedure(Key, 8017, sqlParams);
			}
		}

		#endregion

		#region Static members

		private new static ConfigSection Settings
		{
			get
			{
				return ConfigSection.Settings;
			}
		}

		#endregion
	}
}