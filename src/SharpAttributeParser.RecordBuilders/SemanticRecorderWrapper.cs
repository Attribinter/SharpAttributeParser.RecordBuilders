namespace SharpAttributeParser.RecordBuilders;

using Microsoft.CodeAnalysis;

using System;

/// <inheritdoc cref="ISemanticRecorderWrapper"/>
public sealed class SemanticRecorderWrapper : ISemanticRecorderWrapper
{
    /// <summary>Instantiates a <see cref="SemanticRecorderWrapper"/>, handles wrapping of record-builder targetting recorders in non-record-builder targetting recorders.</summary>
    public SemanticRecorderWrapper() { }

    ISemanticRecorder<TRecord> ISemanticRecorderWrapper.Wrap<TRecord, TRecordBuilder>(ISemanticRecorder<TRecordBuilder> recorder)
    {
        if (recorder is null)
        {
            throw new ArgumentNullException(nameof(recorder));
        }

        return new Recorder<TRecord, TRecordBuilder>(recorder);
    }

    private sealed class Recorder<TRecord, TRecordBuilder> : ISemanticRecorder<TRecord> where TRecordBuilder : IRecordBuilder<TRecord>
    {
        private readonly ISemanticRecorder<TRecordBuilder> WrappedRecorder;

        private readonly TypeRecorder Type;
        private readonly ConstructorRecorder Constructor;
        private readonly NamedRecorder Named;

        private bool HasBeenBuilt;

        public Recorder(ISemanticRecorder<TRecordBuilder> wrappedRecorder)
        {
            WrappedRecorder = wrappedRecorder;

            Type = new(wrappedRecorder.Type);
            Constructor = new(wrappedRecorder.Constructor);
            Named = new(wrappedRecorder.Named);
        }

        ISemanticTypeRecorder ISemanticRecorder.Type => Type;
        ISemanticConstructorRecorder ISemanticRecorder.Constructor => Constructor;
        ISemanticNamedRecorder ISemanticRecorder.Named => Named;

        TRecord ISemanticRecorder<TRecord>.BuildRecord()
        {
            if (HasBeenBuilt)
            {
                throw new InvalidOperationException($"Cannot build a record of the recorded arguments, as one has already been built.");
            }

            HasBeenBuilt = true;

            Type.SetHasBeenBuilt();
            Constructor.SetHasBeenBuilt();
            Named.SetHasBeenBuilt();

            return WrappedRecorder.BuildRecord().Build();
        }

        private static void ThrowModifyAfterBuild() => throw new InvalidOperationException($"Cannot record the argument, as the record of recorded arguments has already been built");

        private sealed class TypeRecorder : ISemanticTypeRecorder
        {
            private readonly ISemanticTypeRecorder WrappedRecorder;

            private bool HasBeenBuilt;

            public TypeRecorder(ISemanticTypeRecorder wrappedRecorder)
            {
                WrappedRecorder = wrappedRecorder;
            }

            public void SetHasBeenBuilt()
            {
                HasBeenBuilt = true;
            }

            bool ISemanticTypeRecorder.TryRecordArgument(ITypeParameterSymbol parameter, ITypeSymbol argument)
            {
                if (HasBeenBuilt)
                {
                    ThrowModifyAfterBuild();
                }

                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                if (argument is null)
                {
                    throw new ArgumentNullException(nameof(argument));
                }

                return WrappedRecorder.TryRecordArgument(parameter, argument);
            }
        }

        private sealed class ConstructorRecorder : ISemanticConstructorRecorder
        {
            private readonly ISemanticConstructorRecorder WrappedRecorder;

            private bool HasBeenBuilt;

            public ConstructorRecorder(ISemanticConstructorRecorder wrappedRecorder)
            {
                WrappedRecorder = wrappedRecorder;
            }

            public void SetHasBeenBuilt()
            {
                HasBeenBuilt = true;
            }

            bool ISemanticConstructorRecorder.TryRecordArgument(IParameterSymbol parameter, object? argument)
            {
                if (HasBeenBuilt)
                {
                    ThrowModifyAfterBuild();
                }

                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                return WrappedRecorder.TryRecordArgument(parameter, argument);
            }
        }

        private sealed class NamedRecorder : ISemanticNamedRecorder
        {
            private readonly ISemanticNamedRecorder WrappedRecorder;

            private bool HasBeenBuilt;

            public NamedRecorder(ISemanticNamedRecorder wrappedRecorder)
            {
                WrappedRecorder = wrappedRecorder;
            }

            public void SetHasBeenBuilt()
            {
                HasBeenBuilt = true;
            }

            bool ISemanticNamedRecorder.TryRecordArgument(string parameterName, object? argument)
            {
                if (HasBeenBuilt)
                {
                    ThrowModifyAfterBuild();
                }

                if (parameterName is null)
                {
                    throw new ArgumentNullException(nameof(parameterName));
                }

                return WrappedRecorder.TryRecordArgument(parameterName, argument);
            }
        }
    }
}
