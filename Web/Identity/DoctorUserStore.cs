using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using Business.Abstract;
using Entities.Concrete;
using Microsoft.AspNetCore.Identity;

namespace Web.Identity
{
    public class DoctorUserStore : IUserPasswordStore<DoctorUser>, IUserRoleStore<DoctorUser>, IUserEmailStore<DoctorUser>, IUserConfirmation<DoctorUser>, IUserClaimStore<DoctorUser>, IUserSecurityStampStore<DoctorUser>
    {
        private readonly IDoctorService _doctorService;
        public DoctorUserStore(IDoctorService doctorService)
        {
            _doctorService = doctorService;
        }
        public void Dispose()
        {
            //throw new System.NotImplementedException();
        }

        public async Task<IdentityResult> CreateAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            _doctorService.Register(user.DoctorEmail, user.PasswordHashed, user.SecurityStamp);
            //await _mediator.Send(new CreateKullaniciCommand { UserName = user.UserName,
                //PasswordHashed = user.SifreHashed, SecurityStamp = user.SecurityStamp }, cancellationToken);
            return IdentityResult.Success;
        }

        public Task<IdentityResult> DeleteAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<DoctorUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //_logger.LogInformation("Kullanıcı KullaniciId araması yapılıyor. {0}", userId);

            //var response = await _mediator.Send(new FindByIdKullaniciQuery { Id = int.Parse(userId) }, cancellationToken);
            Doctor response = _doctorService.Get(Guid.Parse(userId));

            if (response == null)
                return null;

            return new DoctorUser
            {
                DoctorId = response.DoctorId,
                PasswordHashed = response.DoctorPassword,
                DoctorEmail = response.DoctorEmail,
                SecurityStamp = response.SecurityStamp
            };
        }

        public async Task<DoctorUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //_logger.LogInformation("Kullanıcı isim araması yapılıyor. {0}", normalizedUserName);

            //var response = await _mediator.Send(new FindByNameKullaniciQuery { UserName = normalizedUserName.ToLowerInvariant() },
            //cancellationToken);
            Doctor response = _doctorService.GetByEmail(normalizedUserName.ToLowerInvariant());

            if (response == null)
                return null;

            return new DoctorUser
            {
                DoctorId = response.DoctorId,
                PasswordHashed = response.DoctorPassword,
                SecurityStamp = response.SecurityStamp,
                DoctorEmail = response.DoctorEmail
            };
        }

        public Task<string> GetNormalizedUserNameAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.DoctorEmail);
        }

        public Task<string> GetUserIdAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.DoctorId.ToString());
        }

        public Task<string> GetUserNameAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.DoctorEmail);
        }

        public Task SetNormalizedUserNameAsync(DoctorUser user, string normalizedName, CancellationToken cancellationToken)
        {
            //normalized ile işimiz yok
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(DoctorUser user, string userName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //_logger.LogInformation("SetUserName: {0} {1}", user.Id, user.UserName);

            user.DoctorEmail = userName;

            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            //throw new System.NotImplementedException();
            //await _mediator.Send(new UpdateEMARKullaniciCommand
            //{
            //    Id = user.Id,
            //    SecurityStamp = user.SecurityStamp,
            //    SifreHashed = user.SifreHashed
            //});
            _doctorService.Update(new Doctor
            {
                DoctorEmail = user.DoctorEmail,
                SecurityStamp = user.SecurityStamp,
                DoctorId = user.DoctorId,
                DoctorPassword = user.PasswordHashed,
                IsApproved = true,
                DoctorName = "GlaucoT",
                DoctorLastName = "GlaucoT"
            });

            return IdentityResult.Success;
        }

        public Task<string> GetPasswordHashAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.PasswordHashed);
        }

        public Task<bool> HasPasswordAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(!string.IsNullOrWhiteSpace(user.PasswordHashed));
        }

        public Task SetPasswordHashAsync(DoctorUser user, string passwordHash, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.PasswordHashed = passwordHash;

            return Task.CompletedTask;
        }

        public async Task AddToRoleAsync(DoctorUser user, string roleName, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //_logger.LogInformation("Kullanıcı rol tanımalamsı. {0} {1}", user.Id, roleName);

            //if (roleName != "ADMIN" && roleName != "STANDART")
            //    throw new ArgumentException("Yanlış rol adı", nameof(roleName));
            //Rol rol = roleName == "ADMIN" ? Rol.ADMIN : Rol.STANDART;

            //await _mediator.Send(new AddKullaniciRolCommand { Id = user.Id, Rol = rol }, cancellationToken);
        }

        public async Task<IList<string>> GetRolesAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //var kullanici = await _mediator.Send(new FindByIdKullaniciQuery { Id = user.Id }, cancellationToken);

            return new List<string> { "Doctor" };
        }

        public Task<IList<DoctorUser>> GetUsersInRoleAsync(string roleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task<bool> IsInRoleAsync(DoctorUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public Task RemoveFromRoleAsync(DoctorUser user, string roleName, CancellationToken cancellationToken)
        {
            throw new System.NotImplementedException();
        }

        public async Task<DoctorUser> FindByEmailAsync(string normalizedEmail, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //_logger.LogInformation("Kullanıcı isim araması yapılıyor. {0}", normalizedEmail);

            //var response = await _mediator.Send(new FindByNameKullaniciQuery { UserName = normalizedEmail },
                //cancellationToken);

            Doctor response = _doctorService.GetByEmail(normalizedEmail);

            if (response == null)
                return null;

            return new DoctorUser
            {
                DoctorEmail = response.DoctorEmail,
                PasswordHashed = response.DoctorPassword,
                DoctorId = response.DoctorId,
                SecurityStamp = response.SecurityStamp
            };
        }

        public Task<string> GetEmailAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.DoctorEmail);
        }

        public async Task<bool> GetEmailConfirmedAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //var kullanici = await _mediator.Send(new FindByIdKullaniciQuery { Id = user.Id }, cancellationToken);
            Doctor response = _doctorService.Get(user.DoctorId);
            //return kullanici.Onaylandi;
            return response.IsApproved;
        }

        public Task<string> GetNormalizedEmailAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.DoctorEmail);
        }

        public Task SetEmailAsync(DoctorUser user, string email, CancellationToken cancellationToken)
        {
            //email aynı zamanda kullanıcı adı
            return Task.CompletedTask;
        }

        public async Task SetEmailConfirmedAsync(DoctorUser user, bool confirmed, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();
            //_logger.LogInformation("Kullanıcı onaylanıyor: {0}", confirmed);

            //await _mediator.Send(new ConfirmKullaniciCommand { Confirmed = confirmed, KullaniciId = user.Id }, cancellationToken);

            _doctorService.DoctorApprove(user.DoctorId, confirmed);

        }

        public Task SetNormalizedEmailAsync(DoctorUser user, string normalizedEmail, CancellationToken cancellationToken)
        {
            //email aynı zamanda kullanıcı adı
            return Task.CompletedTask;
        }

        public async Task<bool> IsConfirmedAsync(UserManager<DoctorUser> manager, DoctorUser user)
        {
            //var kullanici = await _mediator.Send(new FindByIdKullaniciQuery { Id = user.Id });
            Doctor response = _doctorService.Get(user.DoctorId);

            return response.IsApproved;
        }

        public Task AddClaimsAsync(DoctorUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Claim>> GetClaimsAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            Doctor response = _doctorService.Get(user.DoctorId);
            if (response == null)
                throw new ArgumentException("User not found", nameof(user.DoctorId));

            var list = new List<Claim>
            {
                new Claim(ClaimTypes.Email, response.DoctorEmail),
                new Claim("DoctorId", response.DoctorId.ToString())
            };
            //var kullanici = await _mediator.Send(new FindByIdKullaniciQuery { Id = user.Id }, cancellationToken);
            //if (kullanici == null)
            //    throw new ArgumentException("Kullanıcı bulunamadı", nameof(user.Id));
            //var eczane = await _mediator.Send(new FindEczaneByIdCommand { Id = kullanici.EczaneId }, cancellationToken);
            //var list = new List<Claim>
            //{
            //    new Claim(ClaimTypes.GivenName, kullanici.Ad),
            //    new Claim(ClaimTypes.Surname, kullanici.Soyad),
            //    new Claim(ClaimTypes.MobilePhone, kullanici.CepTel),
            //    new Claim("EczaneId", kullanici.EczaneId.ToString()),
            //};
            //if (eczane == null) return list;

            //list.Add(new Claim("BilgiAsama", eczane.BilgiToplamaAsama.ToString()));
            //list.Add(new Claim("BilgiToplamaBitti", eczane.BilgiToplamaBitti.ToString()));
            //list.Add(new Claim("SehirId", eczane.Il.ToString()));
            //list.Add(new Claim("Entegrator", eczane.EfaturaEntegratorWSUser ?? ""));

            return list;
        }

        public Task<IList<DoctorUser>> GetUsersForClaimAsync(Claim claim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task RemoveClaimsAsync(DoctorUser user, IEnumerable<Claim> claims, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task ReplaceClaimAsync(DoctorUser user, Claim claim, Claim newClaim, CancellationToken cancellationToken)
        {
            throw new NotImplementedException();
        }

        public Task<string> GetSecurityStampAsync(DoctorUser user, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            return Task.FromResult(user.SecurityStamp);
        }

        public Task SetSecurityStampAsync(DoctorUser user, string stamp, CancellationToken cancellationToken)
        {
            cancellationToken.ThrowIfCancellationRequested();

            user.SecurityStamp = stamp;

            return Task.CompletedTask;
            //await _mediator.Send(new SetSecurityStampKullaniciCommand { Id = user.Id, SecurityStamp = stamp }, cancellationToken);
        }
    }
}

