using System.Collections.Generic;
using System.Linq;

namespace Compiler.ErrorHandler
{
    public class ErrorHandler
    {
        private static Dictionary<ErrorType, List<Error>> _errors = new Dictionary<ErrorType, List<Error>>();

        public static List<Error> ObtainErrors(ErrorType tipoError)
        {
            if (!_errors.ContainsKey(tipoError))
            {
                _errors.Add(tipoError, new List<Error>());
            }

            return _errors[tipoError];
        }

        public static void Report(Error error)
        {
            if (error != null)
            {
                ObtainErrors(error.ErrorType).Add(error);
            }
        }

        public static bool HasErrors(ErrorType tipoError) => ObtainErrors(tipoError).Count > 0;

        public static bool HasErrors()
        {
            return HasErrors(ErrorType.Lexical) || HasErrors(ErrorType.Semantic) || HasErrors(ErrorType.Syntactic);
        }

        public static List<Error> ObtainAllErrors()
        {
            return _errors.Values.SelectMany(error => error).ToList();
        }

        public static void Clear()
        {
            _errors.Clear();
        }
    }
}

