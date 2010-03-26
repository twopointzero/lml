namespace twopointzero.Lml.Validation
{
    public static class Validator
    {
        public static ValidatorInstance Create()
        {
            return null;
        }

        public static void IsNotNull<T>(T actualValue, string paramName) where T : class
        {
            ValidatorInstanceExtensions.IsNotNull(null, actualValue, paramName).Validate();
        }

        public static void IsNotNull<T>(T actualValue, string paramName, string message) where T : class
        {
            ValidatorInstanceExtensions.IsNotNull(null, actualValue, paramName, message).Validate();
        }
    }
}