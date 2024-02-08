namespace SharpAttributeParser.RecordBuilders.RecorderWrapperCases.RecorderCases.TypeRecorderCases;

using Moq;

internal sealed class RecorderContext<TRecord, TRecordBuilder> where TRecordBuilder : IRecordBuilder<TRecord>
{
    public static RecorderContext<TRecord, TRecordBuilder> Create()
    {
        IRecorderWrapper wrapper = new RecorderWrapper();

        Mock<ITypeRecorder> wrappedTypeRecorder = new();
        Mock<IRecorder<TRecordBuilder>> wrappedRecorder = new() { DefaultValue = DefaultValue.Mock };

        wrappedRecorder.Setup(static (recorder) => recorder.Type).Returns(wrappedTypeRecorder.Object);

        var recorder = wrapper.Wrap<TRecord, TRecordBuilder>(wrappedRecorder.Object);

        return new(recorder.Type, recorder, wrappedTypeRecorder);
    }

    public ITypeRecorder TypeRecorder { get; }
    public IRecorder<TRecord> Recorder { get; }

    public Mock<ITypeRecorder> WrappedTypeRecorder { get; }

    private RecorderContext(ITypeRecorder typeRecorder, IRecorder<TRecord> recorder, Mock<ITypeRecorder> wrappedTypeRecorder)
    {
        TypeRecorder = typeRecorder;
        Recorder = recorder;

        WrappedTypeRecorder = wrappedTypeRecorder;
    }
}
