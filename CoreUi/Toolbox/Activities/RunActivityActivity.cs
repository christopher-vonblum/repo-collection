using System;
using System.Collections.Generic;
using CoreUi;
using CoreUi.Proxy;

namespace Toolbox.Activities
{
    public class RunActivityActivity : ActivityBase<string, object>
    {
        private readonly ToolBox _toolBox;
        private readonly IInteractionProvider _interactionProvider;

        public RunActivityActivity(ToolBox toolBox, IInteractionProvider interactionProvider)
        {
            _toolBox = toolBox;
            _interactionProvider = interactionProvider;
        }

        public override object Execute(string input)
        {
            Type activity = null;
            
            if (input == null)
            {
                activity = _interactionProvider.RequestDecision<IEnumerable<Type>, Type>(_toolBox.Activities, (e) => e.FullName);
            }
            else
            {
                activity = Type.GetType(input);
            }

            if (activity == null)
            {
                throw new InvalidOperationException("Activity type not found " + input);
            }
            
            var modelType = ExtractActivityDataModel(activity);
            var model = _interactionProvider.CreateInputModel(modelType);

            _interactionProvider.RequestInput(modelType, null, out model);

            return _toolBox.Run(activity, model);
        }
        
        private static Type ExtractActivityDataModel(Type activity)
        {
            return activity.GetInterface("IActivity`2").GetGenericArguments()[0];
        }
    }
}