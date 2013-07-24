using System.Web.UI.WebControls;

namespace Nysf.Web.UserControls
{
	public abstract class UserControl : System.Web.UI.UserControl
	{
		public UserControl() : base() { }

		protected void InsertValidationErrorMessage(string message)
		{
			CustomValidator tempValidator = new CustomValidator();
			tempValidator.IsValid = false;
			tempValidator.Display = ValidatorDisplay.None;
			tempValidator.ErrorMessage = message;
			Page.Form.Controls.Add(tempValidator);
		}
	}
}