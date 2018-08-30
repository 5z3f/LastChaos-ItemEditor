namespace ItemEditor.Structure.Texture
{
    internal class cTextureHeader
    {
        public string Format;
        public byte[][] ImageData;
        public byte[] VersionChunk, DataChunk;
        public int AnimOffset, Version;

        public uint Width,
            FirstMipLevel,
            Height,
            MipMap,
            Bits,
            Frames;
    }
}
