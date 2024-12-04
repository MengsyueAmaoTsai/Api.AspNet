namespace RichillCapital.Api.Contracts;


public static class ApiRoutes
{
    private const string ApiBase = "/api/v{version:apiVersion}";

    public static class General
    {
    }

    public static class Users
    {
        private const string UsersBase = $"{ApiBase}/users";

        public const string List = UsersBase;
        public const string Create = UsersBase;
        public const string Get = $"{UsersBase}/{{userId}}";
        public const string Delete = $"{UsersBase}/{{userId}}";
    }
}