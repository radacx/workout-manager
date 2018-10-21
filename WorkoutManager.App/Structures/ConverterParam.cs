namespace WorkoutManager.App.Structures
{
    internal static class ConverterUtils
    {
        public static ConverterParam GetConverterParam(object param)
        {
            if (param is ConverterParam converterParam)
            {
                return converterParam;
            }

            return ConverterParam.None;
        }    
    }
    
    internal enum ConverterParam
    {
        None,
        Negation,
    }
}