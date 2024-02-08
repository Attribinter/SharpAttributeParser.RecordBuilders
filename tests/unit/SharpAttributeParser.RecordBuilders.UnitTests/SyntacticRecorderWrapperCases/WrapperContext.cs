namespace SharpAttributeParser.RecordBuilders.SyntacticRecorderWrapperCases;

internal sealed class WrapperContext
{
    public static WrapperContext Create()
    {
        SyntacticRecorderWrapper wrapper = new();

        return new(wrapper);
    }

    public ISyntacticRecorderWrapper Wrapper { get; }

    private WrapperContext(ISyntacticRecorderWrapper wrapper)
    {
        Wrapper = wrapper;
    }
}
