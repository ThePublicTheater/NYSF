using System;
using Nysf.Tessitura;

namespace Nysf.UserControls
{

/// <summary>
///     A form the allows the lookup and recovery of an account identified by
///     its email address.
/// </summary>
public partial class AccountFinder : GenericControl
{
    #region Properties

    public string PromptText
    {
        get { return Prompt.Text; }
        set { Prompt.Text = value; }
    }

    public string LookupFailureWarningText
    {
        get { return LookupFailureWarning.InnerText; }
        set { LookupFailureWarning.InnerText = value; }
    }

    public string SubmitButtonText
    {
        get { return SubmitButton.Text; }
        set { SubmitButton.Text = value; }
    }

    public string CheckEmailMessageText
    {
        get { return CheckEmailMessage.Text; }
        set { CheckEmailMessage.Text = value; }
    }

    public int EmailTemplateNumber { get; set; }

    #endregion

    #region Methods

    protected void Page_Load(object sender, EventArgs e)
    {
        if (Session[WebClient.TessSessionKeySessionKey] == null)
            throw new ApplicationException("The AccountFinder requires that " +
                "a Tessitura session key be assigned by the SessionHandler.");

        // TODO: force inclusion of EmailTemplateNumber attribute
    }

    /// <summary>
    ///     Attempt to send a recovery email to the account, or display a
    ///     warning if the account can't be found.
    /// </summary>
    protected void SubmitButton_Click(object sender, EventArgs e)
    {
        try
        {
            if (Page.IsValid)
            {
                bool lookupWasSuccessful =
                    WebClient.SendRecoveryEmail(EmailTextBox.Text.Trim(), EmailTemplateNumber);
                if (lookupWasSuccessful)
                    AccountLookupWizard.ActiveViewIndex = 1;
                else
                    LookupFailureWarning.Visible = true;
            }
        }
        catch (Exception ex)
        {
            P1.Visible = true;
        }
    }

    protected void Page_PreRender(object sender, EventArgs e)
    {
        if (AccountLookupWizard.ActiveViewIndex == 0)
        {
            Page.SetFocus(EmailTextBox);
            Page.Form.DefaultButton = SubmitButton.UniqueID;
        }
    }

    #endregion
}

}