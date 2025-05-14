using CashFlow.Application.UseCases.Users.DeleteProfile;
using CashFlow.Application.UseCases.Users.GetProfile;
using CashFlow.Application.UseCases.Users.Register;
using CashFlow.Application.UseCases.Users.UpdatePassword;
using CashFlow.Application.UseCases.Users.UpdateProfile;
using CashFlow.Communication.Requests;
using CashFlow.Communication.Responses;
using CashFlow.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CashFlow.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {

        [HttpPost]
        [ProducesResponseType(typeof(ResponseRegisteredUserJson), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> RegisterUser([FromServices] IRegisterUserUseCase useCase,[FromBody] RequestRegisterUserJson request)
        {
            var response = await useCase.Execute(request);

            return Created(string.Empty, response);
        }

        [HttpGet]
        [ProducesResponseType(typeof(ResponseProfileShortJson), StatusCodes.Status200OK)]
        [Authorize]
        public async Task<IActionResult> GetProfile([FromServices] IGetUserProfileUseCase useCase)
        {
            var response = await useCase.Execute();
            
            return Ok(response);
        }

        [HttpPut]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdateProfile([FromServices] IUpdateUserUseCase useCase,[FromBody] RequestUpdateUserJson request)
        {
            await useCase.Execute(request);
            
            return NoContent();
        }

        [HttpPut("change-password")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(typeof(ResponseErrorJson), StatusCodes.Status400BadRequest)]
        public async Task<IActionResult> UpdatePassword([FromServices] IUpdatePasswordUseCase useCase, [FromBody] RequestChangePasswordJson request)
        {
            await useCase.Execute(request);
            
            return NoContent();
        }

        [HttpDelete]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> DeleteProfile([FromServices] IDeleteUserAccountUseCase useCase)
        {
            await useCase.Execute();
            
            return NoContent();
        }
        
        
    }
}
