using Microsoft.AspNetCore.Http;
using PhoneContacts.Business.Messages;
using PhoneContacts.Data.DTOs;

namespace PhoneContacts.Business.StatusCode
{
    public static class Hnadler
    {
        public static ReturnDTO SetStatusCode(ReturnDTO statusCode)
        {
            if (statusCode.meta.message == SystemOperationMessages.OperationStatus.Success.ToString())
            {
                statusCode.statusCode = StatusCodes.Status200OK;
                //statusCode.meta.message = "عملیات با موفقیت انجام شد";
            }
            else if (statusCode.meta.message == SystemOperationMessages.OperationStatus.UserIsActive.ToString())
            {
                statusCode.statusCode = StatusCodes.Status200OK;
                //statusCode.meta.message = "این ایمیل از قبل در سیستم ثبت شده است";

            }
            else if (statusCode.meta.message == SystemOperationMessages.OperationStatus.UserIsNotActive.ToString())
            {
                statusCode.statusCode = StatusCodes.Status404NotFound;
                //statusCode.meta.message = "این ایمیل از قبل در سیستم ثبت شده است";

            }
            else if (statusCode.meta.message == SystemOperationMessages.OperationStatus.EmailIsExist.ToString())
            {
                statusCode.statusCode = StatusCodes.Status400BadRequest;
                //statusCode.meta.message = "این ایمیل از قبل در سیستم ثبت شده است";

            }
            else if (statusCode.meta.message == SystemOperationMessages.OperationStatus.PhoneNuberIsExist.ToString())
            {
                statusCode.statusCode = StatusCodes.Status400BadRequest;
                //statusCode.meta.message = "مشکلی در سمت دیتابیس یا سرور پیش آمده";

            }
            else if (statusCode.meta.message == SystemOperationMessages.OperationStatus.ParameterIsNotValid.ToString())
            {
                statusCode.statusCode = StatusCodes.Status400BadRequest;
                //statusCode.meta.message = "یک یا چند پارامتر به درستی ارسال نشده است";

            }
            else if (statusCode.meta.message == SystemOperationMessages.OperationStatus.UserNotFound.ToString())
            {
                statusCode.statusCode = StatusCodes.Status404NotFound;
                //statusCode.meta.message = "کاربری با این مشخصات یافت نشد";

            }
            else if (statusCode.meta.message == SystemOperationMessages.OperationStatus.Null.ToString())
            {
                statusCode.statusCode = StatusCodes.Status400BadRequest;
                //statusCode.meta.message = "مقداری ارسال نشده است";

            }
            else if (statusCode.meta.message == SystemOperationMessages.OperationStatus.DataNoExist.ToString())
            {
                statusCode.statusCode = StatusCodes.Status404NotFound;
                //statusCode.meta.message = "رکوردی در این جدول وجود ندارد";

            }
            else if (statusCode.meta.message == SystemOperationMessages.OperationStatus.EmailNotFount.ToString())
            {
                statusCode.statusCode = StatusCodes.Status404NotFound;
                //statusCode.meta.message = "این ایمیل از قبل در سیستم ثبت نشده است";


            }
            else if (statusCode.meta.message == SystemOperationMessages.OperationStatus.DataBaseError.ToString())
            {
                statusCode.statusCode = StatusCodes.Status500InternalServerError;
                //statusCode.meta.message = "مشکلی در سمت دیتابیس یا سرور پیش آمده";

            } 
            else if (statusCode.meta.message == SystemOperationMessages.OperationStatus.SmsSendingFaild.ToString())
            {
                statusCode.statusCode = StatusCodes.Status500InternalServerError;
                //statusCode.meta.message = "مشکلی در سمت دیتابیس یا سرور پیش آمده";

            } 
            else
            {
                statusCode.statusCode = StatusCodes.Status404NotFound;
                //statusCode.meta.message = "استاتوس کد مناسب دریافت نشد";
            }

            return statusCode;
        }
    }
}
