// Developed by Softeq Development Corporation
// http://www.softeq.com

using System;
using System.Linq;
using AutoMapper;
using EnsureThat;
using FluentValidation;
using Microsoft.Extensions.Logging;
using Softeq.NetKit.Profile.Domain.Infrastructure;
using Softeq.NetKit.ProfileService.Utility.ErrorHandling;
using ValidationException = Softeq.NetKit.ProfileService.Exceptions.ValidationException;

namespace Softeq.NetKit.ProfileService.Services
{
    public class BaseService
    {
        protected readonly ILogger Log;

        protected readonly IUnitOfWork UnitOfWork;

        protected readonly IMapper Mapper;

        protected readonly IServiceProvider ServiceProvider;

        protected BaseService(IUnitOfWork unitOfWork, ILoggerFactory logger, IMapper mapper, IServiceProvider serviceProvider)
        {
            Ensure.That(unitOfWork, nameof(unitOfWork)).IsNotNull();

            Ensure.That(logger, nameof(logger)).IsNotNull();
            Ensure.That(serviceProvider, nameof(serviceProvider)).IsNotNull();

            Log = logger.CreateLogger(GetType());
            UnitOfWork = unitOfWork;
            ServiceProvider = serviceProvider;
            Mapper = mapper;
        }

        protected void ValidateAndThrow<T>(T model)
        {
            var validator = (IValidator<T>)ServiceProvider.GetService(typeof(IValidator<T>));
            if (validator == null)
            {
                throw new InvalidOperationException($"Could not resolve validator for specified model. Model type: {typeof(T)}");
            }

            var result = validator.Validate(model);
            if (!result.IsValid)
            {
                throw new ValidationException(result.Errors.Select(x => new ValidationErrorDto(ErrorCode.ValidationError, x.ErrorMessage, x.PropertyName, x.ErrorCode)));
            }
        }
    }
}