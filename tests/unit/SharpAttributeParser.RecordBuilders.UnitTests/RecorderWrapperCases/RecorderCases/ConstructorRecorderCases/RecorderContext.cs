namespace SharpAttributeParser.RecordBuilders.RecorderWrapperCases.RecorderCases.ConstructorRecorderCases;

using Moq;

internal sealed class RecorderContext<TRecord, TRecordBuilder> where TRecordBuilder : IRecordBuilder<TRecord>
{
    public static RecorderContext<TRecord, TRecordBuilder> Create()
    {
        IRecorderWrapper wrapper = new RecorderWrapper();

        Mock<IConstructorRecorder> wrappedConstructorRecorder = new();
        Mock<IRecorder<TRecordBuilder>> wrappedRecorder = new() { DefaultValue = DefaultValue.Mock };

        wrappedRecorder.Setup(static (recorder) => recorder.Constructor).Returns(wrappedConstructorRecorder.Object);

        var recorder = wrapper.Wrap<TRecord, TRecordBuilder>(wrappedRecorder.Object);

        return new(recorder.Constructor, recorder, wrappedConstructorRecorder);
    }

    public IConstructorRecorder ConstructorRecorder { get; }
    public IRecorder<TRecord> Recorder { get; }

    public Mock<IConstructorRecorder> WrappedConstructorRecorder { get; }

    private RecorderContext(IConstructorRecorder constructorRecorder, IRecorder<TRecord> recorder, Mock<IConstructorRecorder> wrappedConstructorRecorder)
    {
        ConstructorRecorder = constructorRecorder;
        Recorder = recorder;

        WrappedConstructorRecorder = wrappedConstructorRecorder;
    }
}
