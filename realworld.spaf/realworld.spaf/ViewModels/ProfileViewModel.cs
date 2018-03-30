﻿using Bridge.Spaf;
using realworld.spaf.Models;
using realworld.spaf.Models.Response;
using realworld.spaf.Services;
using System.Collections.Generic;
using static Retyped.knockout;

namespace realworld.spaf.ViewModels
{
    class ProfileViewModel : LoadableViewModel
    {
        protected override string ElementId() => SpafApp.ProfileId;

        private readonly IProfileResources _profileResource;

        public ProfileModel ProfileModel { get; set; }

        public ProfileViewModel(IProfileResources profileResource)
        {
            this.ProfileModel = new ProfileModel();
            this._profileResource = profileResource;

            this.PopulateProfile();
        }

        public async void PopulateProfile()
        {
            var loggedUser = SpafApp.Container.Resolve<IUserService>().LoggedUser;
            var profileResponse = await this._profileResource.Get(loggedUser.Username);
            this.ProfileModel.MapMe(profileResponse);
        }

    }

    public class ProfileModel
    {
        public KnockoutObservable<string> Image { get; set; }
        public KnockoutObservable<string> Username { get; set; }
        public KnockoutObservable<string> Bio { get; set; }
        public KnockoutObservable<bool> Following { get; set; }

        public ProfileModel()
        {
            this.Image = ko.observable.Self<string>();
            this.Username = ko.observable.Self<string>();
            this.Bio = ko.observable.Self<string>();
            this.Following = ko.observable.Self<bool>();
        }

        public void MapMe (ProfileResponse profileResponse)
        {
            this.Image.Self(profileResponse.Profile.Image);
            this.Username.Self(profileResponse.Profile.Username);
            this.Bio.Self(profileResponse.Profile.Bio);
            this.Following.Self(profileResponse.Profile.Following);
        }
    }
}
