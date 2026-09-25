using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace XRayJournal.Core
{
    public static class SecurityConstants
    {
        /// <summary>
        /// BCrypt-хеш для пароля по умолчанию.
        /// Используется при создании новых пользователей и при проверке необходимости смены пароля.
        /// </summary>
        public const string DefaultPasswordHash = "$2a$12$VqlwKy2yA/1hwMK9YWHoxuCbS.wZdCS/iZ5.V2FyKcUwsfAqEscOa";
    }
}
