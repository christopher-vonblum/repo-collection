namespace CVB.NET.Reflection.Caching.Wrapper
{
    using System.Reflection;
    using Cached;

    public abstract class MemberInfoWrapperBase<TMemberInfo>
        : ReflectionSubInfoWrapperBase<TMemberInfo, CachedType>
        where TMemberInfo : MemberInfo
    {
        protected MemberInfoWrapperBase(TMemberInfo memberInfo) : base(memberInfo)
        {
        }

        protected override CachedType GetDeclaringReflectionInfo()
        {
            return InnerReflectionInfo.ReflectedType;
        }
    }
}