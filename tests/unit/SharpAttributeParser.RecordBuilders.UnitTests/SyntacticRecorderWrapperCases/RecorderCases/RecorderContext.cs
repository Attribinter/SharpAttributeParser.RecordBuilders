namespace SharpAttributeParser.RecordBuilders.SyntacticRecorderWrapperCases.RecorderCases;

using Moq;

internal sealed class RecorderContext<TRecord, TRecordBuilder> where TRecordBuilder : IRecordBuilder<TRecord>
{
    public static RecorderContext<TRecord, TRecordBuilder> Create()
    {
        ISyntacticRecorderWrapper wrapper = new SyntacticRecorderWrapper();

        Mock<ISyntacticRecorder<TRecordBuilder>> wrappedRecorder = new() { DefaultValue = DefaultValue.Mock };

        var recorder = wrapper.Wrap<TRecord, TRecordBuilder>(wrappedRecorder.Object);

        return new(recorder, wrappedRecorder);
    }

    public ISyntacticRecorder<TRecord> Recorder { get; }

    public Mock<ISyntacticRecorder<TRecordBuilder>> WrappedRecorder { get; }

    private RecorderContext(ISyntacticRecorder<TRecord> recorder, Mock<ISyntacticRecorder<TRecordBuilder>> wrappedRecorder)
    {
        Recorder = recorder;

        WrappedRecorder = wrappedRecorder;
    }
}
