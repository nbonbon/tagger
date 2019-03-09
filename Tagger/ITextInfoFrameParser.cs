namespace Tagger
{
    public interface ITextInfoFrameParser
    {
        int Parse(byte[] fileData, int byteOffset, ref FrameData frameData);
    }
}
