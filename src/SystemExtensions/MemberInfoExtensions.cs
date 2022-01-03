﻿using System;
using System.Reflection;

namespace Decoherence.SystemExtensions
{
    public static class MemberInfoExtensions
    {
        public static bool CanWrite(this MemberInfo memberInfo)
        {
            if (memberInfo is PropertyInfo propertyInfo)
            {
                return propertyInfo.CanWrite;
            }

            return memberInfo is FieldInfo fieldInfo;
        }

        public static bool CanRead(this MemberInfo memberInfo)
        {
            if (memberInfo is PropertyInfo propertyInfo)
            {
                return propertyInfo.CanRead;
            }

            return memberInfo is FieldInfo fieldInfo;
        }

        public static void SetValue(this MemberInfo memberInfo, object? obj, object? value)
        {
            if (memberInfo is PropertyInfo propertyInfo)
            {
                propertyInfo.SetValue(obj, value);
            }
            else if (memberInfo is FieldInfo fieldInfo)
            {
                fieldInfo.SetValue(obj, value);
            }
            else
            {
                throw new InvalidOperationException($"{memberInfo} can not set value.");
            }
        }
    }
}