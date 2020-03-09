namespace irods_Csharp
{
    public class Options
    {
        internal enum iRODSProt_t
        {
            NATIVE_PROT,
            XML_PROT
        }
        public enum FileMode
        {
            Read,
            Write,
            ReadWrite,
        }

        public enum SeekMode
        {
            Start,
            Offset,
            End,
        }
    }
}
