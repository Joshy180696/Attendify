using Attendify.UILayer.Interface;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewEngines;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Attendify.UILayer.Classes
{
    public class ViewRenderService : IViewRenderService
    {
        private readonly ICompositeViewEngine _compositeViewEngine;
        private readonly ITempDataProvider _tempDataProvider;

        public ViewRenderService(ICompositeViewEngine compositeViewEngine, ITempDataProvider tempDataProvider)
        {
            _compositeViewEngine = compositeViewEngine ?? throw new ArgumentNullException(nameof(compositeViewEngine));
            _tempDataProvider = tempDataProvider ?? throw new ArgumentNullException(nameof(tempDataProvider));
        }

        public async Task<string> RenderViewToStringAsync(string viewName, object model, ControllerContext controllerContext)
        {

            var viewData = new ViewDataDictionary(new EmptyModelMetadataProvider(), new ModelStateDictionary())
            {
                Model = model
            };

            var tempData = new TempDataDictionary(controllerContext.HttpContext, _tempDataProvider);

            using (var sw = new StringWriter())
            {
                var viewResult = _compositeViewEngine.FindView(controllerContext, viewName, false);
                if (viewResult.View == null)
                {
                    throw new ArgumentNullException($"View {viewName} was not found.");
                }

                var viewContext = new ViewContext(
                    controllerContext,
                    viewResult.View,
                    viewData,
                    tempData,
                    sw,
                    new HtmlHelperOptions()
                );

                await viewResult.View.RenderAsync(viewContext);
                return sw.ToString();

            }
        }

        public void SetTempDataMessage(ITempDataDictionary tempData, string type = "", string message = "")
        {
            tempData[type] = message;
        }

    }
}
