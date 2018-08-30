namespace ItemEditor.Structure.Texture
{
    internal enum TextureFlag : ulong
    {
        TEX_ALPHACHANNEL = (1UL << 0),
        TEX_32BIT = (1UL << 1),
        TEX_COMPRESSED = (1UL << 2),
        TEX_TRANSPARENT = (1UL << 3),
        TEX_EQUALIZED = (1UL << 4),
        TEX_COMPRESSEDALPHA = (1UL << 5),
        TEX_STATIC = (1UL << 8),
        TEX_CONSTANT = (1UL << 9),
        TEX_GRAY = (1UL << 10),
        TEX_COMPRESS = (1UL << 16),
        TEX_COMPRESSALPHA = (1UL << 17),
        TEX_SINGLEMIPMAP = (1UL << 18),
        TEX_PROBED = (1UL << 19),
        TEX_DISPOSED = (1UL << 20),
        TEX_DITHERED = (1UL << 21),
        TEX_FILTERED = (1UL << 22),
        TEX_COLORIZED = (1UL << 24),
        TEX_WASOLD = (1UL << 30)
    }
}
