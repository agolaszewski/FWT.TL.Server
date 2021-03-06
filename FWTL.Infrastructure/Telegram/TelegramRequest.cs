﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using FluentValidation;
using FluentValidation.Results;
using FluentValidation.Validators;

namespace FWTL.Infrastructure.Telegram
{
    public static class TelegramRequest
    {
        public static async Task<TResult> HandleAsync<TResult>(Func<Task<TResult>> func)
        {
            try
            {
                return await func().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ThrowValidationException(ex);
            }

            throw new NotImplementedException("Unsupported path");
        }

        public static async Task<TResult> HandleAsync<TResult>(Func<Task<TResult>> func, CustomContext context)
        {
            try
            {
                return await func().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                context.AddFailure(ex.Message);
                return default(TResult);
            }

            throw new NotImplementedException("Unsupported path");
        }

        public static async Task HandleAsync(Func<Task> func)
        {
            try
            {
                await func().ConfigureAwait(false);
            }
            catch (Exception ex)
            {
                ThrowValidationException(ex);
            }
        }

        private static void ThrowValidationException(Exception ex)
        {
            throw new ValidationException(new List<ValidationFailure>()
            {
                new ValidationFailure(ex.GetType().FullName, ex.Message)
            });
        }
    }
}