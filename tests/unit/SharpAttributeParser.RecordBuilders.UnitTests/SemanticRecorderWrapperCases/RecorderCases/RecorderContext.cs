namespace SharpAttributeParser.RecordBuilders.SemanticRecorderWrapperCases.RecorderCases;

using Moq;

internal sealed class RecorderContext<TRecord, TRecordBuilder> where TRecordBuilder : IRecordBuilder<TRecord>
{
    public static RecorderContext<TRecord, TRecordBuilder> Create()
    {
        ISemanticRecorderWrapper wrapper = new SemanticRecorderWrapper();

        Mock<ISemanticRecorder<TRecordBuilder>> wrappedRecorder = new() { DefaultValue = DefaultValue.Mock };

        var recorder = wrapper.Wrap<TRecord, TRecordBuilder>(wrappedRecorder.Object);

        return new(recorder, wrappedRecorder);
    }

    public ISemanticRecorder<TRecord> Recorder { get; }

    public Mock<ISemanticRecorder<TRecordBuilder>> WrappedRecorder { get; }

    private RecorderContext(ISemanticRecorder<TRecord> recorder, Mock<ISemanticRecorder<TRecordBuilder>> wrappedRecorder)
    {
        Recorder = recorder;

        WrappedRecorder = wrappedRecorder;
    }
}
