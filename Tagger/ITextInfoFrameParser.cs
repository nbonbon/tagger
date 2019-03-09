namespace Tagger
{
    public interface ITextInfoFrameParser
    {
        void Initialize(byte[] fileData, int byteOffset, uint frameSize, string frameId);
        int Parse(out TextInfoFrame frame);
    }
}
