﻿using FWT.Api.Controllers.User;
using FWT.Core.CQRS;
using FWT.Core.Services.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace FWT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChatController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICurrentUserProvider _userProvider;

        public ChatController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICurrentUserProvider userProvider)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _userProvider = userProvider;
        }

        [HttpGet]
        [Route("/Me")]
        [Authorize]
        public async Task<GetMe.Result> GetChats()
        {
            return await _queryDispatcher.DispatchAsync<GetChats.Query, GetMe.Result>(new GetMe.Query()
            {
                PhoneHashId = _userProvider.PhoneHashId(User)
            });
        }
    }
}