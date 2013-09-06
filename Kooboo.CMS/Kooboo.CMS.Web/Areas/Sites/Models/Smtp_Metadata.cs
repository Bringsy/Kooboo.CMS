#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Sites.Models;
using Kooboo.ComponentModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
namespace Kooboo.CMS.Web.Areas.Sites.Models
{
    [MetadataFor(typeof(Smtp))]
    public class Smtp_Metadata
    {
        [DisplayName("SMTP Server")]
        public string Host { get; set; }
        public string UserName { get; set; }
        [UIHint("Password")]
        public string Password { get; set; }
        [DefaultValue(25)]
        [Required(ErrorMessage = "Required")]
        public int Port { get; set; }
        [DisplayName("Enable SSL")]
        [Required(ErrorMessage = "Required")]
        public bool EnableSsl { get; set; }
        [UIHint("Array")]
        [DisplayName("To address")]
        [Description("The receiver's Email addresses of your contact form")]
        public string[] To { get; set; }
        [DisplayName("From address")]
        [Description("The FROM email address of mails sent out by the system. For example: user registration confirmation email.")]
        public string From { get; set; }
        [DisplayName("Pickup directory location")]
        [Description("Absolute path to the folder where system save mail messages to be processed by the local SMTP server.")]
        public string PickupDirectoryLocation { get; set; }
    }
}