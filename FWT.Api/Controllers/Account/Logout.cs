﻿using System.Threading.Tasks;
using FluentValidation;
using FWT.Core.CQRS;
using FWT.Core.Services.Telegram;
using FWT.Infrastructure.Telegram;
using FWT.Infrastructure.Validation;
using OpenTl.ClientApi;
using OpenTl.Schema;

namespace FWT.Api.Controllers.Account
{
    public class Logout
    {
        public class Handler : IQueryHandler<Query, bool>
        {
            private readonly ITelegramService _telegramService;

            public Handler(ITelegramService telegramService)
            {
                _telegramService = telegramService;
            }

            public async Task<bool> HandleAsync(Query query)
            {
                IClientApi client = await _telegramService.BuildAsync(query.PhoneHashId);

                await TelegramRequest.HandleAsync(() =>
                {
                    return client.AuthService.LogoutAsync();
                });

                return true;
            }
        }

        public class Query : IQuery
        {
            public string PhoneHashId { get; set; }
        }

        public class Result
        {
            public Result(TUser user)
            {
                FirstName = user.FirstName;
                LastName = user.LastName;
                UserName = user.Username;
                PhotoId = user.Photo.As<TUserProfilePhoto>()?.PhotoId;
            }

            public string FirstName { get; }

            public string LastName { get; }

            public long? PhotoId { get; }

            public string UserName { get; }
        }

        public class Validator : AppAbstractValidation<Query>
        {
            public Validator()
            {
                RuleFor(x => x.PhoneHashId).NotEmpty();
            }
        }
    }
}
