using System.Data.SqlClient;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace mmmsl.Areas.Manage
{
    public static class ErrorMessages
    {
        private const int DeleteForeignKeyErrorCode = 547;
        private const int DuplicateKeyErrorCode = 2627;

        public const string Database = "Unable to save changes. Try again, and if the problem persists send Matt an email.";
        public const string DeleteForeignKey = "You must remove associated items first before you can remove this item.";
        public const string DuplicatePrimaryKey = "This item already exists in the database. Please edit or delete the existing item instead of creating a new one.";

        public static void AddDatabaseError(this ModelStateDictionary modelState, DbUpdateException exception)
        {
            var innerException = exception.InnerException as SqlException;

            if (innerException == null) {
                modelState.AddModelError("", Database);
                return;
            }

            switch (innerException.Number) {
                case DeleteForeignKeyErrorCode:
                    modelState.AddModelError("", DeleteForeignKey);
                    break;
                case DuplicateKeyErrorCode:
                    modelState.AddModelError("", DuplicatePrimaryKey);
                    break;
                default:
                    modelState.AddModelError("", Database);
                    break;
            }
        }
    }
}