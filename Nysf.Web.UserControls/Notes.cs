/*
 * Standards for User Controls
 * 
 * Often consists of a MultiView Id="...Views"
 * Each view may have
 * 		- a ValidationSummary CssClass="Warning" DisplayMode="BulletList"
 * 		- a <p> containing a Literal ID="Blurb_Prompt" with prompt text
 * 		- a <div class="SelfEvident"> containing a Button ID="Label_DefaultSubmit"
 * 				Text="_same_as_default_" OnClick="_same_as_default_" to trigger by default on enter
 *		- a <fieldset>s with
 *			- <legend>s (usually SelfEvident) containing
 *				- Literal ID="Blurb_...Legend"
 *			- various <div class="Field" id="Field_...">s containing
 *				- a <div class="Description"> with a
 *					- Label ID="Label_..." AssociatedControlId="Input_..."
 *				- a <div class="Entry"> with
 *					- _input_control_ ID="Input_..." CssClass="FieldScaleX..." MaxLength="..." etc.
 *					- _validator_controls_ ID="Blurb__request_" ControlToValidate="Input_..."
 *							Text="*" Display="Dynamic" CssClass="Warning" ErrorMessage="..."
 *		- a <div class="SubmitSet"> with buttons in LTR order
 *			- Button ID="Label_...Button" Text="..." OnClick="..."
 *				- back buttons usually have CausesValidation="false"
 *	In the code-behind:
 *		- extends Nysf.Web.UserControls.UserControl
 *		- properties for
 *			- non-copy-related options
 *			- copy native to this control and not to one of its subcontrols
 *			- copy properties with custom getter/setters to edit subcontrol text
 *		- a contructor that sets defaults (usu. calling SetDefaults())
 *		- Page_Load
 *			- sets default view on !Page.IsPostBack
 *		- Page_PreRender
 *			- sets focus on element depending on active view
 *		- Do_... methods for button events (often only act if Page.IsValid)
 *		
 * Page Element Naming Conventions
 *		- Public properties
 *			- Copy-related start with "Copy_", then "Label_" for form fields / button text, or
 *					otherwise "Blurb_" for prompts
 *		- Button event responders: "Do_..."
 *		- Prompts, legends IDed "Blurb_..."
 *		- fields "Field_..."
 *		- field labels "Label_..."
 *		- field inputs "Input_..."
 *		- field validators "Blurb_..."
 *		- submits "Label_"
*/