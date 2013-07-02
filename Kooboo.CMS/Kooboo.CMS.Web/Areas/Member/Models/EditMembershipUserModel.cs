﻿#region License
// 
// Copyright (c) 2013, Kooboo team
// 
// Licensed under the BSD License
// See the file LICENSE.txt for details.
// 
#endregion
using Kooboo.CMS.Member.Models;
using Kooboo.CMS.Web.Areas.Member.Models.DataSources;
using Kooboo.CMS.Web.Models;
using Kooboo.Web.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;

namespace Kooboo.CMS.Web.Areas.Member.Models
{
    public class EditMembershipUserModel
    {
        public EditMembershipUserModel()
        {
        }
        public EditMembershipUserModel(MembershipUser membershipUser)
        {
            this.UserName = membershipUser.UserName;
            this.Email = membershipUser.Email;
            this.IsApproved = membershipUser.IsApproved;
            this.IsLockedOut = membershipUser.IsLockedOut;
            this.PasswordQuestion = membershipUser.PasswordQuestion;
            this.PasswordAnswer = membershipUser.PasswordAnswer;
            this.Culture = membershipUser.Culture;
            this.TimeZoneId = membershipUser.TimeZoneId;
            this.Comment = membershipUser.Comment;
            this.Profiles = membershipUser.Profiles;
            this.MembershipGroups = membershipUser.MembershipGroups;
        }
        [RemoteEx("IsUserNameAvailable", "MembershipUser", RouteFields = "MembershipName")]
        [Required(ErrorMessage = "Required")]
        [DisplayName("User name")]
        [Description("The logon name of the membership user.")]
        public virtual string UserName { get; set; }

        [RemoteEx("IsEmailAvailable", "MembershipUser", RouteFields = "MembershipName")]
        [Required(ErrorMessage = "Required")]
        [RegularExpression(RegexPatterns.EmailAddress, ErrorMessage = "Invalid email address")]
        [DisplayName("Email")]
        [Description("The e-mail address for the membership user.")]
        public virtual string Email { get; set; }

        [DisplayName("Is approved")]
        [Description("The value indicating whether the membership user can be authenticated.")]
        public virtual bool IsApproved { get; set; }

        [DisplayName("Is lockedout")]
        [Description("The value indicating whether the membership user is locked out and unable to be validated.")]
        public virtual bool IsLockedOut { get; set; }

        [DisplayName("Password question")]
        [Description("The password question for the membership user.")]
        public virtual string PasswordQuestion { get; set; }

        [DisplayName("Password answer")]
        [Description("The password question answer for the membership user.")]
        public virtual string PasswordAnswer { get; set; }

        [UIHintAttribute("DropDownList")]
        [DataSource(typeof(CultureSelectListDataSource))]
        public virtual string Culture { get; set; }

        [Display(Name = "Time zone")]
        [UIHint("DropDownList")]
        [DataSource(typeof(TimeZonesDataSource))]
        public virtual string TimeZoneId { get; set; }

        [UIHint("MultilineText")]
        public virtual string Comment { get; set; }

        [UIHint("Dictionary")]
        public virtual Dictionary<string, string> Profiles { get; set; }

        [DisplayName("Membership groups")]
        [UIHint("Multiple_DropDownList")]
        [DataSource(typeof(MembershipGroupDataSource))]
        public virtual string[] MembershipGroups { get; set; }
    }
}
