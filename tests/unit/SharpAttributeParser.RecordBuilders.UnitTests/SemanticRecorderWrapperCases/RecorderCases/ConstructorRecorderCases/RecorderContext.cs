namespace SharpAttributeParser.RecordBuilders.SemanticRecorderWrapperCases.RecorderCases.ConstructorRecorderCases;

using Moq;

internal sealed class RecorderContext<TRecord, TRecordBuilder> where TRecordBuilder : IRecordBuilder<TRecord>
{
    public static RecorderContext<TRecord, TRecordBuilder> Create()
    {
        ISemanticRecorderWrapper wrapper = new SemanticRecorderWrapper();

        Mock<ISemanticConstructorRecorder> wrappedConstructorRecorder = new();
        Mock<ISemanticRecorder<TRecordBuilder>> wrappedRecorder = new() { DefaultValue = DefaultValue.Mock };

        wrappedRecorder.Setup(static (recorder) => recorder.Constructor).Returns(wrappedConstructorRecorder.Object);

        var recorder = wrapper.Wrap<TRecord, TRecordBuilder>(wrappedRecorder.Object);

        return new(recorder.Constructor, recorder, wrappedConstructorRecorder);
    }

    public ISemanticConstructorRecorder ConstructorRecorder { get; }
    public ISemanticRecorder<TRecord> Recorder { get; }

    public Mock<ISemanticConstructorRecorder> WrappedConstructorRecorder { get; }

    private RecorderContext(ISemanticConstructorRecorder constructorRecorder, ISemanticRecorder<TRecord> recorder, Mock<ISemanticConstructorRecorder> wrappedConstructorRecorder)
    {
        ConstructorRecorder = constructorRecorder;
        Recorder = recorder;

        WrappedConstructorRecorder = wrappedConstructorRecorder;
    }
}
