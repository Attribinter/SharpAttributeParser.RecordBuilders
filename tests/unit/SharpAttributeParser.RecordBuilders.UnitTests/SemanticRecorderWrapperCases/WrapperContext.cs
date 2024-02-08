namespace SharpAttributeParser.RecordBuilders.SemanticRecorderWrapperCases;

internal sealed class WrapperContext
{
    public static WrapperContext Create()
    {
        SemanticRecorderWrapper wrapper = new();

        return new(wrapper);
    }

    public ISemanticRecorderWrapper Wrapper { get; }

    private WrapperContext(ISemanticRecorderWrapper wrapper)
    {
        Wrapper = wrapper;
    }
}
