using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Http.Controllers;
using System.Web.Http.ModelBinding;

namespace DashBoardAgil.DashBoard.Util
{
	public class CustomModelBinder : IModelBinder
	{
		public bool BindModel(HttpActionContext actionContext, ModelBindingContext bindingContext)
		{
			var key = bindingContext.ModelName;
			var valueProviderResult = bindingContext.ValueProvider.GetValue(key);

			if (valueProviderResult == null)
			{
				return false;
			}

			var attemptedValue = valueProviderResult.AttemptedValue;

			if (attemptedValue != null)
			{
				var list = attemptedValue.Split(new[] { "," }, StringSplitOptions.RemoveEmptyEntries).
						   Select(v => v.Trim()).ToList();

				bindingContext.Model = list;
			}
			else
			{
				bindingContext.Model = new List<string>();
			}
			return true;
		}
	}
}