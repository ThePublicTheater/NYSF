/* General todo:
 * Replace all automatic == "N", etc. structures with more thorough check that throws exceptions
 *		when encountering unexpected values
 * Replace "[]" literal with static variable, set from config, make public so that higher layers
 *		can change based on API-returned defaults
 * Convert all MOS variables to short
 * Make more Tess-specific shortcutted ToBool utility method that rides on Wbd.Utilities.Utility's
 * Implement dynamically-loaded defaults in static Tess()
 * Make Mask() functions to convert special data types to generic parameters
 * Implement receiving webcontentcollection on Tess methods that get (objects that have) webcontent
 * Types that initialize inner collections should not initialize them if they are empty
 * Make sure data retrieved is stored in nullables
 * 
 * Class file arrangement:
 *	. static members
 *	. instance members
 * 
 * Within each members section:
 *	. fields
 *	. shorthand properties
 *	. properties
 *	. constructors
 *	. methods
 *	
 * Within each section:
 * 
 *	. private
 *	. public
 *
 * 
 * 
 * The purpose of Ambitus:
 *	The problems to solve:
 *		The complex data returned by the API is a generic DataSet
 *			Should return strongly-typed object
 *		Many API method, parameter, and return value names are inconsistent, ambiguous, or misnomers
 *			Should be consistent and precise
 *		API methods take a variety of strict combinations that are not clear from the signature
 *			Should be represented by overridden methods that take the various correct sets
 *			and automatically supply omission values
 *		API methods' parameters can often take a per-installation default / constant
 *			Should allow the omission and automatic filling-in of whole-installation defaults
 *		Not all API calls need to be secure
 *		Some simple data types (of parameters and results) are off
 *			Automatically accept and convert
 *		Some methods throw exceptions instead of returning a proper result
 *	
 * To mask:
 *	Login functions should automatically fill in session key
 *	Login functions should automatically look up a promo code
 *	Login functions should automatically make sure the source Id was valid
 *	Phone numbers in the system should be formatted to a standard
 *	
 * Layer 2:
 *	Catch exceptions
 *	Store session keys
 *	Caching generic return values to be acquired when omitting session key from Tess call
 *	Compound functions
 *		login/registration preserves session source ID
 *		login/registration accept promo codes and automatically convert to session ID
 *		registration allows omittance of username - uses email automatically
 *		login/registration encrypts password based on configuration setting
 *	More intuitive function names
 *	Clean up procedures when session destroyed (logout, disconnect seat server, unload order, etc.)
 *	
 * Layer 3
 *	Automatic in-memory tracking of session-related items, such as cart expiration time, etc.
*/