namespace SharpAttributeParser.RecordBuilders.SyntacticRecorderWrapperCases.RecorderCases.ConstructorRecorderCases;

using Moq;

internal sealed class RecorderContext<TRecord, TRecordBuilder> where TRecordBuilder : IRecordBuilder<TRecord>
{
    public static RecorderContext<TRecord, TRecordBuilder> Create()
    {
        ISyntacticRecorderWrapper wrapper = new SyntacticRecorderWrapper();

        Mock<ISyntacticConstructorRecorder> wrappedConstructorRecorder = new();
        Mock<ISyntacticRecorder<TRecordBuilder>> wrappedRecorder = new() { DefaultValue = DefaultValue.Mock };

        wrappedRecorder.Setup(static (recorder) => recorder.Constructor).Returns(wrappedConstructorRecorder.Object);

        var recorder = wrapper.Wrap<TRecord, TRecordBuilder>(wrappedRecorder.Object);

        return new(recorder.Constructor, recorder, wrappedConstructorRecorder);
    }

    public ISyntacticConstructorRecorder ConstructorRecorder { get; }
    public ISyntacticRecorder<TRecord> Recorder { get; }

    public Mock<ISyntacticConstructorRecorder> WrappedConstructorRecorder { get; }

    private RecorderContext(ISyntacticConstructorRecorder constructorRecorder, ISyntacticRecorder<TRecord> recorder, Mock<ISyntacticConstructorRecorder> wrappedConstructorRecorder)
    {
        ConstructorRecorder = constructorRecorder;
        Recorder = recorder;

        WrappedConstructorRecorder = wrappedConstructorRecorder;
    }
}
