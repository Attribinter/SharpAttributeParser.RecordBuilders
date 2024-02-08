namespace SharpAttributeParser.RecordBuilders.SemanticRecorderWrapperCases.RecorderCases.NamedRecorderCases;

using Moq;

internal sealed class RecorderContext<TRecord, TRecordBuilder> where TRecordBuilder : IRecordBuilder<TRecord>
{
    public static RecorderContext<TRecord, TRecordBuilder> Create()
    {
        ISemanticRecorderWrapper wrapper = new SemanticRecorderWrapper();

        Mock<ISemanticNamedRecorder> wrappedNamedRecorder = new();
        Mock<ISemanticRecorder<TRecordBuilder>> wrappedRecorder = new() { DefaultValue = DefaultValue.Mock };

        wrappedRecorder.Setup(static (recorder) => recorder.Named).Returns(wrappedNamedRecorder.Object);

        var recorder = wrapper.Wrap<TRecord, TRecordBuilder>(wrappedRecorder.Object);

        return new(recorder.Named, recorder, wrappedNamedRecorder);
    }

    public ISemanticNamedRecorder NamedRecorder { get; }
    public ISemanticRecorder<TRecord> Recorder { get; }

    public Mock<ISemanticNamedRecorder> WrappedNamedRecorder { get; }

    private RecorderContext(ISemanticNamedRecorder namedRecorder, ISemanticRecorder<TRecord> recorder, Mock<ISemanticNamedRecorder> wrappedNamedRecorder)
    {
        NamedRecorder = namedRecorder;
        Recorder = recorder;

        WrappedNamedRecorder = wrappedNamedRecorder;
    }
}
