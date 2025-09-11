using FluentValidation;
using System;
using System.Linq;
using System.Reflection;

namespace POS_display.Utils.Validators
{
    internal class ValidatorFactory : ValidatorFactoryBase
    {
        private readonly Assembly _assembly;

        public ValidatorFactory(Assembly assembly)
        {
            _assembly = assembly;
        }

        public override IValidator CreateInstance(Type validatorType)
        {
            var instance = _assembly.GetTypes().First(validatorType.IsAssignableFrom);

            return (IValidator)Activator.CreateInstance(instance);
        }
    }
}
