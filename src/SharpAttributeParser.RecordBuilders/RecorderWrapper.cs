namespace SharpAttributeParser.RecordBuilders;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using System;
using System.Collections.Generic;

/// <inheritdoc cref="IRecorderWrapper"/>
public sealed class RecorderWrapper : IRecorderWrapper
{
    /// <summary>Instantiates a <see cref="RecorderWrapper"/>, handles wrapping of record-builder targetting recorders in non-record-builder targetting recorders.</summary>
    public RecorderWrapper() { }

    IRecorder<TRecord> IRecorderWrapper.Wrap<TRecord, TRecordBuilder>(IRecorder<TRecordBuilder> recorder)
    {
        if (recorder is null)
        {
            throw new ArgumentNullException(nameof(recorder));
        }

        return new Recorder<TRecord, TRecordBuilder>(recorder);
    }

    private sealed class Recorder<TRecord, TRecordBuilder> : IRecorder<TRecord> where TRecordBuilder : IRecordBuilder<TRecord>
    {
        private readonly IRecorder<TRecordBuilder> WrappedRecorder;

        private readonly TypeRecorder Type;
        private readonly ConstructorRecorder Constructor;
        private readonly NamedRecorder Named;

        private bool HasBeenBuilt;

        public Recorder(IRecorder<TRecordBuilder> wrappedRecorder)
        {
            WrappedRecorder = wrappedRecorder;

            Type = new(wrappedRecorder.Type);
            Constructor = new(wrappedRecorder.Constructor);
            Named = new(wrappedRecorder.Named);
        }

        ITypeRecorder IRecorder.Type => Type;
        IConstructorRecorder IRecorder.Constructor => Constructor;
        INamedRecorder IRecorder.Named => Named;

        TRecord IRecorder<TRecord>.BuildRecord()
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

        private sealed class TypeRecorder : ITypeRecorder
        {
            private readonly ITypeRecorder WrappedRecorder;

            private bool HasBeenBuilt;

            public TypeRecorder(ITypeRecorder wrappedRecorder)
            {
                WrappedRecorder = wrappedRecorder;
            }

            public void SetHasBeenBuilt()
            {
                HasBeenBuilt = true;
            }

            bool ITypeRecorder.TryRecordArgument(ITypeParameterSymbol parameter, ITypeSymbol argument, ExpressionSyntax syntax)
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

                if (syntax is null)
                {
                    throw new ArgumentNullException(nameof(syntax));
                }

                return WrappedRecorder.TryRecordArgument(parameter, argument, syntax);
            }
        }

        private sealed class ConstructorRecorder : IConstructorRecorder
        {
            private readonly IConstructorRecorder WrappedRecorder;

            private bool HasBeenBuilt;

            public ConstructorRecorder(IConstructorRecorder wrappedRecorder)
            {
                WrappedRecorder = wrappedRecorder;
            }

            public void SetHasBeenBuilt()
            {
                HasBeenBuilt = true;
            }

            bool IConstructorRecorder.TryRecordArgument(IParameterSymbol parameter, object? argument, ExpressionSyntax syntax)
            {
                VerifyCanModify();

                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                if (syntax is null)
                {
                    throw new ArgumentNullException(nameof(syntax));
                }

                return WrappedRecorder.TryRecordArgument(parameter, argument, syntax);
            }

            bool IConstructorRecorder.TryRecordDefaultArgument(IParameterSymbol parameter, object? argument)
            {
                VerifyCanModify();

                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                return WrappedRecorder.TryRecordDefaultArgument(parameter, argument);
            }

            bool IConstructorRecorder.TryRecordParamsArgument(IParameterSymbol parameter, object? argument, IReadOnlyList<ExpressionSyntax> elementSyntax)
            {
                VerifyCanModify();

                if (parameter is null)
                {
                    throw new ArgumentNullException(nameof(parameter));
                }

                if (elementSyntax is null)
                {
                    throw new ArgumentNullException(nameof(elementSyntax));
                }

                return WrappedRecorder.TryRecordParamsArgument(parameter, argument, elementSyntax);
            }

            private void VerifyCanModify()
            {
                if (HasBeenBuilt)
                {
                    ThrowModifyAfterBuild();
                }
            }
        }

        private sealed class NamedRecorder : INamedRecorder
        {
            private readonly INamedRecorder WrappedRecorder;

            private bool HasBeenBuilt;

            public NamedRecorder(INamedRecorder wrappedRecorder)
            {
                WrappedRecorder = wrappedRecorder;
            }

            public void SetHasBeenBuilt()
            {
                HasBeenBuilt = true;
            }

            bool INamedRecorder.TryRecordArgument(string parameterName, object? argument, ExpressionSyntax syntax)
            {
                if (HasBeenBuilt)
                {
                    ThrowModifyAfterBuild();
                }

                if (parameterName is null)
                {
                    throw new ArgumentNullException(nameof(parameterName));
                }

                if (syntax is null)
                {
                    throw new ArgumentNullException(nameof(syntax));
                }

                return WrappedRecorder.TryRecordArgument(parameterName, argument, syntax);
            }
        }
    }
}
