using Microsoft.AspNetCore.Mvc;
using Blabalacar.Models;
using Blabalacar.Models.Entities.User;

namespace Blabalacar.Service;

public interface IUserService
{
    void UpdateSelfUser(Models.User changeUser, UpdateUserBody user);
    void AdminUpdateUser(Models.User changeUser, AdminUpdateUserBody user);
    UserTrip FoundUserTrip(Models.User user, Trip trip);

}