﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RazorPageIdentity.Extensions;

namespace RazorPageIdentity.Controllers
{
    public class IdentityController : Controller
    {
        private readonly CustomClaimsCookieSignInHelper<IdentityUser> _signInHelper;
        private readonly UserManager<IdentityUser> _userManager;
        public IdentityController(CustomClaimsCookieSignInHelper<IdentityUser> signInHelper
            , UserManager<IdentityUser> userManager)
        {
            _signInHelper = signInHelper;
            _userManager = userManager;
        }
        public ViewResult Index() => View(User?.Claims);


        [HttpGet]
        [ActionName("Create")]
        public IActionResult Create_Post()
        {
            return View();
        }

        [HttpPost]
        [ActionName("Create")]
        public async Task<IActionResult> Create_Post([FromForm]string claimType, [FromForm]string claimValue, [FromForm]string claimIssuer)
        {
            claimType = "T1";
            claimValue = "T2";
            claimIssuer = "T3";
            //var user = await _userManager.FindByEmailAsync(User.Identity.Name);
            //Claim claim = new Claim(claimType, claimValue, ClaimValueTypes.String, claimIssuer);
            //await _signInHelper.SignInUserAsync(user, new List<Claim>() { claim } );

            ClaimsIdentity identity = User.Identity as ClaimsIdentity;
            Claim claim = new Claim(claimType, claimValue, ClaimValueTypes.String, claimIssuer);
            identity.AddClaim(claim);
            await _signInHelper.SignInUserAsync(identity);

            return RedirectToAction("Index");
        }

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> CreateUser()
        {
            var newAppUser = new IdentityUser
            {
                UserName = @"!#$%^*()~`-=_+[]{}\|;:', ./<>?©®",
                NormalizedUserName = @"!#$%^*()~`-=_+[]{}\|;:', ./<>?©®",
                Email = "nothing@nothing.com",
                NormalizedEmail = "nothing@nothing.com".ToUpper(),
                LockoutEnabled = true,
                LockoutEnd = DateTime.Now
            };

            string password = "1qaz@WSX";
            var result = await _userManager.CreateAsync(newAppUser, password);
            return Ok();
        }
        [Authorize]
        public IActionResult Authorize()
        {
            return Ok(User.Identity.Name);
        }
        [AllowAnonymous]
        public IActionResult Anonymous()
        {
            return Ok(User.Identity.Name);
        }
    }
}