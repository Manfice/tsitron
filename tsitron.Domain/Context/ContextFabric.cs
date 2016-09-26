namespace tsitron.Domain.Context
{
    public static class ContextFabric
    {
        private static TsitronContext _context;

        public static TsitronContext Context
        {
            get { return _context ?? (_context = new TsitronContext()); }
            set { _context = value; }
        }

        public static void ContextDispose()
        {
            Context.Dispose();
        }
    }
}