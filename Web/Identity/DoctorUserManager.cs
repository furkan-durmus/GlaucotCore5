using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Web.Identity
{
    public class DoctorUserManager : UserManager<DoctorUser>
    {
        public DoctorUserManager(IUserStore<DoctorUser> store, IOptions<IdentityOptions> optionsAccessor, IPasswordHasher<DoctorUser> passwordHasher, IEnumerable<IUserValidator<DoctorUser>> userValidators, IEnumerable<IPasswordValidator<DoctorUser>> passwordValidators, ILookupNormalizer keyNormalizer, IdentityErrorDescriber errors, IServiceProvider services, ILogger<UserManager<DoctorUser>> logger) : base(store, optionsAccessor, passwordHasher, userValidators, passwordValidators, keyNormalizer, errors, services, logger)
        {
        }
    }
}

