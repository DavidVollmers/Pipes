namespace Pipes;

public static class InputConverter
{
    public static bool TryConvertInput<TInput>(object? input, out TInput? convertedInput,
        bool allowSingleEnumerable = true)
    {
        convertedInput = default;

        switch (input)
        {
            case null:
                return false;
            case TInput i:
                convertedInput = i;
                return true;
            case IEnumerable<TInput> e:
                if (!allowSingleEnumerable) return false;
                convertedInput = e.Single();
                return convertedInput != null;
        }

        return false;
    }
}