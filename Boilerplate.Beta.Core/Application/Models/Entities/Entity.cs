using System.ComponentModel.DataAnnotations;

namespace Boilerplate.Beta.Core.Application.Models.Entities
{
	public class Entity
	{
		[Key]
		public Guid Id { get; set; }
		public Guid Property1 { get; set; }
		public string Property2 { get; set; }
	}
}
