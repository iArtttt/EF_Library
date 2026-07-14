namespace Library.Shared.Enums
{
    [Flags]
    public enum Genre
    {
        None = 0,
        Roman = 1,
        Horror = 2,
        Triller = 4,
        Detective = 8,
        History = 16,
        Novel = 32,
        Poem = 64,
        Comedy = 128,
        Dramma = 256,
        FairyTail = 512,
        Learning = 1024,
    }
}
