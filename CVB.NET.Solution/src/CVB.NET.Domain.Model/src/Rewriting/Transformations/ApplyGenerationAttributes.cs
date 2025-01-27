namespace CVB.NET.Domain.Model.Rewriting.Transformations
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using PostSharp.Aspects;
    using PostSharp.Reflection;

    [Serializable]
    public class ApplyGenerationAttributes : TypeLevelAspect, IAspectProvider
    {
        private static readonly CustomAttributeIntroductionAspect
            customAttributeIntroductionAspect =
                new CustomAttributeIntroductionAspect(
                    new ObjectConstruction(typeof (GenerateProxyTypeAttribute)));

        public IEnumerable<AspectInstance> ProvideAspects(object targetElement)
        {
            if (!(targetElement is Type))
            {
                yield break;
            }

            Type t = (Type) targetElement;

            if (!t.GetCustomAttributes(true).OfType<GenerateProxyTypeAspect>().Any())
            {
                yield break;
            }

            yield return new AspectInstance(targetElement, customAttributeIntroductionAspect);
        }
    }
}