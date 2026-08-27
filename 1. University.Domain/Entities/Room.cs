using System.Collections.Generic;

namespace University.Domain.Entities;

public class Room
{
    public int Id { get; set; }
    public string BuildingName { get; set; }
    public string RoomNumber { get; set; }
    public int Capacity { get; set; }

    public ICollection<CourseOffering> CourseOfferings { get; set; } = new List<CourseOffering>();
}
