﻿namespace SharpAttributeParser.RecordBuilders.SemanticRecorderWrapperCases.RecorderCases.TypeRecorderCases;

using Moq;

internal sealed class RecorderContext<TRecord, TRecordBuilder> where TRecordBuilder : IRecordBuilder<TRecord>
{
    public static RecorderContext<TRecord, TRecordBuilder> Create()
    {
        ISemanticRecorderWrapper wrapper = new SemanticRecorderWrapper();

        Mock<ISemanticTypeRecorder> wrappedTypeRecorder = new();
        Mock<ISemanticRecorder<TRecordBuilder>> wrappedRecorder = new() { DefaultValue = DefaultValue.Mock };

        wrappedRecorder.Setup(static (recorder) => recorder.Type).Returns(wrappedTypeRecorder.Object);

        var recorder = wrapper.Wrap<TRecord, TRecordBuilder>(wrappedRecorder.Object);

        return new(recorder.Type, recorder, wrappedTypeRecorder);
    }

    public ISemanticTypeRecorder TypeRecorder { get; }
    public ISemanticRecorder<TRecord> Recorder { get; }

    public Mock<ISemanticTypeRecorder> WrappedTypeRecorder { get; }

    private RecorderContext(ISemanticTypeRecorder typeRecorder, ISemanticRecorder<TRecord> recorder, Mock<ISemanticTypeRecorder> wrappedTypeRecorder)
    {
        TypeRecorder = typeRecorder;
        Recorder = recorder;

        WrappedTypeRecorder = wrappedTypeRecorder;
    }
}