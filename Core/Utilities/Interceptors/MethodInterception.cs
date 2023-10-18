using Castle.DynamicProxy;

namespace Core.Utilities.Interceptors;

public abstract class MethodInterception : MethodInterceptionBaseAttributes
{
    protected virtual void OnBefore(IInvocation invocation){}
    protected virtual void OnAfter(IInvocation invocation){}
    protected virtual void OnSuccess(IInvocation invocation){}
    protected virtual void OnException(IInvocation invocation, Exception e){}
    
    
    public override void Intercept(IInvocation invocation)
    {
        var IsSuccess = true;
        OnBefore(invocation);

        try
        {
            invocation.Proceed();
        }
        catch (Exception e)
        {
            IsSuccess = false;
            OnException(invocation, e);
            throw;
        }
        finally
        {
            if (IsSuccess)
            {
                OnSuccess(invocation);
            }
        }
        OnAfter(invocation);
    }
}