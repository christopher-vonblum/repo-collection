namespace CoreUi.Serialization
{
    public interface ISerializationPackager
    {
        byte[] Pack<TWrapper>(TWrapper wrapper);
        TWrapper UnPack<TWrapper>(byte[] data);
    }
}