using Microsoft.EntityFrameworkCore;
using PhoneContacts.Business.Messages;
using PhoneContacts.Business.Senders;
using PhoneContacts.Data.DatabseContext;
using PhoneContacts.Data.DTOs;
using PhoneContacts.Data.Models;
using Rituna.Common;
using System.Net.Mail;

namespace PhoneContacts.Business.Services.UserService
{

    public class UserServices : IUserServices
    {
        private readonly Context _context;

        public UserServices(Context context)
        {
            _context = context;
        }
        public async Task<ReturnDTO> GetAllUsers()
        {
            ReturnDTO returnDTO = new ReturnDTO();
            try
            {

                List<User> query = await _context.Users
                    .Where(u => u.IsActive == true)
                    .ToListAsync();

                if (query != null && query.Count > 0)
                {
                    List<UserDTO> result = new List<UserDTO>();
                    query.ForEach(u =>
                    {
                        result.Add(new UserDTO
                        {
                            Id = u.Id,
                            Age = (byte)u.Age,
                            Email = u.Email,
                            FirstName = u.FirstName,
                            LastName = u.LastName,
                        });
                    });

                    returnDTO.data = new Data.DTOs.Data()
                    {
                        values = result
                    };
                    returnDTO.meta = new Meta()
                    {
                        message = SystemOperationMessages.OperationStatus.Success.ToString()
                    };

                    return returnDTO;
                }
                else
                {
                    returnDTO.meta = new Meta()
                    {
                        message = SystemOperationMessages.OperationStatus.DataNoExist.ToString()
                    };

                    return returnDTO;
                }
            }
            catch
            {
                returnDTO.meta = new Meta()
                {
                    message = SystemOperationMessages.OperationStatus.DataBaseError.ToString()
                };

                return returnDTO;
            }
        }
        public async Task<ReturnDTO> GetUsersById(long id)
        {

            if (id > 0)
            {
                ReturnDTO returnDTO = new ReturnDTO();
                UserDTO result;

                try
                {
                    User query = _context.Users.FirstOrDefault(u => u.Id == id && u.IsActive == true);
                    if (query != null)
                    {
                        result = new UserDTO()
                        {
                            Id = query.Id,
                            FirstName = query.FirstName,
                            LastName = query.LastName,
                            Email = query.Email,
                            Age = (byte)query.Age,
                        };

                        returnDTO.data = new Data.DTOs.Data()
                        {
                            values = result
                        };
                        returnDTO.meta = new Meta()
                        {
                            message = SystemOperationMessages.OperationStatus.Success.ToString()
                        };

                        return returnDTO;
                    }
                    else
                    {
                        returnDTO.meta = new Meta()
                        {
                            message = SystemOperationMessages.OperationStatus.UserNotFound.ToString()
                        };
                        return returnDTO;
                    }
                }
                catch
                {
                    returnDTO.meta = new Meta()
                    {
                        message = SystemOperationMessages.OperationStatus.DataBaseError.ToString()
                    };
                    return returnDTO;
                }
            }
            else
            {
                return new ReturnDTO()
                {
                    data = null,
                    meta = new Meta()
                    {
                        message = SystemOperationMessages.OperationStatus.ParameterIsNotValid.ToString()
                    }
                };
            }

        }
        public async Task<ReturnDTO> CreateUser(RegisterDTO user)
        {
            ReturnDTO returnDTO = new ReturnDTO();

            if (user != null)
            {
                try
                {
                    string isEmailExist = await IsEmailExist(user.EmailAddress);
                    string isPhoneExist = await IsPhoneNumberExist(user.PhoneNumber);

                    if (isEmailExist == SystemOperationMessages.OperationStatus.EmailIsExist.ToString())
                    {
                        returnDTO.meta = new Meta()
                        {
                            message = SystemOperationMessages.OperationStatus.EmailIsExist.ToString()
                        };
                        return returnDTO;
                    }
                    else if (isPhoneExist == SystemOperationMessages.OperationStatus.PhoneNuberIsExist.ToString())
                    {
                        returnDTO.meta = new Meta()
                        {
                            message = SystemOperationMessages.OperationStatus.PhoneNuberIsExist.ToString()
                        };
                        return returnDTO;
                    }

                    User newUser = new User()
                    {
                        FirstName = user.FirstName.Trim(),
                        LastName = user.LastName.Trim(),
                        Age = (byte)user.Age,
                        Password = PasswordHasher.EncodePasswordMd5(user.Password),
                        Email = user.EmailAddress.Trim().ToLower(),
                        PhoneNumber = user.PhoneNumber.Trim(),
                        VerficationCode = new Random().Next(10000, 99999)
                    };


                    try
                    {
                        string messageToSend = newUser.VerficationCode.ToString();
                        string numberToSend = newUser.PhoneNumber;
                        bool smsResult = await Sender.SMS(numberToSend, messageToSend);

                        if (smsResult == true)
                        {
                            await _context.Users.AddAsync(newUser);
                            await _context.SaveChangesAsync();
                        }
                        else
                        {
                            returnDTO.meta = new Meta()
                            {
                                message = SystemOperationMessages.OperationStatus.SmsSendingFaild.ToString()
                            };
                            return returnDTO;
                        }
                    }
                    catch
                    {
                        returnDTO.meta = new Meta()
                        {
                            message = SystemOperationMessages.OperationStatus.SmsSendingFaild.ToString()
                        };
                        return returnDTO;
                    }






                    returnDTO.meta = new Meta()
                    {
                        message = SystemOperationMessages.OperationStatus.Success.ToString()
                    };
                    return returnDTO;
                }
                catch
                {
                    returnDTO.meta = new Meta()
                    {
                        message = SystemOperationMessages.OperationStatus.DataBaseError.ToString()
                    };
                    return returnDTO;
                }
            }
            else
            {
                returnDTO.meta = new Meta()
                {
                    message = SystemOperationMessages.OperationStatus.Null.ToString()
                };
                return returnDTO;

            }
        }
        public async Task<ReturnDTO> LoginUser(string email, string password)
        {
            ReturnDTO returnDTO = new ReturnDTO();
            UserDTO result;
            if (!String.IsNullOrEmpty(email) && !String.IsNullOrEmpty(password))
            {


                try
                {
                    string Useremail = email.ToLower().Trim();
                    string Userpassword = PasswordHasher.EncodePasswordMd5(password.ToLower().Trim());
                    var query = await
                         _context.Users.FirstOrDefaultAsync(u => u.Email == Useremail && u.Password == Userpassword);

                    if (query != null)
                    {
                        if (query.IsActive == false)
                        {
                            returnDTO.meta = new Meta()
                            {
                                message = SystemOperationMessages.OperationStatus.UserIsNotActive.ToString()
                            };
                            return returnDTO;
                        }

                        result = new UserDTO()
                        {
                            Id = query.Id,
                            FirstName = query.FirstName,
                            LastName = query.LastName,
                            Email = query.Email,
                            Age = (byte)query.Age,
                        };

                        returnDTO.data = new Data.DTOs.Data()
                        {
                            values = result
                        };
                        returnDTO.meta = new Meta()
                        {
                            message = SystemOperationMessages.OperationStatus.Success.ToString()
                        };

                        return returnDTO;
                    }
                    else
                    {
                        returnDTO.meta = new Meta()
                        {
                            message = SystemOperationMessages.OperationStatus.UserNotFound.ToString()
                        };
                        return returnDTO;
                    }
                }
                catch
                {
                    returnDTO.meta = new Meta()
                    {
                        message = SystemOperationMessages.OperationStatus.DataBaseError.ToString()
                    };
                    return returnDTO;
                }
            }
            else
            {
                returnDTO.meta = new Meta()
                {
                    message = SystemOperationMessages.OperationStatus.Null.ToString()
                };
                return returnDTO;
            }
        }
        public async Task<ReturnDTO> UserActivition(string emailAddress, int activisionCode)
        {
            ReturnDTO returnDTO = new ReturnDTO();
            if (!String.IsNullOrEmpty(emailAddress) && activisionCode > 0)
            {
                try
                {
                    emailAddress = emailAddress.Trim().ToLower();
                    User user = await _context.Users.FirstOrDefaultAsync(u => u.Email == emailAddress && u.IsActive == false);
                    if (user != null)
                    {
                        if (user.VerficationCode == activisionCode)
                        {
                            user.IsActive = true;
                            user.VerficationCode = new Random().Next(10000, 99999);
                            _context.Users.Update(user);

                            await _context.SaveChangesAsync();

                            returnDTO.meta = new Meta()
                            {
                                message = SystemOperationMessages.OperationStatus.UserIsActive.ToString()
                            };
                            return returnDTO;

                        }
                        else
                        {
                            returnDTO.meta = new Meta()
                            {
                                message = SystemOperationMessages.OperationStatus.ActiveCodeIsWrong.ToString()
                            };
                            return returnDTO;
                        }
                    }
                    else
                    {
                        returnDTO.meta = new Meta()
                        {
                            message = SystemOperationMessages.OperationStatus.UserNotFound.ToString()
                        };
                        return returnDTO;
                    }
                }
                catch
                {
                    returnDTO.meta = new Meta()
                    {
                        message = SystemOperationMessages.OperationStatus.DataBaseError.ToString()
                    };
                    return returnDTO;
                }
            }
            else
            {
                returnDTO.meta = new Meta()
                {
                    message = SystemOperationMessages.OperationStatus.ParameterIsNotValid.ToString()
                };
                return returnDTO;
            }
        }
        public async Task<Enum> UpdateUser(User user)
        {
            //try
            //{
            //    User userToUpdate = await GetUsersById(user.Id);
            //    if (userToUpdate != null)
            //    {
            //        userToUpdate.FirstName = user.FirstName;
            //        userToUpdate.LastName = user.LastName;
            //        userToUpdate.Email = user.Email;
            //        userToUpdate.Address = user.Address;
            //        userToUpdate.PhoneNumber = user.PhoneNumber;


            //        try
            //        {
            //            _context.Users.Update(userToUpdate);
            //            _context.SaveChangesAsync();
            //            return SystemOperationMessages.OperationStatus.Success;

            //        }
            //        catch
            //        {
            //            return SystemOperationMessages.OperationStatus.DataBaseError;
            //        }

            //    }
            //    else
            //    {
            //        return SystemOperationMessages.OperationStatus.UserNotFount;
            //    }

            //}
            //catch
            //{
            //    return SystemOperationMessages.OperationStatus.Faild;
            //}

            return null;

        }
        public async Task<string> IsEmailExist(string emailAddress)
        {
            if (!string.IsNullOrEmpty(emailAddress))
            {
                try
                {
                    bool result = await _context.Users.AnyAsync(u => u.Email == emailAddress.Trim().ToLower());
                    if (result)
                    {
                        return SystemOperationMessages.OperationStatus.EmailIsExist.ToString();
                    }
                    else
                    {
                        return SystemOperationMessages.OperationStatus.EmailNotFount.ToString();
                    }
                }
                catch
                {
                    return SystemOperationMessages.OperationStatus.DataBaseError.ToString();
                }

            }
            else
            {
                return SystemOperationMessages.OperationStatus.Null.ToString();
            }

        }

        public async Task<string> IsPhoneNumberExist(string phoneNumber)
        {
            if (!string.IsNullOrEmpty(phoneNumber))
            {
                try
                {
                    bool result = await _context.Users.AnyAsync(u => u.PhoneNumber == phoneNumber.Trim().ToLower());
                    if (result)
                    {
                        return SystemOperationMessages.OperationStatus.PhoneNuberIsExist.ToString();
                    }
                    else
                    {
                        return SystemOperationMessages.OperationStatus.PhoneNuberNotFound.ToString();
                    }
                }
                catch
                {
                    return SystemOperationMessages.OperationStatus.DataBaseError.ToString();
                }

            }
            else
            {
                return SystemOperationMessages.OperationStatus.Null.ToString();
            }
        }


    }

}

