using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ToDo_App.Models;
using ToDo_App.ValidationAttributes;

namespace ToDo_App.Services
{
    public class ErrorMessageTranslationService
    {
        private readonly IStringLocalizer<ToDo> _sharedLocalizer;
        public ErrorMessageTranslationService(IStringLocalizer<ToDo> sharedLocalizer)
        {
            _sharedLocalizer = sharedLocalizer;
        }

        public string GetLocalizedError(string errorKey)
        {
            return _sharedLocalizer[errorKey];
        }
    }
}
