﻿namespace RichillCapital.Api.Contracts;


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

    public static class DataFeeds
    {
        private const string DataFeedsBase = $"{ApiBase}/data-feeds";

        public const string List = DataFeedsBase;
    }

    public static class Brokerages
    {
        private const string BrokeragesBase = $"{ApiBase}/brokerages";

        public const string List = BrokeragesBase;
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

    public static class Orders
    {
        private const string OrdersBase = $"{ApiBase}/orders";

        public const string List = OrdersBase;
        public const string Create = OrdersBase;
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

    public static class Snapshots
    {
        private const string SnapshotsBase = $"{ApiBase}/snapshots";

        public const string List = SnapshotsBase;
        public const string Create = SnapshotsBase;
        public const string Get = $"{SnapshotsBase}/{{snapshotId}}";

        public const string Statistics = $"{SnapshotsBase}/statistics";
    }

    public static class SignalSubscriptions
    {
        private const string SignalSubscriptionsBase = $"{ApiBase}/signal-subscriptions";

        public const string List = SignalSubscriptionsBase;
        public const string Create = SignalSubscriptionsBase;
        public const string Get = $"{SignalSubscriptionsBase}/{{signalSubscriptionId}}";
    }
}