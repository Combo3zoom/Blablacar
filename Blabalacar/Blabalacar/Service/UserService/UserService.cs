using System.Security.Claims;
using Blabalacar.Models;
using Blabalacar.Models.Entities.User;
using Blabalacar.Repository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace Blabalacar.Service.User;

public class UserService:IUserService
{
    private readonly IMemoryCache _memoryCache;

    public UserService(IMemoryCache memoryCache)
    {
        _memoryCache = memoryCache;
    }
    public void UpdateSelfUser(Models.User changeUser, UpdateUserBody user)
    {
        changeUser.Name = user.Name;
    }
    
    public void AdminUpdateUser(Models.User changeUser, AdminUpdateUserBody user)
    {
        changeUser.Name = user.Name;
        changeUser.Role = user.Role;
        user.IsVerification = user.IsVerification;
    }

    public UserTrip FoundUserTrip(Models.User user, Trip trip)
    {
        return new UserTrip{User = user, UserId = user.Id, Trip = trip, TripId = trip.Id};
    }
    
}