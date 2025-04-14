using System.Collections.Generic;
using Backend.Challenge.Dtos;

namespace Backend.Challenge.ServiceModels
{
    public class GetUserResponse
    {
        public Dictionary<int, UserDto> Users { get; set; }
    }
}