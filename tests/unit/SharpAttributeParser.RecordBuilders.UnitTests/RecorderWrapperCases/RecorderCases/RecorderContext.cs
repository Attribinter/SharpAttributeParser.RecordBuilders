namespace SharpAttributeParser.RecordBuilders.RecorderWrapperCases.RecorderCases;

using Moq;

internal sealed class RecorderContext<TRecord, TRecordBuilder> where TRecordBuilder : IRecordBuilder<TRecord>
{
    public static RecorderContext<TRecord, TRecordBuilder> Create()
    {
        IRecorderWrapper wrapper = new RecorderWrapper();

        Mock<IRecorder<TRecordBuilder>> wrappedRecorder = new() { DefaultValue = DefaultValue.Mock };

        var recorder = wrapper.Wrap<TRecord, TRecordBuilder>(wrappedRecorder.Object);

        return new(recorder, wrappedRecorder);
    }

    public IRecorder<TRecord> Recorder { get; }

    public Mock<IRecorder<TRecordBuilder>> WrappedRecorder { get; }

    private RecorderContext(IRecorder<TRecord> recorder, Mock<IRecorder<TRecordBuilder>> wrappedRecorder)
    {
        Recorder = recorder;

        WrappedRecorder = wrappedRecorder;
    }
}
