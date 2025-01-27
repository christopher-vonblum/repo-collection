namespace CVB.NET.Abstractions.Ioc.Injection.Lambda
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;

    using CVB.NET.Abstractions.Ioc.Injection.Parameter;
    using CVB.NET.Reflection.Caching.Cached;

    public class DependencyInjectionLambdaGenerator : IDependencyInjectionLambdaGenerator
    {
        private static CachedMethodInfo dependencyMethod;
        private static CachedMethodInfo namedDependencyMethod;

        static DependencyInjectionLambdaGenerator()
        {
            CachedType argType = typeof (Arg);

            dependencyMethod = argType.StaticMethods.Single(m => m.InnerReflectionInfo.Name.Equals(nameof(Arg.Dependency)) && m.ParameterInfos.Length == 1 && m.ParameterInfos.Single().ParameterType == typeof (Type));
            namedDependencyMethod = argType.StaticMethods.Single(m => m.InnerReflectionInfo.Name.Equals(nameof(Arg.Dependency)) && m.ParameterInfos.Length == 2 && m.ParameterInfos.First().ParameterType == typeof(Type) && m.ParameterInfos.Last().ParameterType == typeof(string));
        }

        public Func<object> CreateConstructorInjectionLambda(CachedConstructorInfo injectionConstructor, Func<CachedParameterInfo, string> getDependencyName)
        {
            Type returnType = typeof (object);

            ParameterExpression constructionResult = Expression.Variable(returnType, "constructionResult");

            var returnLabel = Expression.Label(returnType, "returnLabel");

            List<Expression> methodBody = new List<Expression>
                                          {
                                              Expression.Assign(constructionResult,
                                                  Expression.New(injectionConstructor,
                                                      BuildInjectionLambdaArguments(
                                                          injectionConstructor.CachedParameterInfos,
                                                          getDependencyName))),

                                              Expression.Return(returnLabel, constructionResult, returnType),
                                              Expression.Label(returnLabel, constructionResult)
                                          };

            return Expression
                .Lambda<Func<object>>(Expression.Block(new List<ParameterExpression> {constructionResult}, methodBody))
                .Compile();
        }

        public Action<object> CombineMemberInjectionLambdas(Action<object>[] memberInjectionLambdas)
        {
            if (memberInjectionLambdas.Length == 0)
            {
                throw new InvalidOperationException();
            }

            if (memberInjectionLambdas.Length == 1)
            {
                return memberInjectionLambdas[0];
            }

            Type injectionTargetType = typeof(object);

            ParameterExpression injectionTarget = Expression.Parameter(injectionTargetType, "injectionTarget");

            List<Expression> methodBody = new List<Expression>();

            foreach (Action<object> injectionLambda in memberInjectionLambdas)
            {
                methodBody.Add(Expression.Invoke(Expression.Constant(injectionLambda), injectionTarget));
            }

            return Expression
                .Lambda<Action<object>>(Expression.Block(new List<ParameterExpression>(), methodBody), injectionTarget)
                .Compile();
        }

        public Func<object> CombineConstructorInjectionAndMemberInjectionLambdas(Func<object> constructorInjectionLambda, Action<object>[] memberInjectionLambdas)
        {
            if (!memberInjectionLambdas.Any())
            {
                return constructorInjectionLambda;
            }

            Type returnType = typeof(object);

            ParameterExpression constructionResult = Expression.Variable(returnType, "constructionResult");

            var returnLabel = Expression.Label(returnType, "returnLabel");

            List<Expression> methodBody = new List<Expression>
                                          {
                                              Expression.Assign(constructionResult, Expression.Invoke(Expression.Constant(constructorInjectionLambda)))
                                          };

            foreach (Action<object> injectionLambda in memberInjectionLambdas)
            {
                methodBody.Add(Expression.Invoke(Expression.Constant(injectionLambda), constructionResult));
            }

            methodBody.Add(Expression.Return(returnLabel, constructionResult, returnType));
            methodBody.Add(Expression.Label(returnLabel, constructionResult));

            return Expression
                .Lambda<Func<object>>(Expression.Block(new List<ParameterExpression> { constructionResult }, methodBody))
                .Compile();
        }

        public Action<object> CreateMethodInjectionLambda(Type targetType, CachedMethodInfo injectionMethod, Func<CachedParameterInfo, string> getDependencyName)
        {
            ParameterExpression injectionTarget = Expression.Parameter(typeof(object), "injectionTarget");
            ParameterExpression typedInjectionTarget = Expression.Variable(targetType, "typedInjectionTarget");


            List<Expression> methodBody = new List<Expression>
                                          {
                                                Expression.Assign(typedInjectionTarget, Expression.Convert(injectionTarget, targetType)),
                                                Expression.Call(typedInjectionTarget, 
                                                    injectionMethod,
                                                    BuildInjectionLambdaArguments(
                                                        injectionMethod.CachedParameterInfos,
                                                        getDependencyName))
                                          };


            return Expression
                .Lambda<Action<object>>(Expression.Block(new [] { typedInjectionTarget }, methodBody), injectionTarget)
                .Compile();
        }

        public Action<object> CreatePropertyInjectionLambda(Type targetType, CachedPropertyInfo[] injectionProperties, Func<CachedPropertyInfo, string> getDependencyName)
        {
            ParameterExpression injectionTarget = Expression.Parameter(typeof(object), "injectionTarget");

            ParameterExpression typedInjectionTarget = Expression.Parameter(targetType, "typedInjectionTarget");

            List<Expression> injectionExpressions = new List<Expression>
                                                    {
                                                        Expression.Assign(typedInjectionTarget, Expression.Convert(injectionTarget, targetType))
                                                    };

            foreach (PropertyInfo propertyInfo in injectionProperties)
            {
                MemberExpression propertyExpression = Expression.Property(typedInjectionTarget, propertyInfo);

                Expression argInjectionPlaceholder = MakeArgInjectionPlaceholder(propertyInfo.PropertyType, getDependencyName(propertyInfo));

                injectionExpressions.Add(Expression.Assign(propertyExpression, argInjectionPlaceholder));
            }

            return Expression
                    .Lambda<Action<object>>(Expression.Block(new[] { typedInjectionTarget }, injectionExpressions), injectionTarget)
                    .Compile();
        }

        private static Expression[] BuildInjectionLambdaArguments(CachedParameterInfo[] parameterInfos, Func<CachedParameterInfo, string> getDependencyName)
        {
            List<Expression> arguments = new List<Expression>();

            foreach (CachedParameterInfo parmeterInfo in parameterInfos)
            {
                arguments.Add(MakeArgInjectionPlaceholder(parmeterInfo.InnerReflectionInfo.ParameterType, getDependencyName(parmeterInfo)));
            }

            return arguments.ToArray();
        }

        private static Expression MakeArgInjectionPlaceholder(Type placeholderType, string name = null)
        {
            if (name != null)
            {
                return Expression.Convert(Expression.Call(namedDependencyMethod, Expression.Constant(placeholderType), Expression.Constant(name)), placeholderType);
            }

            return Expression.Convert(Expression.Call(dependencyMethod, Expression.Constant(placeholderType)), placeholderType);
        }
    }
}