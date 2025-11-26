using System;
using System.Collections.Generic;

namespace MatchMaking.Models_Temp
{
    public partial class Profile
    {
        public long Id { get; set; }
        public long? UserId { get; set; }
        public string? Firstname { get; set; }
        public string? Middlename { get; set; }
        public string? Lastname { get; set; }
        public string? Gender { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? City { get; set; }
        public string? State { get; set; }
        public string? Country { get; set; }
        public string? Education { get; set; }
        public string? Occupation { get; set; }
        public string? Summary { get; set; }
        public int? GroupId { get; set; }
        public string? FatherSurname { get; set; }
        public string? MotherSurname { get; set; }
        public string? PaternalSurname { get; set; }
        public string? MaternalSurname { get; set; }
        public string? Mosal { get; set; }
        public string? ProfilePhotoPath { get; set; }
        public string? CoverPhotoPath { get; set; }
        public string? Smoking { get; set; }
        public string? Height { get; set; }
        public string? Weight { get; set; }
        public string? HairColor { get; set; }
        public string? EyeColor { get; set; }
        public string? BodyType { get; set; }
        public string? Ethnicity { get; set; }
        public bool? IsActive { get; set; }
        public bool? IsDeleted { get; set; }
        public long? CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public long? LastModifiedBy { get; set; }
        public DateTime? LastModifiedDate { get; set; }
    }
}
