using Employes.Feature.User.Requests;
using Employes.Feature.User.Response;

namespace Employes.Feature.User.Profile;

public class UserProfile: AutoMapper.Profile 
{
    public UserProfile()
    {
        CreateMap<CreateUserRequest, ModelLibrary.Entities.User>();
        CreateMap<CreatePhoneNumberRequest, PhoneNumber>();
        CreateMap<ModelLibrary.Entities.User, ModelLibrary.Entities.User>();
        CreateMap<UpdateUserRequest, ModelLibrary.Entities.User>();
        CreateMap<ModelLibrary.Entities.User, CreateUserRequest>();
        CreateMap<ModelLibrary.Entities.User, UpdateUserRequest>();
        CreateMap<PhoneNumber, CreatePhoneNumberRequest>();
        CreateMap<PhoneNumber, UpdatePhoneNumberRequest>();
        CreateMap<ModelLibrary.Entities.User, UserResponse>();
        CreateMap<PhoneNumber, PhoneNumberResponse>();
    }
    
}