namespace SharpAttributeParser.RecordBuilders.RecorderWrapperCases.RecorderCases.NamedRecorderCases;

using Moq;

internal sealed class RecorderContext<TRecord, TRecordBuilder> where TRecordBuilder : IRecordBuilder<TRecord>
{
    public static RecorderContext<TRecord, TRecordBuilder> Create()
    {
        IRecorderWrapper wrapper = new RecorderWrapper();

        Mock<INamedRecorder> wrappedNamedRecorder = new();
        Mock<IRecorder<TRecordBuilder>> wrappedRecorder = new() { DefaultValue = DefaultValue.Mock };

        wrappedRecorder.Setup(static (recorder) => recorder.Named).Returns(wrappedNamedRecorder.Object);

        var recorder = wrapper.Wrap<TRecord, TRecordBuilder>(wrappedRecorder.Object);

        return new(recorder.Named, recorder, wrappedNamedRecorder);
    }

    public INamedRecorder NamedRecorder { get; }
    public IRecorder<TRecord> Recorder { get; }

    public Mock<INamedRecorder> WrappedNamedRecorder { get; }

    private RecorderContext(INamedRecorder namedRecorder, IRecorder<TRecord> recorder, Mock<INamedRecorder> wrappedNamedRecorder)
    {
        NamedRecorder = namedRecorder;
        Recorder = recorder;

        WrappedNamedRecorder = wrappedNamedRecorder;
    }
}
