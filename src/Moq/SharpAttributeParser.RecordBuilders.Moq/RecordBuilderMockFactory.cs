namespace SharpAttributeParser.Moq;

using global::Moq;
using global::Moq.Language.Flow;

using SharpAttributeParser.Moq.RecordBuilderSetups;

using System;

public sealed class RecordBuilderMockFactory : IRecordBuilderMockFactory
{
    IRecordBuilderMock<TRecord> IRecordBuilderMockFactory.Create<TRecord>()
    {
        Mock<IRecordBuilder<TRecord>> mock = new() { DefaultValue = DefaultValue.Mock };

        return new RecordBuilderMock<TRecord>(mock);
    }

    private sealed class RecordBuilderMock<TRecord> : IRecordBuilderMock<TRecord>
    {
        private readonly Mock<IRecordBuilder<TRecord>> Mock;
        private readonly IRecordBuilderSetup<TRecord> Setup;
        private readonly IRecordBuilderVerification Verify;

        public RecordBuilderMock(Mock<IRecordBuilder<TRecord>> mock)
        {
            Mock = mock;

            Setup = new RecordBuilderSetup<TRecord>(mock);
            Verify = new RecordBuilderVerification<TRecord>(mock);
        }

        Mock<IRecordBuilder<TRecord>> IRecordBuilderMock<TRecord>.Mock => Mock;
        IRecordBuilder<TRecord> IRecordBuilderMock<TRecord>.Object => Mock.Object;
        IRecordBuilderSetup<TRecord> IRecordBuilderMock<TRecord>.Setup => Setup;
        IRecordBuilderVerification IRecordBuilderMock<TRecord>.Verify => Verify;
    }

    private sealed class RecordBuilderSetup<TRecord> : IRecordBuilderSetup<TRecord>
    {
        private readonly Mock<IRecordBuilder<TRecord>> Mock;

        public RecordBuilderSetup(Mock<IRecordBuilder<TRecord>> mock)
        {
            Mock = mock;
        }

        IRecordBuilderSetupBuild<TRecord> IRecordBuilderSetup<TRecord>.Build => new RecordBuilderSetupBuild(Mock.Setup(static (builder) => builder.Build()));

        private sealed class RecordBuilderSetupBuild : IRecordBuilderSetupBuild<TRecord>
        {
            private readonly ISetup<IRecordBuilder<TRecord>, TRecord> Setup;

            public RecordBuilderSetupBuild(ISetup<IRecordBuilder<TRecord>, TRecord> setup)
            {
                Setup = setup;
            }

            IReturnsResult<IRecordBuilder<TRecord>> IRecordBuilderSetupBuild<TRecord>.Returns(TRecord result) => Setup.Returns(result);

            IThrowsResult IRecordBuilderSetupBuild<TRecord>.Throws<TException>() => Setup.Throws<TException>();
            IThrowsResult IRecordBuilderSetupBuild<TRecord>.Throws(Exception exception) => Setup.Throws(exception);
        }
    }

    private sealed class RecordBuilderVerification<TRecord> : IRecordBuilderVerification
    {
        private readonly Mock<IRecordBuilder<TRecord>> Mock;

        public RecordBuilderVerification(Mock<IRecordBuilder<TRecord>> mock)
        {
            Mock = mock;
        }

        void IRecordBuilderVerification.Build(Times times) => Mock.Verify(static (builder) => builder.Build(), times);

        void IRecordBuilderVerification.NoOtherCalls() => Mock.VerifyNoOtherCalls();
    }
}
