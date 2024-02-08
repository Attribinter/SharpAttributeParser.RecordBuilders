namespace SharpAttributeParser.RecordBuilders.SyntacticRecorderWrapperCases.RecorderCases.NamedRecorderCases;

using Moq;

internal sealed class RecorderContext<TRecord, TRecordBuilder> where TRecordBuilder : IRecordBuilder<TRecord>
{
    public static RecorderContext<TRecord, TRecordBuilder> Create()
    {
        ISyntacticRecorderWrapper wrapper = new SyntacticRecorderWrapper();

        Mock<ISyntacticNamedRecorder> wrappedNamedRecorder = new();
        Mock<ISyntacticRecorder<TRecordBuilder>> wrappedRecorder = new() { DefaultValue = DefaultValue.Mock };

        wrappedRecorder.Setup(static (recorder) => recorder.Named).Returns(wrappedNamedRecorder.Object);

        var recorder = wrapper.Wrap<TRecord, TRecordBuilder>(wrappedRecorder.Object);

        return new(recorder.Named, recorder, wrappedNamedRecorder);
    }

    public ISyntacticNamedRecorder NamedRecorder { get; }
    public ISyntacticRecorder<TRecord> Recorder { get; }

    public Mock<ISyntacticNamedRecorder> WrappedNamedRecorder { get; }

    private RecorderContext(ISyntacticNamedRecorder namedRecorder, ISyntacticRecorder<TRecord> recorder, Mock<ISyntacticNamedRecorder> wrappedNamedRecorder)
    {
        NamedRecorder = namedRecorder;
        Recorder = recorder;

        WrappedNamedRecorder = wrappedNamedRecorder;
    }
}
