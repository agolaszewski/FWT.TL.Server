﻿using FWT.Api.Controllers.Dialog;
using FWT.Core.CQRS;
using FWT.Core.Services.User;
using FWT.Infrastructure.Telegram.Parsers.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace FWT.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactsController : ControllerBase
    {
        private readonly ICommandDispatcher _commandDispatcher;
        private readonly IQueryDispatcher _queryDispatcher;
        private readonly ICurrentUserProvider _userProvider;

        public ContactsController(ICommandDispatcher commandDispatcher, IQueryDispatcher queryDispatcher, ICurrentUserProvider userProvider)
        {
            _commandDispatcher = commandDispatcher;
            _queryDispatcher = queryDispatcher;
            _userProvider = userProvider;
        }

        [HttpGet]
        [Authorize]
        public async Task<List<TelegramContact>> GetContacts()
        {
            return await _queryDispatcher.DispatchAsync<GetContacts.Query, List<TelegramContact>>(new GetContacts.Query()
            {
                PhoneHashId = _userProvider.PhoneHashId(User)
            });
        }

        [HttpPost]
        [Authorize]
        [Route("Process/{id}")]
        public async Task Process(int id)
        {
            await _commandDispatcher.DispatchAsync(new Process.Command()
            {
                ContactId = id,
                PhoneHashId = _userProvider.PhoneHashId(User)
            });
        }
    }
}