﻿namespace Cosmetify.Model
{
    class UserModel
    {
        private static UserModel _instance;

        public static UserModel Instance => _instance ?? (_instance = new UserModel());

        public string Email { get; set; }

        public string Password { get; set; }

    }
}
