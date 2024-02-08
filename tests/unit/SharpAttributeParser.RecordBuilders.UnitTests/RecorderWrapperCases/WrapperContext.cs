namespace SharpAttributeParser.RecordBuilders.RecorderWrapperCases;

internal sealed class WrapperContext
{
    public static WrapperContext Create()
    {
        RecorderWrapper wrapper = new();

        return new(wrapper);
    }

    public IRecorderWrapper Wrapper { get; }

    private WrapperContext(IRecorderWrapper wrapper)
    {
        Wrapper = wrapper;
    }
}
