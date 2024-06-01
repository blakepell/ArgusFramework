/*
 * @author            : Blake Pell
 * @initial date      : 2005-09-28
 * @last updated      : 2019-11-17
 * @copyright         : Copyright (c) 2003-2024, All rights reserved.
 * @license           : MIT 
 * @website           : http://www.blakepell.com
 */

using Argus.Extensions;
using System.Net.Mail;

namespace Argus.Network
{
    /// <summary>
    /// This class will allow a client to send e-mail via a specified mail system.
    /// </summary>
    public class Mail
    {
        /// <summary>
        /// The fields that can be populated via PopulateAddressString
        /// </summary>
        public enum PopulateTypes
        {
            /// <summary>
            /// The recipients of the email message.
            /// </summary>
            To,

            /// <summary>
            /// The blind carbon copy recipients of the email message.
            /// </summary>
            Bcc,

            /// <summary>
            /// The carbon copy recipients of the email message.
            /// </summary>
            Cc
        }

        /// <summary>
        /// Constructor
        /// </summary>
        public Mail()
        {
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="mailSubject"></param>
        /// <param name="mailBody"></param>
        /// <param name="isBodyHtml"></param>
        public Mail(string mailTo, string mailSubject, string mailBody, bool isBodyHtml)
        {
            this.To = mailTo;
            this.Subject = mailSubject;
            this.Body = mailBody;
            this.IsBodyHtml = isBodyHtml;
        }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mailTo"></param>
        /// <param name="mailSubject"></param>
        /// <param name="mailBody"></param>
        /// <param name="isBodyHtml"></param>
        /// <param name="networkUsername"></param>
        /// <param name="networkPassword"></param>
        public Mail(string mailTo, string mailSubject, string mailBody, bool isBodyHtml, string networkUsername, string networkPassword)
        {
            this.To = mailTo;
            this.Subject = mailSubject;
            this.Body = mailBody;
            this.IsBodyHtml = isBodyHtml;
            this.NetworkUsername = networkUsername;
            this.NetworkPassword = networkPassword;
        }

        /// <summary>
        /// Who the mail should be sent to.  This should be a string of e-mail addresses separated by commas or semi-colons
        /// </summary>
        public string To { get; set; } = "";

        /// <summary>
        /// Who to blind carbon copy.  This should be a string of e-mail addresses separated by commas or semi-colons.
        /// </summary>
        public string Bcc { get; set; } = "";

        /// <summary>
        /// Who the mail should be sent from.
        /// </summary>
        public string From { get; set; } = "";

        /// <summary>
        /// The e-mail address or addresses to carbon copy separated by a semi-colon or comma.
        /// </summary>
        public string Cc { get; set; } = "";

        /// <summary>
        /// The e-mail address that a recipient should reply to.
        /// </summary>
        public string ReplyTo { get; set; } = "";

        /// <summary>
        /// The subject of the e-mail message.
        /// </summary>
        public string Subject { get; set; } = "";

        /// <summary>
        /// The body of the e-mail message.
        /// </summary>
        public string Body { get; set; } = "";

        /// <summary>
        /// Whether the body of the e-mail is HTML or not.  If set to true the text of the body will be interpreted as HTML.
        /// </summary>
        public bool IsBodyHtml { get; set; }

        /// <summary>
        /// A list of mail attachments.
        /// </summary>
        public List<Attachment> Attachments { get; set; } = new List<Attachment>();

        /// <summary>
        /// The priority of the e-mail message.
        /// </summary>
        public MailPriority Priority { get; set; } = MailPriority.Normal;

        /// <summary>
        /// The mail server that should be used.
        /// </summary>
        public string MailServer { get; set; } = "";

        /// <summary>
        /// The username that will be used to authenticate to the mail server.
        /// </summary>
        public string NetworkUsername { get; set; } = "";

        /// <summary>
        /// The password that will be used to authenticate to the mail server.
        /// </summary>
        public string NetworkPassword { get; set; } = "";

        /// <summary>
        /// The domain that will be authenticated to when connecting to the mail server.
        /// </summary>
        public string NetworkDomain { get; set; } = "";

        /// <summary>
        /// Whether or not SSL encryption is enabled on the mail message.  The default value for this property is True.
        /// </summary>
        public bool EnableSsl { get; set; } = true;

        /// <summary>
        /// The port to connect on.  The default is set at 0 which won't be used.  Any other value will be passed on to the wrapped object.
        /// </summary>
        public int Port { get; set; } = 0;

        /// <summary>
        /// The display name that should go with the email address.
        /// </summary>
        public string FromDisplayName { get; set; } = "";

        /// <summary>
        /// Sends a mail message.  An exception will be thrown on a failure.
        /// </summary>
        public void SendMail()
        {
            var mail = new MailMessage();

            if (string.IsNullOrEmpty(this.From) & !string.IsNullOrEmpty(this.NetworkUsername))
            {
                throw new Exception("Either the 'From' property or the 'NetworkUsername' property must be set in order to send a mail message.");
            }

            if (string.IsNullOrWhiteSpace(this.FromDisplayName))
            {
                mail.From = new MailAddress(this.From);
            }
            else
            {
                mail.From = new MailAddress(this.From, this.FromDisplayName);
            }

            mail.Subject = this.Subject;

            if (!string.IsNullOrEmpty(this.ReplyTo))
            {
                mail.ReplyToList.Add(new MailAddress(this.ReplyTo));
            }

            if (this.To.Contains(",") || this.To.Contains(";"))
            {
                // Convert multiple delimiters to a single one
                this.To = this.To.Replace(",", ";");

                foreach (string buf in this.To.Split(';'))
                {
                    var address = new EmailAddress(buf.Trim());

                    if (address.IsValid())
                    {
                        mail.To.Add(new MailAddress(buf.Trim()));
                    }
                }
            }
            else
            {
                mail.To.Add(new MailAddress(this.To.Trim()));
            }

            if (this.Cc.Contains(",") || this.Cc.Contains(";"))
            {
                // Convert multiple delimiters to a single one
                this.Cc = this.Cc.Replace(",", ";");

                foreach (string buf in this.Cc.Split(';'))
                {
                    var address = new EmailAddress(buf.Trim());

                    if (address.IsValid())
                    {
                        mail.CC.Add(new MailAddress(buf.Trim()));
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.Cc))
                {
                    var address = new EmailAddress(this.Cc.Trim());

                    if (address.IsValid())
                    {
                        mail.CC.Add(new MailAddress(this.Cc.Trim()));
                    }
                }
            }

            if (this.Bcc.Contains(",") || this.Bcc.Contains(";"))
            {
                // Convert multiple delimiters to a single one
                this.Bcc = this.Bcc.Replace(",", ";");

                foreach (string buf in this.Bcc.Split(';'))
                {
                    var address = new EmailAddress(buf.Trim());

                    if (address.IsValid())
                    {
                        mail.Bcc.Add(new MailAddress(buf.Trim()));
                    }
                }
            }
            else
            {
                if (!string.IsNullOrEmpty(this.Bcc))
                {
                    var address = new EmailAddress(this.Bcc.Trim());

                    if (address.IsValid())
                    {
                        mail.Bcc.Add(new MailAddress(this.Bcc.Trim()));
                    }
                }
            }

            mail.Body = this.Body;
            mail.IsBodyHtml = this.IsBodyHtml;

            foreach (var doc in this.Attachments)
            {
                mail.Attachments.Add(doc);
            }

            using (var smtp = new SmtpClient())
            {
                if (!string.IsNullOrEmpty(this.NetworkUsername))
                {
                    smtp.UseDefaultCredentials = false;

                    if (string.IsNullOrEmpty(this.NetworkDomain))
                    {
                        smtp.Credentials = new NetworkCredential(this.NetworkUsername, this.NetworkPassword);
                    }
                    else
                    {
                        smtp.Credentials = new NetworkCredential(this.NetworkUsername, this.NetworkPassword, this.NetworkDomain);
                    }

                    smtp.EnableSsl = this.EnableSsl;
                }

                if (this.Port > 0)
                {
                    smtp.Port = this.Port;
                }

                smtp.Host = this.MailServer;
                smtp.Send(mail);
            }
        }

        /// <summary>
        /// This will take a list of email addresses, format them and put them into the selected field.  This will overwrite any values currently
        /// in that field.
        /// </summary>
        /// <param name="fieldToPopulate"></param>
        /// <param name="values"></param>
        public void PopulateAddressString(PopulateTypes fieldToPopulate, List<string> values)
        {
            var sb = new StringBuilder();

            foreach (string buf in values)
            {
                sb.AppendFormat("{0};", buf);
            }

            switch (fieldToPopulate)
            {
                case PopulateTypes.To:
                    this.To = sb.ToString().Trim(";");

                    break;
                case PopulateTypes.Bcc:
                    this.Bcc = sb.ToString().Trim(";");

                    break;
                case PopulateTypes.Cc:
                    this.Cc = sb.ToString().Trim(";");

                    break;
            }
        }

        /// <summary>
        /// This will take a list of email addresses, format them and put them into the selected field.  If the selected entry is a username
        /// and not an email address, this will attempt to cross reference a user table given the required database connection and sql statement.
        /// The SQL statement must return to fields, "username" and "email".. e.g. select username, email from web_users.  If an email address isn't
        /// found for a username, it will be excluded from the send list.  The database connection must already be initialized and open, this sub
        /// also won't close it so you must close and dispose of it on your own.
        /// </summary>
        /// <param name="fieldToPopulate"></param>
        /// <param name="values"></param>
        public void PopulateAddressString(PopulateTypes fieldToPopulate, List<string> values, IDbConnection conn, string sql)
        {
            if (conn == null)
            {
                throw new Exception("Database connection was null.");
            }

            if (conn.State != ConnectionState.Open)
            {
                throw new Exception("Database connection must be open.");
            }

            var userList = new NameValueCollection();

            var command = conn.CreateCommand();
            command.CommandText = sql;
            var dr = command.ExecuteReader();

            while (dr.Read())
            {
                userList.Add(dr["username"].ToString(), dr["gmail"].ToString());
            }

            var sb = new StringBuilder();


            foreach (string buf in values)
            {
                if (buf.Contains("@"))
                {
                    // It's an e-mail, straight add it.
                    sb.AppendFormat("{0};", buf);
                }
                else
                {
                    // It's a username, attempt to look it up
                    string email = userList[buf];

                    if (string.IsNullOrEmpty(email) == false)
                    {
                        sb.AppendFormat("{0};", email);
                    }
                }
            }

            switch (fieldToPopulate)
            {
                case PopulateTypes.To:
                    this.To = sb.ToString().Trim(";");

                    break;
                case PopulateTypes.Bcc:
                    this.Bcc = sb.ToString().Trim(";");

                    break;
                case PopulateTypes.Cc:
                    this.Cc = sb.ToString().Trim(";");

                    break;
            }

            // Cleanup the database object we created.
            dr.Close();
            command.Dispose();
        }

        /// <summary>
        /// This will take a list of email addresses, format them and put them into the selected field.  If the selected entry is a username
        /// and not an email address, this will attempt to cross reference a user table given the required database connection and sql statement.
        /// The SQL statement must return to fields, "username" and "email".. e.g. select username, email from web_users.  If an email address isn't
        /// found for a username, it will be excluded from the send list.  The database connection must already be initialized and open, this sub
        /// also won't close it so you must close and dispose of it on your own.
        /// </summary>
        /// <param name="fieldToPopulate"></param>
        /// <param name="values"></param>
        public void PopulateAddressString(PopulateTypes fieldToPopulate, string[] values, IDbConnection conn, string sql)
        {
            this.PopulateAddressString(fieldToPopulate, values.ToList(), conn, sql);
        }

        /// <summary>
        /// Adds a new attachment to the attachment list from a file path.
        /// </summary>
        /// <param name="filePath"></param>
        public void AddAttachment(string filePath)
        {
            if (File.Exists(filePath) == false)
            {
                throw new FileNotFoundException();
            }

            var attach = new Attachment(filePath);
            this.Attachments.Add(attach);
        }

        /// <summary>
        /// Returns a string formatted representation of the current mail message.
        /// </summary>
        public override string ToString()
        {
            var sb = new StringBuilder();
            sb.AppendFormat("To: {0}{1}", this.To, "\r\n");

            if (string.IsNullOrWhiteSpace(this.Cc) == false)
            {
                sb.AppendFormat("Cc: {0}{1}", this.Cc, "\r\n");
            }

            if (string.IsNullOrWhiteSpace(this.Bcc) == false)
            {
                sb.AppendFormat("Bcc: {0}{1}", this.Bcc, "\r\n");
            }

            sb.AppendFormat("From: {0}{1}", this.From, "\r\n");

            if (string.IsNullOrWhiteSpace(this.ReplyTo) == false)
            {
                sb.AppendFormat("Reply To: {0}{1}", this.ReplyTo, "\r\n");
            }

            sb.AppendFormat("Subject: {0}{1}", this.Subject, "\r\n");

            if (this.Attachments.Count > 0)
            {
                sb.AppendFormat("Attachments:  {0}{1}", this.Attachments.Count, "\r\n");
                sb.Append("Attachment List: ");

                foreach (var attach in this.Attachments)
                {
                    sb.AppendFormat("{0},", attach.Name);
                }

                sb.Append("\r\n");
            }


            sb.AppendFormat("Body: {0}", this.Body);

            return sb.ToString();
        }
    }
}