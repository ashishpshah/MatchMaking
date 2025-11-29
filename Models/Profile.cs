using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace MatchMaking.Models
{
	public partial class Profile : EntitiesBase
	{
		public override long Id { get; set; }
		public long? UserId { get; set; }
		public string? Firstname { get; set; }
		public string? Fathername { get; set; }
		public string? Mothername { get; set; }
		public string? PaternalSurname { get; set; }
		public string? MaternalSurname { get; set; }
		public string? Mosal { get; set; }
		public string? Gender { get; set; }
		public string? LookingForGender { get; set; }
		public string? MaritalStatus { get; set; }
		public DateTime? DateOfBirth { get; set; }
		public string? Address { get; set; }
		public string? City { get; set; }
		public string? State { get; set; }
		public string? Country { get; set; }
		public string? Education { get; set; }
		public string? Occupation { get; set; }
		public string? Summary { get; set; }
		public int? GroupId { get; set; }
		[NotMapped] public string? GroupName { get; set; }
		public string? Interests { get; set; }
		public string? Smoking { get; set; }
		public string? Height { get; set; }
		public string? Weight { get; set; }
		public string? HairColor { get; set; }
		public string? EyeColor { get; set; }
		public string? BodyType { get; set; }
		public string? Diet { get; set; }
		public string? Language { get; set; }

		[NotMapped] public string? FullName { get { return $"{Firstname} {PaternalSurname}"; } }
		[NotMapped] public string? FullAddress { get { return $"{Firstname} {PaternalSurname}"; } }
		[NotMapped]
		public int Age
		{
			get
			{
				var today = DateTime.Today;
				int age = today.Year - DateOfBirth.Value.Year;
				if (DateOfBirth?.Date > today.AddYears(-age))
					age--;

				return age;
			}
		}
	}

}
