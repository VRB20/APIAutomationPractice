using CoreFramework.Extensions;
using System;

namespace CoreFramework.Services
{
    public static class StepWrappingExtensions
    {
        public static ActionExecuteExtensionHook Execute(this Action theWork) =>
            new ActionExecuteExtensionHook(theWork);

        public static FuncExecuteExtensionHook<TOut> Execute<TOut>(this Func<TOut> theWork) =>
            new FuncExecuteExtensionHook<TOut>(theWork);

        public static void ReportStep(this string theStepDescription, Action action)
        {
            theStepDescription.Log();
            action();
        }

        public static T ReportStep<T>(this string theStepDescription, Func<T> theWorkFunc)
        {
            theStepDescription.Log();
            return theWorkFunc();
        }

        public class ActionExecuteExtensionHook
        {
            private readonly Action _theWorkAction;
            public ActionExecuteExtensionHook(Action theWorkAction) => _theWorkAction = theWorkAction;
            public void AndReportAsStep(string stepName) => stepName.ReportStep(_theWorkAction);
        }

        public class FuncExecuteExtensionHook<TOut>
        {
            private readonly Func<TOut> _theWorkFunc;
            public FuncExecuteExtensionHook(Func<TOut> theWorkFunc) => _theWorkFunc = theWorkFunc;
            public TOut AndReportAsStep(String stepName) => stepName.ReportStep(_theWorkFunc);

        }
    }
}
