namespace RichillCapital.Api.Contracts;


public static class ApiRoutes
{
    private const string ApiBase = "/api/v{version:apiVersion}";

    public static class General
    {
    }

    // Identity
    public static class Users
    {
        private const string UsersBase = $"{ApiBase}/users";

        public const string List = UsersBase;
        public const string Create = UsersBase;
        public const string Get = $"{UsersBase}/{{userId}}";
        public const string Delete = $"{UsersBase}/{{userId}}";
    }

    // Market data
    public static class Instruments
    {
        private const string InstrumentsBase = $"{ApiBase}/instruments";

        public const string List = InstrumentsBase;
        public const string Create = InstrumentsBase;
        public const string Get = $"{InstrumentsBase}/{{symbol}}";
        public const string Delete = $"{InstrumentsBase}/{{symbol}}";
    }

    // Trading
    public static class Accounts
    {
        private const string AccountBase = $"{ApiBase}/accounts";

        public const string List = AccountBase;
        public const string Create = AccountBase;
        public const string Get = $"{AccountBase}/{{accountId}}";
        public const string Delete = $"{AccountBase}/{{accountId}}";
    }

    // Automated trading
    public static class SignalSources
    {
        private const string SignalSourcesBase = $"{ApiBase}/signal-sources";

        public const string List = SignalSourcesBase;
        public const string Create = SignalSourcesBase;
        public const string Get = $"{SignalSourcesBase}/{{signalSourceId}}";
        public const string Delete = $"{SignalSourcesBase}/{{signalSourceId}}";
    }
}