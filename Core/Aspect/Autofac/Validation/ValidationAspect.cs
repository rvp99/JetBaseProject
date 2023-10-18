using System.Reflection;
using Castle.DynamicProxy;
using Core.CrossCuttıingConcerns.Validation;
using Core.Utilities.Interceptors;
using FluentValidation;

namespace Core.Aspect.Autofac.Validation;

public class ValidationAspect : MethodInterception
{
    private Type _validationType;

    public ValidationAspect(Type validationType)
    {
        if (!typeof(IValidator).IsAssignableFrom(validationType))
        {
            throw new Exception("Bu bir doğrulama sınıfı değildir.");
        }
        _validationType = validationType;
    }


    protected override void OnBefore(IInvocation invocation)
    {
        var validator = (IValidator)Activator.CreateInstance(_validationType);
        var entityType = _validationType.BaseType.GetGenericArguments()[0];
        var entities = invocation.Arguments.Where(type => type.GetType() == entityType);

        foreach (var entity in entities)
        {
            ValidationTool.Validate(validator,entity);
        }
    }
}