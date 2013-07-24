using System;

namespace Nysf.Web.UserControls
{
	public partial class SessionHandler : UserControl
	{
		#region Properties

		private bool allowAnonymous;
		public bool AllowAnonymous
		{
			get
			{
				return allowAnonymous;
			}
			set
			{
				if (!(value || AllowAuthenticated))
				{
					throw new ApplicationException(
							"Disallowed anonymous users after disallowing authenticated users.");
				}
				allowAnonymous = value;
			}
		}

		private bool allowAuthenticated;
		public bool AllowAuthenticated
		{
			get
			{
				return allowAuthenticated;
			}
			set
			{
				if (!(value || AllowAnonymous))
				{
					throw new ApplicationException(
							"Disallowed authenticated users after disallowing anonymous users.");
				}
				allowAuthenticated = value;
			}
		}

		private bool allowTemporary;
		public bool AllowTemporary
		{
			get
			{
				return allowTemporary;
			}
			set
			{
				if (!(value || AllowAuthenticated))
				{
					throw new ApplicationException(
							"Disallowed temporary users after disallowing authenticated users.");
				}
				allowTemporary = value;
			}
		}
		private bool requireSsl;
		public bool RequireSsl
		{
			get
			{
				return requireSsl;
			}
			set
			{
				requireSsl = value;
			}
		}

		private int? newSessionSourceId;
		public int? NewSessionSourceId
		{
			get
			{
				return newSessionSourceId;
			}
			set
			{
				newSessionSourceId = value;
			}
		}

		private bool setLastPage;
		public bool SetLastPage
		{
			get
			{
				return setLastPage;
			}
			set
			{
				setLastPage = value;
			}
		}

		#endregion

		#region Methods

		public SessionHandler()
		{
			allowAnonymous = true;
			allowAuthenticated = true;
			allowTemporary = false;
			requireSsl = false;
			newSessionSourceId = null;
			setLastPage = true;
		}

		protected void Page_Init(object sender, EventArgs e)
		{
			BrowserUtility.MaintainSession(AllowAnonymous, AllowAuthenticated,
					AllowTemporary, RequireSsl, NewSessionSourceId, SetLastPage);
		}

		#endregion
	}
}