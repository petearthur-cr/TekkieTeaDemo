using System;
using Cloudreach.Connect.Api;
using io;
namespace TekkieTeaDemo
{
    public class RestService : IRestService
    {
        public double Version => 1.0;

        public string Endpoint => "/implement";

        public string ApplicationKey => "";

        public bool IsPublic => true;

        public IResult Implement(ISynchronousContext context)
        {
            try
            {
                context.LogService.Info("Started restservice");

                string code = context.RequestBody;

                if (String.IsNullOrWhiteSpace(code))
                    return context.CreateErrorResult(500, "You must supply a body for the code");
                
				IoState state = new IoState();
                state.processBootstrap();

                IoObject returnValue = state.onDoCStringWithLabel(state.lobby, code.Trim(), "dotnet:");

                context.LogService.Info("Finished restservice");

                if (returnValue == null)
                    return context.CreateResult("Return value was null");

                if (returnValue is IoNumber)

                    return context.CreateResult((returnValue as IoNumber).asDouble());

                return context.CreateResult(returnValue.ToString());
            }
            catch (Exception e)
            {
                return context.CreateErrorResult(500, e.Message);
            }
        }
    }
}
