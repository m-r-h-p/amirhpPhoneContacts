
namespace PhoneContacts.Business.Messages
{
    public class SystemOperationMessages
    {
        public enum OperationStatus
        {
            Success,
            UserNotFound,
            UserIsActive,
            UserIsNotActive,
            ActiveCodeIsWrong,
            EmailNotFount,
            EmailIsExist,
            PhoneNuberIsExist,
            PhoneNuberNotFound,
            DataBaseError,
            Null,
            ParameterIsNotValid,
            DataNoExist,
            EmailCouldNotSend,
            ContactNotFound,
            SmsSendingFaild
        }
    }
}
