using System.Text.RegularExpressions;
using Argus.Extensions;

namespace Argus.Network
{

    /// <summary>
    /// Basic parsing an validation functions for an e-mail address.
    /// </summary>
    /// <remarks></remarks>
    public class EmailAddress
    {

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="email"></param>
        public EmailAddress(string email)
        {
            Email = email;
        }

        /// <summary>
        /// Whether or not the e-mail address is valid
        /// </summary>
        public bool IsValid()
        {
            return Regex.IsMatch(this.Email, "^([a-zA-Z0-9_\\-\\.]+)@((\\[[0-9]{1,3}\\.[0-9]{1,3}\\.[0-9]{1,3}\\.)|(([a-zA-Z0-9\\-]+\\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\\]?)$");
        }

        /// <summary>
        /// Returns the string representation of the email address.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Email;
        }

        /// <summary>
        /// The e-mail address
        /// </summary>
        /// <value></value>
        /// <returns></returns>
        /// <remarks></remarks>
        public string Email { get; set; } = "";

        /// <summary>
        /// The portion of the email before the @ symbol
        /// </summary>
        public string Username
        {
            get
            {
                if (Email.Contains("@"))
                {
                    return Email.Left(Email.IndexOf("@"));
                }
                else
                {
                    return "";
                }
            }
        }
    }
}