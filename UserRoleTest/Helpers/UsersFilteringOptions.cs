﻿namespace UserRoleTest.Helpers
{
    /// <summary>
    /// Инкапсулирует текстовые параметры для фильтрации коллекции User
    /// </summary>
    public class UsersFilteringOptions
    {
        public string Id { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public string Age { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
    }
}
