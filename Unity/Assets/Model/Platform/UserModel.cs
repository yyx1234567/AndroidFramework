using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace ETModel
{
    public class UserModel
    {
        public Guid tenantId;
        public int CustomerId { get; set; }
        public int EnterpriseId { get; set; }
        public string EnterpriseName { get; set; }
        public bool IsCompleteInfo { get; set; }
        public string Name { get; set; }
        public object Permissions { get; set; }
        public string Roles { get; set; }
        public string Token { get; set; }
        public Guid UserId { get; set; }
        public string UserImg { get; set; }
        public string UserName { get; set; }
        public string ClassName { get; set; }
         public int CourseId { get; set; }
        public string CourseName { get; set; }
        public int CourseEnd { get; set; }
    }
}