namespace SharpAttributeParser.RecordBuilders.SemanticRecorderWrapperCases.RecorderCases;

using Moq;

using System;

using Xunit;

public sealed class BuildRecord
{
    private static TRecord Target<TRecord>(ISemanticRecorder<TRecord> recorder) => recorder.BuildRecord();

    private readonly RecorderContext<object, IRecordBuilder<object>> Context = RecorderContext<object, IRecordBuilder<object>>.Create();

    [Fact]
    public void SingleInvocation_ReturnsBuiltRecord()
    {
        Mock<object> record = new();
        Mock<IRecordBuilder<object>> recordBuilder = new();

        recordBuilder.Setup(static (recordBuilder) => recordBuilder.Build()).Returns(record);
        Context.WrappedRecorder.Setup(static (recorder) => recorder.BuildRecord()).Returns(recordBuilder.Object);

        var actual = Target(Context.Recorder);

        Assert.Same(record, actual);

        recordBuilder.Verify(static (recordBuilder) => recordBuilder.Build(), Times.Once());
        Context.WrappedRecorder.Verify(static (recorder) => recorder.BuildRecord(), Times.Once());
    }

    [Fact]
    public void MultipleInvocations_InvalidOperationException()
    {
        Target(Context.Recorder);

        var exception = Record.Exception(() => Target(Context.Recorder));

        Assert.IsType<InvalidOperationException>(exception);
    }
}
