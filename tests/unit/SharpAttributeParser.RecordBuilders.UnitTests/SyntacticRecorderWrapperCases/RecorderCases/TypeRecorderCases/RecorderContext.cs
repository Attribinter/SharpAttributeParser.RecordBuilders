namespace SharpAttributeParser.RecordBuilders.SyntacticRecorderWrapperCases.RecorderCases.TypeRecorderCases;

using Moq;

internal sealed class RecorderContext<TRecord, TRecordBuilder> where TRecordBuilder : IRecordBuilder<TRecord>
{
    public static RecorderContext<TRecord, TRecordBuilder> Create()
    {
        ISyntacticRecorderWrapper wrapper = new SyntacticRecorderWrapper();

        Mock<ISyntacticTypeRecorder> wrappedTypeRecorder = new();
        Mock<ISyntacticRecorder<TRecordBuilder>> wrappedRecorder = new() { DefaultValue = DefaultValue.Mock };

        wrappedRecorder.Setup(static (recorder) => recorder.Type).Returns(wrappedTypeRecorder.Object);

        var recorder = wrapper.Wrap<TRecord, TRecordBuilder>(wrappedRecorder.Object);

        return new(recorder.Type, recorder, wrappedTypeRecorder);
    }

    public ISyntacticTypeRecorder TypeRecorder { get; }
    public ISyntacticRecorder<TRecord> Recorder { get; }

    public Mock<ISyntacticTypeRecorder> WrappedTypeRecorder { get; }

    private RecorderContext(ISyntacticTypeRecorder typeRecorder, ISyntacticRecorder<TRecord> recorder, Mock<ISyntacticTypeRecorder> wrappedTypeRecorder)
    {
        TypeRecorder = typeRecorder;
        Recorder = recorder;

        WrappedTypeRecorder = wrappedTypeRecorder;
    }
}
