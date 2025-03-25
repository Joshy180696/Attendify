using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Attendify.UILayer.Interface
{
    public interface IViewRenderService
    {
        Task<string> RenderViewToStringAsync(string viewName, object model, ControllerContext controllerContext);
        void SetTempDataMessage(ITempDataDictionary tempData, string type = "", string message = "");

    }
}
