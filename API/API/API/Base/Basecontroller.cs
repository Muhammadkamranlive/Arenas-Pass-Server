using Microsoft.AspNetCore.Mvc;
using Server.Models;

namespace API.API.Base;

public class Basecontroller : ControllerBase
{
    public dynamic CatchException(Exception ex)
    {
        var successResponse = new SuccessResponse
        {
            Message = "Error" + ex.Message + (string.IsNullOrEmpty(ex.InnerException.Message) ? "" : ex.InnerException.Message)
        };
        return StatusCode(5000, successResponse);
    }
}