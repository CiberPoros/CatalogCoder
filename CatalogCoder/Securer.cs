namespace CatalogCoder
{
    public static class Securer
    {
        public static byte[] VisinerTransform(byte[] data, byte[] key, WorkMode workMode)
        {
            if (workMode == WorkMode.NONE)
                return data;

            for (int i = 0; i < data.Length; i++)
                data[i] += (byte)(workMode == WorkMode.ENCRYPT ? key[i % key.Length] : -key[i % key.Length]);

            return data;
        }
    }
}
