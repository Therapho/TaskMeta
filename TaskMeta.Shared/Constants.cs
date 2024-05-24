namespace TaskMeta.Shared
{
    public static class Constants
    {
        public struct Category
        {
            public const int Deposit = 1;
            public const int Withdrawal = 2;
            public const int Transfer = 3;
        }

        public struct Status
        {
            public const int Draft = 1;
            public const int Accepted = 2;
        }
    }
}
