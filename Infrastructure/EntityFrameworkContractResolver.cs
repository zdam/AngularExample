using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using Newtonsoft.Json.Serialization;

namespace AngularExample.Infrastructure
{
    public class EntityFrameworkContractResolver : DefaultContractResolver
    {
        private static readonly IEnumerable<Type> Types = GetEntityTypes();

        private static IEnumerable<Type> GetEntityTypes()
        {
            var types = Assembly.GetExecutingAssembly().GetTypes().Where(t => String.Equals(t.Namespace, "AngularExample", StringComparison.Ordinal));
            return types;
        }

        protected override List<MemberInfo> GetSerializableMembers(Type objectType)
        {
            if (!AllowType(objectType))
                return new List<MemberInfo>();

            var members = base.GetSerializableMembers(objectType);
            members.RemoveAll(memberInfo => (IsMemberEntityWrapper(memberInfo)));
            return members;
        }

        private static bool AllowType(Type objectType)
        {
            return Types.Contains(objectType) || Types.Contains(objectType.BaseType);
        }

        private static bool IsMemberEntityWrapper(MemberInfo memberInfo)
        {
            return memberInfo.Name == "_entityWrapper";
        }
    }
}